using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Analysis;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Analysis;

public class GetLetterForFrequencyFromLib
{
    public class Query(bool fromIntermediaryRune) : IRequest<LetterFrequency>
    {
        public bool FromIntermediaryRune { get; private set; } = fromIntermediaryRune;
    }
    
    public class Handler: IRequestHandler<Query, LetterFrequency>
    {
        public async Task<LetterFrequency> Handle(Query request, CancellationToken cancellationToken)
        {
            var lettersToIncludeArray = new List<string>
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
            };
            
            var letterFrequency = new LetterFrequency();
            
            await using (var context = new LiberContext())
            {
                if (request.FromIntermediaryRune)
                {
                    var letters = context.LiberTextDocumentCharacters
                        .Where(x => lettersToIncludeArray.Contains(x.Character))
                        .ToList();
                    
                    foreach (var letter in letters)
                    {
                        letterFrequency.AddLetter(letter.Character, letter.Count);
                    }
                }
                else
                {
                    var letters = context.TextDocumentCharacters
                        .Where(x => lettersToIncludeArray.Contains(x.Character))
                        .ToList();
                    
                    foreach (var letter in letters)
                    {
                        letterFrequency.AddLetter(letter.Character, letter.Count);
                    }
                }
            }
            
            letterFrequency.UpdateLetterFrequencyDetails();
            
            return letterFrequency;
        }
    }
}