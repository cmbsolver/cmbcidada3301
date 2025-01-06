namespace LiberPrimusAnalysisTool.Entity.Text;

public class RuneDetail
{
    public RuneDetail(string rune, string latin, int value)
    {
        Rune = rune;
        Latin = latin;
        Value = value;
    }

    public string Rune { get; set; }
    
    public string Latin { get; set; }
    
    public int Value { get; set; }
    
    public override string ToString()
    {
        return $"{Rune} {Latin} {Value}";
    }
}