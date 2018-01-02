using Binance.Net.ClientWPF.MVVM;
using Binance.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Net.ClientWPF.ViewModels
{
    public class TradeViewModel : ObservableObject
    {
        #region Symbol
        private string symbol;
        public string Symbol
        {
            get { return symbol; }
            set
            {
                if (symbol == value) return;
                symbol = value;
                RaisePropertyChangedEvent("Symbol");
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
        #region IsBuyer
        private bool _isBuyer;
        public bool IsBuyer
        {
            get { return _isBuyer; }
            set
            {
                if (_isBuyer == value) return;
                _isBuyer = value;
                RaisePropertyChangedEvent("IsBuyer");
            }
        }
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
    }
}
