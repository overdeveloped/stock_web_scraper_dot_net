using System;

namespace StockScannerCommonCode.model
{
    public class CandleMACD
    {
        public int id { get; set; }
        public DateTime date_time { get; set; }
        public double price_open { get; set; }
        public double price_close { get; set; }
        public double price_high { get; set; }
        public double price_low { get; set; }
        public string company_id { get; set; }
        public double macd{ get; set; }
        public double signal { get; set; }
        public double hist { get; set; }
        public double sma200 { get; set; }

        public CandleMACD()
        {
        }

        public CandleMACD(DateTime date_time, double price_open, double price_close, double price_high, double price_low, string company_id, double macd, double signal, double hist, double sma200)
        {
            this.date_time = date_time;
            this.price_open = price_open;
            this.price_close = price_close;
            this.price_high = price_high;
            this.price_low = price_low;
            this.company_id = company_id;
            this.macd = macd;
            this.signal = signal;
            this.hist = hist;
            this.sma200 = sma200;
        }

        public override string ToString()
        {
            return $"date_time: {this.date_time}, price_open: {this.price_open}, price_close: {this.price_close}" +
                $", price_high: {this.price_high}, price_low: {this.price_low}, company_id: {this.company_id}" +
                $", macd: {this.macd}, signal: {this.signal}, hist: {this.hist}, sma200: {this.sma200}";
        }

    }
}
