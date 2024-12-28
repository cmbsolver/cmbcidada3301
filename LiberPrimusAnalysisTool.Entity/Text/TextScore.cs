namespace LiberPrimusAnalysisTool.Entity.Text;

public class TextScore
{
    public TextScore(string originalText, string text, ulong score, string permutationString, double iocScore)
    {
        Text = text;
        Score = score;
        PermutationString = permutationString;
        IocScore = iocScore;
    }

    public string Text { get; set; }
    
    public ulong Score { get; set; }
    
    public string PermutationString { get; set; }
    
    public double IocScore { get; set; }
}