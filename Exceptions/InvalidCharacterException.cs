using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.Exceptions
{
    public class InvalidCharacterException : ArgumentException, ICustomException
    {
        public InvalidCharacterException(char character) : base($"One of the character provided to a method is not valid. Character: [{character}]")
        {
            _errorMessage = $"В данном контексте нельзя использовать символ '{character}'.\n\nОбратите внимание на примеры сообщений.";
        }

        public string ErrorMessage => _errorMessage;

        private string _errorMessage;

        public ExceptionsLevels Level => ExceptionsLevels.LEVEL_WARN;
    }
}
