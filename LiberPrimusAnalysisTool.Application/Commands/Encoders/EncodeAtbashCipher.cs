using System.Text;
using MediatR;

public class EncodeAtbashCipher
{
    public class Command : IRequest<string>
    {
        public Command(string text, string alphabet)
        {
            Text = text;
            Alphabet = alphabet.Split(",", StringSplitOptions.RemoveEmptyEntries);
        }

        public string Text { get; set; }

        public string[] Alphabet { get; set; }
    }

    public class Handler : IRequestHandler<Command, string>
    {
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var text = request.Text;
            var alphabet = request.Alphabet;

            var result = new StringBuilder();
            foreach (var c in text)
            {
                if (char.IsLetter(c))
                {
                    var index = Array.IndexOf(alphabet, c.ToString());
                    var reversedIndex = alphabet.Length - 1 - index;
                    var reversedChar = alphabet[reversedIndex];
                    result.Append(char.IsUpper(c) ? char.ToUpper(reversedChar[0]) : reversedChar[0]);
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