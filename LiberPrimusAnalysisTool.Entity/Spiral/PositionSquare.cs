namespace LiberPrimusAnalysisTool.Entity.Spiral;

public class PositionSquare: ICloneable
{
    public PositionSquare(int x, int y, string squareValue)
    {
        X = x;
        Y = y;
        SquareValue = squareValue;
        HasBeenVisited = false;
    }

    public int X { get; private set; }
    
    public int Y { get; private set; }
    
    public bool HasBeenVisited { get; private set; }
    
    public string SquareValue { get; private set; }
    
    public void Visit()
    {
        while (!HasBeenVisited)
        {
            HasBeenVisited = true;
        }
    }

    public object Clone()
    {
        return new PositionSquare(X, Y, SquareValue);
    }
}