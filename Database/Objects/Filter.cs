using AutoDealersHelper.TelegramBot.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.Database.Objects
{
    public class Filter
    {
        public List<BaseType> Brands { get; set; }
        public List<BaseType> Models { get; set; }
        public List<BaseType> States { get; set; }
        public List<BaseType> Cities { get; set; }
        public List<BaseType> GearBoxes { get; set; }
        public List<BaseType> Fuels { get; set; }

        public List<int> Price { get; set; }
        public List<int> Volume { get; set; }
        public List<int> Mileage { get; set; }
        public List<int> Year { get; set; }

        public Filter()
        {
            Brands = new List<BaseType>() { new BaseType() };
            Models = new List<BaseType>() { new BaseType() };
            States = new List<BaseType>() { new BaseType() };
            Cities = new List<BaseType>() { new BaseType() };
            GearBoxes = new List<BaseType>() { new BaseType() };
            Fuels = new List<BaseType>() { new BaseType() };

            Price = new List<int>() { 0 };
            Volume = new List<int>() { 0 };
            Mileage = new List<int>() { 0 };
            Year = new List<int>() { 0 };
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append($"{CommandHelper.commandNames[CommandNameId.C_BRAND]}: \t" + GetCollectionAsString(Brands));
            result.Append($"{CommandHelper.commandNames[CommandNameId.C_MODEL]}: \t" + GetCollectionAsString(Models));
            result.Append($"{CommandHelper.commandNames[CommandNameId.C_YEAR]}: \t" + GetValuePairAsString(Year));
            result.Append($"{CommandHelper.commandNames[CommandNameId.C_PRICE]} ($): \t" + GetValuePairAsString(Price));
            result.Append($"{CommandHelper.commandNames[CommandNameId.C_MILEAGE]} (тыс.км.): \t" + GetValuePairAsString(Mileage));
            result.Append($"{CommandHelper.commandNames[CommandNameId.C_VOLUME]} (л): \t" + GetValuePairAsString(Volume));
            result.Append($"{CommandHelper.commandNames[CommandNameId.C_STATE]}: \t" + GetCollectionAsString(States));
            result.Append($"{CommandHelper.commandNames[CommandNameId.C_CITY]}: \t" + GetCollectionAsString(Cities));
            result.Append($"{CommandHelper.commandNames[CommandNameId.C_GEARBOX]}: \t" + GetCollectionAsString(GearBoxes));
            result.Append($"{CommandHelper.commandNames[CommandNameId.C_FUEL]}: \t" + GetCollectionAsString(Fuels));

            return result.ToString();
        }

        private string GetCollectionAsString(List<BaseType> collection)
        {
            if (collection.Count == 1 && collection[0].Number == 0)
                return "Любой\n";

            StringBuilder sb = new StringBuilder();
            foreach (var n in collection)
            {
                sb.Append(n.Name + ", ");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        private string GetValuePairAsString(List<int> pair)
        {
            if (pair.Count == 1)
                return "Любой\n";

            return $"{pair[0]} - {pair[1]}\n";
        }
    }
}
