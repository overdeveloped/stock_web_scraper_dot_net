using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using System.Security.Policy;

namespace StockScannerCommonCode
{
    public class WebScraper : IWebScraper
    {
        /// <summary>
        /// Counts pages
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int CountResultsPages(string fullUrl)
        {
            StringBuilder sb = new StringBuilder();

            List<FinvizCompany> results = new List<FinvizCompany>();
            HtmlWeb web = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc;

            // Check the pagination so that we know how much iteration is needed
            doc = web.Load($"{fullUrl}");
            IList<HtmlNode> paginationData = doc.QuerySelectorAll(".screener_pagination");
            Helpers.outputToFile("increasing_volume_count_raw", doc.DocumentNode.OuterHtml);

            int resultsPages = 1;

            if (paginationData != null && paginationData.Count > 0)
            {
                var pageNumers = paginationData.First().SelectNodes("a");
                string totalPages = pageNumers[pageNumers.Count() - 2].InnerHtml;
                if (pageNumers != null && pageNumers.Count > 1)
                {
                    resultsPages = Convert.ToInt32(pageNumers[pageNumers.Count() - 2].InnerHtml);
                }
            }

            return resultsPages * 20;
        }


        /// <summary>
        /// Sends custom request to Finviz
        /// </summary>
        /// <returns></returns>
        public List<FinvizCompany> GetCustomWatchList(string url, string name)
        {
            return getResults(url, name);
        }




        // Returns several pages of Finviz results
        public List<FinvizCompany> GetOversold()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb_row = new StringBuilder();

            //List<HtmlNodeWrapper> results = new List<HtmlNodeWrapper>();
            List<FinvizCompany> results = new List<FinvizCompany>();
            HtmlWeb web = new HtmlWeb();

            ////HtmlAgilityPack.HtmlDocument doc = web.Load("https://finviz.com/screener.ashx?v=111&f=fa_div_high,fa_pe_low,sh_avgvol_o300");
            ///

            HtmlAgilityPack.HtmlDocument doc;

            for (int index = 1; index < 100; index += 20)
            {
                doc = web.Load($"https://finviz.com/screener.ashx?v=111&f=sh_price_u20,ta_perf_ddown,ta_perf2_d5u,ta_rsi_os40&r={index}");
                //https://finviz.com/screener.ashx?v=111&f=sh_price_u20,ta_perf_ddown,ta_perf2_d5u,ta_rsi_os40&ft=3&r=21
                //https://finviz.com/screener.ashx?v=111&f=cap_mega&o=ticker
                Helpers.outputToFile("oversold_raw", doc.DocumentNode.OuterHtml);

                int tableCount = 0;
                foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
                {
                    if (tableCount > 13)
                    {
                        int rowCount = 0;
                        foreach (HtmlNode row in table.SelectNodes("tr"))
                        {
                            if (rowCount > 0)
                            {
                                sb_row.Clear();
                                foreach (HtmlNode cell in row.SelectNodes("td"))
                                {
                                    sb_row.Append($"|{cell.InnerText.Trim()}");
                                    sb.AppendLine(cell.InnerText.Trim());
                                    //results.Add(new HtmlNodeWrapper { tableName = table.Id, cellText = cell.InnerText.Trim() });
                                }

                                string[] companyProps = sb_row.ToString().Split('|');
                                if (companyProps.Count() > 11)
                                {

                                    results.Add(new FinvizCompany() {
                                        Ticker = companyProps[2],
                                        Company = companyProps[3],
                                        Sector = companyProps[4],
                                        Industry = companyProps[5],
                                        Country = companyProps[6],
                                        MarketCap = companyProps[7],
                                        PE = companyProps[8],
                                        Price = companyProps[9],
                                        Change = companyProps[10],
                                        Volume = companyProps[11],
                                    });
                                }
                            }
                            rowCount++;
                        }
                    }
                    tableCount++;
                }
            }

            Helpers.outputToFile("oversold_processed", sb.ToString());

            return results;

        }

        public List<FinvizCompany> GetMegaCompanies()
        {
            string url = "https://finviz.com/screener.ashx?v=111&f=cap_mega";
            return getResults(url, "mega");
        }



