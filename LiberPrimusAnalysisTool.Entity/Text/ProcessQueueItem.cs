using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiberPrimusAnalysisTool.Entity.Text;

public class ProcessQueueItem
{
    [Required]
    [Column("ID")]
    public Guid Id { get; set; }
    
    [Required]
    [Column("HOPPER_STRING")]
    public string HopperString { get; set; }
    
    public static ProcessQueueItem GenerateQueueItem(string hopperString)
    {
        return new ProcessQueueItem
        {
            Id = Guid.NewGuid(),
            HopperString = hopperString
        };
    }
}