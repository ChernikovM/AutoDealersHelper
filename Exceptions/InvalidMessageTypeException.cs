using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace AutoDealersHelper.Exceptions
{
    class InvalidMessageTypeException : ArgumentException, ICustomException
    {
        public InvalidMessageTypeException(MessageType type) : base($"Invalid message type[{type}]. (Expected: Text)")
        {
        }

        public string ErrorMessage => null;

        public ExceptionsLevels Level => ExceptionsLevels.LEVEL_WARN;
    }
}