        /// <summary>
        /// Find blue chip stocks
        /// Market cap > 200B
        /// Average volume (90 days) > 500K
        /// </summary>
        public List<FinvizCompany> GetTech()
        {
            string url = "https://finviz.com/screener.ashx?v=111&f=cap_large,sec_technology,sh_avgvol_o500";
            return getResults(url, "tech");
        }

        /// <summary>
        /// Find stocks with increasing average volume in preparation for a move upwards
        /// https://www.youtube.com/watch?v=sP9u2oiwikU
        /// </summary>
        /// <returns></returns>
        public async Task<List<FinvizCompany>> GetIncreasedAverageVolumeAsync()
        {
            StringBuilder sb = new StringBuilder();

            List<FinvizCompany> results = new List<FinvizCompany>();
            HtmlWeb web = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc;

            // Check the pagination so that we know how much iteration is needed
            doc = web.Load($"https://finviz.com/screener.ashx?v=111&f=cap_small,sh_curvol_o5000,sh_price_u10,sh_relvol_o1.5");
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

            // Go through each page
            for (int index = 1; index < 20 * iterationScaler; index += 20)
            {
                doc = web.Load($"https://finviz.com/screener.ashx?v=111&f=cap_small,sh_curvol_o5000,sh_price_u10,sh_relvol_o1.5&r={index}");
                Helpers.outputToFile("increasing_volume_raw", doc.DocumentNode.OuterHtml);

                IList<HtmlNode> tableData = doc.QuerySelectorAll(".screener-body-table-nw");
                string paginationDataString = paginationData.First().InnerHtml;

                string[] companyTemp = new string[11];
                int tempCounter = 0;

                for (int dIndex = 0; dIndex < tableData.Count; dIndex++)
                {
                    HtmlNode anchor = tableData[dIndex].SelectNodes("a").FirstOrDefault();

                    if (anchor.SelectNodes("span") != null && anchor.SelectNodes("span").Count > 0)
                    {
                        companyTemp[tempCounter] = anchor.SelectNodes("span").FirstOrDefault().InnerText;
                    }
                    else
                    {
                        companyTemp[tempCounter] = anchor.InnerText;
                    }

                    tempCounter++;

                    if (tempCounter > 10)
                    {
                        FinvizCompany fCompany = new FinvizCompany();
                        fCompany.Ticker = companyTemp[1];
                        fCompany.Company = companyTemp[2];
                        fCompany.Sector = companyTemp[3];
                        fCompany.Industry = companyTemp[4];
                        fCompany.Country = companyTemp[5];
                        fCompany.MarketCap = companyTemp[6];
                        fCompany.PE = companyTemp[7];
                        fCompany.Price = companyTemp[8];
                        fCompany.Change = companyTemp[9];
                        fCompany.Volume = companyTemp[10];

                        results.Add(fCompany);
                        tempCounter = 0;
                        sb.AppendLine(fCompany.ToString());
                    }
                }
            }

            Helpers.outputToFile("increasing_volume_processed", sb.ToString());

            return results;
        }

        /// <summary>
        /// Find blue chip stocks ASYNCHRONOUSLY
        /// Market cap > 200B
        /// Average volume (90 days) > 500K
        /// </summary>
        public async Task<List<FinvizCompany>> GetTechAsync()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb_row = new StringBuilder();

            //List<HtmlNodeWrapper> results = new List<HtmlNodeWrapper>();
            List<FinvizCompany> results = new List<FinvizCompany>();
            HtmlWeb web = new HtmlWeb();

            ////HtmlAgilityPack.HtmlDocument doc = web.Load("https://finviz.com/screener.ashx?v=111&f=fa_div_high,fa_pe_low,sh_avgvol_o300");
            ///

            HtmlAgilityPack.HtmlDocument doc;

            // Check the pagination so that we know how much iteration is needed
            doc = web.Load($"https://finviz.com/screener.ashx?v=111&f=cap_mega,sec_technology,sh_avgvol_o500&r");
            //doc = web.Load($"https://finviz.com/screener.ashx?v=111&f=cap_large,sec_technology,sh_avgvol_o500&r");
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

