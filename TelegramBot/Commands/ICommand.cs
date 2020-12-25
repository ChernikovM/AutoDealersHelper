using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    public interface ICommand
    {
        string Name { get; }
        ChatStates RequiredStateForRun { get; }
        Task Execute(Message mes, Bot bot);
        ReplyKeyboardMarkup Keyboard { get; set; }
        ReplyKeyboardMarkup GetKeyboard();
    }
}
