using LiberPrimusAnalysisTool.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class ScoreText
{
    public class Command: IRequest<ulong>  
    {
        public Command(string text, List<string> wordList)
        {
            Text = text;
            WordList = wordList;
        }

        public string Text { get; set; }
        
        public List<string> WordList { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, ulong>
    {
        public async Task<ulong> Handle(Command request, CancellationToken cancellationToken)
        {
            ulong count = 0;

            await Parallel.ForEachAsync(request.WordList, async (word, cancellationToken) =>
            {
                if (request.Text.Contains(word))
                {
                    count += (ulong)word.Length ^ 2;
                }
            });
            
            return count;
        }
    }
}