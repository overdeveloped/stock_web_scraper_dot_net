using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockScannerCommonCode.model;
using StockScannerCommonCode.Strategies;

namespace StockScannerCommonCode.charting
{
    public class ChartData
    {
        private List<Candle> priceAction;
        private Dictionary<EnumIndicator, bool> indicators;

        public ChartData(List<Candle> priceAction, Dictionary<EnumIndicator, bool> indicators)
        {
            this.priceAction = priceAction;
            this.indicators = indicators;
        }

        public List<Candle> GetPriceAction()
        {
            return this.priceAction;
        }

        public void SetPriceAction(List<Candle> priceAction)
        {
            this.priceAction = priceAction;
        }

        public Dictionary<EnumIndicator, bool> GetIndicators()
        {
            return this.indicators;
        }
    }
}
