using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockScannerCommonCode.charting;
using StockScannerCommonCode.model;

namespace StockScannerCommonCode.Strategies
{
    internal class VolumeOscillator : IStrategy
    {
        // Standardise how strategies are written with Interface
        // Find Doji bars and check what happens near them
        // Look for gradient in volume
        // Volume decreasing during a pullback indicates a downtrend

        // https://www.youtube.com/watch?v=Q9lPu2rJPaA

        // https://www.youtube.com/watch?v=fRfPJ_HbaYE
        // Volume Oscillator indicator:
        // Shows the strength of a trend. If it is high then the current price direction might continue. Also follows the higher high rule
        // Daily chart - 14, 28
        // 15 min - 5, 20

        // Livestream
        // https://www.youtube.com/watch?v=XAArQPKFBBY


        private List<Candle> backTestData;
        private RollingAverage slow;
        private RollingAverage fast;

        public VolumeOscillator(List<Candle> backTestData)
        {
            this.backTestData = backTestData;
            this.slow = new RollingAverage(28, false);
            this.fast = new RollingAverage(14, false);
        }

        List<TradeSignal> IStrategy.RunStrategy()
        {
            List<TradeSignal> tradeSignals= new List<TradeSignal>();

            foreach (Candle row in this.backTestData)
            {
                slow.advance(row.volume);

            }


            return tradeSignals;
        }
    }
}
