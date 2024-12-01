namespace LiberPrimusUi.Models;

public class CreditItemTemplate
{
    public CreditItemTemplate(string label, string link)
    {
        Label = $"{label} - {link}";
        Link = link;
    }
    
    public string Label { get; }
    public string Link { get; }
}