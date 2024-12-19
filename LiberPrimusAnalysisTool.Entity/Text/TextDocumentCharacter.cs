using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiberPrimusAnalysisTool.Entity.Text;

public class TextDocumentCharacter
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Required]
    [Column("FILE_ID")]
    public long TextDocumentId { get; set; }
    
    [Required]
    [Column("CHARACTER")]
    public string Character { get; set; }
    
    [Required]
    [Column("COUNT")]
    public long Count { get; set; }
}