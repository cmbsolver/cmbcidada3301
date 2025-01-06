namespace LiberPrimusAnalysisTool.Entity.Text;

public class RuneDetailMapping
{
    public RuneDetailMapping(RuneDetail fromRune, RuneDetail toRune)
    {
        FromRune = fromRune;
        ToRune = toRune;
    }

    public RuneDetail FromRune { get; set; }
    
    public RuneDetail ToRune { get; set; }
    
    public override string ToString()
    {
        return $"{FromRune} -> {ToRune}";
    }
}