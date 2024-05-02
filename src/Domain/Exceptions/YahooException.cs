namespace Domain.Exceptions;
public class YahooException : Exception
{
    public YahooException(string message) : base(message) { }
    public YahooException() : base() { }
}
