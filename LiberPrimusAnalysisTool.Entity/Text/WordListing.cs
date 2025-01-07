namespace LiberPrimusAnalysisTool.Entity.Text;

public class WordListing
{
    public WordListing(string word, int position, string runeglish)
    {
        Word = word;
        Position = position;
        Runeglish = runeglish;
    }

    public string Word { get; set; }
    
    public string Runeglish { get; set; }
    
    public int Position { get; set; }

    public string GetRunePattern()
    {
        Dictionary<int, string> patternDictionary = new();
        List<string> runes = new();
        var counter = 1;
        
        foreach (var character in Word)
        {
            if (character == '\'')
            {
                runes.Add("'");
                continue;
            }
            
            if (patternDictionary.Any(x => x.Value == character.ToString()))
            {
                var pattern = patternDictionary.First(x => x.Value == character.ToString());
                runes.Add(pattern.Key.ToString());
            }
            else
            {
                runes.Add(counter.ToString());
                patternDictionary.Add(counter, character.ToString());
                counter++;
            }
        }
        
        return string.Join(string.Empty, runes);
    }
    
    public override string ToString()
    {
        return $"({Position}) {Word} - {Runeglish}";
    }
}