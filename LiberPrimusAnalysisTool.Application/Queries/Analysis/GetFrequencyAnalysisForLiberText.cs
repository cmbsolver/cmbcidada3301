using System.Text;
using LiberPrimusAnalysisTool.Entity.Analysis;
using MediatR;

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
                // Generate all permutations of substitution possibilities
                var permutations = GeneratePermutations(substitutionPossibilities.ToArray());

                // Perform string replacements for each permutation
                foreach (var permutation in permutations)
                {
                    var sb = new StringBuilder();

                    foreach (var xchar in text)
                    {
                        var replacement = permutation.FirstOrDefault(x => x.Letter == xchar.ToString().ToUpper());
                        if (replacement != null)
                        {
                            sb.Append(replacement.GetCurrent());
                        }
                        else
                        {
                            sb.Append(xchar);
                        }
                    }

                    await File.AppendAllTextAsync(request.Output, request.Input  + Environment.NewLine, cancellationToken);
                    await File.AppendAllTextAsync(request.Output, sb.ToString() + Environment.NewLine, cancellationToken);
                    await File.AppendAllTextAsync(request.Output, Environment.NewLine, cancellationToken);
                    await File.AppendAllTextAsync(request.Output, Environment.NewLine, cancellationToken);
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
                        sb.Append(replacement.GetCurrent());
                    }
                    else
                    {
                        sb.Append(xchar);
                    }
                }

                await File.AppendAllTextAsync(request.Output, request.Input + Environment.NewLine, cancellationToken);
                await File.AppendAllTextAsync(request.Output, sb.ToString() + Environment.NewLine, cancellationToken);
                await File.AppendAllTextAsync(request.Output, Environment.NewLine, cancellationToken);
                await File.AppendAllTextAsync(request.Output, Environment.NewLine, cancellationToken);
            }
        }

        public SubstitutionPossibility GetSubstitutionPossibility(string letter, double frquency, LetterFrequency liberFrequency)
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
        
        private List<List<SubstitutionPossibility>> GeneratePermutations(
            SubstitutionPossibility[] substitutionPossibilities, 
            SubstitutionPossibility[]? currentArray = null, 
            List<List<SubstitutionPossibility>>? result = null,
            int currentListIndex = 0)
        {
            // Setting the result if null.
            if (result == null)
            {
                result = new List<List<SubstitutionPossibility>>();
            }
            
            // Setting the current array up.
            var currentList = new List<SubstitutionPossibility>();
            if (currentArray != null)
            {
                currentList.AddRange(currentArray);
            }

            while(!substitutionPossibilities[currentListIndex].HasNextSubstitution())
            {
                var addValue = substitutionPossibilities[currentListIndex].GetNext();
                if (addValue != null) currentList.Add(addValue);

                if (currentList.Count >= substitutionPossibilities.Length)
                {
                    result.Add(currentList);
                    currentList = new List<SubstitutionPossibility>();
                    if (currentArray != null) currentList.AddRange(currentArray);
                }
                else
                {
                    GeneratePermutations(substitutionPossibilities, currentList.ToArray(), result, currentListIndex + 1);
                }
            }
            
            substitutionPossibilities[currentListIndex].Reset();

            return result;
        }
        
    }
}