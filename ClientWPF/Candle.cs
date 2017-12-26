using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Objects;

namespace Binance.Net.ClientWPF
{
    public class Candle
    {
        public int exchange { get; set; }
        public KlineInterval Interval { get; set; }


        /// <summary> The time this candlestick opened </summary>
        public DateTime OpenTime { get; set; }
        /// <summary> The price at which this candlestick opened </summary>
        public decimal Open { get; set; }
        /// <summary> The highest price in this candlestick </summary>
        public decimal High { get; set; }
        /// <summary> The lowest price in this candlestick </summary>
        public decimal Low { get; set; }
        /// <summary> The price at which this candlestick closed </summary>
        public decimal Close { get; set; }
        /// <summary> The volume traded during this candlestick </summary>
        public decimal Volume { get; set; }
        /// <summary> The close time of this candlestick </summary>
        public DateTime CloseTime { get; set; }
        /// <summary> The volume traded during this candlestick in the asset form </summary>
        public decimal AssetVolume { get; set; }
        /// <summary> The amount of trades in this candlestick </summary>
        public int Trades { get; set; }
        /// <summary> Taker buy base asset volume </summary>
        public decimal TakerBuyBaseAssetVolume { get; set; }
        /// <summary> Taker buy quote asset volume </summary>
        public decimal TakerBuyQuoteAssetVolume { get; set; }

        public Candle()
        {

        }
        public Candle(BinanceKline klineCandle, KlineInterval interval)
        {
            exchange = 1;
            Interval = interval;
            OpenTime = klineCandle.OpenTime;
            Open = klineCandle.Open;
            High = klineCandle.High;
        }
    }
}

