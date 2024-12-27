namespace LiberPrimusAnalysisTool.Entity.Text;

public class TextScore
{
    public TextScore(string text, ulong score, string permutationString)
    {
        Text = text;
        Score = score;
        PermutationString = permutationString;
    }

    public string Text { get; set; }
    
    public ulong Score { get; set; }
    
    public string PermutationString { get; set; }
}