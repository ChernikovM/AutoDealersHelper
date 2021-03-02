using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.Parsers
{
    public abstract class AbstractParser
    {
        public abstract List<Advertisement> Parse();
    }
}
