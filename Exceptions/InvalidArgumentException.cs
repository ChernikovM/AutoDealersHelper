using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.Exceptions
{
    public class InvalidArgumentException : ArgumentException, ICustomException
    {
        public InvalidArgumentException(string argument) : base($"One of the arguments provided to a method is not valid. Argument: [{argument}]", argument)
        {
            _errorMessage = $"Кажется, что-то не так с введенными данными. \n\nПроверьте корректность значения '{argument}'";
        }

        public string ErrorMessage => _errorMessage;

        private string _errorMessage;

        public ExceptionsLevels Level => ExceptionsLevels.LEVEL_WARN;
    }
}
