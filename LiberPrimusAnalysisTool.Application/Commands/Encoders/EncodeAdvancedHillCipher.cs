using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Encoders;

public class EncodeAdvancedHillCipher
{
    public class Command : IRequest<string>
    {
        public Command(string text, int[,] keyMatrix, char[] alphabet)
        {
            Text = text.ToUpper();
            KeyMatrix = keyMatrix;
            Alphabet = alphabet.Select(c => c).ToArray();
        }

        public string Text { get; set; }
        public int[,] KeyMatrix { get; set; }
        public char[] Alphabet { get; set; }
    }

    public class Handler : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            string text = request.Text;
            int[,] keyMatrix = request.KeyMatrix;
            char[] alphabet = request.Alphabet;
            int matrixSize = keyMatrix.GetLength(0);
            int alphabetSize = alphabet.Length;
            StringBuilder result = new();

            // Ensure text length is a multiple of matrix size
            if (text.Length % matrixSize != 0)
            {
                int paddingLength = matrixSize - (text.Length % matrixSize);
                text = text.PadRight(text.Length + paddingLength, alphabet[0]); // Padding with the first character of the alphabet
            }

            for (int i = 0; i < text.Length; i += matrixSize)
            {
                int[] vector = new int[matrixSize];
                for (int j = 0; j < matrixSize; j++)
                {
                    vector[j] = Array.IndexOf(alphabet, text[i + j]);
                }

                int[] encodedVector = new int[matrixSize];
                for (int row = 0; row < matrixSize; row++)
                {
                    encodedVector[row] = 0;
                    for (int col = 0; col < matrixSize; col++)
                    {
                        encodedVector[row] += keyMatrix[row, col] * vector[col];
                    }
                    encodedVector[row] %= alphabetSize; // Modulo alphabet size
                }

                for (int j = 0; j < matrixSize; j++)
                {
                    result.Append(alphabet[encodedVector[j]]);
                }
            }

            return result.ToString();
        }
    }
}