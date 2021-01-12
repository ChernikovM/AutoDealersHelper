namespace AutoDealersHelper.TelegramBot.Commands
{
    interface ICommandValidatable
    {
        bool Validate(Database.Objects.User user);
    }
}