            for (int index = 1; index < 20 * iterationScaler; index += 20)
            {
                doc = web.Load($"https://finviz.com/screener.ashx?v=111&f=cap_large,sec_technology,sh_avgvol_o500&r={index}");
                //doc = web.Load($"https://finviz.com/screener.ashx?v=111&f=cap_mega,sec_technology,sh_avgvol_o500&r={index}");
                //https://finviz.com/screener.ashx?v=111&f=sh_price_u20,ta_perf_ddown,ta_perf2_d5u,ta_rsi_os40&ft=3&r=21
                //https://finviz.com/screener.ashx?v=111&f=cap_mega&o=ticker
                Helpers.outputToFile("tech_raw", doc.DocumentNode.OuterHtml);

                IList<HtmlNode> tableData = doc.QuerySelectorAll(".screener-body-table-nw");
                string paginationDataString = paginationData.First().InnerHtml;

                string[] companyTemp = new string[11];
                int tempCounter = 0;

                for (int dIndex = 0; dIndex < tableData.Count; dIndex++)
                {
                    HtmlNode anchor = tableData[dIndex].SelectNodes("a").FirstOrDefault();

                    if (anchor.SelectNodes("span") != null && anchor.SelectNodes("span").Count > 0)
                    {
                        companyTemp[tempCounter] = anchor.SelectNodes("span").FirstOrDefault().InnerText;
                    }
                    else
                    {
                        companyTemp[tempCounter] = anchor.InnerText;
                    }

                    tempCounter++;

                    if (tempCounter > 10)
                    {
                        FinvizCompany fCompany = new FinvizCompany();
                        fCompany.Ticker = companyTemp[1];
                        fCompany.Company = companyTemp[2];
                        fCompany.Sector = companyTemp[3];
                        fCompany.Industry = companyTemp[4];
                        fCompany.Country = companyTemp[5];
                        fCompany.MarketCap = companyTemp[6];
                        fCompany.PE = companyTemp[7];
                        fCompany.Price = companyTemp[8];
                        fCompany.Change = companyTemp[9];
                        fCompany.Volume = companyTemp[10];

                        results.Add(fCompany);
                        tempCounter = 0;
                    }
                }
            }

            Helpers.outputToFile("tech_processed", sb.ToString());

