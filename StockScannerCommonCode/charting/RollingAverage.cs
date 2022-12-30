using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScannerCommonCode.charting
{
    public class RollingAverage
    {
        private int period;
        private bool exponential;
        private int rollingIndex;
        private double rollingSum;
        private double average;
        private double[] rollingSample;
        private bool sampleFull;

        // EMA
        // https://www.investopedia.com/terms/e/ema.asp
        // https://stackoverflow.com/questions/8450020/calculate-exponential-moving-average-on-a-queue-in-c-sharp

        private double smoothing;
        private double previousEMA;
        private bool firstRun;
        private int count;



        public RollingAverage(int period, bool exponential, double smoothing = 2)
        {
            this.period = period;
            this.exponential = exponential;
            this.smoothing = smoothing;
            this.rollingIndex = 0;
            this.rollingSum = 0;
            this.average = 0;
            this.rollingSample = new double[period];
            this.firstRun = true;
            this.sampleFull = false;
            this.count = 0;
        }

        public void advance(double newValue)
        {
            // https://www.youtube.com/watch?v=ezcwBDsDviE
            count++;
            double sma = getSMA(newValue);

            if (this.exponential)
            {
                if (this.firstRun) this.previousEMA = sma;
                double multiplier = this.smoothing / (this.period + 1);
                this.average = (newValue - this.previousEMA) * multiplier + this.previousEMA;
                this.previousEMA = this.average;
            }
            else
            {
                this.average = sma;
            }

            rollingIndex = (rollingIndex + 1) % period;
            if (rollingIndex == 0)
            {
                sampleFull = true;
                this.firstRun = false;
            }
        }

        private double getSMA(double newValue)
        {
            double ave = 0;
            rollingSum = rollingSum - rollingSample[rollingIndex] + newValue;
            rollingSample[rollingIndex] = newValue;
            if (count < period)
            {
                ave = rollingSum / count;
            }
            else
            {
                ave = rollingSum / period;
            }
            return ave;
        }

        public double getAverage()
        {
            return this.average;
        }

        public bool getSampleFull()
        {
            return this.sampleFull;
        }

        public double[] getRollingSample()
        {
            return this.rollingSample;
        }

    }
}
