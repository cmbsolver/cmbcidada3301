namespace LiberPrimusAnalysisTool.Utility.Message;

/// <summary>
/// Message that is sent.
/// </summary>
public class MessageBus : IMessageBus
{
    /// <summary>
    /// Delegate for the message event.
    /// </summary>
    public delegate void MessageEventHandler(object sender, MessageSentEventArgs e);
    
    /// <summary>
    /// Event that is triggered when a message is sent.
    /// </summary>
    public event IMessageBus.MessageEventHandler? MessageEvent;
    
    /// <summary>
    /// Sends a message to the specified screen.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="screen"></param>
    public void SendMessage(string message, string screen)
    {
        MessageEvent?.Invoke(this, new MessageSentEventArgs(message, screen));
    }
}