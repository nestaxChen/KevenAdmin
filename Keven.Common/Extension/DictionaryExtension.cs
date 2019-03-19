using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class DictionaryExtension
    {
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue = default(TValue))
        {
            if (dic == null)
                return defaultValue;
            try
            {
                if (dic.ContainsKey(key))
                {
                    return (TValue)dic[key];
                }
                else
                    return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static void Set<Tkey, TValue>(this IDictionary<Tkey, TValue> dic, Tkey key, TValue value)
        {
            if (dic.ContainsKey(key))
                dic[key] = value;
            else
                dic.Add(key, value);
        }

        public static dynamic ToDynamic<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            dynamic x = new System.Dynamic.ExpandoObject();
            foreach (KeyValuePair<TKey, TValue> key in dictionary)
            {
                ((IDictionary<TKey, TValue>)x).Add(key.Key, key.Value);
            }
            return x;
        }
    }
}
