using Binance.Net.ClientWPF.MVVM;
using Binance.Net.Objects;
using NoobsMuc.Coinmarketcap.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Net.ClientWPF.ViewModels
{
    public class TradeViewModel : ObservableObject
    {
        #region Symbol SymbolAsset SymbolCurrency
        private string[] symbolPair = new string[0];
        protected void EnsureSymbolPairs(bool force = false)
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

        private string symbol;
        public string Symbol
        {
            get { return symbol; }
            set
            {
                if (symbol == value) return;
                symbol = value;
                RaisePropertyChangedEvent("Symbol");

                EnsureSymbolPairs(true);
                RaisePropertyChangedEvent("SymbolAsset");
                RaisePropertyChangedEvent("SymbolCurrency");
            }
        }

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
        #endregion

        #region Id
        private long id;
        public long Id
        {
            get { return id; }
            set
            {
                if (id == value) return;
                id = value;
                RaisePropertyChangedEvent("Id");
            }
        }
        #endregion
        #region Price
        private decimal price;
        public decimal Price
        {
            get { return price; }
            set
            {
                if (price == value) return;
                price = value;
                RaisePropertyChangedEvent("Price");
            }
        }
        #endregion
        #region Quantity
        private decimal quantity;
        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity == value) return;
                quantity = value;
                RaisePropertyChangedEvent("Quantity");
            }
        }
        #endregion
        #region Commission
        private decimal _commission;
        public decimal Commission
        {
            get { return _commission; }
            set
            {
                if (_commission == value) return;
                _commission = value;
                RaisePropertyChangedEvent("Commission");
            }
        }
        #endregion
        #region CommissionAsset
        private string _commissionAsset;
        public string CommissionAsset
        {
            get { return _commissionAsset; }
            set
            {
                if (_commissionAsset == value) return;
                _commissionAsset = value;
                RaisePropertyChangedEvent("CommissionAsset");
            }
        }
        #endregion
        #region Time
        private DateTime time;
        public DateTime Time
        {
            get { return time; }
            set
            {
                if (time == value) return;
                time = value;
                RaisePropertyChangedEvent("Time");
            }
        }
        #endregion
        #region IsBuyer BuyerSeller
        private bool _isBuyer;
        public bool IsBuyer
        {
            get { return _isBuyer; }
            set
            {
                if (_isBuyer == value) return;
                _isBuyer = value;
                RaisePropertyChangedEvent("IsBuyer");
                RaisePropertyChangedEvent("BuyerSeller");
            }
        }
        public string BuySell { get { return IsBuyer ? "Buy" : "Sell"; } }
        #endregion
        #region IsMaker
        private bool _isMaker;
        public bool IsMaker
        {
            get { return _isMaker; }
            set
            {
                if (_isMaker == value) return;
                _isMaker = value;
                RaisePropertyChangedEvent("IsMaker");
            }
        }
        #endregion
        #region IsBestMatch
        private bool _isBestMatch;
        public bool IsBestMatch
        {
            get { return _isBestMatch; }
            set
            {
                if (_isBestMatch == value) return;
                _isBestMatch = value;
                RaisePropertyChangedEvent("IsBestMatch");
            }
        }
        #endregion

        #region Currency
        private BinanceSymbolViewModel _tradeSymbol;
        public BinanceSymbolViewModel TradeSymbol
        {
            get { return _tradeSymbol; }
            set
            {
                if (_tradeSymbol == value) return;
                _tradeSymbol = value;
                RaisePropertyChangedEvent("TradeSymbol");

                CurrencyCurrentValue = _tradeSymbol.Price;
            }
        }
        #endregion
        #region CurrencyCurrentValue
        private decimal _currencyCurrentValue;
        public decimal CurrencyCurrentValue
        {
            get { return _currencyCurrentValue; }
            set
            {
                if (_currencyCurrentValue == value) return;
                _currencyCurrentValue = value;
                RaisePropertyChangedEvent("CurrencyCurrentValue");
                RaisePropertyChangedEvent("PercentChange");
            }
        }
        #endregion
        #region PercentChange
        public string PercentChange
        {
            get { return CurrencyCurrentValue==0?"":$"{(((CurrencyCurrentValue/Price)-1)*100).ToString("#0.00")}%"; }
        }
        #endregion

        //public TradeViewModel(BinanceStreamTrade data)
        //{
        //    Symbol = data.Symbol;
        //    Price = data.Price;
        //    Quantity = data.Quantity;
        //    //Commission = ;
        //    LastTradeID = data.LastTradeId;
        //    Time = data.TradeTime;
        //    BuyerIsMaker = data.BuyerIsMaker;
        //    //IsBestMatch = data.
        //}

        public TradeViewModel(string symbol, BinanceTrade trade)
        {
            Symbol = symbol;
            Price = trade.Price;
            Quantity = trade.Quantity;
            Commission = trade.Commission;
            CommissionAsset = trade.CommissionAsset;
            Time = trade.Time;
            IsBuyer = trade.IsBuyer;
            IsMaker = trade.IsMaker;
            IsBestMatch = trade.IsBestMatch;
        }
    }
}
