namespace Messages;

public class TextMessage
{
    public string Text { get; set; }

    public TextMessage(string text)
    {
        Text = text;
    }
}