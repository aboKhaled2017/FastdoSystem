using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Services
{
    public class DiscountClassifier
    {
        private string _oldDiscount { get; set; }
        private string _forClass { get; set; }
        private double _discountForThisClass { get; set; }
        private List<Tuple<string, double>> getOldDiscount()
        {
            return JsonConvert.DeserializeObject<List<Tuple<string, double>>>(_oldDiscount);
        }
        public DiscountClassifier(string oldDiscount, string forClass, double discountForThisClass)
        {
            this._oldDiscount = oldDiscount;
            this._forClass = forClass;
            this._discountForThisClass = discountForThisClass;
        }
        public static string GetNewPrice(string oldDiscount, string forClass, double discountForClass)
        {
            var discountClassifier = new DiscountClassifier(oldDiscount, forClass, discountForClass);
            var newEntry= new Tuple<string, double>(forClass,discountForClass);
            List<Tuple<string, double>> newDiscountEntries = null;
            if (string.IsNullOrEmpty(oldDiscount))
                newDiscountEntries = new List<Tuple<string, double>> { newEntry};
            else
            {
                var oldDiscountEntries= discountClassifier.getOldDiscount();
                var disEntryForClass= oldDiscountEntries.FirstOrDefault(c => c.Item1 == forClass);
                if (disEntryForClass == null)
                {
                    oldDiscountEntries.Add(newEntry);
                    newDiscountEntries = oldDiscountEntries;
                }
                else
                {
                    newDiscountEntries = oldDiscountEntries.Where(c => c.Item1 != forClass).ToList();
                    newDiscountEntries.Add(newEntry);
                }
            }
            return JsonConvert.SerializeObject(newDiscountEntries);
        }
    }
}
