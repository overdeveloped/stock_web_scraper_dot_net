using HtmlAgilityPack;
using System.Reflection;
using System.Text;
using SeldonStockScannerAPI.models;
using SeldonStockScannerAPI.Connections;

namespace SeldonStockScannerAPI.WebScraper
{
    public class SeldonWebScraper : IWebScraper
    {
        private readonly IWebConnection webConnection;

        public SeldonWebScraper(IWebConnection webConnection)
        {
            this.webConnection = webConnection;
        }

        public string GetTestHTML()
        {
            HtmlDocument doc = this.webConnection.GetWebsiteByUrl("test");
            return doc.Text;
        }

        public List<string> GetCompletePlus500()
        {
            List<string> results = new List<string>();

            HtmlDocument doc = this.webConnection.GetWebsiteByUrl("https://www.plus500.com/en/instruments#indicesf");

            HtmlNodeCollection tableRowSpans = doc.DocumentNode.SelectNodes("//tr//span");

            int count = 2;
            foreach (var tr in tableRowSpans)
            {
                if (count % 2 == 0)
                {
                    results.Add(tr.InnerText);
                }

                count++;
            }

            return results;

            HtmlNodeCollection tableData = doc.DocumentNode.SelectNodes("//div[@id='stocks']/div/div/div/div/table/tbody/tr/td[@class='sym']/span");

            StringBuilder sbtables = new StringBuilder();
            if (tableData != null)
            {
                foreach (var row in tableData)
                {
                    sbtables.AppendLine(row.InnerText.Trim());
                    results.Add(row.InnerText.Trim());
                }
            }

            Helpers.outputToFile("a_plus500_tables", sbtables.ToString());



            return results;

        }

        #region - FINVIZ -
        /// <summary>
        /// Sends custom request to Finviz
        /// </summary>
        /// <returns></returns>
        public List<FinvizCompanyEntity> GetCustomFinvizScan(string url, string name)
        {
            Helpers.outputToFile($"{name}_url", url);
            StringBuilder sb = new StringBuilder();
            List<FinvizCompanyEntity> results = new List<FinvizCompanyEntity>();
            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc;

            // Check the pagination so that we know how much iteration is needed
            doc = web.Load($"{url}");
            IList<HtmlNode> paginationData = doc.QuerySelectorAll(".screener_pagination");

            int iterationScaler = 1;

            if (paginationData != null && paginationData.Count > 0)
            {
                var pageNumers = paginationData.First().SelectNodes("a");


                if (pageNumers != null && pageNumers.Count > 1)
                {
                    iterationScaler = pageNumers.Count - 1;
                }
            }

            // Check page by page
            for (int index = 1; index < 20 * iterationScaler; index += 20)
            {
                doc = web.Load($"{url}&r={index}");
                Helpers.outputToFile($"{name}_raw", doc.DocumentNode.OuterHtml);

                IList<HtmlNode> tableData = doc.QuerySelectorAll(".screener-views-table");
                // Get specific data (granular)
                HtmlNodeCollection tableTextAllColumns = doc.DocumentNode.SelectNodes("//*[@id='screener-views-table']/tr[5]/td[1]/table/tr/td/table/tr/td");

                HtmlNodeCollection tableTextRows = doc.DocumentNode.SelectNodes("//*[@id='screener-views-table']/tr[5]/td[1]/table/tr/td/table/tr");

                if (tableTextRows != null)
                {
                    sb.AppendLine($"********************{tableTextRows.Count}**********************");

                    foreach (var tableTextRow in tableTextRows)
                    {
                        var children = tableTextRow.GetChildElements();

                        FinvizCompanyEntity fCompany = new FinvizCompanyEntity();
                        fCompany.Ticker = children.ToList()[1].InnerText;
                        fCompany.Company = children.ToList()[2].InnerText;
                        fCompany.Sector = children.ToList()[3].InnerText;
                        fCompany.Industry = children.ToList()[4].InnerText;
                        fCompany.Country = children.ToList()[5].InnerText;
                        fCompany.MarketCap = children.ToList()[6].InnerText;
                        fCompany.PE = children.ToList()[7].InnerText;
                        fCompany.Price = children.ToList()[8].InnerText;
                        fCompany.Change = children.ToList()[9].InnerText;
                        fCompany.Volume = children.ToList()[10].InnerText;

                        results.Add(fCompany);
                        sb.AppendLine($"******************************************");
                        sb.AppendLine($"SYMBOL: {fCompany.Ticker}");
                        sb.AppendLine($"NAME: {fCompany.Company}");
                        sb.AppendLine($"SECTOR: {fCompany.Sector}");
                        sb.AppendLine($"INDUSTRY: {fCompany.Industry}");
                        sb.AppendLine($"COUNTRY: {fCompany.Country}");
                        sb.AppendLine($"MARKETCAP: {fCompany.MarketCap}");
                        sb.AppendLine($"PE: {fCompany.PE}");
                        sb.AppendLine($"PRICE: {fCompany.Price}");
                        sb.AppendLine($"CHANGE: {fCompany.Change}");
                        sb.AppendLine($"VOLUME: {fCompany.Volume}");
                    }
                }
            }

            Helpers.outputToFile($"{name}_processed", sb.ToString());
            Helpers.outputToFile($"{name}_count", results.Count.ToString());

            return results;
        }

