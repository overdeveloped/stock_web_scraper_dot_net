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
    public class MacdLiveEngine
    {
        // MACD
        private RollingAverage ema9 = new RollingAverage(9, true);
        private RollingAverage ema12 = new RollingAverage(12, true);
        private RollingAverage ema26 = new RollingAverage(26, true);
        private RollingAverage signal = new RollingAverage(9, true);
        // TREND
        RollingAverage sma200 = new RollingAverage(200, false);

        private string symbol;
        private double money = 0;
        private int countBuys = 0;
        private int countSells = 0;
        private int countProfit = 0;
        private int countStoppedOut = 0;
        // CROSSOVER
        private bool MACDwasAbove = false;
        private bool isInUptrend = false;

        // BUY SETUP
        List<TradeSignal> tradeSetups = new List<TradeSignal>();

        RollingAverage previousLows = new RollingAverage(10, false); // Using this class purely for the rolling sample
        RollingAverage previousHighs = new RollingAverage(10, false); // Using this class purely for the rolling sample

        public MacdLiveEngine(string symbol)
        {
            this.symbol = symbol;
        }
        public string getSymbol()
        {
            return this.symbol;
        }

        public List<TradeSignal> getTradeSetups()
        {
            return this.tradeSetups;
        }

        public bool Advance(Candle candle)
        {
            DataAccess da = new DataAccess();
            StringBuilder sb = new StringBuilder();
            bool hasSignal = false;

            DateTime TheTime = candle.date_time;
            string Symbol = candle.company_id;
            double Open = candle.price_open;
            double Close = candle.price_close;
            double High = candle.price_high;
            double Low = candle.price_low;

            //double MACD = candle.macd;
            //double Signal = candle.signal;
            //double Hist = candle.hist;
            previousLows.advance(Low);
            previousHighs.advance(High);

            // TREND
            sma200.advance(Convert.ToDouble(Close));
            // MACD
            ema9.advance(Convert.ToDouble(Close));
            ema12.advance(Convert.ToDouble(Close));
            ema26.advance(Convert.ToDouble(Close));
            double MACD = ema12.getAverage() - ema26.getAverage();
            signal.advance(MACD);
            double Hist = MACD - signal.getAverage();

            // Check the trend
            isInUptrend = Close > sma200.getAverage();

            // Insert into database
            da.InsertCandleMACD(new CandleMACD(TheTime, Open, Close, High, Low, Symbol, MACD, signal.getAverage(), Hist, sma200.getAverage()));

            // Check for decending crossover
            if (MACD < signal.getAverage() && MACD > 0 && !isInUptrend && MACDwasAbove)
            {
                hasSignal = true;
                // SELL SETUP
                double previousSwingHigh = previousHighs.getRollingSample().Max();
                double distFromHighToEntry = previousSwingHigh - Close;
                double StopLoss = previousSwingHigh + (distFromHighToEntry / 100 * 20);
                double profitLevel = Close - (StopLoss - Close) * 1.5;
                TradeSignal sell = new TradeSignal(TheTime, Symbol, Close, StopLoss, profitLevel, EnumSignalType.Sell);
                tradeSetups.Add(sell);

                // Add signal to database
                da.InsertTradeSignal(sell);

                countSells++;

                previousHighs.advance(High);

                // CHECK FOR CLOSED TRADES
                foreach (var trade in tradeSetups)
                {
                    if (!trade.getClosed() && trade.getSignalType() == EnumSignalType.Sell)
                    {
                        if (trade.checkStatus(Close) == EnumTradeStatus.StoppedOut)
                        {
                            double loss = trade.getStopLoss() - trade.getEntry();
                            money -= loss;
                            countStoppedOut++;
                        }

                        if (trade.checkStatus(Close) == EnumTradeStatus.HitProfit)
                        {
                            double profit =  trade.getEntry() - trade.getProfitLevel();
                            money += profit;
                            countProfit++;
                        }
                    }
                }
            }

            // Check for ascending crossover
            if (MACD > signal.getAverage() && MACD < 0 && isInUptrend && !MACDwasAbove)
            {
                hasSignal = true;
                // BUY SETUP
                double previousSwingLow = previousLows.getRollingSample().Min();
                double distFromLowToEntry = Close - previousSwingLow;
                double StopLoss = previousSwingLow - (distFromLowToEntry / 100 * 20);
                double profitLevel = Close + (Close - StopLoss) * 1.5;
                TradeSignal buy = new TradeSignal(TheTime, Symbol, Close, StopLoss, profitLevel, EnumSignalType.Buy);
                tradeSetups.Add(buy);

                // Add signal to database
                da.InsertTradeSignal(buy);

                // Add signal to list view on dashboard
                ListViewItem item = new ListViewItem(candle.company_id);

                item.SubItems.Add(TheTime.ToString());
                item.SubItems.Add(Close.ToString());
                item.SubItems.Add(StopLoss.ToString());
                item.SubItems.Add(profitLevel.ToString());

                //listView.Groups["Buy Signals"].Items.Add(item);
                //listView.Items.Add(item);

                countBuys++;

                previousLows.advance(Low);

                // CHECK FOR CLOSED TRADES
                foreach (var trade in tradeSetups)
                {
                    if (!trade.getClosed() && trade.getSignalType() == EnumSignalType.Buy)
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
            }

            // Update crossover bool
            MACDwasAbove = (MACD > signal.getAverage());

            sb.AppendLine(TheTime + " " + isInUptrend + " " + MACDwasAbove);

            string macds = sb.ToString();
            double percentage = Convert.ToDouble(countProfit) / ((Convert.ToDouble(countStoppedOut) + Convert.ToDouble(countProfit)) / 100);
            return hasSignal;
        }

        public bool Advance(CandleMACD candle)
        {
            StringBuilder sb = new StringBuilder();
            bool hasSignal = false;

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
                hasSignal = true;
                // BUY SETUP
                double previousSwingLow = previousLows.getRollingSample().Min();
                double distFromLowToEntry = Close - previousSwingLow;
                double StopLoss = previousSwingLow - (distFromLowToEntry / 100 * 20);
                double profitLevel = Close + (Close - StopLoss) * 1.5;
                TradeSignal buy = new TradeSignal(TheTime, Symbol, Close, StopLoss, profitLevel, EnumSignalType.Buy);
                tradeSetups.Add(buy);

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
                foreach (var trade in tradeSetups)
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
            }
            else
            {
                MACDwasAbove = (MACD > Signal);
            }

            sb.AppendLine(TheTime + " " + isInUptrend + " " + MACDwasAbove);

            string macds = sb.ToString();
            double percentage = Convert.ToDouble(countProfit) / ((Convert.ToDouble(countStoppedOut) + Convert.ToDouble(countProfit)) / 100);
            return hasSignal;
        }
    }
}
