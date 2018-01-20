using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Binance.Net.Objects;
using Binance.Net.ClientWPF.MVVM;
using Binance.Net.ClientWPF.ViewModels;
using Binance.Net.ClientWPF.UserControls;
using Binance.Net.ClientWPF.MessageBox;
using System.IO;

using TicTacTec.TA.Library;
using System;
using System.Threading;

namespace Binance.Net.ClientWPF
{
    public class MainViewModel : ObservableObject
    {
        public static List<MainViewModel> MainViewModels { get; private set; } = new List<MainViewModel>();


        public Func<double, string> NumberFormatter { get { return (value) => value.ToString("#0.##########"); } }
        public Func<DateTime, string> DateHoursFormatter { get { return (value) => value.UTCToLocal().ToString("MMM d, h:00"); } }

        public List<KlineInterval> KlineIntervals = Enum.GetValues(typeof(KlineInterval)).Cast<KlineInterval>().ToList();


        private ObservableCollection<BinanceSymbolViewModel> allPrices;
        public ObservableCollection<BinanceSymbolViewModel> AllPrices
        {
            get { return allPrices; }
            set
            {
                allPrices = value;
                RaisePropertyChangedEvent("AllPrices");
            }
        }

        private BinanceSymbolViewModel selectedSymbol;
        public BinanceSymbolViewModel SelectedSymbol
        {
            get { return selectedSymbol; }
            set
            {
                selectedSymbol = value;
                RaisePropertyChangedEvent("SymbolIsSelected");
                RaisePropertyChangedEvent("SelectedSymbol");

                _selectedSymbolAsset = selectedSymbol?.SymbolAsset;
                _selectedSymbolCurrency = selectedSymbol?.SymbolCurrency;
                RaisePropertyChangedEvent("SelectedSymbolAsset");
                RaisePropertyChangedEvent("SelectedSymbolCurrency");

                RaisePropertyChangedEvent("SelectedSymbolAssetOptions");
                RaisePropertyChangedEvent("SelectedSymbolCurrencyOptions");

                ChangeSymbol();
            }
        }

        #region SelectedSymbolAsset
        private string _selectedSymbolAsset;
        public string SelectedSymbolAsset
        {
            get { return _selectedSymbolAsset; }
            set
            {
                if (_selectedSymbolAsset == value) return;
                _selectedSymbolAsset = value;

                SelectedSymbol = AllPrices.Where(price => price.SymbolAsset == SelectedSymbolAsset && price.SymbolCurrency == SelectedSymbolCurrency).FirstOrDefault();

                //RaisePropertyChangedEvent("SelectedSymbolAsset");
            }
        }
        #endregion
        #region SelectedSymbolCurrency
        private string _selectedSymbolCurrency;
        public string SelectedSymbolCurrency
        {
            get { return _selectedSymbolCurrency; }
            set
            {
                if (_selectedSymbolCurrency == value) return;
                _selectedSymbolCurrency = value;

                SelectedSymbol = AllPrices.Where(price => price.SymbolAsset == SelectedSymbolAsset && price.SymbolCurrency == SelectedSymbolCurrency).FirstOrDefault();

                //RaisePropertyChangedEvent("SelectedSymbolCurrency");
            }
        }
        #endregion

        public ObservableCollection<string> SelectedSymbolAssetOptions { get { return SelectedSymbolCurrency == null ? null : CurrencyDictionary?.GetValueOrDefault(SelectedSymbolCurrency); } }
        public ObservableCollection<string> SelectedSymbolCurrencyOptions { get { return SelectedSymbolAsset == null ? null : AssetDictionary?.GetValueOrDefault(SelectedSymbolAsset); } }

        public Dictionary<string, ObservableCollection<string>> CurrencyDictionary { get; protected set; } = new Dictionary<string, ObservableCollection<string>>();
        public Dictionary<string, ObservableCollection<string>> AssetDictionary { get; protected set; } = new Dictionary<string, ObservableCollection<string>>();

        public bool SymbolIsSelected
        {
            get { return SelectedSymbol != null; }
        }

