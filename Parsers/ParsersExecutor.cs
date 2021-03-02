using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.Parsers
{
    public class ParsersExecutor
    {
        private ParsersExecutor() 
        {
            _parsers = new List<AbstractParser>()
            {
                ParserAutoRia.GetInstance(Program.Config.AutoRiaToken),

            };
        }

        private static ParsersExecutor _instance;

        public static ParsersExecutor Instance => _instance ??= new ParsersExecutor();

        private List<AbstractParser> _parsers;

        public void Execute()
        {
            List<Advertisement> newAdvertisements = new List<Advertisement>();

            foreach (var parser in _parsers)
            {
                newAdvertisements.AddRange(parser.Parse());
            }

            if (newAdvertisements.Count == 0) //нет новых объяв
                return;

            AdvertisementsHandler.Run(newAdvertisements);
        }
    }
}
