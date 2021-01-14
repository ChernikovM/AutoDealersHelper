using System;

namespace AutoDealersHelper.Exceptions
{
    public class InvalidChatStateException : ArgumentException, ICustomException
    {
        public InvalidChatStateException() : base("Value does not fall within the expected range.", "ChatStateId")
        {
        }

        public ExceptionsLevels Level => ExceptionsLevels.LEVEL_WARN;
        public string ErrorMessage => $"Ой, кажется мне срочно нужна перезагрузка.\n\nНажмите на команду /start и я смогу перезагрузиться.";
    }
}
