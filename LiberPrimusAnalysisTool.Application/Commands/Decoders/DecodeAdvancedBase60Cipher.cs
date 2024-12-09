using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Decoders;

public class DecodeAdvancedBase60Cipher
{
    public class Command : IRequest<string>
    {
        public Command(string text, char[] alphabet, int chunkSize)
        {
            Text = text;
            Alphabet = alphabet;
            ChunkSize = chunkSize;
        }

        public string Text { get; set; }
        public char[] Alphabet { get; set; }
        public int ChunkSize { get; set; }
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
            var alphabetIndex = alphabet.Select((c, i) => new { c, i }).ToDictionary(x => x.c, x => x.i);

            foreach (char c in text)
            {
                if (alphabetIndex.ContainsKey(c))
                {
                    currentBaseValue.Append(c);
                    if (currentBaseValue.Length >= request.ChunkSize)
                    {
                        result.Append(DecodeBase60Value(currentBaseValue.ToString(), alphabetIndex, baseSize));
                        currentBaseValue.Clear();
                    }
                }
                else
                {
                    result.Append(c);
                    
                    if (currentBaseValue.Length > 0)
                    {
                        result.Append(DecodeBase60Value(currentBaseValue.ToString(), alphabetIndex, baseSize));
                    }
                    
                    currentBaseValue.Clear();
                }
            }

            if (currentBaseValue.Length > 0)
            {
                result.Append(DecodeBase60Value(currentBaseValue.ToString(), alphabetIndex, baseSize));
            }

            return result.ToString();
        }

        private char DecodeBase60Value(string base60Value, Dictionary<char, int> alphabetIndex, int baseSize)
        {
            int value = 0;

            foreach (char c in base60Value)
            {
                value = value * baseSize + alphabetIndex[c];
            }

            return (char)value;
        }
    }
}