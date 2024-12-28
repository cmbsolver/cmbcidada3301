using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Encoders
{
    public class EncodeAffineCipher
    {
        public class Command : IRequest<string>
        {
            public Command(string text, int multiplier, int shift, string alphabet)
            {
                Text = text;
                Multiplier = multiplier;
                Shift = shift;
                Alphabet = alphabet.ToLower().Split(",", StringSplitOptions.RemoveEmptyEntries);
            }

            public string Text { get; set; }
            public int Multiplier { get; set; }
            public int Shift { get; set; }
            public string[] Alphabet { get; set; }
        }

        public class Handler : IRequestHandler<Command, string>
        {
            public Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var text = request.Text;
                var a = request.Multiplier;
                var b = request.Shift;
                var alphabet = request.Alphabet;
                var m = alphabet.Length;

                var result = new StringBuilder();
                foreach (var c in text)
                {
                    if (char.IsLetter(c))
                    {
                        var index = Array.IndexOf(alphabet, char.ToLower(c).ToString());
                        var encodedIndex = (a * index + b) % m;
                        var encodedChar = alphabet[encodedIndex][0];
                        result.Append(char.IsUpper(c) ? char.ToUpper(encodedChar) : encodedChar);
                    }
                    else
                    {
                        result.Append(c);
                    }
                }

                return Task.FromResult(result.ToString());
            }
        }
    }
}