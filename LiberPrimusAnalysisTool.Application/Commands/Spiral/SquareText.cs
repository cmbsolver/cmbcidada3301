using LiberPrimusAnalysisTool.Entity.Spiral;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Spiral;

public class SquareText
{
    public class Command : IRequest<MatrixSquare>
    {
        public Command(string text, TunnelStyle tunnelStyle, SpaceShift spaceShift)
        {
            Text = text;
            TunnelStyle = tunnelStyle;
            SpaceShift = spaceShift;
        }

        public string Text { get; private set; }
        
        public TunnelStyle TunnelStyle { get; private set; }
        
        public SpaceShift SpaceShift { get; private set; }
    }
    
    public class Handler : IRequestHandler<Command, MatrixSquare>
    {
        public Task<MatrixSquare> Handle(Command request, CancellationToken cancellationToken)
        {
            var text = request.Text;
            var squareSize = (int)System.Math.Ceiling(System.Math.Sqrt(text.Length));
            
            if (request.TunnelStyle == TunnelStyle.TunnelOut && squareSize % 2 == 0)
            {
                squareSize++;
            }
            
            var square = new char[squareSize, squareSize];
            var index = 0;

            for (var y = 0; y < squareSize; y++)
            {
                for (var x = 0; x < squareSize; x++)
                {
                    if (index < text.Length)
                    {
                        square[x, y] = text[index];
                        index++;
                    }
                    else
                    {
                        square[x, y] = '•';
                        index++;
                    }
                }
            }

            List<int> rowsWithAllSpaces = new();
            for (var y = 0; y < squareSize; y++)
            {
                bool rowHasAllSpaces = true;
                for (var x = 0; x < squareSize; x++)
                {
                    if (square[x, y] != '•')
                    {
                        rowHasAllSpaces = false;
                        break;
                    }
                }
                
                if (rowHasAllSpaces)
                {
                    rowsWithAllSpaces.Add(y);
                }
            }

            // If there are rows with all spaces, we need to figure out where to place them.
            if (rowsWithAllSpaces.Any())
            {
                var repositionSquare = new char[squareSize, squareSize];
                
                switch (request.SpaceShift)
                {
                    case SpaceShift.Bottom:
                        repositionSquare = square;
                        break;
                    case SpaceShift.Left:
                        // Fill in the left.
                        for (var y = 0; y < squareSize; y++)
                        {
                            for (var x = 0; x < rowsWithAllSpaces.Count; x++)
                            {
                                repositionSquare[x, y] = '•';
                            }
                        }
                        
                        index = 0;
                        
                        for (var y = 0; y < squareSize; y++)
                        {
                            for (var x = rowsWithAllSpaces.Count; x < squareSize; x++)
                            {
                                if (index < text.Length)
                                {
                                    repositionSquare[x, y] = text[index];
                                    index++;
                                }
                                else
                                {
                                    repositionSquare[x, y] = '•';
                                    index++;
                                }
                            }
                        }
                        break;
                    case SpaceShift.Right:
                        // Fill in the left.
                        for (var y = 0; y < squareSize; y++)
                        {
                            for (var x = squareSize - rowsWithAllSpaces.Count; x < squareSize; x++)
                            {
                                repositionSquare[x, y] = '•';
                            }
                        }
                        
                        index = 0;
                        
                        for (var y = 0; y < squareSize; y++)
                        {
                            for (var x = 0; x < squareSize - rowsWithAllSpaces.Count; x++)
                            {
                                if (index < text.Length)
                                {
                                    repositionSquare[x, y] = text[index];
                                    index++;
                                }
                                else
                                {
                                    repositionSquare[x, y] = '•';
                                    index++;
                                }
                            }
                        }
                        break;
                    case SpaceShift.Top:
                        // Fill in the top.
                        for (var y = 0; y < rowsWithAllSpaces.Count; y++)
                        {
                            for (var x = 0; x < squareSize; x++)
                            {
                                repositionSquare[x, y] = '•';
                            }
                        }
                        
                        index = 0;
                        
                        for (var y = rowsWithAllSpaces.Count; y < squareSize; y++)
                        {
                            for (var x = 0; x < squareSize; x++)
                            {
                                if (index < text.Length)
                                {
                                    repositionSquare[x, y] = text[index];
                                    index++;
                                }
                                else
                                {
                                    repositionSquare[x, y] = '•';
                                    index++;
                                }
                            }
                        }
                        break;
                }
                
                square = repositionSquare;
            }

            MatrixSquare matrixSquare = new(squareSize);
            
            for (var y = 0; y < squareSize; y++)
            {
                for (var x = 0; x < squareSize; x++)
                {
                    matrixSquare.AddSquareValue(x, y, square[x, y].ToString());
                }
            }
            
            return Task.FromResult(matrixSquare);
        }
    }
}