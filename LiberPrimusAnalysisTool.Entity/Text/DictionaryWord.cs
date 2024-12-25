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
}