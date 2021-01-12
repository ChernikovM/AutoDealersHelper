using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    interface ICommandWithKeyboard
    {
        ReplyKeyboardMarkup Keyboard { get; }
        ReplyKeyboardMarkup GetKeyboard(Dictionary<string, AbstractCommand> availableCommands, AbstractCommand previousCommand, int columns = 2)
        {
            List<List<KeyboardButton>> rowsList = new List<List<KeyboardButton>>();

            if (availableCommands != null)
            {
                int i = 0;

                List<KeyboardButton> currentRow = new List<KeyboardButton>();
                foreach (var com in availableCommands)
                {
                    if (i % columns == 0 && currentRow.Count != 0)
                    {
                        rowsList.Add(currentRow);
                        currentRow = new List<KeyboardButton>();
                    }

                    currentRow.Add(com.Value.Name);
                    ++i;
                }
                rowsList.Add(currentRow);
            }

            if (previousCommand != null)
                rowsList.Add(new List<KeyboardButton>() { new KeyboardButton(CommandHelper.commandNames[CommandNameId.C_BACK]) } );

            return new ReplyKeyboardMarkup(rowsList, resizeKeyboard: true);
        }
    }
}
