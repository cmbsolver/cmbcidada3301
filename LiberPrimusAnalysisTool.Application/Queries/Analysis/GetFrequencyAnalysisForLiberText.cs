using System.Text;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Analysis;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusAnalysisTool.Application.Queries.Analysis;

public class GetFrequencyAnalysisForLiberText
{
    public class Query: INotification
    {
        public Query(string input, bool fromIntermediaryRune, bool isPermuteCombinations, string output, string[] charactersToExclude)
        {
            Input = input;
            FromIntermediaryRune = fromIntermediaryRune;
            Output = output;
            IsPermuteCombinations = isPermuteCombinations;
            CharactersToExclude = charactersToExclude;
        }

        public string Input { get; private set; }
        
        public bool FromIntermediaryRune { get; private set; }
        
        public string Output { get; private set; }
        
        public bool IsPermuteCombinations { get; private set; }
        
        public string[] CharactersToExclude { get; private set; }
    }
    
    public class Handler: INotificationHandler<Query>
    {
        private readonly IMediator _mediator;
        
        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(Query request, CancellationToken cancellationToken)
        {
            List<string> wordList = new();
            double highestScore = 0;
            StringBuilder fileText = new();
            List<TextScore> textScores = new();
            
            using (var context = new LiberContext())
            {
                if (request.FromIntermediaryRune)
                {
                    wordList = await context.DictionaryWords.Select(x => x.RuneglishWordText).ToListAsync();
                }
                else
                {
                    wordList = await context.DictionaryWords.Select(x => x.DictionaryWordText).ToListAsync();
                }
            }
            
            List<SubstitutionPossibility> substitutionPossibilities = new();

            // Get the letter frequency from the Liber text
            LetterFrequency liberFrequency =
                await _mediator.Send(
                    new GetLetterForFrequencyFromLib.Query(
                        request.FromIntermediaryRune ? "intermediary" : "letters"), 
                    cancellationToken);

            // Get the letter frequency from the text
            var letterFrequency = new LetterFrequency();

            var text = await File.ReadAllTextAsync(request.Input);
            foreach (var letter in text)
            {
                var upperLetter = char.ToUpper(letter);
                letterFrequency.AddLetter(upperLetter.ToString());
            }

            letterFrequency.UpdateLetterFrequencyDetails();

            // Now we need to compare the letter frequency from the Liber text with the letter frequency from the text
            foreach (var letterFrequencyDetail in letterFrequency.LetterFrequencyDetails)
            {
                if (liberFrequency.LetterFrequencyDetails.Any(x => x.Letter == letterFrequencyDetail.Letter))
                {
                    substitutionPossibilities.Add(GetSubstitutionPossibility(letterFrequencyDetail.Letter,
                        letterFrequencyDetail.Frequency, liberFrequency));
                }
            }

            if (request.IsPermuteCombinations)
            {
                var possArray = substitutionPossibilities.ToArray();
                // Perform string replacements for each permutation
                foreach (var permutation in GeneratePermutations(possArray))
                {
                    var sb = new StringBuilder();

                    foreach (var xchar in text)
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
                            fileText.AppendLine(request.Input);
                            fileText.AppendLine($"Score {textScore.Score}");
                            fileText.AppendLine(textScore.PermutationString);
                            fileText.AppendLine(textScore.Text);
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
                    var replacement = substitutionPossibilities.FirstOrDefault(x => x.Letter == xchar.ToString().ToUpper());
                    if (replacement != null)
                    {
                        sb.Append(replacement.GetCurrentReplacementLetter());
                    }
                    else
                    {
                        sb.Append(xchar);
                    }
                }

                await File.AppendAllTextAsync(request.Output, request.Input + Environment.NewLine);
                await File.AppendAllTextAsync(request.Output, sb.ToString() + Environment.NewLine);
                await File.AppendAllTextAsync(request.Output, Environment.NewLine);
                await File.AppendAllTextAsync(request.Output, Environment.NewLine);
            }
        }
        
        private bool AddAndReorderScore(TextScore score, ref List<TextScore> textScores)
        {
            bool needToWriteNewFile = false;
            ulong[] topScores = textScores.Select(x => x.Score).ToArray();
            textScores.Add(score);
            textScores = textScores.OrderByDescending(x => x.Score).ToList();
            if (textScores.Count > 100)
            {
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
        
        private IEnumerable<List<SubstitutionPossibility>> GeneratePermutations(
            SubstitutionPossibility[] substitutionPossibilities, 
            SubstitutionPossibility[]? currentArray = null, 
            int currentListIndex = 0)
        {
            // Setting the current array up.
            var currentList = new List<SubstitutionPossibility>();
            if (currentArray != null)
            {
                currentList.AddRange(currentArray);
            }

            while (substitutionPossibilities[currentListIndex].HasNextSubstitution())
            {
                var addValue = substitutionPossibilities[currentListIndex].GetNext();
                if (addValue != null) currentList.Add(addValue);

                if (currentList.Count >= substitutionPossibilities.Length)
                {
                    yield return currentList;
                    currentList = new List<SubstitutionPossibility>();
                    if (currentArray != null) currentList.AddRange(currentArray);
                }
                else
                {
                    foreach (var permutation in GeneratePermutations(substitutionPossibilities, currentList.ToArray(), currentListIndex + 1))
                    {
                        yield return permutation;
                    }
                }
            }

            substitutionPossibilities[currentListIndex].Reset();
        }
        
    }
}