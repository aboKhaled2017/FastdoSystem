using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.backendsys
{
    public static class PremitivesEx
    {
        public static string JoinToDelimitedStr(this IEnumerable<string> arr,string delimiter)
        {
            if (arr.Count() == 0) return "";
            string str = arr.ElementAt(0);
            for(int i = 1; i < arr.Count(); i++)
            {
                str += delimiter + arr.ElementAt(i);
            }
            return str;
        }
    }
}