        #endregion


        #region - FIDELITY -
        public Dictionary<string, string> GetFTSE100()
        {
            Dictionary<string, string> results = new Dictionary<string, string>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://www.fidelity.co.uk/shares/ftse-100/");
            string page = doc.DocumentNode.OuterHtml;
            Helpers.outputToFile("ftse100_html_raw", page);

            // Get the table
            HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//table");
            string table = htmlNodes.FirstOrDefault().InnerText;
            Helpers.outputToFile("ftse100_html_table", table);

            HtmlNodeCollection tableBody = htmlNodes.FirstOrDefault().SelectNodes("tbody");
            HtmlNodeCollection tableRows = tableBody.FirstOrDefault().SelectNodes("tr");

            StringBuilder sb = new StringBuilder();
            foreach (var row in tableRows)
            {
                if (row.SelectNodes("td").Count >= 2)
                {
                    string symbol = row.SelectNodes("td")[0].InnerText;
                    string name = row.SelectNodes("td")[1].InnerText;

                    // Correct the symbol format so that they match Yahoo Finance
                    if (symbol.Last().Equals('.'))
                    {
                        symbol = symbol + 'L';
                    }
                    else if (symbol.Contains(".A"))
                    {
                        symbol = symbol.Split('.')[0] + "-A.L";
                    }
                    else if (!symbol.Contains(".L"))
                    {
                        symbol = symbol + ".L";
                    }

                    if (symbol.Contains("RDSb"))
                    {
                        continue;
                    }

                    if (symbol.Contains("MRW"))
                    {
                        continue;
                    }

                    if (symbol.Contains("RDS"))
                    {
                        symbol = "SHEL.L";
                    }

                    if (symbol.Contains("LSE"))
                    {
                        symbol = "LSEG.L";
                    }

                    if (symbol.Contains("NMC"))
                    {
                        symbol = "NMHLY";
                    }

                    // Remove special chars from name
                    if (name.Contains('\''))
                    {
                        name = name.Replace("\'", "");
                    }
                    sb.AppendLine(symbol);
                    results.Add(symbol, name);
                }
            }

            Helpers.outputToFile("ftse100_html_table_elements", sb.ToString());

            return results;
        }

        #endregion


        #region - YAHOO FINANCE -
        public double GetCurrentValue(string symbol)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb_row = new StringBuilder();

            List<FinvizCompanyEntity> results = new List<FinvizCompanyEntity>();
            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = new HtmlDocument();

            try
            {
                doc = web.Load($"https://uk.finance.yahoo.com/quote/{symbol}?p={symbol}&.tsrc=fin-srch");
                //Helpers.outputToFile($"current_value_raw_{symbol}", doc.DocumentNode.OuterHtml);
            }
            catch (Exception ex)
            {
                Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"Symbol = {symbol}. Web scraper problem: {ex.Message}");
            }

            // This one:
            HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//fin-streamer[@class='Fw(b) Fz(36px) Mb(-4px) D(ib)']");
            //HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//fin-streamer[@data-test='qsp-price']");

            double price = 0.0d;
            try
            {
                if (htmlNodes != null)
                {
                    if (htmlNodes.Nodes().Count() > 0)
                    {
                        // ERROR: htmlNodes IS NULL FOR "AKAM". THE TARGETED NODE EXISTS. UNCLEAR WHY IT DIDN'T WORK
                        // ERROR: INDEX OUT OF RANGE FOR "CHPT". NO NODES IN htmlNodes OBJECT
                        // There are now checks for this but I need to find where the problem is in the scraper
                        price = Convert.ToDouble(htmlNodes.Nodes().ToList()[0].InnerHtml);
                    }
                }
                else
                {
                    Helpers.outputToFile($"{DateTime.Now.ToString("yyyy-MM-dd_HH-mm")}_htmlNodes_error_{symbol}", doc.DocumentNode.OuterHtml);
                }
            }
            catch (Exception ex)
            {
                Helpers.error(MethodBase.GetCurrentMethod().DeclaringType.Name, $"Symbol = {symbol}. Web scraper problem: {ex.Message}");
                price = 0.0d;
            }

            //Helpers.outputToFile($"price_{symbol}", price.ToString());

            return Convert.ToDouble(price);
        }


        #endregion

    }



}
