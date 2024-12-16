namespace LiberPrimusAnalysisTool.Entity.Image;

public class PixelBlock
{
    public PixelBlock()
    {
        Pixels = new List<Pixel>();
    }
    
    public List<Pixel> Pixels { get; set; }
    
    public int Width { get; set; }
    
    public int Height { get; set; }
}