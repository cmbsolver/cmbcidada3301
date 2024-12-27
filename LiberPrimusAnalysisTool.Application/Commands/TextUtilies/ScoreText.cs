using LiberPrimusAnalysisTool.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class ScoreText
{
    public class Command: IRequest<Tuple<ulong, double>>  
    {
        public Command(string text, List<string> wordList)
        {
            Text = text;
            WordList = wordList;
        }

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
            
            double ioc = CalculateIncidenceOfCoincidence(request.Text);
            
            return new Tuple<ulong, double>(count, ioc);
        }
        
        public double CalculateIncidenceOfCoincidence(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0.0;
            }

            var letterCounts = new Dictionary<char, int>();
            int totalLetters = 0;

            foreach (var c in text)
            {
                if (char.IsLetter(c))
                {
                    char upperChar = char.ToUpper(c);
                    if (letterCounts.ContainsKey(upperChar))
                    {
                        letterCounts[upperChar]++;
                    }
                    else
                    {
                        letterCounts[upperChar] = 1;
                    }
                    totalLetters++;
                }
            }

            double ic = 0.0;
            foreach (var count in letterCounts.Values)
            {
                ic += count * (count - 1);
            }

            if (totalLetters > 1)
            {
                ic /= totalLetters * (totalLetters - 1);
            }

            return ic;
        }
    }
}