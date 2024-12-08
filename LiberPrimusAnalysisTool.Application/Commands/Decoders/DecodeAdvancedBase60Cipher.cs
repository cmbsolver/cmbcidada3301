using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Decoders;

public class DecodeAdvancedBase60Cipher
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
            StringBuilder currentBaseValue = new();

            foreach (char c in text)
            {
                if (Array.IndexOf(alphabet, c) != -1)
                {
                    currentBaseValue.Append(c);
                }
                else
                {
                    if (currentBaseValue.Length > 0)
                    {
                        result.Append(DecodeBase60Value(currentBaseValue.ToString(), alphabet));
                        currentBaseValue.Clear();
                    }
                    result.Append(c);
                }
            }

            if (currentBaseValue.Length > 0)
            {
                result.Append(DecodeBase60Value(currentBaseValue.ToString(), alphabet));
            }

            return result.ToString();
        }

        private char DecodeBase60Value(string base60Value, char[] alphabet)
        {
            int baseSize = alphabet.Length;
            int value = 0;

            foreach (char c in base60Value)
            {
                value = value * baseSize + Array.IndexOf(alphabet, c);
            }

            return (char)value;
        }
    }
}