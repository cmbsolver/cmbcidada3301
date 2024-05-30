using System.Text;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class FixUpControlChars
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
        public Handler(ICharacterRepo characterRepo)
        {
        }
        
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (var rune in request.Text)
            {
                string character = null;

                switch (rune)
                {
                    case '␍':
                    case '␊':
                        character = Environment.NewLine;
                        break;
                    case '␁':
                    case '␗':
                        character = " ";
                        break;
                    case '␄':
                        character = ".";
                        break;
                    default:
                        character = null;
                        break;
                }
                
                if (character != null)
                {
                    sb.Append(character);
                }
                else
                {
                    sb.Append(rune);
                }
            }
            
            return Task.FromResult(sb.ToString());
        }
    }
}