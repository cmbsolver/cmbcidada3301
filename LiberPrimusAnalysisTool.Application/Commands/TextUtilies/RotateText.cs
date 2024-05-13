using System.Text;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies;

/// <summary>
/// Rotation direction for the text
/// </summary>
public enum RotationDirection
{
    Clockwise,
    CounterClockwise
}

/// <summary>
/// Padding direction for the text
/// </summary>
public enum PaddingDirection
{
    Left,
    Right,
    Center
}

/// <summary>
/// Text Rotate Command
/// </summary>
public class RotateText
{
    /// <summary>
    /// The command
    /// </summary>
    public class Command: IRequest<List<string>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text"></param>
        /// <param name="rotation"></param>
        /// <param name="padding"></param>
        public Command(List<string> text, RotationDirection rotation, PaddingDirection padding)
        {
            Text = text;
            Rotation = rotation;
            Padding = padding;
        }
        
        /// <summary>
        /// Text to rotate
        /// </summary>
        public List<string> Text { get; set; }
        
        /// <summary>
        /// Rotation direction
        /// </summary>
        public RotationDirection Rotation { get; set; }
        
        /// <summary>
        /// padding direction
        /// </summary>
        public PaddingDirection Padding { get; set; }
    }
    
    public async Task<List<string>> Handle(Command request, CancellationToken cancellationToken)
    {
        var rotatedText = new List<string>();
        var maxLength = request.Text.Max(x => x.Length);
        var padding = maxLength - 1;
        var paddingChar = ' ';
        
        // Get the max line length
        var maxLineLength = request.Text.Max(x => x.Length);
        

        return rotatedText;
    }
}