using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace AutoDealersHelper.Exceptions
{
    public enum ExceptionsLevels
    {
        LEVEL_INFO,
        LEVEL_WARN,
        LEVEL_ERROR,
        LEVEL_FATAL,
    }

    public static class ExceptionHandler
    {
        public static string Execute(Exception ex, NLog.Logger logger)
        {
            string errorMessage = "Ой-ой, кажется что-то пошло не так.\n\nПохоже, нужно обращаться в поддержку [ссылка]"; //TODO: доработать текст

            if ((ex is ICustomException))
            {
                ICustomException currentEx = ex as ICustomException;

                switch (currentEx.Level)
                {
                    case ExceptionsLevels.LEVEL_WARN:
                        logger.Warn(ex);
                        break;
                    case ExceptionsLevels.LEVEL_INFO:
                        logger.Info(ex);
                        break;
                    case ExceptionsLevels.LEVEL_FATAL:
                        logger.Fatal(ex);
                        break;
                    case ExceptionsLevels.LEVEL_ERROR:
                        logger.Error(ex);
                        break;
                    default:
                        break;
                }

                return currentEx.ErrorMessage;
            }
            else
                logger.Error(ex); //TODO: неизвестная ошибка - кинуть в логи ERROR, время, юзера десериализованного, команду им отправленную

            return errorMessage;
        }
    }
}
