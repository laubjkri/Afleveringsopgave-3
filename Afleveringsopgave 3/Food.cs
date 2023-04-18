using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KL_Utils;

namespace Afleveringsopgave_3
{
    public class Food
    {
        // TODO: Add category string property
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                ThrowOnInvalidName(value);
                _name = value;
            }
        }
        public int CategoryIndex 
        {
            get
            {
                return _categoryIndex;
            }
            set
            {
                ThrowOnInvalidCategoryIndex(value);
                _categoryIndex = value;
            }
        }

        public string CategoryString
        {
            get
            {
                return _categoriesList.GetItem(_categoryIndex);
            }
            set
            {
                int index = GetCategoryIndexOrThrow(value);                
                _categoryIndex = index;
            }
        }

        public double Co2FromFarming { get; set; }
        public double Co2FromILUC { get; set; }
        public double Co2FromProcessing { get; set; }
        public double Co2FromPackaging { get; set; }
        public double Co2FromTransport { get; set; }
        public double Co2FromRetail { get; set; }
        public double Co2Total
        {
            get {
                return (
                    Co2FromFarming +
                    Co2FromILUC +
                    Co2FromProcessing +
                    Co2FromPackaging +
                    Co2FromTransport +
                    Co2FromRetail
                );
            }
        }

        // Properties to convert doubles to formatted strings. To avoid many decimal places.
        private readonly string _format = "F2";
        public string Co2FromFarmingFormatted => Co2FromFarming.ToString(_format);
        public string Co2FromILUCFormatted => Co2FromILUC.ToString(_format);
        public string Co2FromProcessingFormatted => Co2FromProcessing.ToString(_format);
        public string Co2FromPackagingFormatted => Co2FromPackaging.ToString(_format);
        public string Co2FromTransportFormatted => Co2FromTransport.ToString(_format);
        public string Co2FromRetailFormatted => Co2FromRetail.ToString(_format);
        public string Co2TotalFormatted => Co2Total.ToString(_format);


        public static ReadOnlyCollection<string> Categories { get { return _categoriesList.AsReadOnly(); } }

        public Food(
            string name,
            int category,
            double co2FromFarming,
            double co2FromILUC,
            double co2FromProcessing,
            double co2FromPackaging,
            double co2FromTransport,
            double co2FromRetail
            )
        {
            Name = name;
            CategoryIndex = category;
            Co2FromFarming = co2FromFarming;
            Co2FromILUC = co2FromILUC;
            Co2FromProcessing = co2FromProcessing;
            Co2FromPackaging = co2FromPackaging;
            Co2FromTransport = co2FromTransport;
            Co2FromRetail = co2FromRetail;            
        }

        // Constructor with category as string
        public Food(
            string name,
            string category,
            double co2FromFarming,
            double co2FromILUC,
            double co2FromProcessing,
            double co2FromPackaging,
            double co2FromTransport,
            double co2FromRetail
            )
            : this(
                name,
                GetCategoryIndexOrThrow(category),
                co2FromFarming,    
                co2FromILUC,
                co2FromProcessing,
                co2FromPackaging,
                co2FromTransport,
                co2FromRetail
            )
        {}

        public static bool NameIsValid(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        public static bool CategoryIndexIsValid(int category)
        {
            return category >= 0 && category <= Categories.Count - 1;
        }

        private string _name;
        private int _categoryIndex;


        private static DictionaryList<string> _categoriesList = new() {
            "Drikkevarer",
            "Kød/fjerkræ",
            "Grøntsager",
            "Brød/bageartikler",
            "Korn-/gryn-/bælgfrugtprodukter",
            "Mælk/æg/erstatningsprodukter",
            "Krydderier/konserveringsmidler mv.",
            "Slik/sukkervarer",
            "Frugt",
            "Frugt/grøntsagsprodukter",
            "Spiseolie/-fedt",
            "Tilberedte/konserverede fødevarer",
            "Fisk og skaldyr"
        };
        
        private static int GetCategoryIndexOrThrow(string category)
        {
            try
            {
                return _categoriesList.GetIndex(category);
            }
            catch (KeyNotFoundException)
            {
                if (string.IsNullOrWhiteSpace(category))
                {
                    category = "(empty)";
                }

                throw new Exception($"Category \"{category}\" not found.");
            }            
        }        

        private static void ThrowOnInvalidName(string name)
        {
            if (!NameIsValid(name))
                throw new ArgumentException("Invalid name.");
        }

        private static void ThrowOnInvalidCategoryIndex(int category) 
        {
            if (!CategoryIndexIsValid(category))
                throw new ArgumentException("Invalid category.");
        }
    }
}
