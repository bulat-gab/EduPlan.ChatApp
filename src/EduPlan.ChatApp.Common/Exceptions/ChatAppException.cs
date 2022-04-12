namespace EduPlan.ChatApp.Common.Exceptions;

public class ChatAppException : Exception
{
    public ChatAppException(string message) : base(message)
    {
    }

    public ChatAppException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
