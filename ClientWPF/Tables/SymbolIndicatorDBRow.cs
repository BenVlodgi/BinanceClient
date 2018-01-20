using Binance.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.Net.ClientWPF.Tables
{
    /// <summary> Holds calcuated indicators for specific currency|asset intervals </summary>
    [DBTable("SymbolIndicator")] public class SymbolIndicatorDBRow : StorageTableBase<SymbolIndicatorDBRow>
    {
        [DBProperty(primaryKeyLevel: 0)] public int Exchange { get; set; }
        [DBProperty(primaryKeyLevel: 0)] public string Asset { get; set; }
        [DBProperty(primaryKeyLevel: 0)] public string Currency { get; set; }
        [DBProperty(primaryKeyLevel: 0)] public KlineInterval Interval { get; set; }
        /// <summary> The time the corrosponding to when the candlestick opened </summary>
        [DBProperty(primaryKeyLevel: 0)] public DateTime OpenTime { get; set; }
        /// <summary> Was this candle closed when the calculations were made </summary>
        [DBProperty] public bool ClosedWhenCalcuated { get; set; }
        [DBProperty] double MACD { get; set; }
        [DBProperty] double MACDSignal { get; set; }
        [DBProperty] double MACDHist { get; set; }
        [DBProperty] double RSI { get; set; }
    }
}
