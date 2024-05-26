namespace LiberPrimusAnalysisTool.Utility.Message;

/// <summary>
/// The event arguments for when a message is sent.
/// </summary>
public class MessageSentEventArgs
{
    /// <summary>
    /// The event arguments for when a message is sent.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="screen"></param>
    public MessageSentEventArgs(string message, string screen)
    {
        Message = message;
        Screen = screen;
    }
    
    /// <summary>
    /// The message that was sent.
    /// </summary>
    public string Message { get; private set; }
    
    /// <summary>
    /// The screen that the message was sent from.
    /// </summary>
    public string Screen { get; private set; }
}