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
        //static ICoinmarketcapClient cmcc;
        static public IEnumerable<Currency> Currencies { get; private set; }


        static CoinMarket()
        {
            UpdateCurrencies();
        }

        static void UpdateCurrencies()
        {
            int attempts = 0;
            TimeSpan attemptLength = default(TimeSpan);
            IEnumerable<Currency> currencies = null;
            do
            {
                attempts++;
                var task = new Task(() =>
                {
                    try
                    {
                        DateTime start = DateTime.Now;
                        ICoinmarketcapClient cmcc = new CoinmarketcapClient();
                        currencies = cmcc.GetCurrencies(100000);
                        DateTime end = DateTime.Now;
                        attemptLength = end - start;
                    }
                    catch
                    {

                    }
                });
                task.Start();

                //TODO: This hangs on occasion

                task.Wait(1000);
            } while (currencies == null);
            Currencies = currencies;

            if(attempts>1)
            {
                // This get annoying
                //new MessageBox.MessageBoxService().ShowMessage($"CoinMarket Took {attempts} attempts to get the data. Success took {attemptLength.TotalSeconds} seconds.", "Coinmarket took multiple attempts to retrieve data.", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
            }
        }

        static public Currency GetCurrency(string symbol)
        {
            while(Currencies == null)
            {
                System.Threading.Thread.Sleep(10);
            }

            symbol = Global.GetBinanceSymbolName(symbol);

            return Currencies.Where(currency => currency.Symbol == symbol).FirstOrDefault();
        }
    }
}
