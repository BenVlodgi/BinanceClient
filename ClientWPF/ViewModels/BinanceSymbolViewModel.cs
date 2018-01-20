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
using System.Windows;
using TicTacTec.TA.Library;
using System.Windows.Media;
using System.Threading.Tasks;
using Binance.Net.ClientWPF.MessageBox;

namespace Binance.Net.ClientWPF.ViewModels
{
    public class BinanceSymbolViewModel : ObservableObject
    {
        public string BinanceTradeURL { get { return @"https://www.binance.com/trade.html?symbol=" + SymbolAsset + "_" + SymbolCurrency; } }
        public string BinanceTradeDetailURL { get { return @"https://www.binance.com/tradeDetail.html?symbol=" + SymbolAsset + "_" + SymbolCurrency; } }

        private void EnsureSymbolPairs(bool force = false)
        {
            if (symbolPair.Length == 0 || force)
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

                EnsureSymbolPairs(true);
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
            private set
            {
                orders = value;
                RaisePropertyChangedEvent("Orders");
            }
        }

        private ObservableCollection<TradeViewModel> trades;
        public ObservableCollection<TradeViewModel> Trades
        {
            get { return trades; }
            set
            {
                trades = value;
                RaisePropertyChangedEvent("Trades");
            }
        }

        private ObservableCollection<AggregateTradeViewModel> aggregateTrades;
        public ObservableCollection<AggregateTradeViewModel> AggregateTrades
        {
            get { return aggregateTrades; }
            set
            {
                aggregateTrades = value;
                RaisePropertyChangedEvent("AggregateTrades");
            }
        }

        #region Charts

        #region KlineInterval
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
        #endregion

        #region Klines
        private Dictionary<KlineInterval, ObservableCollection<BinanceKline>> klinesDictionary = Enum.GetValues(typeof(KlineInterval)).Cast<KlineInterval>().ToDictionary(val => val, val => new ObservableCollection<BinanceKline>());
        public ObservableCollection<BinanceKline> Klines
        {
            get
            {
                return klinesDictionary[KlineInterval];
            }
            set
            {
                klinesDictionary[KlineInterval] = value;
                RaisePropertyChangedEvent("Klines");
            }
        }
        #endregion

        #region CandleSticks
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
        #endregion

        #region ChartDateLabels
        private string[] _chartDateLabels;
        public string[] ChartDateLabels
        {
            get { return _chartDateLabels; }
            set
            {
                _chartDateLabels = value;
                RaisePropertyChangedEvent("ChartDateLabels");
            }
        }
        #endregion

        #region ChartHourLabels
        private string[] _chartHourLabels;
        public string[] ChartHourLabels
        {
            get { return _chartHourLabels; }
            set
            {
                if (_chartHourLabels == value) return;
                _chartHourLabels = value;
                RaisePropertyChangedEvent("ChartHourLabels");
            }
        }
        #endregion

        #region ChartVisibleSteps
        private int _ChartVisibleSteps;
        public int ChartVisibleSteps
        {
            get { return _ChartVisibleSteps; }
            set
            {
                if (_ChartVisibleSteps == value) return;
                _ChartVisibleSteps = value;
                RaisePropertyChangedEvent("ChartVisibleSteps");
                RaisePropertyChangedEvent("ChartCurrentEndSpot");
                RaisePropertyChangedEvent("ChartMaxSteps");
            }
        }
        #endregion

        #region ChartTotalSteps ChartMaxSteps ChartZoomUpperBound
        private int _chartTotalSteps;
        public int ChartTotalSteps
        {
            get { return _chartTotalSteps; }
            set
            {
                if (_chartTotalSteps == value) return;
                _chartTotalSteps = value;
                RaisePropertyChangedEvent("ChartTotalSteps");
                RaisePropertyChangedEvent("ChartMaxSteps");
                RaisePropertyChangedEvent("ChartZoomUpperBound");
            }
        }

        public int ChartMaxSteps
        {
            get { return ChartTotalSteps - ChartVisibleSteps; }
        }

        public int ChartZoomUpperBound
        {
            get { return ChartTotalSteps + 1; }
        }
        #endregion

