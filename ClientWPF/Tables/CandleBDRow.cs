using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Objects;

namespace Binance.Net.ClientWPF
{
    [DBTable("Candle")]
    public class CandleDBRow : StorageTableBase<CandleDBRow>
    {
        [DBProperty(primaryKeyLevel: 0)] public int Exchange { get; set; }
        [DBProperty(primaryKeyLevel: 0)] public string Asset { get; set; }
        [DBProperty(primaryKeyLevel: 0)] public string Currency { get; set; }
        [DBProperty(primaryKeyLevel: 0)] public KlineInterval Interval { get; set; }

        /// <summary> The time this candlestick opened </summary>
        [DBProperty(primaryKeyLevel: 0)] public DateTime OpenTime { get; set; }
        /// <summary> The price at which this candlestick opened </summary>
        [DBProperty] public decimal Open { get; set; }
        /// <summary> The highest price in this candlestick </summary>
        [DBProperty] public decimal High { get; set; }
        /// <summary> The lowest price in this candlestick </summary>
        [DBProperty] public decimal Low { get; set; }
        /// <summary> The price at which this candlestick closed </summary>
        [DBProperty] public decimal Close { get; set; }
        /// <summary> The volume traded during this candlestick </summary>
        [DBProperty] public decimal Volume { get; set; }
        /// <summary> The close time of this candlestick </summary>
        [DBProperty] public DateTime CloseTime { get; set; }
        /// <summary> The volume traded during this candlestick in the asset form </summary>
        [DBProperty] public decimal AssetVolume { get; set; }
        /// <summary> The amount of trades in this candlestick </summary>
        [DBProperty] public int Trades { get; set; }
        /// <summary> Taker buy base asset volume </summary>
        [DBProperty] public decimal TakerBuyBaseAssetVolume { get; set; }
        /// <summary> Taker buy quote asset volume </summary>
        [DBProperty] public decimal TakerBuyQuoteAssetVolume { get; set; }

        public CandleDBRow() { }
        public CandleDBRow(BinanceKline klineCandle, string asset, string currency, KlineInterval interval)
        {
            Exchange = 1; // Binance
            Asset = asset;
            Currency = currency;
            Interval = interval;
            OpenTime = klineCandle.OpenTime;
            Open = klineCandle.Open;
            High = klineCandle.High;
            Low = klineCandle.Low;
            Close = klineCandle.Close;
            Volume = klineCandle.Volume;
            CloseTime = klineCandle.CloseTime;
            AssetVolume = klineCandle.AssetVolume;
            Trades = klineCandle.Trades;
            TakerBuyBaseAssetVolume = klineCandle.TakerBuyBaseAssetVolume;
            TakerBuyQuoteAssetVolume = klineCandle.TakerBuyQuoteAssetVolume;
        }

        public BinanceKline ToBinanceKline()
        {
            return new BinanceKline()
            {
                OpenTime = this.OpenTime,
                Open = this.Open,
                High = this.High,
                Low = this.Low,
                Close = this.Close,
                Volume = this.Volume,
                CloseTime = this.CloseTime,
                AssetVolume = this.AssetVolume,
                Trades = this.Trades,
                TakerBuyBaseAssetVolume = this.TakerBuyBaseAssetVolume,
                TakerBuyQuoteAssetVolume = this.TakerBuyQuoteAssetVolume,
            };
        }
    }
}

