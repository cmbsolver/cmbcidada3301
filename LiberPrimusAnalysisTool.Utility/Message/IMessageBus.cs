namespace LiberPrimusAnalysisTool.Utility.Message;

/// <summary>
/// Message bus interface.
/// </summary>
public interface IMessageBus
{
    /// <summary>
    /// Event that is triggered when a message is sent.
    /// </summary>
    delegate void MessageEventHandler(object sender, MessageSentEventArgs e);
    
    /// <summary>
    /// Message that is sent.
    /// </summary>
    event MessageEventHandler MessageEvent;

    /// <summary>
    /// Sends a message to the specified screen.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="screen"></param>
    void SendMessage(string message, string screen);
}