        #region ChartCurrentSpot ChartCurrentEndSpot
        private int _ChartCurrentSpot;
        public int ChartCurrentSpot
        {
            get { return _ChartCurrentSpot; }
            set
            {
                if (_ChartCurrentSpot == value) return;
                _ChartCurrentSpot = value;
                RaisePropertyChangedEvent("ChartCurrentSpot");
                RaisePropertyChangedEvent("ChartCurrentEndSpot");
            }
        }

        public int ChartCurrentEndSpot
        {
            get { return ChartCurrentSpot + ChartVisibleSteps; }
        }

        #endregion

        #region MACDPlot
        private SeriesCollection _macdPlot;
        public SeriesCollection MACDPlot
        {
            get { return _macdPlot; }
            set
            {
                if (_macdPlot == value) return;
                _macdPlot = value;
                RaisePropertyChangedEvent("MACDPlot");
            }
        }
        #endregion
        #region MACDStatus
        private string _MACDStatus;
        public string MACDStatus
        {
            get { return _MACDStatus; }
            set
            {
                if (_MACDStatus == value) return;
                _MACDStatus = value;
                RaisePropertyChangedEvent("MACDStatus");
            }
        }
        #endregion

        #region RSIPlot
        private SeriesCollection _RSIPlot;
        public SeriesCollection RSIPlot
        {
            get { return _RSIPlot; }
            set
            {
                if (_RSIPlot == value) return;
                _RSIPlot = value;
                RaisePropertyChangedEvent("RSIPlot");
            }
        }
        #endregion
        #region RSIStatus
        private string _RSIStatus;
        public string RSIStatus
        {
            get { return _RSIStatus; }
            set
            {
                if (_RSIStatus == value) return;
                _RSIStatus = value;
                RaisePropertyChangedEvent("RSIStatus");
            }
        }
        #endregion

        #endregion

        public BinanceSymbolViewModel(string symbol, decimal price)
        {
            this.symbol = symbol;
            this.price = price;

            orders = new ObservableCollection<OrderViewModel>();
            trades = new ObservableCollection<TradeViewModel>();
            aggregateTrades = new ObservableCollection<AggregateTradeViewModel>();

            ChartVisibleSteps = 24;
        }

