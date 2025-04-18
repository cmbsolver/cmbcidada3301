using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Decoders;

public class DecodeCaesarCipher
{
    public class Command : IRequest<string>
    {
        public Command(string alphabet, string text, int key)
        {
            Alphabet = alphabet.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim().ToUpper()).ToArray();
            Text = text.ToUpper();
            Key = new int[1] { key };
        }
        
        public Command(string alphabet, string text, string key)
        {
            Alphabet = alphabet.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim().ToUpper()).ToArray();
            Text = text.ToUpper();
            Key = key.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x.Trim())).ToArray();
        }
        
        public string[] Alphabet { get; set; }
        public string Text { get; set; }
        public int[] Key { get; set; }
    }
    
    public class Handler: IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            StringBuilder result = new();
            string[] alphabet = request.Alphabet;
            string text = request.Text;
            int[] key = request.Key;

            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    char currentChar = text[i];
                    int alphabetIndex = Array.IndexOf(alphabet, currentChar.ToString());

                    if (alphabetIndex != -1)
                    {
                        int shift = key.Length == 1 ? key[0] : key[i];
                        shift = shift % alphabet.Length; // Ensure shift is within bounds
                        int newIndex = (alphabetIndex - shift + alphabet.Length) % alphabet.Length;
                        result.Append(alphabet[newIndex]);
                    }
                    else
                    {
                        result.Append(currentChar); // Append the character as is if it's not in the alphabet
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return result.ToString();
        }
    }
}