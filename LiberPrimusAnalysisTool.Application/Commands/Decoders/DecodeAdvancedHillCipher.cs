using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Decoders;

public class DecodeAdvancedHillCipher
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

            // Calculate the inverse of the key matrix
            int[,] inverseKeyMatrix = InverseMatrix(keyMatrix, alphabetSize);

            for (int i = 0; i < text.Length; i += matrixSize)
            {
                int[] vector = new int[matrixSize];
                for (int j = 0; j < matrixSize; j++)
                {
                    vector[j] = Array.IndexOf(alphabet, text[i + j]);
                }

                int[] decodedVector = new int[matrixSize];
                for (int row = 0; row < matrixSize; row++)
                {
                    decodedVector[row] = 0;
                    for (int col = 0; col < matrixSize; col++)
                    {
                        decodedVector[row] += inverseKeyMatrix[row, col] * vector[col];
                    }

                    decodedVector[row] %= alphabetSize; // Modulo alphabet size
                    if (decodedVector[row] < 0) decodedVector[row] += alphabetSize; // Ensure positive values
                }

                for (int j = 0; j < matrixSize; j++)
                {
                    result.Append(alphabet[decodedVector[j]]);
                }
            }

            return result.ToString();
        }

        private int[,] InverseMatrix(int[,] matrix, int mod)
        {
            int n = matrix.GetLength(0);
            int[,] adjugate = new int[n, n];
            int determinant = Determinant(matrix, n);
            int determinantInverse = ModInverse(determinant, mod);

            if (determinantInverse == -1)
            {
                throw new ArgumentException("Matrix is not invertible.");
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    adjugate[i, j] = (Cofactor(matrix, i, j) * determinantInverse) % mod;
                    if (adjugate[i, j] < 0)
                    {
                        adjugate[i, j] += mod;
                    }
                }
            }

            return adjugate;
        }

        private int Determinant(int[,] matrix, int n)
        {
            if (n == 1)
            {
                return matrix[0, 0];
            }

            int determinant = 0;
            int sign = 1;

            for (int i = 0; i < n; i++)
            {
                int[,] subMatrix = GetSubMatrix(matrix, 0, i, n);
                determinant += sign * matrix[0, i] * Determinant(subMatrix, n - 1);
                sign = -sign;
            }

            return determinant;
        }

        private int[,] GetSubMatrix(int[,] matrix, int excludingRow, int excludingCol, int n)
        {
            int[,] subMatrix = new int[n - 1, n - 1];
            int r = -1;

            for (int i = 0; i < n; i++)
            {
                if (i == excludingRow)
                {
                    continue;
                }

                r++;
                int c = -1;

                for (int j = 0; j < n; j++)
                {
                    if (j == excludingCol)
                    {
                        continue;
                    }

                    subMatrix[r, ++c] = matrix[i, j];
                }
            }

            return subMatrix;
        }

        private int Cofactor(int[,] matrix, int row, int col)
        {
            int n = matrix.GetLength(0);
            int[,] subMatrix = GetSubMatrix(matrix, row, col, n);
            int sign = ((row + col) % 2 == 0) ? 1 : -1;
            return sign * Determinant(subMatrix, n - 1);
        }

        private int ModInverse(int a, int mod)
        {
            int m0 = mod, t, q;
            int x0 = 0, x1 = 1;

            if (mod == 1)
            {
                return 0;
            }

            while (a > 1)
            {
                q = a / mod;
                t = mod;
                mod = a % mod;
                a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0)
            {
                x1 += m0;
            }

            return x1;
        }
    }
}