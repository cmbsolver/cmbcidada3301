using System.Text;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

public class TransposeLatinToRune
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

            string[] multiStringArray = new string[7] { "ING", "OE", "EO", "IO", "EA", "AE", "TH" };

            foreach (var multiString in multiStringArray)
            {
                while (request.Text.Contains(multiString))
                {
                    request.Text = request.Text.Replace(
                        multiString, 
                        _characterRepo.GetRuneFromChar(multiString.ToString().ToUpper()));
                }
            }
            
            foreach (var latin in request.Text)
            {
                if (_characterRepo.IsRune(latin.ToString()))
                {
                    sb.Append(latin);
                }
                else
                {
                    var character = _characterRepo.GetRuneFromChar(latin.ToString().ToUpper());
                    if (character != null && character.Length > 0)
                    {
                        sb.Append(character);
                    }
                    else
                    {
                        sb.Append(latin);
                    }
                }
            }
            
            return Task.FromResult(sb.ToString());
        }
    }
}