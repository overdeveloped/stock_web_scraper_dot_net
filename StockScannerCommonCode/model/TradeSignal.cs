using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockScannerCommonCode.Strategies;

namespace StockScannerCommonCode.model
{
    public class TradeSignal
    {
        public int id { get; set; }
        public DateTime the_time { get; set; }
        public string symbol { get; set; }
        public double entry { get; set; }
        public double stop_loss { get; set; }
        public double profit_level { get; set; }
        public EnumSignalType signalType;
        public bool closed;
        private EnumTradeStatus status = EnumTradeStatus.StillOpen;

        public TradeSignal()
        {
        }

        public TradeSignal(DateTime openDate, string symbol, double buyIn, double stopLoss, double profitLevel, EnumSignalType signalType)
        {
            this.the_time = openDate;
            this.symbol = symbol;
            this.entry = buyIn;
            this.stop_loss = stopLoss;
            this.profit_level = profitLevel;
            this.signalType = signalType;
            this.closed = false;
        }

        public DateTime getOpenDate()
        {
            return this.the_time;
        }

        public string getSymbol()
        {
            return this.symbol;
        }

        public double getEntry()
        {
            return this.entry;
        }

        public double getStopLoss()
        {
            return this.stop_loss;
        }

        public double getProfitLevel()
        {
            return this.profit_level;
        }

        public EnumSignalType getSignalType()
        {
            return this.signalType;
        }

        public bool getClosed()
        {
            return this.closed;
        }

        public EnumTradeStatus getStatus()
        {
            return this.status;
        }

        public EnumTradeStatus checkStatus(double currentPrice)
        {
            if (this.getSignalType() == EnumSignalType.Sell)
            {
                if (currentPrice > this.stop_loss)
                {
                    this.closed = true;
                    this.status = EnumTradeStatus.StoppedOut;
                    return this.status;
                }
                else if (currentPrice <= this.profit_level)
                {
                    this.closed = true;
                    this.status = EnumTradeStatus.HitProfit;
                    return this.status;
                }
                else
                {
                    return EnumTradeStatus.StillOpen;
                }
            }
            else
            {
                if (currentPrice < this.stop_loss)
                {
                    this.closed = true;
                    this.status = EnumTradeStatus.StoppedOut;
                    return this.status;
                }
                else if (currentPrice >= this.profit_level)
                {
                    this.closed = true;
                    this.status = EnumTradeStatus.HitProfit;
                    return this.status;
                }
                else
                {
                    return EnumTradeStatus.StillOpen;
                }
            }
        }
    }
}
