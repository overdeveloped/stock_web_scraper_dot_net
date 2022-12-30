using StockScannerCommonCode.charting;
using StockScannerCommonCode.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockScannerCommonCode.Strategies
{
    public class BacktestEngine
    {
        private List<CandleMACD> backTestData;
        private int trendScore;

        public BacktestEngine(List<CandleMACD> backtestData)
        {
            this.backTestData = backtestData;
            this.trendScore = 0;
        }

        public List<TradeSignal> RunStrategyMACD()
        {
            int countBuys = 0;
            int countSells = 0;
            int countProfit = 0;
            int countStoppedOut = 0;
            // CROSSOVER
            bool MACDwasAbove = false;
            bool isAbove200SMA = false;

            // BUY SETUP
            List<TradeSignal> tradeSetups = new List<TradeSignal>();

            RollingAverage previousLows = new RollingAverage(10, false); // Using this class purely for the rolling sample
            RollingAverage previousHighs = new RollingAverage(10, false); // Using this class purely for the rolling sample
                                                                          //DataAccess dataAccess = new DataAccess();
                                                                          //dataAccess.ClearSignalTable();

            StringBuilder sb = new StringBuilder();

            foreach (var row in backTestData)
            {
                DateTime TheTime = row.date_time;
                string Symbol = row.company_id;
                double Close = row.price_close;
                double Low = row.price_low;
                double High = row.price_high;
                double MACD = row.macd;
                double Signal = row.signal;
                double Hist = row.hist;
                previousLows.advance(Low);
                previousHighs.advance(High);

                // Check the trend
                isAbove200SMA = Close > row.sma200;

                // Check if we are in prime trading time
                if (isInPrimeTime(TheTime))
                {
                    // Check for decending crossover
                    if (MACD < Signal && MACD > 0 && this.trendScore <= -3 && MACDwasAbove)
                    {
                        // SELL SETUP
                        double previousSwingHigh = previousHighs.getRollingSample().Max();
                        double distFromHighToEntry = previousSwingHigh - Close;
                        double StopLoss = previousSwingHigh + (distFromHighToEntry / 100 * 20);
                        double profitLevel = Close - (StopLoss - Close) * 1.5;
                        TradeSignal sell = new TradeSignal(TheTime, Symbol, Close, StopLoss, profitLevel, EnumSignalType.Sell);
                        tradeSetups.Add(sell);

                        countSells++;

                        previousHighs.advance(High);
                    }

                    // Check for ascending crossover
                    if (MACD > Signal && MACD < 0 && this.trendScore >= 3 && !MACDwasAbove)
                    {
                        // BUY SETUP
                        double previousSwingLow = previousLows.getRollingSample().Min();
                        double distFromLowToEntry = Close - previousSwingLow;
                        double StopLoss = previousSwingLow - (distFromLowToEntry / 100 * 20);
                        double profitLevel = Close + (Close - StopLoss) * 1.5;
                        TradeSignal buy = new TradeSignal(TheTime, Symbol, Close, StopLoss, profitLevel, EnumSignalType.Buy);
                        tradeSetups.Add(buy);

                        countBuys++;

                        // Update crossover bool
                        MACDwasAbove = (MACD > Signal);

                        previousLows.advance(Low);
                    }
                }

                // CHECK FOR CLOSED TRADES
                foreach (TradeSignal trade in tradeSetups)
                {
                    if (!trade.getClosed())
                    {
                        trade.checkStatus(Close);
                    }
                }

                MACDwasAbove = (MACD > Signal);

                sb.AppendLine(TheTime + " " + isAbove200SMA + " " + MACDwasAbove);
            }

            string macds = sb.ToString();
            double percentage = Convert.ToDouble(countProfit) / ((Convert.ToDouble(countStoppedOut) + Convert.ToDouble(countProfit)) / 100);

            return tradeSetups;
        }

        // HELPERS
        private bool isInPrimeTime(DateTime time)
        {
            if ((time.Hour > 9 && time.Hour < 12) ||
                (time.Hour > 13 && time.Hour < 17))
            {
                return true;
            }

            return false;
        }

        private void updateTrend(bool isInUptrend)
        {
            if (isInUptrend && this.trendScore < 10)
            {
                this.trendScore++;
            }
            else if (!isInUptrend && this.trendScore > -10)
            {
                this.trendScore--;
            }
        }
    }
}
