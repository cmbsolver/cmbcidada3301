using LiberPrimusAnalysisTool.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class ScoreText
{
    public class Command: IRequest<Tuple<ulong, double>>  
    {
        public Command(string originalText, string text, List<string> wordList)
        {
            OriginalText = originalText;
            Text = text;
            WordList = wordList;
        }
        
        public string OriginalText { get; set; }

        public string Text { get; set; }
        
        public List<string> WordList { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, Tuple<ulong, double>>
    {
        public async Task<Tuple<ulong, double>> Handle(Command request, CancellationToken cancellationToken)
        {
            ulong count = 0;

            await Parallel.ForEachAsync(request.WordList, async (word, cancellationToken) =>
            {
                if (request.Text.Contains(word))
                {
                    count += (ulong)word.Length ^ 2;
                }
            });
            
            double ioc = CalculateIndexOfCoincidence(request.OriginalText, request.Text) * 100;
            
            return new Tuple<ulong, double>(count, ioc);
        }
        
        private double CalculateIndexOfCoincidence(string originalText, string text)
        {
            if (originalText.Length != text.Length)
            {
                throw new ArgumentException("Texts must be of the same length");
            }

            int matchCount = 0;
            int length = originalText.Length;

            for (int i = 0; i < length; i++)
            {
                if (originalText[i] == text[i])
                {
                    matchCount++;
                }
            }

            return (double)matchCount / length;
        }
        
        
    }
}