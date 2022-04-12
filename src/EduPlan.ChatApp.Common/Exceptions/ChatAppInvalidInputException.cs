namespace EduPlan.ChatApp.Common.Exceptions;

public class ChatAppInvalidInputException : ChatAppException
{
    public ChatAppInvalidInputException(string message) : base(message)
    {
    }
}
