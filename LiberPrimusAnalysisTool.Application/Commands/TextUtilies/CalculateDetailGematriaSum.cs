using System.Text;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class CalculateDetailGematriaSum
{
    public class Command : IRequest<Tuple<long, long[], long[]>>
    {
        public Command(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, Tuple<long, long[], long[]>>
    {
        private readonly ICharacterRepo _characterRepo;
        public Handler(ICharacterRepo characterRepo)
        {
            _characterRepo = characterRepo;
        }
        
        public Task<Tuple<long, long[], long[]>> Handle(Command request, CancellationToken cancellationToken)
        {
            long sum = 0;
            List<long> numbers = new List<long>();
            List<long> wordSums = new List<long>();
            
            long currentWordSum = 0;
            foreach (var rune in request.Text)
            {
                if (_characterRepo.IsRune(rune.ToString(), false) || rune == '\'' || rune == '"')
                {
                    var characterValue = _characterRepo.GetValueFromRune(rune.ToString());
                    if (characterValue != null)
                    {
                        numbers.Add(characterValue);
                        currentWordSum += characterValue;
                        sum += characterValue;
                    }
                }
                else
                {
                    if (currentWordSum > 0)
                    {
                        wordSums.Add(currentWordSum);
                    }
                    currentWordSum = 0;
                }
            }

            if (currentWordSum > 0)
            {
                wordSums.Add(currentWordSum);
            }

            return Task.FromResult(new Tuple<long, long[], long[]>(sum, numbers.ToArray(), wordSums.ToArray()));
        }
    }
}