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

    }
}
