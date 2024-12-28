using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Decoders
{
    public class DecodeAffineCipher
    {
        public class Command : IRequest<string>
        {
            public Command(string text, int? multiplier, int? shift, string alphabet)
            {
                Text = text;
                Multiplier = multiplier;
                Shift = shift;
                Alphabet = alphabet.ToLower().Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            }

            public string Text { get; set; }
            public int? Multiplier { get; set; }
            public int? Shift { get; set; }
            public string[] Alphabet { get; set; }
        }

        public class Handler : IRequestHandler<Command, string>
        {
            public Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var text = request.Text;
                var alphabet = request.Alphabet;
                var m = alphabet.Length;

                if (request.Multiplier.HasValue && request.Shift.HasValue)
                {
                    return Task.FromResult(Decode(text, request.Multiplier.Value, request.Shift.Value, alphabet, m));
                }
                else
                {
                    for (int a = 1; a < m; a++)
                    {
                        if (Gcd(a, m) != 1) continue; // 'a' must be coprime with 'm'
                        for (int b = 0; b < m; b++)
                        {
                            var decodedText = Decode(text, a, b, alphabet, m);
                            // Here you would typically check if 'decodedText' makes sense
                            // For simplicity, we return the first valid result
                            return Task.FromResult(decodedText);
                        }
                    }
                }

                return Task.FromResult(string.Empty);
            }

            private string Decode(string text, int a, int b, string[] alphabet, int m)
            {
                var result = new StringBuilder();
                foreach (var c in text)
                {
                    if (char.IsLetter(c))
                    {
                        var index = Array.IndexOf(alphabet, char.ToLower(c).ToString());
                        var decodedIndex = ModInverse(a, m) * (index - b + m) % m;
                        var decodedChar = alphabet[decodedIndex][0];
                        result.Append(char.IsUpper(c) ? char.ToUpper(decodedChar) : decodedChar);
                    }
                    else
                    {
                        result.Append(c);
                    }
                }
                return result.ToString();
            }

            private int Gcd(int a, int b)
            {
                while (b != 0)
                {
                    int temp = b;
                    b = a % b;
                    a = temp;
                }
                return a;
            }

            private int ModInverse(int a, int m)
            {
                for (int x = 1; x < m; x++)
                {
                    if ((a * x) % m == 1)
                        return x;
                }
                return 1; // Should not happen if 'a' is coprime with 'm'
            }
        }
    }
}