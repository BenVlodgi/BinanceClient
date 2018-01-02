using NoobsMuc.Coinmarketcap.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Net.ClientWPF
{
    public static class CoinMarket
    {
        static ICoinmarketcapClient cmcc;
        static public IEnumerable<Currency> Currencies { get; private set; }

        static Dictionary<string, string> symbolReplacements = new Dictionary<string, string>()
        {
            { "IOTA", "MIOTA" }
        };

        static CoinMarket()
        {
            cmcc = new CoinmarketcapClient();
            Currencies = cmcc.GetCurrencies(100000);
        }

        static public Currency GetCurrency(string symbol)
        {
            if (symbolReplacements.ContainsKey(symbol))
                symbol = symbolReplacements[symbol];

            return Currencies.Where(currency => currency.Symbol == symbol).FirstOrDefault();
        }
    }
}
