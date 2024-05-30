using System.Text;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class TransposeRuneToLatin
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
        private readonly ICharacterRepo _characterRepo;
        public Handler(ICharacterRepo characterRepo)
        {
            _characterRepo = characterRepo;
        }
        
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (var rune in request.Text)
            {
                var character = _characterRepo.GetCharFromRune(rune.ToString());
                if (character != null && character.Length > 0)
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