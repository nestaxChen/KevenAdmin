using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Common.Extension
{
    public static class ListExtension
    {
        public static T ToRandomOne<T>(this IList<T> list, T defaultValue = default(T))
        {
            try
            {
                if (list == null || list.Count == 0)
                    return defaultValue;

                int total = list.Count;

                return list[(new Random()).Next(0, total)];
            }
            catch
            {
                return defaultValue;
            }
        }

        public static List<T> ToRandom<T>(this IList<T> list, int count, bool isRepeat = false)
        {
            if (list == null || list.Count == 0 || count <= 0)
                return default(List<T>);

            Random random = new Random((int)DateTime.Now.Ticks);
            var oldList = list;
            var newList = new List<T>();
            for (var i = 0; i < count; i++)
            {
                if (oldList.Count == 0)
                {
                    newList.Add(default(T));
                    continue;
                }
                var newOne = oldList[random.Next(0, oldList.Count)];
                newList.Add(newOne);
                if (!isRepeat)
                {
                    oldList.Remove(newOne);
                }
            }
            return newList;
        }
    }
}
