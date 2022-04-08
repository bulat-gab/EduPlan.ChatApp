namespace EduPlan.ChatApp.Api.Exceptions;

public class ChatAppInvalidInputException : ChatAppException
{
    public ChatAppInvalidInputException(string message) : base(message)
    {
    }
}
