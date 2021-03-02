using AutoDealersHelper.Exceptions;
using AutoDealersHelper.Parsers;
using AutoDealersHelper.TelegramBot;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AutoDealersHelper
{
    static class Program
    {
        static Program()
        {
            Config = GetConfigFromJsonFile("config.json");
            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        public static NLog.Logger logger; //TODO: все заприватить и нормально использовать в ддругих классах
        public static Bot bot;
        public static Config Config;

        static void Main(string[] args)
        {
            try
            {
                Start();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Execute(ex, logger);
            }
        }

        private static void Start()
        {
            bot = Bot.GetInstance(logger);

            bot.StartReceiving();

            SchedulerService.Instance.ScheduleTask(22, 0, 1, () => Shutdown(0));
            SchedulerService.Instance.ScheduleTask(DateTime.Now.Hour, DateTime.Now.Minute + 1, 1, () => ParsersExecutor.Instance.Execute());

            string command;
            do
            {
                command = Console.ReadLine();

                if (command == "parse")
                    ParsersExecutor.Instance.Execute();
            }
            while (command != "-sh"); //shutdown
            Shutdown(1);
        }

        private static Config GetConfigFromJsonFile(string fileName)
        {
            string configPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            if (File.Exists(configPath) == false)
                throw new FileNotFoundException($"File [{fileName}] does not exist in folder [{Directory.GetCurrentDirectory()}]", configPath);

            string configJsonContent = File.ReadAllText(configPath);

            return JsonConvert.DeserializeObject<Config>(configJsonContent);
        }

        private static void Shutdown(int code)
        {
            bot.StopReceiving();
            logger.Info($"Shutting down with code {code}.");
            Environment.Exit(code);
        }
    }
}
