using HtmlAgilityPack;
using SeldonStockScannerAPI.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeldonStockScannerAPI.WebScraper
{
    public interface IWebScraper
    {
        string GetTestHTML();

        List<string> GetCompletePlus500();

        // Finviz website
        List<FinvizCompanyEntity> GetCustomFinvizScan(string url, string name);

        // Fidelity website
        Dictionary<string, string> GetFTSE100();

        // Yahoo website
        double GetCurrentValue(string symbol);
    }
}
