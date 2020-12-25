using AutoDealersHelper.TelegramBot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutoDealersHelper
{
    static class Program
    {
        private const string _configFileName = "config.json";
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private static Bot _bot;

        static void Main(string[] args)
        {
            Start();
            asd();
        }

        public static async Task asd()
        {
            await Task.Run( () => { int i = 5; });
        }

        private static void Start()
        {
            Config config = ReadConfigFromJson();

            if (config == null)
                return;

            try
            {
                _bot = Bot.GetInstance(config, _logger);

                _bot.StartReceiving();
                Console.ReadLine(); //TODO: сделать таймер и запуск парсинга ресурсов по времени (1 раз/минута)
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            finally
            {
                _bot.StopReceiving();
            }
        }

        private static Config ReadConfigFromJson()
        {
            try
            {
                string configPath = Path.Combine(Directory.GetCurrentDirectory(), _configFileName);

                if (File.Exists(configPath) == false)
                    throw new FileNotFoundException("",configPath);

                string configJsonContent = File.ReadAllText(configPath);

                return JsonConvert.DeserializeObject<Config>(configJsonContent);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
                return null;
            }
        }   
    }
}
