using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.Exceptions
{
    public class UndecidedParameterException : ArgumentNullException, ICustomException
    {
        public UndecidedParameterException(string paramName) : base(paramName)
        {
            _errorMessage = $"{paramName}(и) не выбрана(ы).";
        }

        private readonly string _errorMessage;

        public string ErrorMessage => _errorMessage;

        public ExceptionsLevels Level => ExceptionsLevels.LEVEL_WARN;
    }
}
