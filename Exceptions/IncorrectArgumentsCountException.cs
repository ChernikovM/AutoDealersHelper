using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.Exceptions
{
    public class IncorrectArgumentsCountException : ArgumentException, ICustomException
    {
        public IncorrectArgumentsCountException(int expected, int fact) : base($"Incorrect number of arguments: expected: {expected}, fact: {fact}.")
        {
            _errorMes = $"Кажется, здесь {((fact > expected)?"больше":"меньше")} значений, чем нужно.";
        }

        private string _errorMes;
        public string ErrorMessage => _errorMes;

        public ExceptionsLevels Level => ExceptionsLevels.LEVEL_WARN;
    }
}
