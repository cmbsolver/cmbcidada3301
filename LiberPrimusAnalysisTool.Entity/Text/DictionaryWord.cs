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

    public override string ToString()
    {
        return $"{DictionaryWordText} - {RuneglishWordText} - {RuneWordText} - {GemSum}";
    }
}