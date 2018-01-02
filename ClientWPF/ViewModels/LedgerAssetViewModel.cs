using Binance.Net.ClientWPF.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Net.ClientWPF.ViewModels
{
    public class LedgerAssetViewModel : ObservableObject
    {
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
                RaisePropertyChangedEvent("Gains");
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


    }
}
