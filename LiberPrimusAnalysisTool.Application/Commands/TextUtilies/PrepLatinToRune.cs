using System.Text;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class PrepLatinToRune
{
    public class Command : IRequest<string>
    {
        public Command(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, string>
    {
        public Handler()
        {
        }
        
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            request.Text = request.Text.ToUpper();
            
            while (request.Text.Contains("QU"))
            {
                request.Text = request.Text.Replace(
                    "QU", 
                    "CW");
            }
            
            while (request.Text.Contains("Z"))
            {
                request.Text = request.Text.Replace(
                    "Z", 
                    "S");
            }
            
            while (request.Text.Contains("K"))
            {
                request.Text = request.Text.Replace(
                    "K", 
                    "C");
            }
            
            while (request.Text.Contains("Q"))
            {
                request.Text = request.Text.Replace(
                    "Q", 
                    "C");
            }
            
            while (request.Text.Contains("V"))
            {
                request.Text = request.Text.Replace(
                    "V", 
                    "U");
            }
            
            return Task.FromResult(request.Text);
        }
    }
}