        #region SelectedLedgerAsset
        protected LedgerAssetViewModel _selectedLedgerAsset;
        public LedgerAssetViewModel SelectedLedgerAsset
        {
            get { return _selectedLedgerAsset; }
            set
            {
                _selectedLedgerAsset = value;
                RaisePropertyChangedEvent("SelectedLedgerAsset");

                if (value != null)
                {
                    if (SelectedSymbolAssetOptions != null && SelectedSymbolAssetOptions.Contains(value.Asset))
                        SelectedSymbolAsset = value.Asset;
                    else if (value.Asset == "BTC" && (SelectedSymbolCurrency == "BTC" || string.IsNullOrWhiteSpace(SelectedSymbolCurrency)))
                        SelectedSymbol = AllPrices?.Where(symbol => symbol.Symbol == $"BTCUSDT").FirstOrDefault();
                    else
                        SelectedSymbol = AllPrices?.Where(symbol => symbol.Symbol == $"{value.Asset}BTC").FirstOrDefault();
                }
            }
        }
        #endregion

        #region SelectedLedgerTrade
        private TradeViewModel _selectedLedgerTrade;
        public TradeViewModel SelectedLedgerTrade
        {
            get { return _selectedLedgerTrade; }
            set
            {
                if (_selectedLedgerTrade != value)
                {
                    _selectedLedgerTrade = value;
                    RaisePropertyChangedEvent("SelectedLedgerTrade");
                }

                if (value != null && SelectedSymbolCurrency != value.SymbolCurrency)
                {
                    SelectedSymbolCurrency = value.SymbolCurrency;
                }
            }
        }
        #endregion

        private ObservableCollection<AssetViewModel> assets;
        public ObservableCollection<AssetViewModel> Assets
        {
            get { return assets; }
            set
            {
                assets = value;
                RaisePropertyChangedEvent("Assets");
            }
        }

        #region Ledger
        protected ObservableCollection<LedgerAssetViewModel> _ledger;
        public ObservableCollection<LedgerAssetViewModel> Ledger
        {
            get { return _ledger; }
            set
            {
                if (_ledger == value) return;
                _ledger = value;
                RaisePropertyChangedEvent("Ledger");
            }
        }
        #endregion

        private bool settingsOpen = true;
        public bool SettingsOpen
        {
            get { return settingsOpen; }
            set
            {
                settingsOpen = value;
                RaisePropertyChangedEvent("SettingsOpen");
            }
        }

        private string apiKey;
        public string ApiKey
        {
            get { return apiKey; }
            set
            {
                apiKey = value;
                RaisePropertyChangedEvent("ApiKey");

                if (value != null && apiSecret != null)
                    BinanceDefaults.SetDefaultApiCredentials(value, apiSecret);
            }
        }

        private string apiSecret;
        public string ApiSecret
        {
            get { return apiSecret; }
            set
            {
                apiSecret = value;
                RaisePropertyChangedEvent("ApiSecret");

                if (value != null && apiKey != null)
                    BinanceDefaults.SetDefaultApiCredentials(apiKey, value);
            }
        }

        public ICommand BuyCommand { get; set; }
        public ICommand SellCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ICommand SettingsCommand { get; set; }
        public ICommand CloseSettingsCommand { get; set; }

        private IMessageBoxService messageBoxService;
        private SettingsWindow settings;
        private object orderLock;
        private object tradeLock;
        private BinanceSocketClient binanceSocketClient;


        Dictionary<string, string> mainConfig;
        Storage storage;

