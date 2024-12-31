using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Decoders;

public class DecodeVigenereCipher
{
    public class Command: IRequest<string>
    {
        public Command(string[] alphabet, string key, string text)
        {
            Alphabet = alphabet;
            Key = key;
            Text = text;
        }

        public string[] Alphabet { get; set; }

        public string Key { get; set; }

        public string Text { get; set; }
    }

    public class Handler: IRequestHandler<Command, string>
    {
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var alphabet = request.Alphabet;
            var key = request.Key;
            var text = request.Text;

            var keyIndex = 0;
            var decodedText = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (char.IsLetter(c))
                {
                    var offset = char.IsUpper(c) ? 'A' : 'a';
                    var keyChar = key[keyIndex];
                    var keyCharOffset = char.IsUpper(keyChar) ? 'A' : 'a';
                    var textIndex = Array.IndexOf(alphabet, c.ToString().ToUpper());
                    var keyIndexInAlphabet = Array.IndexOf(alphabet, keyChar.ToString().ToUpper());
                    var decodedCharIndex = (textIndex - keyIndexInAlphabet + alphabet.Length) % alphabet.Length;
                    var decodedChar = alphabet[decodedCharIndex];
                    decodedText.Append(char.IsUpper(c) ? decodedChar : decodedChar.ToLower());
                    keyIndex = (keyIndex + 1) % key.Length;
                }
                else
                {
                    decodedText.Append(c);
                }
            }

            return Task.FromResult(decodedText.ToString());
        }
    }
}