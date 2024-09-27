using System.Text;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class CalculateGematriaSum
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
            long sum = 0;
            
            foreach (var rune in request.Text)
            {
                var character = _characterRepo.GetValueFromRune(rune.ToString());
                if (character != null)
                {
                    sum += character;
                }
            }
            
            return Task.FromResult(sum.ToString());
        }
    }
}