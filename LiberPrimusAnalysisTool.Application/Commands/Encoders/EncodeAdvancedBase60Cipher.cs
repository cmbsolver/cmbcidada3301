using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Encoders;

public class EncodeAdvancedBase60Cipher
{
    public class Command : IRequest<string>
    {
        public Command(string text, char[] alphabet)
        {
            Text = text;
            Alphabet = alphabet;
        }

        public string Text { get; set; }
        public char[] Alphabet { get; set; }
    }

    public class Handler : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            string text = request.Text;
            char[] alphabet = request.Alphabet;
            int baseSize = alphabet.Length;
            StringBuilder result = new();
            StringBuilder nonAlphabetChars = new();

            foreach (char c in text)
            {
                if (Array.IndexOf(alphabet, c) == -1)
                {
                    nonAlphabetChars.Append(c);
                    continue;
                }

                int value = c;
                StringBuilder baseValue = new();

                do
                {
                    baseValue.Insert(0, alphabet[value % baseSize]);
                    value /= baseSize;
                } while (value > 0);

                result.Append(baseValue);
            }

            return result.ToString();
        }
    }
}