        public MainViewModel()
        {
            MainViewModel.MainViewModels.Add(this);


            BinanceDefaults.SetDefaultLogVerbosity(Logging.LogVerbosity.Warning);
            TextWriter tw = new StreamWriter("log.txt"); // This is not threadsafe!
            BinanceDefaults.SetDefaultLogOutput(tw);

            // DB
            storage = new Storage();

            // Load key and secret
            string configLocation = "config.ini";
            if (File.Exists(configLocation))
            {
                mainConfig = File.ReadAllLines(configLocation).ToDictionary(line => line.Split('=')[0], line => line.Split('=')[1].Trim());
                var apiKey = mainConfig.GetValue("APIKey");
                var apiSecret = mainConfig.GetValue("APISecret");
                if (!string.IsNullOrWhiteSpace(apiKey))
                    ApiKey = apiKey;
                if (!string.IsNullOrWhiteSpace(apiSecret))
                    ApiSecret = apiSecret;
            }
            else
            {
                mainConfig = new Dictionary<string, string>();
                mainConfig.Add("APIKey", "");
                mainConfig.Add("APISecret", "");
                File.WriteAllLines(configLocation, mainConfig.Select(kvp => $"{kvp.Key}={kvp.Value}").ToList());
            }

            // Should be done with DI
            messageBoxService = new MessageBoxService();
            orderLock = new object();

            BuyCommand = new DelegateCommand(Buy);
            SellCommand = new DelegateCommand(Sell);
            CancelCommand = new DelegateCommand(Cancel);
            SettingsCommand = new DelegateCommand(Settings);
            CloseSettingsCommand = new DelegateCommand(CloseSettings);


            binanceSocketClient = new BinanceSocketClient();

            var allSymbolsTask = Task.Run(() => GetAllSymbols());

            SubscribeUserStream();

            allSymbolsTask.Wait();


            Task.Run(() =>
            {
                // Doing this voilates the ratelimit
                //GetAllHistory("BTC");
                //GetAllHistory("ETH");
                //GetAllHistory("USDT");
                //GetAllHistory("BNB");
            });

        }

