using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.TelegramBot.Commands
{
    public interface IExplanationString
    {
        ExplanationStringsId ExpStringId { get; }
    }
}
