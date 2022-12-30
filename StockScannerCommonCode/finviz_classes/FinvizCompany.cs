using Newtonsoft.Json;

namespace StockScannerCommonCode
{
    // The property names reflect the column titles on the Finviz results page
    public class FinvizCompany
    {
        public string Ticker { get; set; }
        public string Company { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public string Country { get; set; }
        public string MarketCap { get; set; }
        public string PE { get; set; }
        public string Price { get; set; }
        public string Change { get; set; }
        public string Volume { get; set; }

        public FinvizCompany()
        {

        }

        public FinvizCompany(string ticker, string company, string sector, string industry, string country,
                        string marketCap, string pe, string price, string change, string volume)
        {
            this.Ticker = ticker;
            this.Company = company;
            this.Sector = sector;
            this.Industry = industry;
            this.Country = country;
            this.MarketCap = marketCap;
            this.PE = pe;
            this.Price = price;
            this.Change = change;
            this.Volume = volume;
        }
        public override string ToString()
        {
            return $"Ticker: {this.Ticker}, Company: {this.Company}, Sector: {this.Sector}, Industry: {this.Industry}" +
                $", Country: {this.Country}, MarketCap: {this.MarketCap}, PE: {this.PE}, Price: {this.Price}" +
                $", Change: {this.Change}, Volume: {this.Volume}";
        }

        public FinvizCompany Duplicate()
        {
            return new FinvizCompany(
                this.Ticker,
                this.Company,
                this.Sector,
                this.Industry,
                this.Country,
                this.MarketCap,
                this.PE,
                this.Price,
                this.Change,
                this.Volume);
        }
    }
}
