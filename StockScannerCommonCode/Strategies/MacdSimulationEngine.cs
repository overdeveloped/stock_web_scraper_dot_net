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
    public class MacdSimulationEngine
    {
        private string symbol;
        private DataAccess dataAccess = new DataAccess();
        private double money = 0;
        private int countBuys = 0;
        private int countProfit = 0;
        private int countStoppedOut = 0;
        // CROSSOVER
        private bool MACDwasAbove = false;
        private bool isInUptrend = false;

        // BUY SETUP
        List<TradeSignal> buySetups = new List<TradeSignal>();
        RollingAverage previousLows = new RollingAverage(10, false); // Using this class purely for the rolling sample

        public MacdSimulationEngine(string symbol)
        {
            this.symbol = symbol;
            this.dataAccess.ClearSignalTable();
        }
        public string getSymbol()
        {
            return this.symbol;
        }

        public List<TradeSignal> getBuySetups()
        {
            return this.buySetups;
        }

        public TradeSignal Advance(CandleMACD candle)
        {
            StringBuilder sb = new StringBuilder();
            //bool hasSignal = false;

            DateTime TheTime = candle.date_time;
            string Symbol = candle.company_id;
            double Close = candle.price_close;
            double Low = candle.price_low;
            double MACD = candle.macd;
            double Signal = candle.signal;
            double Hist = candle.hist;
            previousLows.advance(Low);

            // Check the trend
            isInUptrend = Close > candle.sma200;

            // Check for crossover
            if (MACD > Signal && MACD < 0 && isInUptrend && !MACDwasAbove)
            {
                //hasSignal = true;
                // BUY SETUP
                double previousSwingLow = previousLows.getRollingSample().Min();
                double distFromLowToEntry = Close - previousSwingLow;
                double StopLoss = previousSwingLow - (distFromLowToEntry / 100 * 20);
                double profitLevel = Close + (Close - StopLoss) * 1.5;
                TradeSignal buy = new TradeSignal(TheTime, Symbol, Close, StopLoss, profitLevel, EnumSignalType.Buy);
                buySetups.Add(buy);

                // Add signal to list view on dashboard
                ListViewItem item = new ListViewItem(candle.company_id);

                item.SubItems.Add(TheTime.ToString());
                item.SubItems.Add(Close.ToString());
                item.SubItems.Add(StopLoss.ToString());
                item.SubItems.Add(profitLevel.ToString());

                //listView.Groups["Buy Signals"].Items.Add(item);
                //listView.Items.Add(item);

                countBuys++;

                // Update crossover bool
                MACDwasAbove = (MACD > Signal);

                previousLows.advance(Low);

                // CHECK FOR CLOSED TRADES
                foreach (var trade in buySetups)
                {
                    if (!trade.getClosed())
                    {
                        if (trade.checkStatus(Close) == EnumTradeStatus.StoppedOut)
                        {
                            double loss = trade.getEntry() - trade.getStopLoss();
                            money -= loss;
                            countStoppedOut++;
                        }

                        if (trade.checkStatus(Close) == EnumTradeStatus.HitProfit)
                        {
                            double profit = trade.getProfitLevel() - trade.getEntry();
                            money += profit;
                            countProfit++;
                        }
                    }
                }

                return buy;
            }
            else
            {
                MACDwasAbove = (MACD > Signal);
            }

            sb.AppendLine(TheTime + " " + isInUptrend + " " + MACDwasAbove);

            string macds = sb.ToString();
            double percentage = Convert.ToDouble(countProfit) / ((Convert.ToDouble(countStoppedOut) + Convert.ToDouble(countProfit)) / 100);
            return null;
        }
    }
}
