using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiberPrimusAnalysisTool.Entity.Text;

public class DictionaryWord
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Required]
    [Column("DICT_WORD")]
    public string DictionaryWordText { get; set; }
    
    [Required]
    [Column("DICT_RUNEGLISH")]
    public string RuneglishWordText { get; set; }
    
    [Required]
    [Column("DICT_RUNE")]
    public string RuneWordText { get; set; }
    
    [Required]
    [Column("GEM_SUM")]
    public long GemSum { get; set; }

    [Required]
    [Column("DICT_WORD_LENGTH")]
    public int DictionaryWordLength { get; set; }
    
    [Required]
    [Column("DICT_RUNEGLISH_LENGTH")]
    public int RuneglishWordLength { get; set; }
    
    [Required]
    [Column("DICT_RUNE_LENGTH")]
    public int RuneWordLength { get; set; }
    
    public string GetRunePattern()
    {
        Dictionary<int, string> patternDictionary = new();
        List<string> runes = new();
        var counter = 1;
        
        foreach (var character in RuneWordText)
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
        return $"{DictionaryWordText} - {RuneglishWordText} - {RuneWordText} - {GemSum}";
    }
}