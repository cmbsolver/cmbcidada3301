using System.Text;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Analysis;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusAnalysisTool.Application.Queries.Analysis;

public class GetFrequencyAnalysisForRuneText
{
    public class Query: INotification
    {
        public Query(string input, bool isPermuteCombinations, string output, string mode, string[] charactersToExclude)
        {
            Input = input;
            Output = output;
            IsPermuteCombinations = isPermuteCombinations;
            Mode = mode;
            CharactersToExclude = charactersToExclude;
        }

        public string Input { get; private set; }
        
        public string Output { get; private set; }
        
        public string Mode { get; private set; }
        
        public bool IsPermuteCombinations { get; private set; }
        
        public string[] CharactersToExclude { get; private set; }
    }
    
    public class Handler: INotificationHandler<Query>
    {
        private readonly IMediator _mediator;
        
        private bool _isPermuteComplete = false;
        
        private Queue<SubstitutionPossibility[]> _theQueue = new Queue<SubstitutionPossibility[]>();
        
        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(Query request, CancellationToken cancellationToken)
        {
            List<string> wordList;
            StringBuilder fileText = new();
            List<TextScore> textScores = new();
            
            using (var context = new LiberContext())
            {
                wordList = await context.DictionaryWords.Select(x => x.RuneWordText).ToListAsync();
            }
            
            List<SubstitutionPossibility> substitutionPossibilities = new();

            // Get the letter frequency from the Liber text
            LetterFrequency dbFrequency =
                await _mediator.Send(new GetLetterForFrequencyFromLib.Query(request.Mode));

            // Get the letter frequency from the text
            var runeFrequency = new LetterFrequency();

            var text = await File.ReadAllTextAsync(request.Input);
            text = text.Trim();
            foreach (var letter in text)
            {
                var upperLetter = char.ToUpper(letter).ToString();
                if (!request.CharactersToExclude.Any(x => x == upperLetter))
                {
                    runeFrequency.AddLetter(upperLetter);
                }
            }

            runeFrequency.UpdateLetterFrequencyDetails();

            // Now we need to compare the letter frequency from the Liber text with the letter frequency from the text
            foreach (var runeFrequencyDetail in runeFrequency.LetterFrequencyDetails)
            {
                substitutionPossibilities.Add(GetSubstitutionPossibility(runeFrequencyDetail.Letter,
                    runeFrequencyDetail.Frequency, dbFrequency));
            }

            if (request.IsPermuteCombinations)
            {
                var possArray = substitutionPossibilities.ToArray();

                // Perform string replacements for each permutation
                _ = Task.Run(() =>  GeneratePermutations(possArray), CancellationToken.None);
                
                while(!_isPermuteComplete || _theQueue.Count > 0)
                {
                    while (_theQueue.Count <= 0)
                    {
                        await Task.Delay(1000);
                    }
                    
                    var permutation = _theQueue.Dequeue();
                    
                    var sb = new StringBuilder();

                    foreach (var xchar in text)
                    {
                        if (!request.CharactersToExclude.Any(x => x == xchar.ToString().ToUpper()))
                        {
                            var replacement = permutation.FirstOrDefault(x => x.Letter == xchar.ToString().ToUpper());
                            if (replacement != null)
                            {
                                sb.Append(replacement.GetCurrentReplacementLetter());
                            }
                            else
                            {
                                sb.Append(xchar);
                            }
                        }
                    }

                    // Now we need to score the text for matches.
                    // Highest matches are output to the files.
                    var score = await _mediator.Send(new ScoreText.Command(sb.ToString(), wordList));
                    var permutationString = string.Join(",",
                        permutation.Select(x => { return $"{x.Letter} -> {x.GetCurrentReplacementLetter()}"; }));
                    
                    if (AddAndReorderScore(new TextScore(sb.ToString(), score, permutationString), ref textScores))
                    {
                        fileText.Clear();
                        foreach (var textScore in textScores)
                        {
                            var transposition = await _mediator.Send(new TransposeRuneToLatin.Command(textScore.Text));
                            
                            fileText.AppendLine(request.Input);
                            fileText.AppendLine($"Score {textScore.Score}");
                            fileText.AppendLine(textScore.PermutationString);
                            fileText.AppendLine(textScore.Text);
                            fileText.AppendLine(Environment.NewLine);
                            fileText.AppendLine(transposition);
                            fileText.AppendLine(Environment.NewLine);
                        }
                        
                        await File.WriteAllTextAsync(request.Output, fileText.ToString());
                    }
                }
            }
            else
            {
                var sb = new StringBuilder();

                foreach (var xchar in text)
                {
                    if (!request.CharactersToExclude.Any(x => x == xchar.ToString().ToUpper()))
                    {
                        var replacement =
                            substitutionPossibilities.FirstOrDefault(x => x.Letter == xchar.ToString().ToUpper());
                        if (replacement != null)
                        {
                            sb.Append(replacement.GetCurrentReplacementLetter());
                        }
                        else
                        {
                            sb.Append(xchar);
                        }
                    }
                    else
                    {
                        sb.Append(xchar);
                    }
                }
                
                await File.AppendAllTextAsync(request.Output, request.Input + Environment.NewLine);
                await File.AppendAllTextAsync(request.Output, sb + Environment.NewLine);
                await File.AppendAllTextAsync(request.Output, Environment.NewLine);
                await File.AppendAllTextAsync(request.Output, Environment.NewLine);
            }
        }
        
