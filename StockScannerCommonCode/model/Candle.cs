using System;

namespace StockScannerCommonCode.model
{
    public class Candle
    {
        public int id { get; set; }
        public DateTime date_time { get; set; }
        public double price_open { get; set; }
        public double price_close { get; set; }
        public double price_high { get; set; }
        public double price_low { get; set; }
        public int volume { get; set; }
        public string company_id { get; set; }

        public Candle()
        {
        }

        public Candle(DateTime date_time, double price_open, double price_close, double price_high, double price_low, int volume, string company_id)
        {
            this.date_time = date_time;                
            this.price_open = price_open;
            this.price_close = price_close;
            this.price_high = price_high;
            this.price_low = price_low;
            this.volume = volume;
            this.company_id = company_id;
        }
         
        public override string ToString()
        {
            return $"date_time: {this.date_time}, price_open: {this.price_open}, price_close: {this.price_close}" +
                $", price_high: {this.price_high}, price_low: {this.price_low}, volume: {this.volume}, company_id: {this.company_id}";
        }
    }
}
