using System.Text;

namespace LiberPrimusAnalysisTool.Entity.Spiral;

public class MatrixSquare(int squareLength)
{
    private int SquareWidth { get; } = squareLength;

    private int SquareHeight { get; } = squareLength;

    private SpiralDirection SpiralDirection { get; set; } = SpiralDirection.Clockwise;

    private TunnelStyle TunnelStyle { get; set; } = TunnelStyle.TunnelIn;

    private StartPosition StartPosition { get; set; } = StartPosition.UpperLeft;

    private List<PositionSquare> PositionSquares { get; set; } = new();

    private int? _currentX;

    private int? _currentY;

    private NextDirection _nextDirection = NextDirection.Right;

    public void AddSquareValue(int x, int y, string squareValue)
    {
        var positionSquare = new PositionSquare(x, y, squareValue);
        PositionSquares.Add(positionSquare);
    }

    public void ChangeSpiralDirection(SpiralDirection spiralDirection)
    {
        SpiralDirection = spiralDirection;
    }

    public void ChangeTunnelStyle(TunnelStyle tunnelStyle)
    {
        TunnelStyle = tunnelStyle;
    }

    public void ChangeStartPosition(StartPosition startPosition)
    {
        StartPosition = startPosition;
    }

    public PositionSquare? Next()
    {
        if (TunnelStyle == TunnelStyle.TunnelIn)
        {
            if ((_currentX == null || _currentY == null))
            {
                // Initialize starting position
                switch (StartPosition)
                {
                    case StartPosition.UpperLeft:
                        _currentX = 0;
                        _currentY = 0;
                        _nextDirection =
                            SpiralDirection == SpiralDirection.Clockwise ? NextDirection.Right : NextDirection.Down;
                        break;
                    case StartPosition.UpperRight:
                        _currentX = SquareWidth - 1;
                        _currentY = 0;
                        _nextDirection =
                            SpiralDirection == SpiralDirection.Clockwise ? NextDirection.Down : NextDirection.Left;
                        break;
                    case StartPosition.LowerLeft:
                        _currentX = 0;
                        _currentY = SquareHeight - 1;
                        _nextDirection =
                            SpiralDirection == SpiralDirection.Clockwise ? NextDirection.Up : NextDirection.Right;
                        break;
                    case StartPosition.LowerRight:
                        _currentX = SquareWidth - 1;
                        _currentY = SquareHeight - 1;
                        _nextDirection =
                            SpiralDirection == SpiralDirection.Clockwise ? NextDirection.Right : NextDirection.Up;
                        break;
                    case StartPosition.Center:
                        StartPosition = StartPosition.Center;
                        _currentX = (int)Math.Ceiling((decimal)SquareWidth / 2);
                        _currentY = (int)Math.Ceiling((decimal)SquareWidth / 2);
                        _nextDirection = NextDirection.Right;
                        break;
                }
            }
        }
        else
        {
            if ((_currentX == null || _currentY == null))
            {
                StartPosition = StartPosition.Center;
                _currentX = (int)Math.Ceiling((decimal)SquareWidth / 2);
                _currentY = (int)Math.Ceiling((decimal)SquareWidth / 2);
                _nextDirection = NextDirection.Right;
            }
        }

        // Get the current position square
        PositionSquare? positionSquare = PositionSquares.FirstOrDefault(value => value.X == _currentX && value.Y == _currentY);
        positionSquare?.Visit();

        AssignNextPosition();

        return positionSquare;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();
        for (int y = 0; y < SquareHeight; y++)
        {
            for (int x = 0; x < SquareWidth; x++)
            {
                var positionSquare = PositionSquares.FirstOrDefault(value => value.X == x && value.Y == y);
                if (positionSquare != null)
                {
                    stringBuilder.Append(positionSquare.SquareValue);
                }
                else
                {
                    stringBuilder.Append('•');
                }
            }

            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }
    
    public int GetCount()
    {
        return this.PositionSquares.Count;
    }

    private void AssignNextPosition()
    {
        bool IsVisited(int? x, int? y) => PositionSquares.Any(ps => ps.X == x && ps.Y == y && ps.HasBeenVisited);
        bool IsThereSpace(int? x, int? y) => PositionSquares.Any(ps => ps.X == x && ps.Y == y);

        if (TunnelStyle == TunnelStyle.TunnelIn)
        {
            switch (_nextDirection)
            {
                case NextDirection.Right:
                    if (IsThereSpace(_currentX + 1, _currentY) && !IsVisited(_currentX + 1, _currentY))
                    {
                        _currentX++;
                    }
                    else
                    {
                        _nextDirection = SpiralDirection == SpiralDirection.Clockwise
                            ? NextDirection.Down
                            : NextDirection.Up;
                        _currentY = SpiralDirection == SpiralDirection.Clockwise ? _currentY + 1 : _currentY - 1;
                    }

                    break;
                case NextDirection.Left:
                    if (IsThereSpace(_currentX - 1, _currentY) && !IsVisited(_currentX - 1, _currentY))
                    {
                        _currentX--;
                    }
                    else
                    {
                        _nextDirection = SpiralDirection == SpiralDirection.Clockwise
                            ? NextDirection.Up
                            : NextDirection.Down;
                        _currentY = SpiralDirection == SpiralDirection.Clockwise ? _currentY - 1 : _currentY + 1;
                    }

                    break;
                case NextDirection.Down:
                    if (IsThereSpace(_currentX, _currentY + 1) && !IsVisited(_currentX, _currentY + 1))
                    {
                        _currentY++;
                    }
                    else
                    {
                        _nextDirection = SpiralDirection == SpiralDirection.Clockwise
                            ? NextDirection.Left
                            : NextDirection.Right;
                        _currentX = SpiralDirection == SpiralDirection.Clockwise ? _currentX - 1 : _currentX + 1;
                    }

                    break;
                case NextDirection.Up:
                    if (IsThereSpace(_currentX, _currentY - 1) && !IsVisited(_currentX, _currentY - 1))
                    {
                        _currentY--;
                    }
                    else
                    {
                        _nextDirection = SpiralDirection == SpiralDirection.Clockwise
                            ? NextDirection.Right
                            : NextDirection.Left;
                        _currentX = SpiralDirection == SpiralDirection.Clockwise ? _currentX + 1 : _currentX - 1;
                    }

                    break;
            }
        }
        else
        {
            switch (_nextDirection)
            {
                case NextDirection.Down:
                    if (SpiralDirection == SpiralDirection.Clockwise)
                    {
                        if (!IsVisited(_currentX - 1, _currentY))
                        {
                            _nextDirection = NextDirection.Left;
                            _currentX--;
                        }
                        else
                        {
                            _currentY++;
                        }
                    }
                    else
                    {
                        if (!IsVisited(_currentX + 1, _currentY))
                        {
                            _nextDirection = NextDirection.Right;
                            _currentX++;
                        }
                        else
                        {
                            _currentY++;
                        }
                    }
                    break;
                case NextDirection.Up:
                    if (SpiralDirection == SpiralDirection.Clockwise)
                    {
                        if (!IsVisited(_currentX + 1, _currentY))
                        {
                            _nextDirection = NextDirection.Right;
                            _currentX++;
                        }
                        else
                        {
                            _currentY--;
                        }
                    }
                    else
                    {
                        if (!IsVisited(_currentX - 1, _currentY))
                        {
                            _nextDirection = NextDirection.Left;
                            _currentX--;
                        }
                        else
                        {
                            _currentY--;
                        }
                    }
                    break;
                case NextDirection.Left:
                    if (SpiralDirection == SpiralDirection.Clockwise)
                    {
                        if (!IsVisited(_currentX, _currentY - 1))
                        {
                            _nextDirection = NextDirection.Up;
                            _currentY--;
                        }
                        else
                        {
                            _currentX--;
                        }
                    }
                    else
                    {
                        if (!IsVisited(_currentX, _currentY + 1))
                        {
                            _nextDirection = NextDirection.Down;
                            _currentY++;
                        }
                        else
                        {
                            _currentX--;
                        }
                    }
                    break;
                case NextDirection.Right:
                    if (SpiralDirection == SpiralDirection.Clockwise)
                    {
                        if (!IsVisited(_currentX, _currentY + 1))
                        {
                            _nextDirection = NextDirection.Down;
                            _currentY++;
                        }
                        else
                        {
                            _currentX++;
                        }
                    }
                    else
                    {
                        if (!IsVisited(_currentX, _currentY - 1))
                        {
                            _nextDirection = NextDirection.Up;
                            _currentY--;
                        }
                        else
                        {
                            _currentX++;
                        }
                    }
                    break;
            }
        }
    }
}