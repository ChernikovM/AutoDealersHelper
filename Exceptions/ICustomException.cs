using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.Exceptions
{
    public interface ICustomException
    {
        string ErrorMessage { get; }

        ExceptionsLevels Level { get; }

    }
}
