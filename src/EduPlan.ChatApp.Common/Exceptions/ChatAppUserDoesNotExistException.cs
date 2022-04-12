namespace EduPlan.ChatApp.Common.Exceptions;

public class ChatAppUserDoesNotExistException : ChatAppException
{
    public ChatAppUserDoesNotExistException(string message) : base(message)
    {
    }
}