        public void Cancel(object o)
        {
            var order = (OrderViewModel)o;
            Task.Run(() =>
            {
                using (var client = new BinanceClient())
                {
                    var result = client.CancelOrder(SelectedSymbol.Symbol, order.Id);
                    if (result.Success)
                        messageBoxService.ShowMessage("Order canceled!", "Sucess", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    else
                        messageBoxService.ShowMessage($"Order canceling failed: {result.Error.Message}", "Failed", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            });
        }

        public void Buy(object o)
        {
            Task.Run(() =>
            {
                using (var client = new BinanceClient())
                {
                    var result = client.PlaceOrder(SelectedSymbol.Symbol, OrderSide.Buy, OrderType.Limit, SelectedSymbol.TradeAmount, price: SelectedSymbol.TradePrice, timeInForce: TimeInForce.GoodTillCancel);
                    if (result.Success)
                        messageBoxService.ShowMessage("Order placed!", "Sucess", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    else
                        messageBoxService.ShowMessage($"Order placing failed: {result.Error.Message}", "Failed", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            });
        }

        public void Sell(object o)
        {
            Task.Run(() =>
            {
                using (var client = new BinanceClient())
                {
                    var result = client.PlaceOrder(SelectedSymbol.Symbol, OrderSide.Sell, OrderType.Limit, SelectedSymbol.TradeAmount, price: SelectedSymbol.TradePrice, timeInForce: TimeInForce.GoodTillCancel);
                    if (result.Success)
                        messageBoxService.ShowMessage("Order placed!", "Sucess", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    else
                        messageBoxService.ShowMessage($"Order placing failed: {result.Error.Message}", "Failed", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            });
        }

        private void Settings(object o)
        {
            settings = new SettingsWindow(this);
            settings.ShowDialog();
        }

        private void CloseSettings(object o)
        {
            settings?.Close();
            settings = null;

            SubscribeUserStream();
        }

        private async Task GetAllSymbols()
        {
            using (var client = new BinanceClient())
            {
                var result = await client.GetAllPricesAsync();
                if (result.Success)
                {
                    AllPrices = new ObservableCollection<BinanceSymbolViewModel>(result.Data.Select(r => new BinanceSymbolViewModel(r.Symbol, r.Price)).OrderBy(s => s.SymbolCurrency).ThenBy(s => s.SymbolAsset).ToList());

                    foreach (var price in AllPrices)
                    {
                        if (CurrencyDictionary.ContainsKey(price.SymbolCurrency))
                        {
                            if (!CurrencyDictionary[price.SymbolCurrency].Contains(price.SymbolAsset))
                                CurrencyDictionary[price.SymbolCurrency].Add(price.SymbolAsset);
                        }
                        else CurrencyDictionary.Add(price.SymbolCurrency, new ObservableCollection<string> { price.SymbolAsset });

                        if (AssetDictionary.ContainsKey(price.SymbolAsset))
                        {
                            if (!AssetDictionary[price.SymbolAsset].Contains(price.SymbolCurrency))
                                AssetDictionary[price.SymbolAsset].Add(price.SymbolCurrency);
                        }
                        else AssetDictionary.Add(price.SymbolAsset, new ObservableCollection<string> { price.SymbolCurrency });
                    }

                    var cdKeys = CurrencyDictionary.Keys.ToList();
                    var adKeys = AssetDictionary.Keys.ToList();
                    for (int k = 0; k < cdKeys.Count; k++) CurrencyDictionary[cdKeys[k]] = new ObservableCollection<string>(CurrencyDictionary[cdKeys[k]].OrderBy(v => v));
                    for (int k = 0; k < adKeys.Count; k++) AssetDictionary[adKeys[k]] = new ObservableCollection<string>(AssetDictionary[adKeys[k]].OrderBy(v => v));
                }
                else
                    messageBoxService.ShowMessage($"Error getting all symbols data.\n{result.Error.Message}", $"Error Code: {result.Error.Code}", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            List<Task> tasks = new List<Task>();
            foreach (var symbol in AllPrices)
            {
                var task = new Task(() => binanceSocketClient.SubscribeToKlineStream(symbol.Symbol.ToLower(), KlineInterval.OneMinute, (data) =>
                {
                    symbol.Price = data.Data.Close;
                }));
                tasks.Add(task);
                task.Start();
            }
            Task.WaitAll(tasks.ToArray());
        }



        private void Get24HourStats()
        {
            if (SelectedSymbol is null)
                return;
            var symbol = SelectedSymbol;

            using (var client = new BinanceClient())
            {
                var result = client.Get24HPrice(symbol.Symbol);
                if (result.Success)
                {
                    symbol.HighPrice = result.Data.HighPrice;
                    symbol.LowPrice = result.Data.LowPrice;
                    symbol.Volume = result.Data.Volume;
                    symbol.PriceChangePercent = result.Data.PriceChangePercent;
                }
                else
                    messageBoxService.ShowMessage($"Error getting 24 hour stats.\n{result.Error.Message}", $"Error Code: {result.Error.Code}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetOrders(BinanceSymbolViewModel symbol = null)
        {
            if (symbol == null) symbol = SelectedSymbol;
            if (symbol == null) return;

            using (var client = new BinanceClient())
            {
                var result = client.GetAllOrders(symbol.Symbol);
                if (result.Success)
                {
                    symbol.AddOrders(result.Data.OrderByDescending(d => d.Time).Select(o => new OrderViewModel()
                    {
                        Id = o.OrderId,
                        ExecutedQuantity = o.ExecutedQuantity,
                        OriginalQuantity = o.OriginalQuantity,
                        Price = o.Price,
                        Side = o.Side,
                        Status = o.Status,
                        Symbol = o.Symbol,
                        Time = o.Time,
                        Type = o.Type
                    }));
                }
                else
                    messageBoxService.ShowMessage($"Error getting orders.\n{result.Error.Message}", $"Error Code: {result.Error.Code}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetTrades(BinanceSymbolViewModel symbol = null)
        {
            if (symbol == null)
                symbol = SelectedSymbol;

            using (var client = new BinanceClient())
            {
                //var result = client.GetMyTrades(Global.GetBinanceSymbolName(symbol.Symbol)); // Not sure if this will fix IOTA or not
                var result = client.GetMyTrades(symbol.Symbol);
                if (result.Success)
                {
                    //SelectedSymbol.AggregateTrades = new ObservableCollection<AggregateTradeViewModel>(result.Data.OrderByDescending(d => d.Timestamp).Select(t => new AggregateTradeViewModel(symbol, t)));
                    symbol.Trades = new ObservableCollection<TradeViewModel>(result.Data.OrderByDescending(d => d.Time).Select(t => new TradeViewModel(symbol.Symbol, t) { TradeSymbol = AllPrices.Where(price => price.Symbol == symbol.Symbol).FirstOrDefault() }));

                    var ledgerAsset = Ledger.Where(ledge => ledge.Asset == symbol.SymbolAsset).FirstOrDefault();
                    if (ledgerAsset != null)
                    {
                        ledgerAsset.AddTrades(symbol.Trades);
                    }
                }
                else
                    messageBoxService.ShowMessage($"Error getting trade data for [{symbol.Symbol}]: {result.Error.Message}", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SubscribeUserStream()
        {
            if (string.IsNullOrWhiteSpace(ApiKey) || string.IsNullOrWhiteSpace(ApiSecret))
                return;

            Task.Run(() =>
            {
                using (var client = new BinanceClient())
                {
                    var startOkay = client.StartUserStream();
                    if (!startOkay.Success)
                        messageBoxService.ShowMessage($"Error Starting UserStream.\n{startOkay.Error.Message}", $"Error {startOkay.Error.Code}", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        binanceSocketClient.SubscribeToAccountUpdateStream(startOkay.Data.ListenKey, OnAccountUpdate);
                        binanceSocketClient.SubscribeToOrderUpdateStream(startOkay.Data.ListenKey, OnOrderUpdate);
                        binanceSocketClient.SubscribeToTradesStream(startOkay.Data.ListenKey, OnTradesUpdate);
                    }

                    var accountResult = client.GetAccountInfo();
                    if (accountResult.Success)
                    {
                        Assets = new ObservableCollection<AssetViewModel>(accountResult.Data.Balances.Where(b => b.Free != 0 || b.Locked != 0).Select(b => new AssetViewModel() { Asset = b.Asset, Free = b.Free, Locked = b.Locked }).ToList());
                        Ledger = new ObservableCollection<LedgerAssetViewModel>(accountResult.Data.Balances.Where(b => b.Free != 0 || b.Locked != 0 || b.Asset =="MTH").Select(b => new LedgerAssetViewModel() { Asset = b.Asset, Amount = b.Total }).OrderByDescending(ledge => ledge.ValueUSD).ToList());

                        GetLedgerHistory();
                        GetLedgerTrades();
                    }
                    else
                    {
                        messageBoxService.ShowMessage($"Error Getting AccountInfo.\n{accountResult.Error.Message}", $"Error {accountResult.Error.Code}", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            });
        }

        private void ChangeSymbol()
        {
            if (SelectedSymbol != null)
            {
                selectedSymbol.TradeAmount = 0;
                selectedSymbol.TradePrice = selectedSymbol.Price;
                Task.Run(() =>
                {
                    var symbol = SelectedSymbol; // use this to check if we should cancel because the symbol has changed

                    //Thread.Sleep(100); // Incase the user is blowing through items // doesn't seem to help

                    // check again, because this is async, symbol might be null now
                    if (symbol != null || symbol != SelectedSymbol)
                    {
                        if (!String.IsNullOrEmpty(ApiKey) && !String.IsNullOrEmpty(ApiSecret))
                            GetOrders();

                        if (symbol != SelectedSymbol) return;
                        //GetTrades();
                        if (symbol != SelectedSymbol) return;
                        Get24HourStats();
                        if (symbol != SelectedSymbol) return;
                        symbol.GetHistory(storage);
                    }
                });
            }

        }

        private void OnAccountUpdate(BinanceStreamAccountInfo data)
        {
            Assets = new ObservableCollection<AssetViewModel>(data.Balances.Where(b => b.Free != 0 || b.Locked != 0).Select(b => new AssetViewModel() { Asset = b.Asset, Free = b.Free, Locked = b.Locked }).ToList());

            // Update Ledger
            foreach (var balance in data.Balances)
            {
                var ledge = Ledger.Where(l => balance.Asset == l.Asset).FirstOrDefault();

                // Only worth tracking if we have a balance to monitor
                if (balance.Free != 0 || balance.Locked != 0)
                {
                    if (ledge == null) Ledger.Add(new LedgerAssetViewModel() { Asset = balance.Asset, Amount = balance.Total });
                    else ledge.Amount = balance.Total;
                }
                else if (ledge != null)
                    Ledger.Remove(ledge); // needs to be dispatched
            }
            //GetLedgerTrades();
        }

        private void GetLedgerTrades()
        {
            if (Ledger == null) return;

            new Task(() => Ledger.AsParallel().ForAll(ledge => AllPrices.Where(price => price.SymbolAsset == ledge.Asset).AsParallel().ForAll(pair => GetTrades(pair)))).Start();
        }

        private void GetLedgerHistory()
        {
            if (Ledger == null) return;

            //new Task(() => Ledger.AsParallel().ForAll(ledge =>
            //{
            //    var pair = AllPrices.Where(price => price.SymbolAsset == ledge.Asset && price.SymbolCurrency == "BTC").FirstOrDefault();
            //    if (pair != null)
            //    {
            //        ledge.MACDBTC = "Aquiring";
            //        ledge.RSIBTC = "Aquiring";
            //        pair.GetHistory(storage);
            //        ledge.MACDBTC = pair.MACDStatus;
            //        ledge.RSIBTC = pair.RSIStatus;
            //    }
            //})).Start();

            //Takes a lot longer to load when in parallel
            foreach(var ledge in Ledger)
            {
                var pair = AllPrices.Where(price => price.SymbolAsset == ledge.Asset && price.SymbolCurrency == "BTC").FirstOrDefault();
                if (pair == null) continue;
                pair.GetHistory(storage);
                ledge.MACDBTC = pair.MACDStatus;
                ledge.RSIBTC = pair.RSIStatus;
            }
        }

        private void GetAllHistory(string currency = null)
        {
            var t = new Task(() => AllPrices.Where(p => string.IsNullOrWhiteSpace(currency) || p.SymbolCurrency == currency).AsParallel().ForAll(pair => pair.GetHistory(storage)));
            t.Start();
            t.Wait();
        }


        private void OnOrderUpdate(BinanceStreamOrderUpdate data)
        {
            var symbol = AllPrices.SingleOrDefault(a => a.Symbol == data.Symbol);
            if (symbol == null)
                return;

            lock (orderLock)
            {
                var trade = symbol?.Orders?.SingleOrDefault(o => o.Id == data.OrderId);
                if (trade == null)
                {
                    if (data.RejectReason != OrderRejectReason.None || data.ExecutionType != ExecutionType.New)
                        // Order got rejected, no need to show
                        return;

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        symbol.AddOrder(new OrderViewModel()
                        {
                            ExecutedQuantity = data.AccumulatedQuantityOfFilledTrades,
                            Id = data.OrderId,
                            OriginalQuantity = data.Quantity,
                            Price = data.Price,
                            Side = data.Side,
                            Status = data.Status,
                            Symbol = data.Symbol,
                            Time = data.Time,
                            Type = data.Type
                        });
                    });
                }
                else
                {
                    trade.ExecutedQuantity = data.AccumulatedQuantityOfFilledTrades;
                    trade.Status = data.Status;
                }
            }
        }

        private void OnTradesUpdate(BinanceStreamTrade data)
        {
            //var symbol = AllPrices.SingleOrDefault(a => a.Symbol == data.Symbol);
            //if (symbol == null) return;
            //
            //lock (tradeLock)
            //{
            //    var trade = symbol?.AggregateTrades?.SingleOrDefault(t => t.AggregateTradeID == data.AggregatedTradeId);
            //    if (trade == null)
            //    {
            //        Application.Current.Dispatcher.Invoke(() =>
            //        {
            //            symbol.AddAggregateTrade(new AggregateTradeViewModel(data));
            //        });
            //    }
            //    else
            //    {
            //        // Possibly Update Trade if there is new info
            //    }
            //}
        }
    }
}
