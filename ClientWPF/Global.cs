using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Net.ClientWPF
{
    public class Global
    {

        private static string[] coinSymbols = new string[]
        {
             "ADA","ADX","AION","AMB","ARK","ARN","AST","BAT","BCC","BCD","BCPT","BNB","BNT","BQX","BRD","BTC","BTG","BTS"
            ,"CDT","CMT","CND","CTR","DASH","DGD","DLT","DNT","EDO","ELF","ENG","ENJ","EOS","ETC","ETH","EVX","FUEL","FUN"
            ,"GAS","GTO","GVT","GXS","HSR","ICN","ICX","IOTA","KMD","KNC","LEND","LINK","LRC","LSK","LTC"
            ,"MANA","MCO","MDA","MOD","MTH","MTL","NEBL","NEO","NULS","OAX","OMG","OST","Pair","POE","POWR","PPT"
            ,"QSP","QTUM","RCN","RDN","REQ","SALT","SNGLS","SNM","SNT","STORJ","STRAT","SUB","TNB","TNT","TRX","USDT"
            ,"VEN","VIB","WABI","WAVES","WINGS","WTC","XLM","XMR","XRP","XVG","XZC","YOYO","ZEC","ZRX"
            ,"TRIG","LUN","NAV","APPC","VIBE"
        };

        private static Dictionary<string, string> symbolReplacements = new Dictionary<string, string>()
        {
            { "IOTA", "MIOTA" }
        };

        public static string GetBinanceSymbolName(string symbol)
        {
            return symbolReplacements.ContainsKey(symbol) ? symbolReplacements[symbol] : symbol;
        }

        public static string[] GetAllSymbols()
        {
            return coinSymbols.ToArray();
        }

        public static string[] SplitTradeSymbols(string symbols)
        {
            int length = symbols.Length;
            var firsts = new List<string>();
            var seconds = new List<string>();

            for (int pos = 3; pos <= length - 3; pos++)
            {
                firsts.Add(symbols.Substring(0, pos));
                seconds.Add(symbols.Substring(pos));
            }

            for (int i = 0; i < firsts.Count(); i++)
            {
                if (coinSymbols.Contains(firsts[i]) && coinSymbols.Contains(seconds[i]))
                {
                    return new string[] { firsts[i], seconds[i] };
                }

            }
            return new string[0];
        }



    }
}
