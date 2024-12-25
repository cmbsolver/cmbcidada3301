using LiberPrimusAnalysisTool.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class ScoreText
{
    public class Command: IRequest<double>  
    {
        public Command(string text, List<string> wordList)
        {
            Text = text;
            WordList = wordList;
        }

        public string Text { get; set; }
        
        public List<string> WordList { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, double>
    {
        public async Task<double> Handle(Command request, CancellationToken cancellationToken)
        {
            var count = 0;

            foreach (var word in request.WordList)
            {
                if (request.Text.ToUpper().Contains(word) && word.Length > 2)
                {
                    count += word.Length;
                }
            }

            double score = ((double)count / (double)request.Text.Length) * 100;
            return score;
        }
    }
}