        private bool AddAndReorderScore(TextScore score, ref List<TextScore> textScores)
        {
            bool needToWriteNewFile = false;
            ulong[] topScores = textScores.Select(x => x.Score).ToArray();
            textScores.Add(score);
            if (textScores.Count > 100)
            {
                textScores = textScores.OrderByDescending(x => x.Score).ToList();
                textScores = textScores.Take(100).ToList();
                var currScores = textScores.Select(x => x.Score).ToArray();
                
                if (topScores != currScores)
                {
                    needToWriteNewFile = true;
                }
            }

            return needToWriteNewFile;
        }

        private SubstitutionPossibility GetSubstitutionPossibility(string letter, double frquency, LetterFrequency liberFrequency)
        {
            List<string> possibleSubstitutions = new();
            string hightestPossibleMatch = string.Empty;
            double hightestMatch = double.MaxValue;
            
            // Finding the closest match
            foreach (var liberFrequencyDetail in liberFrequency.LetterFrequencyDetails)
            {
                double match = System.Math.Abs(liberFrequencyDetail.Frequency - frquency);
                
                if (match < hightestMatch)
                {
                    hightestMatch = match;
                    hightestPossibleMatch = liberFrequencyDetail.Letter;
                }
            }
            
            // Add the closest match to the possible substitutions
            possibleSubstitutions.Add(hightestPossibleMatch);
            
            // Add the other possible substitutions
            var hightestPossibleIndex = liberFrequency.LetterFrequencyDetails
                .FindIndex(x => x.Letter != hightestPossibleMatch);
            
            // Add the next 4 possible substitutions
            int remaining = 4;
            int currIndexPosition = hightestPossibleIndex;
            bool cantGoAnyLower = false;
            bool cantGoAnyUpper = false;
            
            // lowers
            while(currIndexPosition > 0 && remaining > 2)
            {
                currIndexPosition--;

                if (currIndexPosition >= 0)
                {
                    possibleSubstitutions.Add(liberFrequency.LetterFrequencyDetails[currIndexPosition].Letter);
                }

                cantGoAnyLower = currIndexPosition <= 0;
                
                remaining--;
            }
            
            // uppers
            currIndexPosition = hightestPossibleIndex;
            while(currIndexPosition < liberFrequency.LetterFrequencyDetails.Count - 1 && remaining > 0)
            {
                currIndexPosition++;

                if (currIndexPosition <= liberFrequency.LetterFrequencyDetails.Count - 1)
                {
                    possibleSubstitutions.Add(liberFrequency.LetterFrequencyDetails[currIndexPosition].Letter);
                }

                cantGoAnyUpper = currIndexPosition >= liberFrequency.LetterFrequencyDetails.Count - 1;
                
                remaining--;
            }
            
            // Add the last possible substitutions
            if (remaining > 0)
            {
                if (cantGoAnyLower)
                {
                    currIndexPosition = hightestPossibleIndex;
                    while(currIndexPosition < liberFrequency.LetterFrequencyDetails.Count - 1 && remaining > 0)
                    {
                        currIndexPosition++;

                        if (currIndexPosition <= liberFrequency.LetterFrequencyDetails.Count - 1)
                        {
                            possibleSubstitutions.Add(liberFrequency.LetterFrequencyDetails[currIndexPosition].Letter);
                        }

                        remaining--;
                    }
                }
                else if (cantGoAnyUpper)
                {
                    currIndexPosition = hightestPossibleIndex;
                    while(currIndexPosition > 0 && remaining > 0)
                    {
                        currIndexPosition--;

                        if (currIndexPosition >= 0)
                        {
                            possibleSubstitutions.Add(liberFrequency.LetterFrequencyDetails[currIndexPosition].Letter);
                        }

                        remaining--;
                    }
                }
            }
            
            return new SubstitutionPossibility(letter, possibleSubstitutions.ToArray());
        }
        
        private async Task GeneratePermutations(
            SubstitutionPossibility[] substitutionPossibilities, 
            SubstitutionPossibility[]? currentArray = null, 
            int currentSubstituionListIndex = 0)
        {
            var currIdx = currentSubstituionListIndex;
            
            // Setting the current array up.
            var currentList = new List<SubstitutionPossibility>();
            if (currentArray != null)
            {
                currentList.AddRange(currentArray);
            }

            while (substitutionPossibilities[currIdx].HasNextSubstitution())
            {
                var addValue = substitutionPossibilities[currIdx].GetNext();
                if (addValue == null) 
                    break;
                
                currentList.Add(addValue);

                // We should enqueue the current list if it is the last element in the array.
                if (currentList.Count == substitutionPossibilities.Length &&
                    currIdx == substitutionPossibilities.Length - 1)
                {
                    _theQueue.Enqueue(currentList.ToArray());
                    currentList = new List<SubstitutionPossibility>();
                    if (currentArray != null)
                    {
                        currentList.AddRange(currentArray);
                    }

                    while (_theQueue.Count >= 10000000)
                    {
                        await Task.Delay(10000);
                    }
                }
                else if (currIdx < substitutionPossibilities.Length - 1)
                {
                    await GeneratePermutations(substitutionPossibilities, currentList.ToArray(), currIdx + 1);
                    currentList = new List<SubstitutionPossibility>();
                    if (currentArray != null)
                    {
                        currentList.AddRange(currentArray);
                    }
                }
            }

            substitutionPossibilities[currIdx].Reset();

            if (currIdx == 0)
            {
                _isPermuteComplete = true;
            }
        }
        
        
    }
}