using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys.Services
{
    public class DiscountClassifier<T>
    {
        private string _oldDiscount { get; set; }
        private T _forClassId { get; set; }
        private double _discountForThisClass { get; set; }
        private List<Tuple<T, double>> getOldDiscount()
        {
            return JsonConvert.DeserializeObject<List<Tuple<T, double>>>(_oldDiscount);
        }
        public DiscountClassifier(string oldDiscount, T forClassId, double discountForThisClass)
        {
            this._oldDiscount = oldDiscount;
            this._forClassId = forClassId;
            this._discountForThisClass = discountForThisClass;
        }
        public static string ReplaceClassForDiscount(string oldDiscount,T oldForClass,T newForClassId)
        {
            var oldDiscountEntries= JsonConvert.DeserializeObject<List<Tuple<T, double>>>(oldDiscount);
            var updatedEntry = oldDiscountEntries.SingleOrDefault(e => e.Item1.Equals(oldForClass));

            var filteredEntries = oldDiscountEntries
                .Where(e => !e.Item1.Equals(oldForClass)).ToList();
            if(!filteredEntries.Any(e=>e.Item1.Equals(newForClassId)))
            {
                var newEntry = new Tuple<T, double>(newForClassId, updatedEntry.Item2);
                filteredEntries.Add(newEntry);
            }
            return JsonConvert.SerializeObject(filteredEntries);
        }
        public static string RemoveClassForDiscount(string oldDiscount, T forClassId)
        {
            var oldDiscountEntries = JsonConvert.DeserializeObject<List<Tuple<T, double>>>(oldDiscount);
            var deletedEntry = oldDiscountEntries.SingleOrDefault(d => d.Item1.Equals(forClassId));
            if(deletedEntry!=null)
            {
                oldDiscountEntries.Remove(deletedEntry);
                if (oldDiscountEntries.Count == 0)
                    return null;
            }
            return JsonConvert.SerializeObject(oldDiscountEntries);
        }
        public static string GetNewDiscount(string oldDiscount, T forClassId, double discountForClass)
        {
            var discountClassifier = new DiscountClassifier<T>(oldDiscount, forClassId, discountForClass);
            var newEntry= new Tuple<T, double>(forClassId,discountForClass);
            List<Tuple<T, double>> newDiscountEntries = null;
            if (string.IsNullOrEmpty(oldDiscount))
                newDiscountEntries = new List<Tuple<T, double>> { newEntry};
            else
            {
                var oldDiscountEntries= discountClassifier.getOldDiscount();
                var disEntryForClass= oldDiscountEntries.FirstOrDefault(c => c.Item1.Equals(forClassId));
                if (disEntryForClass == null)
                {
                    oldDiscountEntries.Add(newEntry);
                    newDiscountEntries = oldDiscountEntries;
                }
                else
                {
                    newDiscountEntries = oldDiscountEntries.Where(c =>! c.Item1.Equals(forClassId)).ToList();
                    newDiscountEntries.Add(newEntry);
                }
            }
            return JsonConvert.SerializeObject(newDiscountEntries);
        }
    }
}
