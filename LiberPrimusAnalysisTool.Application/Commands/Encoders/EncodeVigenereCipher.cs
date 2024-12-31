using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Encoders;

public class EncodeVigenereCipher
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
            var encodedText = new StringBuilder();
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
                    var encodedCharIndex = (textIndex + keyIndexInAlphabet) % alphabet.Length;
                    var encodedChar = alphabet[encodedCharIndex];
                    encodedText.Append(char.IsUpper(c) ? encodedChar : encodedChar.ToLower());
                    keyIndex = (keyIndex + 1) % key.Length;
                }
                else
                {
                    encodedText.Append(c);
                }
            }

            return Task.FromResult(encodedText.ToString());
        }
    }
}