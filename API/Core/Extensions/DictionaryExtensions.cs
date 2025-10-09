using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrIncrement<TKey>(this Dictionary<TKey, double> dictionary, TKey key)
        {
            if (key == null) return;

            if (dictionary.ContainsKey(key))
                dictionary[key]++;
            else
                dictionary[key] = 1;
        }
    }
}