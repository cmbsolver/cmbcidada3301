using System.Text;
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
            
            StringBuilder sb = new StringBuilder(); 

            for (int i = 0; i < request.Text.Length; i++)
            {
                var xchar = request.Text[i];

                switch (xchar)
                {
                    case 'I':
                        if (((i + 1) < (request.Text.Length)) && request.Text[i + 1] == 'O')
                        {
                            sb.Append("IO");
                            i++;
                        }
                        else if (((i + 1) < (request.Text.Length)) && request.Text[i + 1] == 'A')
                        {
                            sb.Append("IO");
                            i++;
                        }
                        else
                        {
                            sb.Append("I");
                        }

                        break;

                    default:
                        sb.Append(xchar.ToString());
                        break;
                }
            }

            return Task.FromResult(sb.ToString());
        }
    }
}