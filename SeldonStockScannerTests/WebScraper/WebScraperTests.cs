using SeldonStockScannerAPI.WebScraper;
using Moq;
using HtmlAgilityPack;
using System.Reflection;
using SeldonStockScannerAPI.Connections;

namespace SeldonStockScannerTests.WebScraper
{
    public class WebScraperTests
    {
        // https://stackoverflow.com/questions/7114169/asp-net-mvc-unit-testing-html-that-is-generated-by-tagbuilders

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDummyWebCall()
        {
            // https://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file
            // https://stackoverflow.com/questions/6080596/how-can-i-load-this-file-into-an-nunit-test
            //string resourcePath = assembly.GetManifestResourceNames().Single(s => s.EndsWith("SeldonStockScannerTests.Resources.WebScraper.example.txt"));
            //var assembly = Assembly.GetExecutingAssembly();

            string thing = SeldonStockScannerTests.Properties.Resources.example1;
            Mock<IWebConnection> mockeWebConnetion = new Mock<IWebConnection>();
            mockeWebConnetion.Setup(m => m.GetWebsiteByUrl("test")).Returns(() =>
            {
                var doc = new HtmlDocument();
                //doc.LoadHtml(thing);
                doc.LoadHtml("ding dong");
                return doc;
            });

            IWebScraper scraper = new SeldonWebScraper(mockeWebConnetion.Object);

            string testResult = scraper.GetTestHTML();

            Assert.IsTrue(testResult == "ding dong");


        }

        [Test]
        public void TestPlus500WebScrape() 
        {
            string plus500AllInstrumentsHTML = SeldonStockScannerTests.Properties.Resources.plus500allinstruments;
            Mock<IWebConnection> mockeWebConnetion = new Mock<IWebConnection>();
            mockeWebConnetion.Setup(m => m.GetWebsiteByUrl("https://www.plus500.com/en/instruments#indicesf")).Returns(() =>
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(plus500AllInstrumentsHTML);
                return doc;
            });

            IWebScraper scraper = new SeldonWebScraper(mockeWebConnetion.Object);
            List<string> results = scraper.GetCompletePlus500();

            Assert.IsTrue(results.Count == 2141);
        }

    }
}