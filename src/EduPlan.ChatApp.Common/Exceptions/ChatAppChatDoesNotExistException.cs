namespace EduPlan.ChatApp.Common.Exceptions;

public class ChatAppChatDoesNotExistException : ChatAppException
{
    public ChatAppChatDoesNotExistException(string message) : base(message)
    {
    }
}
