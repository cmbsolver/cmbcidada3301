namespace LiberPrimusAnalysisTool.Entity.Analysis;

public class LetterFrequency
{
    public LetterFrequency()
    {
        LetterFrequencyDetails = new List<LetterFrequencyDetail>();
    }

    public List<LetterFrequencyDetail> LetterFrequencyDetails { get; private set; }

    public void AddLetter(string letter)
    {
        if (LetterFrequencyDetails.Any(x => x.Letter == letter))
        {
            var letterFrequencyDetail = LetterFrequencyDetails.First(x => x.Letter == letter);
            letterFrequencyDetail.Increment();
        }
        else
        {
            var letterFrequencyDetail = new LetterFrequencyDetail(letter);
            letterFrequencyDetail.Increment();
            LetterFrequencyDetails.Add(letterFrequencyDetail);
        }
    }
    
    public void AddLetter(string letter, long count)
    {
        if (LetterFrequencyDetails.Any(x => x.Letter == letter))
        {
            var letterFrequencyDetail = LetterFrequencyDetails.First(x => x.Letter == letter);
            letterFrequencyDetail.AddOccurrences(count);
        }
        else
        {
            var letterFrequencyDetail = new LetterFrequencyDetail(letter);
            letterFrequencyDetail.AddOccurrences(count);
            LetterFrequencyDetails.Add(letterFrequencyDetail);
        }
    }
    
    public void UpdateLetterFrequencyDetails()
    {
        for (var i = 0; i < LetterFrequencyDetails.Count; i++)
        {
            LetterFrequencyDetails[i].UpdateFrequency(LetterFrequencyDetails.Sum(x => x.Occurrences));    
        }
    }
}