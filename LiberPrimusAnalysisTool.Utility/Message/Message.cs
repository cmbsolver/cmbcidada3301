namespace LiberPrimusAnalysisTool.Utility.Message;

public class Message
{
    public Message(string messageText, string screen)
    {
        MessageText = messageText;
        Screen = screen;
    }

    public string MessageText { get; set; }
    
    public string Screen { get; set; }
}