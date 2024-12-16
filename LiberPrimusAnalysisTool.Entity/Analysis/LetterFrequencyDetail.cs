namespace LiberPrimusAnalysisTool.Entity.Analysis;

public class LetterFrequencyDetail
{
    public LetterFrequencyDetail(string letter)
    {
        Letter = letter;
    }

    public string Letter { get; private set; }
    
    public int Occurrences { get; private set; }
    
    public double Frequency { get; private set; }
    
    public void Increment()
    {
        Occurrences++;
    }
    
    public void UpdateFrequency(int totalLetters)
    {
        Frequency = (double)Occurrences / (double)totalLetters;
        Frequency = Frequency * 100;
    }
}