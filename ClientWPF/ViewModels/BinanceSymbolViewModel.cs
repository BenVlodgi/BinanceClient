using System.Collections.ObjectModel;
using System.Linq;
using Binance.Net.ClientWPF.MVVM;
using Binance.Net.Objects;
using System.Collections.Generic;
using System;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace Binance.Net.ClientWPF.ViewModels
{
    public class BinanceSymbolViewModel : ObservableObject
    {

        private void EnsureSymbolPairs()
        {
            if (symbolPair.Length == 0)
            {
                symbolPair = Global.SplitTradeSymbols(Symbol);
                if (symbolPair.Length == 0)
                {
                    symbolPair = new string[] { "***", "***" };
                }
            }
        }

        private string[] symbolPair = new string[0];
        public string SymbolAsset
        {
            get
            {
                EnsureSymbolPairs();
                return symbolPair?[0] ?? "";
            }
        }

        public string SymbolCurrency
        {
            get
            {
                EnsureSymbolPairs();
                return symbolPair?[1] ?? "";
            }
        }

        public string SymbolText
        {
            get
            {
                EnsureSymbolPairs();
                return $"{symbolPair?[0]} | {symbolPair?[1]}";
            }
        }

        private string symbol;
        public string Symbol
        {
            get { return symbol; }
            set
            {
                symbol = value;
                RaisePropertyChangedEvent("Symbol");

                symbolPair = new string[0];
                RaisePropertyChangedEvent("SymbolText");
                RaisePropertyChangedEvent("SymbolAsset");
                RaisePropertyChangedEvent("SymbolCurrency");
            }
        }

        private decimal price;
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                RaisePropertyChangedEvent("Price");
            }
        }

        private decimal priceChangePercent;
        public decimal PriceChangePercent
        {
            get { return priceChangePercent; }
            set
            {
                priceChangePercent = value;
                RaisePropertyChangedEvent("PriceChangePercent");
            }
        }

        private decimal highPrice;
        public decimal HighPrice
        {
            get { return highPrice; }
            set
            {
                highPrice = value;
                RaisePropertyChangedEvent("HighPrice");
            }
        }

        private decimal lowPrice;
        public decimal LowPrice
        {
            get { return lowPrice; }
            set
            {
                lowPrice = value;
                RaisePropertyChangedEvent("LowPrice");
            }
        }

        private decimal volume;
        public decimal Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                RaisePropertyChangedEvent("Volume");
            }
        }

        private decimal tradeAmount;
        public decimal TradeAmount
        {
            get { return tradeAmount; }
            set
            {
                tradeAmount = value;
                RaisePropertyChangedEvent("TradeAmount");
            }
        }

        private decimal tradePrice;
        public decimal TradePrice
        {
            get { return tradePrice; }
            set
            {
                tradePrice = value;
                RaisePropertyChangedEvent("TradePrice");
            }
        }

        private ObservableCollection<OrderViewModel> orders;
        public ObservableCollection<OrderViewModel> Orders
        {
            get { return orders; }
            set
            {
                orders = value;
                RaisePropertyChangedEvent("Orders");
            }
        }

        private KlineInterval klineInterval = KlineInterval.OneHour;
        public KlineInterval KlineInterval
        {
            get { return klineInterval; }
            set
            {
                klineInterval = value;
                RaisePropertyChangedEvent("KlineInterval");
                RaisePropertyChangedEvent("Klines");
            }
        }

        private Dictionary<KlineInterval, ObservableCollection<BinanceKline>> klines = Enum.GetValues(typeof(KlineInterval)).Cast<KlineInterval>().ToDictionary(val => val, val => new ObservableCollection<BinanceKline>());
        public ObservableCollection<BinanceKline> Klines
        {
            get
            {
                return klines[KlineInterval];
            }
            set
            {
                klines[KlineInterval] = value;
                RaisePropertyChangedEvent("Klines");
            }
        }

        public void AddKline(KlineInterval interval, BinanceKline klineValue)
        {
            klines[interval].Add(klineValue);

            if (interval == klineInterval)
                RaisePropertyChangedEvent("Klines");
        }

        public void ClearKlines()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var klineTable in klines)
                {
                    klineTable.Value.Clear();
                }
            });
        }

        public void AddKlines(KlineInterval interval, IEnumerable<BinanceKline> klineValues)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                CandleSticks = new SeriesCollection { new CandleSeries { Values = new ChartValues<OhlcPoint>(klineValues.Select(kline => kline.ToOhlcPoint())) } };
                CandleStickLabels = klineValues.Select(kline => kline.OpenTime.ToString("dd MMM")).Distinct().ToArray();
                
                foreach (var kline in klineValues)
                {
                    klines[interval].Add(kline);

                    if (interval == klineInterval)
                    {
                        RaisePropertyChangedEvent("Klines");
                    }
                }
            });
        }

        private SeriesCollection candleSticks;
        public SeriesCollection CandleSticks
        {
            get { return candleSticks; }
            set
            {
                candleSticks = value;
                RaisePropertyChangedEvent("CandleSticks");
            }
        }
        private string[] candleStickLabels;
        public string[] CandleStickLabels
        {
            get { return candleStickLabels; }
            set
            {
                candleStickLabels = value;
                RaisePropertyChangedEvent("CandleStickLabels");
            }
        }


        public BinanceSymbolViewModel(string symbol, decimal price)
        {
            this.symbol = symbol;
            this.price = price;
        }

        public void AddOrder(OrderViewModel order)
        {
            Orders.Add(order);
            Orders.OrderByDescending(o => o.Time);
            RaisePropertyChangedEvent("Orders");
        }
    }
}
