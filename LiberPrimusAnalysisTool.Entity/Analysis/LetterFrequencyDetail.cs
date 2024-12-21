namespace LiberPrimusAnalysisTool.Entity.Analysis;

public class LetterFrequencyDetail
{
    public LetterFrequencyDetail(string letter)
    {
        Letter = letter;
        Occurrences = 0;
        Frequency = 0;
    }

    public string Letter { get; private set; }
    
    public long Occurrences { get; private set; }
    
    public double Frequency { get; private set; }
    
    public void Increment()
    {
        Occurrences++;
    }
    
    public void AddOccurrences(long occurrences)
    {
        Occurrences += occurrences;
    }
    
    public void UpdateFrequency(long totalLetters)
    {
        Frequency = (double)Occurrences / (double)totalLetters;
        Frequency = Frequency * 100;
    }
}