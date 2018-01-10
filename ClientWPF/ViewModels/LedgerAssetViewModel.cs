using Binance.Net.ClientWPF.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Net.ClientWPF.ViewModels
{
    class LedgerTradeValues
    {
        public decimal HeldAmount { get; set; } = 0m;
        public decimal CurrencyAmount { get; set; } = 0m;
    }

    public class LedgerAssetViewModel : ObservableObject
    {
        #region Constructors
        public LedgerAssetViewModel()
        {
            trades = new ObservableCollection<TradeViewModel>();
        }
        #endregion

        #region Trades
        private ObservableCollection<TradeViewModel> trades;
        public ObservableCollection<TradeViewModel> Trades
        {
            get { return trades; }
            protected set
            {
                trades = value;
                RaisePropertyChangedEvent("Trades");
            }
        }
        #endregion

        #region Asset
        protected string _asset;
        public string Asset
        {
            get { return _asset; }
            set
            {
                if (_asset == value) return;
                _asset = value;
                RaisePropertyChangedEvent("AssetSymbol");
            }
        }
        #endregion
        #region Amount
        protected decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                if (_amount == value) return;
                _amount = value;
                RaisePropertyChangedEvent("Amount");
            }
        }
        #endregion
        #region CoinValueUSD
        public decimal? CoinValueUSD
        {
            get { return CoinMarket.GetCurrency(Asset)?.PriceUsd?.ParseToNullableDecimal(); }
        }
        #endregion
        #region CoinValueBTC
        public decimal? CoinValueBTC
        {
            get { return CoinMarket.GetCurrency(Asset)?.PriceBtc?.ParseToNullableDecimal(); }
        }
        #endregion
        #region ValueUSD
        public decimal? ValueUSD
        {
            get { return Amount * CoinValueUSD; }
        }
        #endregion
        #region ValueBTC
        public decimal? ValueBTC
        {
            get { return Amount * CoinValueBTC; }
        }
        #endregion
        #region CostUSD
        protected decimal _costUSD;
        public decimal CostUSD
        {
            get { return _costUSD; }
            set
            {
                if (_costUSD == value) return;
                _costUSD = value;
                RaisePropertyChangedEvent("CostUSD");
            }
        }
        #endregion
        #region CostBTC
        protected decimal _costBTC;
        public decimal CostBTC
        {
            get { return _costBTC; }
            set
            {
                if (_costBTC == value) return;
                _costBTC = value;
                RaisePropertyChangedEvent("CostBTC");
            }
        }
        #endregion
        #region GainsPercentage
        protected decimal _gainsPercentage;
        public decimal GainsPercentage
        {
            get { return _gainsPercentage; }
            set
            {
                if (_gainsPercentage == value) return;
                _gainsPercentage = value;
                RaisePropertyChangedEvent("GainsPercentage");
                RaisePropertyChangedEvent("GainsPercentageText");
            }
        }
        public string GainsPercentageText { get { return GainsPercentage.ToString("0.##") + "%"; } }
        #endregion
        #region GainsUSD
        private decimal _gainsUSD;
        public decimal GainsUSD
        {
            get { return _gainsUSD; }
            set
            {
                if (_gainsUSD == value) return;
                _gainsUSD = value;
                RaisePropertyChangedEvent("GainsUSD");
            }
        }
        #endregion
        #region MACDBTC
        protected string _MACDBTC;
        public string MACDBTC
        {
            get { return _MACDBTC; }
            set
            {
                if (_MACDBTC == value) return;
                _MACDBTC = value;
                RaisePropertyChangedEvent("MACDBTC");
            }
        }
        #endregion
        #region RSIBTC
        protected string _RSIBTC;
        public string RSIBTC
        {
            get { return _RSIBTC; }
            set
            {
                if (_RSIBTC == value) return;
                _RSIBTC = value;
                RaisePropertyChangedEvent("RSIBTC");
            }
        }
        #endregion
        #region MACDETH
        protected string _MACDETH;
        public string MACDETH
        {
            get { return _MACDETH; }
            set
            {
                if (_MACDETH == value) return;
                _MACDETH = value;
                RaisePropertyChangedEvent("MACDETH");
            }
        }
        #endregion
        #region RSIETH
        protected string _RSIETH;
        public string RSIETH
        {
            get { return _RSIETH; }
            set
            {
                if (_RSIETH == value) return;
                _RSIETH = value;
                RaisePropertyChangedEvent("RSIETH");
            }
        }
        #endregion

        public void AddTrades(IEnumerable<TradeViewModel> trades)
        {
            var tempTrades = Trades.ToList();
            foreach (var trade in trades)
            {
                var existingTrade = tempTrades.Where(t => t.Symbol == trade.Symbol && t.Time == trade.Time).FirstOrDefault();
                if (existingTrade == null)
                {
                    tempTrades.Add(trade);
                }
                else
                {
                    tempTrades.ReplaceInPlace(existingTrade, trade);
                }
            }
            Trades = new ObservableCollection<TradeViewModel>(tempTrades.OrderByDescending(trade => trade.Time));
            CalculateGainsPercentage();
        }

        public void CalculateGainsPercentage()
        {
            if (Asset == "BTC") return;


            
            //// This need to be modified to get coinmarkets BTC or USD value at time, and convert everything into that value trade
            // foreach trade, from first to last:

            //var tradeValues = new Dictionary<string, LedgerTradeValues>();
            //foreach (var trade in Trades.OrderByDescending(t => t.Time))
            //{
            //    if (!tradeValues.ContainsKey(trade.SymbolCurrency))
            //        tradeValues.Add(trade.SymbolCurrency, new LedgerTradeValues());
            //
            //    var tradeValue = tradeValues[trade.SymbolCurrency];
            //
            //    if (trade.IsBuyer == false && tradeValue.HeldAmount == 0)
            //        continue;
            //
            //    if(trade.IsBuyer)
            //    {
            //        tradeValue.HeldAmount += trade.Quantity;
            //        tradeValue.CurrencyAmount -= trade.Price * trade.Quantity;
            //    }
            //    else
            //    {
            //        var maxAmount = tradeValue.HeldAmount - trade.Quantity < 0 ? tradeValue.HeldAmount : trade.Quantity;
            //        tradeValue.HeldAmount -= maxAmount;
            //        tradeValue.CurrencyAmount += trade.Price * maxAmount;
            //    }
            //}
            //
            //decimal USDGainedLost = 0;
            //foreach (var tradeHistory in tradeValues)
            //{
            //    // tradeHistory.Value.HeldAmount = how much is currently invested
            //    // tradeHistory.Value.CurrencyAmount = how much of this currency we gained/lost investing
            //    // convert HeldAmount into current value and add to currency amount, this is how much we gained, convert to USD to see gains/losses
            //
            //    var price = MainViewModel.MainViewModels.FirstOrDefault()?.AllPrices?.Where(p => p.SymbolAsset == Asset && p.SymbolCurrency == tradeHistory.Key).FirstOrDefault();
            //    if (price == null) return;
            //    var coinmarketCurrencyPrice = CoinMarket.GetCurrency(tradeHistory.Key);
            //    if (coinmarketCurrencyPrice == null) return;
            //
            //    decimal capital = 0;
            //    capital += tradeHistory.Value.HeldAmount * price.Price;
            //    capital += tradeHistory.Value.CurrencyAmount;
            //
            //    decimal capitalUSD = decimal.Parse(coinmarketCurrencyPrice.PriceUsd) * capital;
            //    USDGainedLost += capitalUSD;
            //}
            //
            //GainsUSD = USDGainedLost;





            //decimal heldAmount = Amount;
            //decimal heldAsBTC = CoinValueBTC.Value;
            //foreach (var trade in Trades.OrderByDescending(t => t.Time))
            //{
            //    //How much of this trade comprises our current held value.
            //    if (trade.IsBuyer)
            //    {
            //        heldAmount -= trade.Quantity;
            //
            //        if (trade.SymbolCurrency == "BTC")
            //        {
            //            heldAsBTC -= trade.Price * trade.Quantity;
            //        }
            //        else
            //        {
            //            var mvm = MainViewModel.MainViewModels.FirstOrDefault();
            //            var conversionPrice = mvm.AllPrices.Where(price => price.SymbolAsset == trade.SymbolAsset && price.SymbolCurrency == "BTC").SingleOrDefault();
            //            // Held BTC -= this trades rate converted to BTC * quantity
            //            //             this gets us the BTC value of that trade as it equates to now
            //            heldAsBTC -= trade.Price / conversionPrice.Price * trade.Quantity;
            //
            //        }
            //    }
            //    else
            //    {
            //        heldAmount += trade.Quantity;
            //        if (trade.SymbolCurrency == "BTC")
            //        {
            //            heldAsBTC += trade.Price * trade.Quantity;
            //        }
            //        else
            //        {
            //            var mvm = MainViewModel.MainViewModels.FirstOrDefault();
            //            var conversionPrice = mvm.AllPrices.Where(price => price.SymbolAsset == trade.SymbolAsset && price.SymbolCurrency == "BTC").SingleOrDefault();
            //            // Held BTC += this trades rate converted to BTC * quantity
            //            //             this gets us the BTC value of that trade as it equates to now
            //            heldAsBTC += trade.Price / conversionPrice.Price * trade.Quantity;
            //        }
            //    }
            //}
            //
            //// We now have the orginal value of the BTC we started with
            //// convert this currency to BTC and see percent diff
            //
            //GainsPercentage = ((heldAsBTC / ValueBTC.Value) - 1) * 100;
        }
    }
}
