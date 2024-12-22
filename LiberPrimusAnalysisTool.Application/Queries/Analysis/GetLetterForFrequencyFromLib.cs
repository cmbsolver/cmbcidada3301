using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Analysis;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Analysis;

public class GetLetterForFrequencyFromLib
{
    public class Query: IRequest<LetterFrequency>
    {
        public Query(string mode = null)
        {
            Mode = mode;
        }

        public string Mode { get; set; }
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
            
            var runesToIncludeArray = new List<string>
            {
                "ᛝ", "ᛟ", "ᛇ", "ᛡ", "ᛠ", "ᚫ", "ᚦ", "ᚠ", "ᚢ", "ᚩ", "ᚱ", "ᚳ", "ᚷ", "ᚹ",
                "ᚻ", "ᚾ", "ᛁ", "ᛄ", "ᛈ", "ᛉ", "ᛋ", "ᛏ", "ᛒ", "ᛖ", "ᛗ", "ᛚ", "ᛞ", "ᚪ",
                "ᚣ"
            };
            
            var letterFrequency = new LetterFrequency();
            
            await using (var context = new LiberContext())
            {
                switch (request.Mode)
                {
                    case "runes":
                        var rletters = context.RuneTextDocumentCharacters
                            .Where(x => runesToIncludeArray.Contains(x.Character))
                            .ToList();
                    
                        foreach (var letter in rletters)
                        {
                            letterFrequency.AddLetter(letter.Character, letter.Count);
                        }
                        break;
                    case "letters":
                        var letters = context.TextDocumentCharacters
                            .Where(x => lettersToIncludeArray.Contains(x.Character))
                            .ToList();
                    
                        foreach (var letter in letters)
                        {
                            letterFrequency.AddLetter(letter.Character, letter.Count);
                        }
                        break;
                    case "intermediary":
                        var iletters = context.LiberTextDocumentCharacters
                            .Where(x => lettersToIncludeArray.Contains(x.Character))
                            .ToList();
                    
                        foreach (var letter in iletters)
                        {
                            letterFrequency.AddLetter(letter.Character, letter.Count);
                        }
                        break;
                    default:
                        var lletters = context.TextDocumentCharacters
                            .Where(x => lettersToIncludeArray.Contains(x.Character))
                            .ToList();
                    
                        foreach (var letter in lletters)
                        {
                            letterFrequency.AddLetter(letter.Character, letter.Count);
                        }
                        break;
                }
            }
            
            letterFrequency.UpdateLetterFrequencyDetails();
            
            return letterFrequency;
        }
    }
}