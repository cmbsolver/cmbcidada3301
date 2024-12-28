using System.Text;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Chains;

public class HexDecodeRunes
{
    public class Command: IRequest<string>
    {
        public Command(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, string>
    {
        private readonly ICharacterRepo _characterRepo;
        
        public Handler(ICharacterRepo characterRepo)
        {
            _characterRepo = characterRepo;
        }
        
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var fileName = request.FileName;
            var runes = File.ReadAllText(fileName);
            var decodedRunes = new StringBuilder();
            var decodedText = new StringBuilder();

            for (var i = 0; i < runes.Length; i++)
            {
                if (_characterRepo.IsRune(runes[i].ToString(), true))
                {
                    var rune = _characterRepo.GetHexValFromRune(runes[i].ToString());
                    decodedRunes.Append(rune);
                }
                else
                {
                    if (decodedRunes.Length > 0)
                    {
                        try
                        {
                            var bytes = Convert.FromBase64String(decodedRunes.ToString());
                            decodedText.Append(Encoding.ASCII.GetString(bytes));
                            decodedRunes.Clear();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }

            return decodedText.ToString();
        }
    }
    
}