            return results;
        }

        public List<FinvizCompany> GetAllEnergy()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb_row = new StringBuilder();

            //List<HtmlNodeWrapper> results = new List<HtmlNodeWrapper>();
            List<FinvizCompany> results = new List<FinvizCompany>();
            HtmlWeb web = new HtmlWeb();

            ////HtmlAgilityPack.HtmlDocument doc = web.Load("https://finviz.com/screener.ashx?v=111&f=fa_div_high,fa_pe_low,sh_avgvol_o300");
            ///

            HtmlAgilityPack.HtmlDocument doc;

            // Check the pagination so that we know how much iteration is needed
            doc = web.Load($"https://finviz.com/screener.ashx?v=111&f=sec_energy");
            IList<HtmlNode> paginationData = doc.QuerySelectorAll(".screener_pagination");

            int iterationScaler = getNumberOfPages(paginationData);

            for (int index = 1; index < 20 * iterationScaler; index += 20)
            {
                doc = web.Load($"https://finviz.com/screener.ashx?v=111&f=sec_energy&r={index}");
                Helpers.outputToFile("energy_raw", doc.DocumentNode.OuterHtml);

                IList<HtmlNode> tableData = doc.QuerySelectorAll(".screener-body-table-nw");
                //IList<HtmlNode> paginationData = doc.QuerySelectorAll(".screener_pagination");
                string paginationDataString = paginationData.First().InnerHtml;

                string[] companyTemp = new string[11];
                int tempCounter = 0;
                //foreach (var tableDatum in tableData)
                for (int dIndex = 0; dIndex < tableData.Count; dIndex++)
                {
                    HtmlNode anchor = tableData[dIndex].SelectNodes("a").FirstOrDefault();

                    if (anchor.SelectNodes("span") != null && anchor.SelectNodes("span").Count > 0)
                    {
                        companyTemp[tempCounter] = anchor.SelectNodes("span").FirstOrDefault().InnerText;
                    }
                    else
                    {
                        companyTemp[tempCounter] = anchor.InnerText;
                    }

                    tempCounter++;

                    if (tempCounter > 10)
                    {
                        FinvizCompany fCompany = new FinvizCompany();
                        fCompany.Ticker = companyTemp[1];
                        fCompany.Company = companyTemp[2];
                        fCompany.Sector = companyTemp[3];
                        fCompany.Industry = companyTemp[4];
                        fCompany.Country = companyTemp[5];
                        fCompany.MarketCap = companyTemp[6];
                        fCompany.PE = companyTemp[7];
                        fCompany.Price = companyTemp[8];
                        fCompany.Change = companyTemp[9];
                        fCompany.Volume = companyTemp[10];

                        results.Add(fCompany);
                        tempCounter = 0;
                        sb.AppendLine(fCompany.ToString());
                    }
                }
            }

            Helpers.outputToFile("energy_processed", sb.ToString());

            return results;

        }

        public List<FinvizCompany> GetOversoldBouncePlay()
        {
            string url = "https://finviz.com/screener.ashx?v=111&f=sh_price_u20,ta_averagetruerange_o0.25,ta_perf_ddown,ta_perf2_d5u,ta_rsi_os40&ft=3";
            StringBuilder sb = new StringBuilder();

            //List<HtmlNodeWrapper> results = new List<HtmlNodeWrapper>();
            List<FinvizCompany> results = new List<FinvizCompany>();
            HtmlWeb web = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc;

            // Check the pagination so that we know how much iteration is needed
            doc = web.Load(url);
            IList<HtmlNode> paginationData = doc.QuerySelectorAll(".screener_pagination");

            int iterationScaler = getNumberOfPages(paginationData);

            for (int index = 1; index < 20 * iterationScaler; index += 20)
            {
                doc = web.Load($"{url}&r={index}");
                Helpers.outputToFile("bounce_raw", doc.DocumentNode.OuterHtml);

                IList<HtmlNode> tableData = doc.QuerySelectorAll(".screener-body-table-nw");

                string[] companyTemp = new string[11];
                int tempCounter = 0;
                //foreach (var tableDatum in tableData)
                for (int dIndex = 0; dIndex < tableData.Count; dIndex++)
                {
                    HtmlNode anchor = tableData[dIndex].SelectNodes("a").FirstOrDefault();

                    if (anchor.SelectNodes("span") != null && anchor.SelectNodes("span").Count > 0)
                    {
                        companyTemp[tempCounter] = anchor.SelectNodes("span").FirstOrDefault().InnerText;
                    }
                    else
                    {
                        companyTemp[tempCounter] = anchor.InnerText;
                    }

                    tempCounter++;

                    if (tempCounter > 10)
                    {
                        FinvizCompany fCompany = new FinvizCompany();
                        fCompany.Ticker = companyTemp[1];
                        fCompany.Company = companyTemp[2];
                        fCompany.Sector = companyTemp[3];
                        fCompany.Industry = companyTemp[4];
                        fCompany.Country = companyTemp[5];
                        fCompany.MarketCap = companyTemp[6];
                        fCompany.PE = companyTemp[7];
                        fCompany.Price = companyTemp[8];
                        fCompany.Change = companyTemp[9];
                        fCompany.Volume = companyTemp[10];

                        results.Add(fCompany);
                        tempCounter = 0;
                        sb.AppendLine(fCompany.ToString());
                    }
                }
            }

            Helpers.outputToFile("bounce_processed", sb.ToString());

            return results;
        }

        public async Task<List<FinvizCompany>> GetOversoldBouncePlayAsync()
        {
            string url = "https://finviz.com/screener.ashx?v=111&f=sh_price_u20,ta_averagetruerange_o0.25,ta_perf_ddown,ta_perf2_d5u,ta_rsi_os40&ft=3";
            StringBuilder sb = new StringBuilder();

            //List<HtmlNodeWrapper> results = new List<HtmlNodeWrapper>();
            List<FinvizCompany> results = new List<FinvizCompany>();
            HtmlWeb web = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc;

            // Check the pagination so that we know how much iteration is needed
            doc = web.Load(url);
            IList<HtmlNode> paginationData = doc.QuerySelectorAll(".screener_pagination");

            int iterationScaler = getNumberOfPages(paginationData);

            for (int index = 1; index < 20 * iterationScaler; index += 20)
            {
                doc = web.Load($"{url}&r={index}");
                Helpers.outputToFile("bounce_raw", doc.DocumentNode.OuterHtml);

                IList<HtmlNode> tableData = doc.QuerySelectorAll(".screener-body-table-nw");

                string[] companyTemp = new string[11];
                int tempCounter = 0;
                //foreach (var tableDatum in tableData)
                for (int dIndex = 0; dIndex < tableData.Count; dIndex++)
                {
                    HtmlNode anchor = tableData[dIndex].SelectNodes("a").FirstOrDefault();

                    if (anchor.SelectNodes("span") != null && anchor.SelectNodes("span").Count > 0)
                    {
                        companyTemp[tempCounter] = anchor.SelectNodes("span").FirstOrDefault().InnerText;
                    }
                    else
                    {
                        companyTemp[tempCounter] = anchor.InnerText;
                    }

                    tempCounter++;

                    if (tempCounter > 10)
                    {
                        FinvizCompany fCompany = new FinvizCompany();
                        fCompany.Ticker = companyTemp[1];
                        fCompany.Company = companyTemp[2];
                        fCompany.Sector = companyTemp[3];
                        fCompany.Industry = companyTemp[4];
                        fCompany.Country = companyTemp[5];
                        fCompany.MarketCap = companyTemp[6];
                        fCompany.PE = companyTemp[7];
                        fCompany.Price = companyTemp[8];
                        fCompany.Change = companyTemp[9];
                        fCompany.Volume = companyTemp[10];

                        results.Add(fCompany);
                        tempCounter = 0;
                        sb.AppendLine(fCompany.ToString());
                    }
                }
            }

            Helpers.outputToFile("bounce_processed", sb.ToString());

            return results;
        }

        // Find short squeeze opportunities
        public List<FinvizCompany> GetShortSqueeze()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb_row = new StringBuilder();

            List<FinvizCompany> results = new List<FinvizCompany>();
            HtmlWeb web = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc;

            // TODO: IF THERE IS ONLY ONE PAGE OF RESULTS THEN THE LAST ITEM GETS DUPLICATED. WE NEED TO 
            // CHECK FOR A HTML ELEMENT THAT INDICATES IF THERE ARE MORE PAGES
            //for (int index = 1; index < 100; index += 20)
            for (int index = 1; index < 20; index += 20)
            {
                doc = web.Load($"https://finviz.com/screener.ashx?v=151&f=sh_short_high,ta_highlow50d_b0to10h&ft=3&o=ticker&r={index}");
                //https://finviz.com/screener.ashx?v=111&f=sh_price_u20,ta_perf_ddown,ta_perf2_d5u,ta_rsi_os40&ft=3&r=21
                //https://finviz.com/screener.ashx?v=111&f=cap_mega&o=ticker
                Helpers.outputToFile("short_squeeze_raw", doc.DocumentNode.OuterHtml);

                // 121 OF THESE
                IList<HtmlNode> tableData = doc.QuerySelectorAll(".screener-body-table-nw");

                sb.Clear();
                string[] companyTemp = new string[11];
                int tempCounter = 0;
                //foreach (var tableDatum in tableData)
                for (int dIndex = 0; dIndex < tableData.Count; dIndex++)
                {
                    HtmlNode anchor = tableData[dIndex].SelectNodes("a").FirstOrDefault();

                    if (anchor.SelectNodes("span") != null && anchor.SelectNodes("span").Count > 0)
                    {
                        companyTemp[tempCounter] = anchor.SelectNodes("span").FirstOrDefault().InnerText;
                    }
                    else
                    {
                        companyTemp[tempCounter] = anchor.InnerText;
                    }

                    tempCounter++;

                    if (tempCounter > 10)
                    {
                        FinvizCompany fCompany = new FinvizCompany();
                        fCompany.Ticker = companyTemp[1];
                        fCompany.Company = companyTemp[2];
                        fCompany.Sector = companyTemp[3];
                        fCompany.Industry = companyTemp[4];
                        fCompany.Country = companyTemp[5];
                        fCompany.MarketCap = companyTemp[6];
                        fCompany.PE = companyTemp[7];
                        fCompany.Price = companyTemp[8];
                        fCompany.Change = companyTemp[9];
                        fCompany.Volume = companyTemp[10];

                        results.Add(fCompany);
                        tempCounter = 0;
                    }
                }
            }

            Helpers.outputToFile("short_squeeze_processed", sb.ToString());

            // Include plus500 listed company so that something shows in the list view:
            FinvizCompany testCompany = new FinvizCompany();
            testCompany.Ticker = "IBM";
            testCompany.Company = "International Bidniz Machines";
            testCompany.Sector = "Electrics";
            testCompany.Industry = "Stuff";
            testCompany.Country = "Everywhere";
            testCompany.MarketCap = "45345";
            testCompany.PE = "4534";
            testCompany.Price = "4";
            testCompany.Change = "5";
            testCompany.Volume = "354634264634";

            //results.Add(testCompany);

            return results;
        }

        public async Task<List<FinvizCompany>> GetShortSqueezeAsync()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb_row = new StringBuilder();

            List<FinvizCompany> results = new List<FinvizCompany>();
            HtmlWeb web = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc;

            // TODO: IF THERE IS ONLY ONE PAGE OF RESULTS THEN THE LAST ITEM GETS DUPLICATED. WE NEED TO 
            // CHECK FOR A HTML ELEMENT THAT INDICATES IF THERE ARE MORE PAGES
            //for (int index = 1; index < 100; index += 20)
            for (int index = 1; index < 20; index += 20)
            {
                doc = web.Load($"https://finviz.com/screener.ashx?v=151&f=sh_short_high,ta_highlow50d_b0to10h&ft=3&o=ticker&r={index}");
                //https://finviz.com/screener.ashx?v=111&f=sh_price_u20,ta_perf_ddown,ta_perf2_d5u,ta_rsi_os40&ft=3&r=21
                //https://finviz.com/screener.ashx?v=111&f=cap_mega&o=ticker
                Helpers.outputToFile("short_squeeze_raw", doc.DocumentNode.OuterHtml);

                // 121 OF THESE
                IList<HtmlNode> tableData = doc.QuerySelectorAll(".screener-body-table-nw");

                sb.Clear();
                string[] companyTemp = new string[11];
                int tempCounter = 0;
                //foreach (var tableDatum in tableData)
                for (int dIndex = 0; dIndex < tableData.Count; dIndex++)
                {
                    HtmlNode anchor = tableData[dIndex].SelectNodes("a").FirstOrDefault();

                    if (anchor.SelectNodes("span") != null && anchor.SelectNodes("span").Count > 0)
                    {
                        companyTemp[tempCounter] = anchor.SelectNodes("span").FirstOrDefault().InnerText;
                    }
                    else
                    {
                        companyTemp[tempCounter] = anchor.InnerText;
                    }

                    tempCounter++;

                    if (tempCounter > 10)
                    {
                        FinvizCompany fCompany = new FinvizCompany();
                        fCompany.Ticker = companyTemp[1];
                        fCompany.Company = companyTemp[2];
                        fCompany.Sector = companyTemp[3];
                        fCompany.Industry = companyTemp[4];
                        fCompany.Country = companyTemp[5];
                        fCompany.MarketCap = companyTemp[6];
                        fCompany.PE = companyTemp[7];
                        fCompany.Price = companyTemp[8];
                        fCompany.Change = companyTemp[9];
                        fCompany.Volume = companyTemp[10];

                        results.Add(fCompany);
                        tempCounter = 0;
                    }
                }
            }

            Helpers.outputToFile("short_squeeze_processed", sb.ToString());

            // Include plus500 listed company so that something shows in the list view:
            FinvizCompany testCompany = new FinvizCompany();
            testCompany.Ticker = "IBM";
            testCompany.Company = "International Bidniz Machines";
            testCompany.Sector = "Electrics";
            testCompany.Industry = "Stuff";
            testCompany.Country = "Everywhere";
            testCompany.MarketCap = "45345";
            testCompany.PE = "4534";
            testCompany.Price = "4";
            testCompany.Change = "5";
            testCompany.Volume = "354634264634";

            //results.Add(testCompany);

            return results;
        }


        private List<FinvizCompany> getResults(string url, string name)
        {
            StringBuilder sb = new StringBuilder();
            List<FinvizCompany> results = new List<FinvizCompany>();
            HtmlWeb web = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc;

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

            for (int index = 1; index < 20 * iterationScaler; index += 20)
            {
                doc = web.Load($"{url}&r={index}");
                Helpers.outputToFile($"{name}_raw", doc.DocumentNode.OuterHtml);

                IList<HtmlNode> tableData = doc.QuerySelectorAll(".screener-views-table");
                // Get specific data (granular)
                HtmlNodeCollection tableTextAllColumns = doc.DocumentNode.SelectNodes("//*[@id='screener-views-table']/tr[5]/td[1]/table/tr/td/table/tr/td");

                HtmlNodeCollection tableTextRows = doc.DocumentNode.SelectNodes("//*[@id='screener-views-table']/tr[5]/td[1]/table/tr/td/table/tr");

                for (int i = 0; i < tableTextRows.Count; i++)
                {
                    var children = tableTextRows[i].GetChildElements();

                    FinvizCompany fCompany = new FinvizCompany();
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

                }
            }

            Helpers.outputToFile($"{name}_processed", sb.ToString());
            Helpers.outputToFile($"{name}_count", results.Count.ToString());

            return results;
        }

        public List<FinvizCompany> TestWebScraper()
        {
            string testHtml = File.ReadAllText("web_scrape_test.txt");

            HtmlWeb web = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(testHtml);


            StringBuilder sb = new StringBuilder();
            StringBuilder sb_row = new StringBuilder();

            //List<HtmlNodeWrapper> results = new List<HtmlNodeWrapper>();
            List<FinvizCompany> results = new List<FinvizCompany>();

            ////HtmlAgilityPack.HtmlDocument doc = web.Load("https://finviz.com/screener.ashx?v=111&f=fa_div_high,fa_pe_low,sh_avgvol_o300");
            ///

            // Check the pagination so that we know how much iteration is needed
            //doc = web.Load($"https://finviz.com/screener.ashx?v=111&f=cap_mega,sec_technology,sh_avgvol_o500&r");
            IList<HtmlNode> paginationData = doc.QuerySelectorAll(".screener_pagination");

            int numberOfPages = getNumberOfPages(paginationData);

            for (int index = 1; index < 20 * numberOfPages; index += 20)
            {
                doc = web.Load($"https://finviz.com/screener.ashx?v=111&f=cap_mega,sec_technology,sh_avgvol_o500&r={index}");
                //https://finviz.com/screener.ashx?v=111&f=sh_price_u20,ta_perf_ddown,ta_perf2_d5u,ta_rsi_os40&ft=3&r=21
                //https://finviz.com/screener.ashx?v=111&f=cap_mega&o=ticker
                Helpers.outputToFile("tech_raw", doc.DocumentNode.OuterHtml);

                IList<HtmlNode> tableData = doc.QuerySelectorAll(".screener-body-table-nw");
                //IList<HtmlNode> paginationData = doc.QuerySelectorAll(".screener_pagination");
                string paginationDataString = paginationData.First().InnerHtml;

                if (paginationData.Count > 0)
                {
                    var nextButton = paginationData.First().SelectNodes("b");

                    foreach (var node in paginationData)
                    {
                        if (node.SelectNodes("b") != null && node.SelectNodes("b").Count < 0)
                        {
                            string thing = node.SelectNodes("b").First().InnerText;
                        }
                    }
                }

                string[] companyTemp = new string[11];
                int tempCounter = 0;
                //foreach (var tableDatum in tableData)
                for (int dIndex = 0; dIndex < tableData.Count; dIndex++)
                {
                    HtmlNode anchor = tableData[dIndex].SelectNodes("a").FirstOrDefault();

                    if (anchor.SelectNodes("span") != null && anchor.SelectNodes("span").Count > 0)
                    {
                        companyTemp[tempCounter] = anchor.SelectNodes("span").FirstOrDefault().InnerText;
                    }
                    else
                    {
                        companyTemp[tempCounter] = anchor.InnerText;
                    }

                    tempCounter++;

                    if (tempCounter > 10)
                    {
                        FinvizCompany fCompany = new FinvizCompany();
                        fCompany.Ticker = companyTemp[1];
                        fCompany.Company = companyTemp[2];
                        fCompany.Sector = companyTemp[3];
                        fCompany.Industry = companyTemp[4];
                        fCompany.Country = companyTemp[5];
                        fCompany.MarketCap = companyTemp[6];
                        fCompany.PE = companyTemp[7];
                        fCompany.Price = companyTemp[8];
                        fCompany.Change = companyTemp[9];
                        fCompany.Volume = companyTemp[10];

                        results.Add(fCompany);
                        tempCounter = 0;
                    }
                }
            }

            Helpers.outputToFile("oversold_processed", sb.ToString());

            return results;

        }

        public List<string> GetCompletePlus500()
        {
            List<string> results = new List<string>();

            // REAL API
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.plus500.com/en/Instruments#Indicesf");
            string page = doc.DocumentNode.OuterHtml;
            Helpers.outputToFile("plus500", page);
            //HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//table[@class='instruments-table']");


            // TEST STRING
            //string testHtml = Helpers.getFromFile("plus500");
            //HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            // TODO: REPLACE WITH REAL HTML
            //doc.LoadHtml(testHtml);
            HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//table[@class='instruments-table']");

            int tableCount = 0;
            foreach (HtmlNode table in htmlNodes)
            {
                HtmlNodeCollection rowCollection = table.SelectNodes("tr");

                if (tableCount > 7 && tableCount < 10)
                {
                    foreach (HtmlNode row in rowCollection)
                    {
                        HtmlNodeCollection dataCollection = row.SelectNodes("td");
                        string symbolTranslated = "";

                        foreach (HtmlNode cell in dataCollection)
                        {
                            string symbol = cell.InnerText.Split(' ')[0].Trim();
                            if (symbol.Contains("-L"))
                            {
                                // TODO: SHOULD THIS HAVE A SUFFIX?
                                symbolTranslated = symbol.Replace("-L", ".LON");
                            }
                            else
                            {
                                symbolTranslated = symbol;
                            }

                            results.Add(symbolTranslated);
                            break;
                        }
                    }
                }
                tableCount++;
            }

            // The symbol names need converting. e.g. "-L" need to change to ".LON"

            StringBuilder sb = new StringBuilder();
            foreach (string item in results)
            {
                sb.AppendLine(item);
            }
            Helpers.outputToFile("plus500symbols", sb.ToString());

            return results;
        }

        public Dictionary<string, string> getFTSE100()
        {
            Dictionary<string, string> results = new Dictionary<string, string>();

            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.fidelity.co.uk/shares/ftse-100/");
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

        #region - YAHOO FINANCE -
        public double GetCurrentValue(string symbol)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb_row = new StringBuilder();

            List<FinvizCompany> results = new List<FinvizCompany>();
            HtmlWeb web = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();

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

        private int getNumberOfPages(IList<HtmlNode> paginationData)
        {
            int numberOfPages = 0;
            if (paginationData != null && paginationData.Count > 0)
            {
                var pageNumers = paginationData.First().SelectNodes("a");

                if (pageNumers != null && pageNumers.Count > 0)
                {
                    numberOfPages = pageNumers.Count - 1;
                }
            }
            return numberOfPages;
        }
    }



}
