using System;
using Binance.Net.Objects;
using LiveCharts.Defaults;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NoobsMuc.Coinmarketcap.Client;

namespace Binance.Net.ClientWPF
{
    public static class Extensions
    {
        public static TV GetValue<TK, TV>(this Dictionary<TK, TV> dictionary, TK key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            return default(TV);
        }

        public static IEnumerable<Type> GetTypesWithAttribute(this Assembly assembly, Type attribute)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(attribute, true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        public static OhlcPoint ToOhlcPoint(this BinanceKline kline)
        {
            return new OhlcPoint((double)kline.Open, (double)kline.High, (double)kline.Low, (double)kline.Close);
        }

        public static decimal? ParseToNullableDecimal(this string value)
        {
            return decimal.TryParse(value, out decimal parsed) ? (decimal?)parsed : null;
        }

        public static void ReplaceInPlace<T>(this List<T> list, T replaceMe, T with)
        {
            var replaceables = list.Where(element => element.Equals(replaceMe)).ToArray();
            for (int i = 0; i < replaceables.Count(); i++)
            {
                list[list.IndexOf(replaceables[i])] = with;
            }
        }

        private static TimeSpan UTCOffset = DateTime.Now - DateTime.UtcNow;
        public static DateTime UTCToLocal(this DateTime dateTime)
        {
            return dateTime + UTCOffset;
        }
        public static DateTime UTCFromLocal(this DateTime dateTime)
        {
            return dateTime - UTCOffset;
        }

        public static double ReRange(this double value, double oldMin, double oldMax, double newMin, double newMax)
        {
            double oldRange;
            return (oldRange = oldMax - oldMin) == 0
                ? newMin
                : (((value - oldMin) * (newMax - newMin)) / oldRange) + newMin;
        }

        public static double ReRangeKnownRange(this double value, double oldMin, double oldRange, double newMin, double newRange)
        {
            return oldRange == 0
                ? newMin
                : (((value - oldMin) * newRange) / oldRange) + newMin;
        }
    }
}