        bool gettingHistory = false;
        bool updateHistoryWhenFinished = false;
        public void GetHistory(Storage storageInstance)
        {
            if (gettingHistory)
            {
                updateHistoryWhenFinished = true;
                return;
            }

            gettingHistory = true;
            DateTime now = DateTime.UtcNow;


            if (string.IsNullOrWhiteSpace(MACDStatus)) MACDStatus = "Aquiring";
            if (string.IsNullOrWhiteSpace(RSIStatus)) RSIStatus = "Aquiring";

            

            int haveAtleastThisManyKlines = 72;

            //Only get as much history as we need
            //default total klines
            int desiredLookBack;
            if (Klines == null || Klines.Count == 0 || Klines.Count < haveAtleastThisManyKlines)
            {
                // We have no klines, go get some
                desiredLookBack = haveAtleastThisManyKlines;
            }
            else
            {
                //We have klines, but we are out of date, so we'll get our current one, plus any that happenend 
                //Get the latest kline
                var latestKline = Klines.OrderByDescending(k => k.CloseTime).FirstOrDefault();
                var hours = (int)(now - latestKline.CloseTime).TotalHours;

                desiredLookBack = hours + 1;
            }

            var start = now - new TimeSpan(desiredLookBack, 0, 0);


            using (var client = new BinanceClient())
            {
                this.KlineInterval = KlineInterval.OneHour;

                var usedInterval = KlineInterval;
                var usedSymbol = Symbol;

                var result = client.GetKlines(usedSymbol, usedInterval, start);
                if (result.Success)
                {
                    //TODO: Use Parallel
                    //result.Data.AsParallel().ForAll(kline => new CandleDBRow(kline, SymbolAsset, SymbolCurrency, usedInterval) { Connection = storageInstance.DBConnection }.Save());

                    foreach (var kline in result.Data)
                    {
                        new CandleDBRow(kline, SymbolAsset, SymbolCurrency, usedInterval) { Connection = storageInstance.DBConnection }.Save();
                    }

                    AddKlines(KlineInterval.OneHour, result.Data.ToList());
                }
                else new MessageBoxService().ShowMessage($"Getting Candles for symbol {Symbol} failed: {result.Error.Message}", $"Code: {result.Error.Code}", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

            }


            gettingHistory = false;
            if (updateHistoryWhenFinished)
            {
                updateHistoryWhenFinished = false;
                GetHistory(storageInstance);
                // Note: This will use the previous storageInstance, if the instance needs to ever be different, then we can save it later above
            }
        }



        public void AddOrder(OrderViewModel order)
        {
            Orders.Add(order);
            Orders.OrderByDescending(o => o.Time);
            RaisePropertyChangedEvent("Orders");
        }

        public void AddOrders(IEnumerable<OrderViewModel> value)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var order in value)
                {
                    var containedOrder = Orders.Where(o => o.Id == order.Id).FirstOrDefault();
                    if (containedOrder is null)
                    {
                        Orders.Add(order);
                    }
                    else
                    {
                        Orders[orders.IndexOf(containedOrder)] = order;
                    }
                }
                Orders = new ObservableCollection<OrderViewModel>(Orders.OrderByDescending(order => order.Time));
            });
        }

        public void AddTrade(TradeViewModel trade)
        {
            Trades.Add(trade);
            Trades.OrderByDescending(t => t.Time);
            RaisePropertyChangedEvent("Trades");
        }

        public void AddAggregateTrade(AggregateTradeViewModel aggregateTrade)
        {
            AggregateTrades.Add(aggregateTrade);
            AggregateTrades.OrderByDescending(t => t.Time);
            RaisePropertyChangedEvent("AggregateTrades");
        }


        public void ClearKlines()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var klineTable in klinesDictionary)
                {
                    klineTable.Value.Clear();
                }
            });
        }

        private void AddKline(KlineInterval interval, BinanceKline klineValue)
        {
            klinesDictionary[interval].Add(klineValue);

            if (interval == klineInterval)
                RaisePropertyChangedEvent("Klines");
        }

        private void AddKlines(KlineInterval interval, List<BinanceKline> klineValues)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var indexesToReplace = new Dictionary<int, int>(); // index in new klineValues, index in Klines
                var indexesToAppend = new List<int>(); //index in new klineValues

                bool klinesChanged = false;
                for (int kvi = 0; kvi < klineValues.Count(); kvi++)
                {
                    var kline = klineValues[kvi];

                    //don't add duplicates
                    var dup = klinesDictionary[KlineInterval].Where(k => k.OpenTime == kline.OpenTime).SingleOrDefault();
                    if (dup == null)
                    {
                        indexesToAppend.Add(kvi);
                        klinesDictionary[interval].Add(kline);
                    }
                    else
                    {
                        var dupIndex = klinesDictionary[KlineInterval].IndexOf(dup);
                        indexesToReplace.Add(kvi, dupIndex);
                        klinesDictionary[interval][dupIndex] = kline;
                    }

                    if (!klinesChanged && interval == klineInterval) klinesChanged = true;
                }

                if (klinesChanged) RaisePropertyChangedEvent("Klines");

                if (CandleSticks == null)
                {
                    CandleSticks = new SeriesCollection { new CandleSeries { Title = "Candles", Values = new ChartValues<OhlcPoint>(klinesDictionary[interval].Select(kline => kline.ToOhlcPoint())) } };
                    ChartDateLabels = klineValues.Select(kline => kline.OpenTime.UTCToLocal().ToString("MMM d")).ToArray();
                    ChartHourLabels = klineValues.Select(kline => kline.OpenTime.UTCToLocal().ToString("h:00")).ToArray();
                }
                else
                {
                    var series = CandleSticks.Where(cs => cs.Title == "Candles").FirstOrDefault();
                    // Update the existing klines
                    foreach (var itr in indexesToReplace)
                    {
                        // Update the existing OHLC point with dup info for animations to work
                        //series.Values[itr.Value] = klineValues[itr.Key].ToOhlcPoint();
                        ((OhlcPoint)series.Values[itr.Value]).UpdateValues(klineValues[itr.Key]);
                    }

                    // Append the new ones
                    foreach (var ita in indexesToAppend)
                    {
                        series.Values.Add(klineValues[ita].ToOhlcPoint());
                    }
                }

                double[] outMACD = null;
                double[] outMACDSignal = null;

                double[] outMACDHist = null;
                double[] outMACDHistGrowth = null;
                double[] outMACDHistLoss = null;

                double[] outRSI = null;

                var tasks = new Task[]
                {
                    new Task(()=> { CalculateMACD(false, out outMACD, out outMACDSignal, out outMACDHist); outMACDHist.SplitAndPad(0d, out outMACDHistGrowth, out outMACDHistLoss); }),
                    new Task(()=> { CalculateRSI(out outRSI); }),
                };
                foreach (var task in tasks) task.Start();
                Task.WaitAll(tasks);

                //CalculateMACD(false, out double[] outMACD, out double[] outMACDSignal, out double[] outMACDHist);
                //CalculateRSI(out double[] outRSI);
                //outMACDHist.SplitAndPad(0d, out double[] outMACDHistGrowth, out double[] outMACDHistLoss);

                if (MACDPlot == null)
                {
                    MACDPlot = new SeriesCollection
                    {
                        new LineSeries {PointGeometry = null, Fill = Brushes.Transparent, Title="MACD", Values = new ChartValues<double>(outMACD)},
                        new LineSeries {PointGeometry = null, Fill = Brushes.Transparent, Title="MACD Signal", Values = new ChartValues<double>(outMACDSignal)},
                        new LineSeries {PointGeometry = null, Stroke = Brushes.Green, AreaLimit = 0, Title = "MACD Growth Trend", Values = new ChartValues<double>(outMACDHistGrowth)},
                        new LineSeries {PointGeometry = null, Stroke = Brushes.Gray, AreaLimit = 0, Title = "MACD Loss Trend", Values = new ChartValues<double>(outMACDHistLoss)},
                    };
                }
                else
                {
                    var macdSeries = MACDPlot.Where(cs => cs.Title == "MACD").FirstOrDefault();
                    foreach (var itr in indexesToReplace) macdSeries.Values[itr.Key] = outMACD[itr.Key];
                    foreach (var ita in indexesToAppend) macdSeries.Values.Add(outMACD[ita]);

                    var macdSignalSeries = MACDPlot.Where(cs => cs.Title == "MACD Signal").FirstOrDefault();
                    foreach (var itr in indexesToReplace) macdSignalSeries.Values[itr.Key] = outMACDSignal[itr.Key];
                    foreach (var ita in indexesToAppend) macdSignalSeries.Values.Add(outMACDSignal[ita]);

                    var macdGrowthTrendSeries = MACDPlot.Where(cs => cs.Title == "MACD Growth Trend").FirstOrDefault();
                    foreach (var itr in indexesToReplace) macdGrowthTrendSeries.Values[itr.Key] = outMACDHistGrowth[itr.Key];
                    foreach (var ita in indexesToAppend) macdGrowthTrendSeries.Values.Add(outMACDHistGrowth[ita]);

                    var macdLossTrendSeries = MACDPlot.Where(cs => cs.Title == "MACD Loss Trend").FirstOrDefault();
                    foreach (var itr in indexesToReplace) macdLossTrendSeries.Values[itr.Key] = outMACDHistLoss[itr.Key];
                    foreach (var ita in indexesToAppend) macdLossTrendSeries.Values.Add(outMACDHistLoss[ita]);
                }

                if (RSIPlot == null)
                {
                    RSIPlot = new SeriesCollection
                    {
                        new LineSeries {PointGeometry = null, Fill = Brushes.Transparent, Title="RSI", Values = new ChartValues<double>(outRSI)},
                    };
                }
                else
                {
                    var rsiSeries = RSIPlot.Where(cs => cs.Title == "RSI").FirstOrDefault();
                    foreach (var itr in indexesToReplace) rsiSeries.Values[itr.Key] = outRSI[itr.Key];
                    foreach (var ita in indexesToAppend) rsiSeries.Values.Add(outRSI[ita]);
                }

                ChartTotalSteps = Klines.Count;
                ChartVisibleSteps = Klines.Count;

                // Get recommondations
                #region MACD Status

                //for (int macdi = outMACDHist.Length - 1; macdi >= 0; macdi--)
                //{
                //    //TODO: Generate Trends for recommondations    
                //}
                MACDStatus = outMACDHist?.Length > 0 ? (outMACDHist.LastOrDefault() >= 0) ? "Growing" : "Losing" : "";

                #endregion
                #region RSI Status
                var rsiPos = outRSI.LastOrDefault();
                RSIStatus = rsiPos > 30 ? rsiPos >= 70 ? "Over-Bought" : "Typical" : "Over-Sold";
                #endregion
            });
        }

        public void CalculateMACD(bool resize, out double[] outMACDRslt, out double[] outMACDSign, out double[] outMACDHist)
        {
            int fastPeriod = 12;
            int slowPeriod = 26;
            int signalPeriod = 9;

            int macdLookback = Core.MacdLookback(fastPeriod, slowPeriod, signalPeriod);

            double[] close = this.Klines.Select(k => (double)k.Close).ToArray();

            if (close.Length < macdLookback)
            {
                // We don't have enough data to fill these
                // fill these with empties, and get out
                outMACDRslt = Enumerable.Repeat(double.NaN, close.Length).ToArray();
                outMACDSign = Enumerable.Repeat(double.NaN, close.Length).ToArray();
                outMACDHist = Enumerable.Repeat(double.NaN, close.Length).ToArray();
                return;
            }

            outMACDRslt = new double[close.Length - macdLookback];
            outMACDSign = new double[close.Length - macdLookback];
            outMACDHist = new double[close.Length - macdLookback];

            //var retCode = Core.MovingAverage(0, close.Length - 1, close, lookback + 1, Core.MAType.Sma, out int outBegIdx, out int outNbElement, output);
            var macdReturnCode = Core.Macd(macdLookback, close.Length - 1, close, fastPeriod, slowPeriod, signalPeriod, out int outBegIdx, out int outNbElement, outMACDRslt, outMACDSign, outMACDHist);


            //fill in return arrays with NaNs
            if (resize)
            {
                var closeMin = close.Min();
                var closeMax = close.Max();
                var closeRange = closeMax - closeMin;

                var macdRsltMin = outMACDRslt.Min();
                var macdRsltMax = outMACDRslt.Max();
                var macdRsltRng = macdRsltMax - macdRsltMin;


                var macdHistMin = outMACDHist.Min();
                var macdHistMax = outMACDHist.Max();
                var macdHistRng = macdHistMax - macdHistMin;

                outMACDRslt = Enumerable.Repeat(double.NaN, outMACDRslt.Length - close.Length).Concat(outMACDRslt.Select(val => val.ReRangeKnownRange(macdRsltMin, macdRsltRng, closeMin, closeRange))).ToArray();
                outMACDSign = Enumerable.Repeat(double.NaN, outMACDSign.Length - close.Length).Concat(outMACDSign.Select(val => val.ReRangeKnownRange(macdRsltMin, macdRsltRng, closeMin, closeRange))).ToArray();
                outMACDHist = Enumerable.Repeat(double.NaN, outMACDHist.Length - close.Length).Concat(outMACDHist.Select(val => val.ReRangeKnownRange(macdHistMin, macdHistRng, closeMin, closeRange))).ToArray();
            }
            else
            {
                outMACDRslt = Enumerable.Repeat(double.NaN, macdLookback).Concat(outMACDRslt).ToArray();
                outMACDSign = Enumerable.Repeat(double.NaN, macdLookback).Concat(outMACDSign).ToArray();
                outMACDHist = Enumerable.Repeat(double.NaN, macdLookback).Concat(outMACDHist).ToArray();
            }
        }

        public void CalculateRSI(out double[] outRSI)
        {
            int period = 14;

            int rsiLookback = Core.RsiLookback(period);
            //rsiLookback = rsiLookback < 0 ? 0 : rsiLookback;

            double[] close = this.Klines.Select(k => (double)k.Close).ToArray();

            if (close.Length < rsiLookback)
            {
                // We don't have enough data to fill this
                // fill this with empties, and get out
                outRSI = Enumerable.Repeat(double.NaN, close.Length).ToArray();
                return;
            }

            outRSI = new double[close.Length - rsiLookback];
            Core.Rsi(rsiLookback, close.Length - 1, close, period, out int outBegIdx, out int outNbElement, outRSI);

            outRSI = Enumerable.Repeat(double.NaN, close.Length - outRSI.Length).Concat(outRSI).ToArray();
        }


    }
}
