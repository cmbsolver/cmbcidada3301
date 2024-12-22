using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Analysis;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Analysis;

public class GetRuneForFrequencyFromLib
{
    public class Query() : IRequest<LetterFrequency>
    {
    }

    public class Handler : IRequestHandler<Query, LetterFrequency>
    {
        public async Task<LetterFrequency> Handle(Query request, CancellationToken cancellationToken)
        {
            var lettersToIncludeArray = new List<string>
            {
                "ᛝ", "ᛟ", "ᛇ", "ᛡ", "ᛠ", "ᚫ", "ᚦ", "ᚠ", "ᚢ", "ᚩ", "ᚱ", "ᚳ", "ᚷ", "ᚹ",
                "ᚻ", "ᚾ", "ᛁ", "ᛄ", "ᛈ", "ᛉ", "ᛋ", "ᛏ", "ᛒ", "ᛖ", "ᛗ", "ᛚ", "ᛞ", "ᚪ",
                "ᚣ"
            };

            var letterFrequency = new LetterFrequency();

            await using (var context = new LiberContext())
            {
                var letters = context.RuneTextDocumentCharacters
                    .Where(x => lettersToIncludeArray.Contains(x.Character))
                    .ToList();

                foreach (var letter in letters)
                {
                    letterFrequency.AddLetter(letter.Character, letter.Count);
                }
            }

            letterFrequency.UpdateLetterFrequencyDetails();

            return letterFrequency;
        }
    }
}