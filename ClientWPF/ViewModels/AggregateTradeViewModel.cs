using Binance.Net.ClientWPF.MVVM;
using Binance.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Net.ClientWPF.ViewModels
{
    public class AggregateTradeViewModel : ObservableObject
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
        #region AggregateTradeID
        private long aggregateTradeID;
        public long AggregateTradeID
        {
            get { return aggregateTradeID; }
            set
            {
                if (aggregateTradeID == value) return;
                aggregateTradeID = value;
                RaisePropertyChangedEvent("AggregateTradeID");
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
        #region FirstTradeID
        private long _firstTradeID;
        public long FirstTradeID
        {
            get { return _firstTradeID; }
            set
            {
                if (_firstTradeID == value) return;
                _firstTradeID = value;
                RaisePropertyChangedEvent("FirstTradeID");
            }
        }
        #endregion
        #region LastTradeID
        private long _lastTradeID;
        public long LastTradeID
        {
            get { return _lastTradeID; }
            set
            {
                if (_lastTradeID == value) return;
                _lastTradeID = value;
                RaisePropertyChangedEvent("LastTradeID");
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
        #region BuyerIsMaker
        private bool _buyerIsMaker;
        public bool BuyerIsMaker
        {
            get { return _buyerIsMaker; }
            set
            {
                if (_buyerIsMaker == value) return;
                _buyerIsMaker = value;
                RaisePropertyChangedEvent("BuyerIsMaker");
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

        public AggregateTradeViewModel() { }

        public AggregateTradeViewModel(BinanceStreamTrade data)
        {
            Symbol = data.Symbol;
            Price = data.Price;
            Quantity = data.Quantity;
            FirstTradeID = data.FirstTradeId;
            LastTradeID = data.LastTradeId;
            Time = data.TradeTime;
            BuyerIsMaker = data.BuyerIsMaker;
            //IsBestMatch = data.
        }
        public AggregateTradeViewModel(string symbol, BinanceAggregatedTrades data)
        {
            Symbol = symbol;
            Price = data.Price;
            Quantity = data.Quantity;
            FirstTradeID = data.FirstTradeId;
            LastTradeID = data.LastTradeId;
            Time = data.Timestamp;
            BuyerIsMaker = data.BuyerWasMaker;
            IsBestMatch = data.WasBestPriceMatch;
        }
    }
}
