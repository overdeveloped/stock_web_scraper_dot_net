using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using StockScannerCommonCode.model;
using StockScannerCommonCode.Strategies;

namespace StockScannerCommonCode.charting
{
    // HANDY TIPS ON CANDLESTICK SERIES:
    // https://stackoverflow.com/questions/12983199/candlestick-multiple-y-values
    // DOCS:
    // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.datavisualization.charting.series?view=netframework-4.8
    // Multiple chart areas:
    // https://stackoverflow.com/questions/17303378/creating-multiple-charts-and-the-relation-between-chart-series-chartarea


    public class DomChart
    {
        private Chart formChart;
        private DataTable dataTableMarket = new DataTable();
        //private DataTable dataTableMACD = new DataTable();
        private List<ChartArea> chartAreas = new List<ChartArea>();
        private List<Series> series = new List<Series>();





        private Series seriesPrice = new Series("PriceAction");
        private Series sma50Price = new Series("MA50");
        private Series MACD = new Series("MACD");
        private Series HIST = new Series("HIST");
        private Series ema50signal = new Series("Test Signal");
        private Series sma200trend = new Series("Trend Line");
        private Series signal = new Series("Signal");
        private Series signalBuy = new Series("BUY");
        private Series signalStop = new Series("STOP");
        private Series signalProfit = new Series("PROFIT");
        private Series signalSell = new Series("SELL");
        private ChartArea chartAreaMarket = new ChartArea("Market");
        private ChartArea chartAreaMACD = new ChartArea("MACD");
        // Indicators
        private RollingAverage sma50 = new RollingAverage(50, false);
        private RollingAverage sma200 = new RollingAverage(200, false);
        private RollingAverage ema50 = new RollingAverage(50, true, 2);
        private RollingAverage ema10 = new RollingAverage(10, true, 2);
        // MACD
        private RollingAverage ema12 = new RollingAverage(12, true, 2);
        private RollingAverage ema26 = new RollingAverage(26, true, 2);
        // Signal
        private RollingAverage ema9 = new RollingAverage(9, true, 2);
        public double[] rolling50Sample = new double[50];
        public int rolling50Index = 0;
        public double rolling50Sum = 0;
        public double ave = 0;
        public bool sampleFull = false;

        public DomChart(Chart formChart)
        {
            this.formChart = formChart;
            this.formChart.ChartAreas.Clear();
            this.formChart.Series.Clear();
            this.formChart.Legends.Clear();

            initialise();
        }

        public DomChart(Chart formChart, DataTable dataTable)
        {
            this.formChart = formChart;
            this.dataTableMarket = dataTable;
            initialise();
        }

        private void initialise()
        {
            SetupFormChart();
            //SetChartAreas();
            SetChartTitle("Price Action");
            //SetLegend();
            //SetSeries();
            //SetData();
        }

        public DataTable GetDatatable()
        {
            return this.dataTableMarket;
        }

        private void SetupFormChart()
        {
            // Setting the properties of the chart
            //The background colour of the chart
            formChart.BackColor = Color.FromArgb(211, 223, 240);
            //The gradient of the background colour of the chart
            formChart.BackGradientStyle = GradientStyle.TopBottom;
            //The border colour of the chart,
            formChart.BorderlineColor = Color.FromArgb(26, 59, 105);
            //The border line style of the chart
            formChart.BorderlineDashStyle = ChartDashStyle.Solid;
            //The width of the chart border line
            formChart.BorderlineWidth = 4;
            //The skin of the chart border
            formChart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
        }

        private void AddChartArea()
        {
            ChartArea chartArea = new ChartArea("Price");
            chartArea.AxisY.Interval = 20;
            //chartArea.AxisY.LabelStyle.Format = "C";
            chartArea.BackColor = Color.FromArgb(64, 165, 191, 228);
            chartArea.BackGradientStyle = GradientStyle.TopBottom;
            chartArea.BackSecondaryColor = Color.White;
            chartArea.BorderColor = Color.FromArgb(255, 255, 0, 255);
            chartArea.ShadowColor = Color.Transparent;
            chartArea.AxisX.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartArea.AxisY.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartArea.AxisX.LineWidth = 2;
            chartArea.AxisY.LineWidth = 2;
            chartArea.AxisY.Title = "Price";
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisX.Interval = 0; //Set to 0 means automatically assigned by the control
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Days;
            chartArea.AxisX.LabelStyle.IsStaggered = true;
            //chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm";
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd";
            //chartArea.AxisX.LabelStyle.Angle = 45;
            chartArea.AxisX.IsLabelAutoFit = true;
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.Interval = 0;
            chartArea.CursorX.IntervalOffset = 0;
            chartArea.CursorX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisX.ScrollBar.IsPositionedInside = false;

            chartArea.AxisY.ScaleBreakStyle.Enabled = true;
            chartArea.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 47;
            chartArea.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            chartArea.AxisY.ScaleBreakStyle.Spacing = 10;
            chartArea.AxisY.ScaleBreakStyle.LineColor = Color.Black;
            chartArea.AxisY.ScaleBreakStyle.LineWidth = 1;
            chartArea.CursorY.IsUserEnabled = true;
            chartArea.CursorY.IsUserSelectionEnabled = true;
            chartArea.CursorY.Interval = 0;
            chartArea.CursorY.IntervalOffset = 0;
            chartArea.CursorY.IntervalType = DateTimeIntervalType.Hours;

            chartArea.AxisY.ScaleView.Zoomable = true;

            //chartAreaMarket.AxisX.Minimum = new DateTime(2021, 1, 1).ToOADate();
            //chartAreaMarket.AxisY.Minimum = 120;
            //chartAreaMarket.AxisY.Maximum = 180;

            this.chartAreas.Add(chartArea);
        }


        private void SetChartTitle(string titleString)
        {
            // Setting the title of the chart
            Title title = new Title();
            //title content 
            title.Text = titleString;
            //Font of title
            title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            //Title font colour
            title.ForeColor = Color.FromArgb(26, 59, 105);
            //Title shadow colour
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            //Title shadow offset
            title.ShadowOffset = 3;
            formChart.Titles.Add(title);
        }

        private void SetLegend()
        {
            // Setting the attributes of the legend
            //Note that you need to delete the legend that comes with the original control
            this.formChart.Legends.Clear();

            Legend legend = new Legend("Default");
            legend.Alignment = StringAlignment.Center;
            legend.Docking = Docking.Bottom;
            legend.LegendStyle = LegendStyle.Column;
            //this.myChart.Legends.Add(legend);

            // Add header separator of type line
            legend.HeaderSeparator = LegendSeparatorStyle.Line;
            legend.HeaderSeparatorColor = Color.Gray;

            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Colour";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            //myChart.Legends["Default"].CellColumns.Add(firstColumn);

            // Add Legend Text column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Name";
            secondColumn.Text = "#LEGENDTEXT";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            //myChart.Legends["Default"].CellColumns.Add(secondColumn);

            // Add AVG cell column
            LegendCellColumn avgColumn = new LegendCellColumn();
            avgColumn.Text = "#AVG{N2}";
            avgColumn.HeaderText = "Avg";
            avgColumn.Name = "AvgColumn";
            avgColumn.HeaderBackColor = Color.WhiteSmoke;
            //myChart.Legends["Default"].CellColumns.Add(avgColumn);

            // Add Total cell column
            LegendCellColumn totalColumn = new LegendCellColumn();
            totalColumn.Text = "#TOTAL{N1}";
            totalColumn.HeaderText = "Total";
            totalColumn.Name = "TotalColumn";
            totalColumn.HeaderBackColor = Color.WhiteSmoke;
            //myChart.Legends["Default"].CellColumns.Add(totalColumn);

            // Set Min cell column attributes
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Text = "#MIN{N1}";
            minColumn.HeaderText = "Min";
            minColumn.Name = "MinColumn";
            minColumn.HeaderBackColor = Color.WhiteSmoke;
            //myChart.Legends["Default"].CellColumns.Add(minColumn);

            // Set Max cell column attributes
            LegendCellColumn maxColumn = new LegendCellColumn();
            maxColumn.Text = "#MAX{N1}";
            maxColumn.HeaderText = "Max";
            maxColumn.Name = "MaxColumn";
            maxColumn.HeaderBackColor = Color.WhiteSmoke;
            //myChart.Legends["Default"].CellColumns.Add(maxColumn);
        }




        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ShowPriceActionChart(List<CandleMACD> data)
        {
            if (data.Count() <= 0)
            {
                return;
            }

            // CHART AREA
            formChart.ChartAreas.Clear();
            ChartArea chartArea = new ChartArea("Price");
            chartArea.AxisY.Interval = 20;
            //chartArea.AxisY.LabelStyle.Format = "C";
            chartArea.BackColor = Color.FromArgb(64, 165, 191, 228);
            chartArea.BackGradientStyle = GradientStyle.TopBottom;
            chartArea.BackSecondaryColor = Color.White;
            chartArea.BorderColor = Color.FromArgb(255, 255, 0, 255);
            chartArea.ShadowColor = Color.Transparent;
            chartArea.AxisX.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartArea.AxisY.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartArea.AxisX.LineWidth = 2;
            chartArea.AxisY.LineWidth = 2;
            chartArea.AxisY.Title = "Price";
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisX.Interval = 0; //Set to 0 means automatically assigned by the control
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Days;
            chartArea.AxisX.LabelStyle.IsStaggered = true;
            //chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm";
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd";
            //chartArea.AxisX.LabelStyle.Angle = 45;
            chartArea.AxisX.IsLabelAutoFit = true;
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.Interval = 0;
            chartArea.CursorX.IntervalOffset = 0;
            chartArea.CursorX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisX.ScrollBar.IsPositionedInside = false;

            chartArea.AxisY.ScaleBreakStyle.Enabled = true;
            chartArea.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 47;
            chartArea.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            chartArea.AxisY.ScaleBreakStyle.Spacing = 10;
            chartArea.AxisY.ScaleBreakStyle.LineColor = Color.Black;
            chartArea.AxisY.ScaleBreakStyle.LineWidth = 1;
            chartArea.CursorY.IsUserEnabled = true;
            chartArea.CursorY.IsUserSelectionEnabled = true;
            chartArea.CursorY.Interval = 0;
            chartArea.CursorY.IntervalOffset = 0;
            chartArea.CursorY.IntervalType = DateTimeIntervalType.Hours;

            chartArea.AxisY.ScaleView.Zoomable = true;

            formChart.ChartAreas.Add(chartArea);

            double marketMin = data.AsEnumerable().Min(x => x.price_low);
            double marketMax = data.AsEnumerable().Max(x => x.price_high);
            double padding = (marketMax - marketMin) / 100 * 50;
            chartArea.AxisY.Minimum = marketMin - padding;
            chartArea.AxisY.Maximum = marketMax + padding;


            // SERIES
            formChart.Series.Clear();

            Series priceSeries = new Series();
            priceSeries.Points.Clear();
            priceSeries.ChartType = SeriesChartType.Candlestick;
            priceSeries.BorderWidth = 1;
            priceSeries.ShadowOffset = 0;
            priceSeries.IsVisibleInLegend = true;
            priceSeries.IsValueShownAsLabel = false;
            priceSeries.Color = Color.MediumPurple;
            priceSeries.XValueType = ChartValueType.DateTime;
            priceSeries.MarkerStyle = MarkerStyle.None;
            priceSeries.MarkerSize = 5;
            priceSeries["PriceUpColor"] = "Green";
            priceSeries["PriceDownColor"] = "Red";
            //priceSeries.ChartArea = "Price";
            // This gets rid of the gaps in the data:
            priceSeries.IsXValueIndexed = true;
            formChart.Series.Add(priceSeries);

            Series sma200Series = new Series();
            sma200Series.Points.Clear();
            sma200Series.ChartType = SeriesChartType.Line;
            priceSeries.IsValueShownAsLabel = false;
            sma200Series.Color = Color.Red;
            sma200Series.YValueType = ChartValueType.Double;
            sma200Series.XValueType = ChartValueType.DateTime;
            priceSeries.ChartArea = "Price";
            // This gets rid of the gaps in the data:
            sma200Series.IsXValueIndexed = true;
            formChart.Series.Add(sma200Series);



            foreach (CandleMACD row in data)
            {
                priceSeries.Points.AddXY(row.date_time, row.price_high, row.price_low, row.price_open, row.price_close);
                sma200Series.Points.AddXY(row.date_time, row.sma200);
            }

        }

        public void ShowPriceActionAndMacd(List<CandleMACD> data)
        {
            if (data.Count() <= 0)
            {
                return;
            }

            // CHART AREA
            formChart.ChartAreas.Clear();

            ChartArea chartAreaPrice = new ChartArea("ChartAreaPrice");
            chartAreaPrice.AxisY.Interval = 20;
            //chartArea.AxisY.LabelStyle.Format = "C";
            chartAreaPrice.BackColor = Color.FromArgb(64, 165, 191, 228);
            chartAreaPrice.BackGradientStyle = GradientStyle.TopBottom;
            chartAreaPrice.BackSecondaryColor = Color.White;
            chartAreaPrice.BorderColor = Color.FromArgb(255, 255, 0, 255);
            chartAreaPrice.ShadowColor = Color.Transparent;
            chartAreaPrice.AxisX.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaPrice.AxisY.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaPrice.AxisX.LineWidth = 2;
            chartAreaPrice.AxisY.LineWidth = 2;
            chartAreaPrice.AxisY.Title = "Price";
            chartAreaPrice.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaPrice.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaPrice.AxisX.MajorGrid.LineWidth = 1;
            chartAreaPrice.AxisY.MajorGrid.LineWidth = 1;
            chartAreaPrice.AxisX.MajorTickMark.Enabled = false;
            chartAreaPrice.AxisY.MajorTickMark.Enabled = false;
            chartAreaPrice.AxisX.MajorGrid.Enabled = false;
            chartAreaPrice.AxisY.MajorGrid.Enabled = false;
            chartAreaPrice.AxisX.Interval = 0; //Set to 0 means automatically assigned by the control
            chartAreaPrice.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartAreaPrice.AxisX.IntervalType = DateTimeIntervalType.Days;
            chartAreaPrice.AxisX.LabelStyle.IsStaggered = true;
            //chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            chartAreaPrice.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm";
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd";
            //chartArea.AxisX.LabelStyle.Angle = 45;
            chartAreaPrice.AxisX.IsLabelAutoFit = true;
            chartAreaPrice.CursorX.IsUserEnabled = true;
            chartAreaPrice.CursorX.IsUserSelectionEnabled = true;
            chartAreaPrice.CursorX.Interval = 0;
            chartAreaPrice.CursorX.IntervalOffset = 0;
            chartAreaPrice.CursorX.IntervalType = DateTimeIntervalType.Hours;
            chartAreaPrice.AxisX.ScaleView.Zoomable = true;
            chartAreaPrice.AxisX.ScrollBar.IsPositionedInside = false;

            chartAreaPrice.AxisY.ScaleBreakStyle.Enabled = true;
            chartAreaPrice.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 47;
            chartAreaPrice.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            chartAreaPrice.AxisY.ScaleBreakStyle.Spacing = 10;
            chartAreaPrice.AxisY.ScaleBreakStyle.LineColor = Color.Black;
            chartAreaPrice.AxisY.ScaleBreakStyle.LineWidth = 1;
            chartAreaPrice.CursorY.IsUserEnabled = true;
            chartAreaPrice.CursorY.IsUserSelectionEnabled = true;
            chartAreaPrice.CursorY.Interval = 0;
            chartAreaPrice.CursorY.IntervalOffset = 0;
            chartAreaPrice.CursorY.IntervalType = DateTimeIntervalType.Hours;

            chartAreaPrice.AxisY.ScaleView.Zoomable = true;

            double marketMin = data.AsEnumerable().Min(x => x.price_low);
            double marketMax = data.AsEnumerable().Max(x => x.price_high);
            double padding = (marketMax - marketMin) / 100 * 50;
            chartAreaPrice.AxisY.Minimum = marketMin - padding;
            chartAreaPrice.AxisY.Maximum = marketMax + padding;

            formChart.ChartAreas.Add(chartAreaPrice);


            ChartArea chartAreaMACD = new ChartArea("ChartAreaMACD");
            chartAreaMACD.AxisY.Interval = 20;
            //chartArea.AxisY.LabelStyle.Format = "C";
            chartAreaMACD.BackColor = Color.FromArgb(64, 165, 191, 228);
            chartAreaMACD.BackGradientStyle = GradientStyle.TopBottom;
            chartAreaMACD.BackSecondaryColor = Color.White;
            chartAreaMACD.BorderColor = Color.FromArgb(255, 255, 0, 255);
            chartAreaMACD.ShadowColor = Color.Transparent;
            chartAreaMACD.AxisX.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaMACD.AxisY.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaMACD.AxisX.LineWidth = 2;
            chartAreaMACD.AxisY.LineWidth = 2;
            chartAreaMACD.AxisY.Title = "MACD";
            chartAreaMACD.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaMACD.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaMACD.AxisX.MajorGrid.LineWidth = 1;
            chartAreaMACD.AxisY.MajorGrid.LineWidth = 1;
            chartAreaMACD.AxisX.MajorTickMark.Enabled = false;
            chartAreaMACD.AxisY.MajorTickMark.Enabled = false;
            //chartAreaMACD.AxisX.MajorGrid.Enabled = false;
            //chartAreaMACD.AxisY.MajorGrid.Enabled = false;
            chartAreaMACD.AxisX.Interval = 0; //Set to 0 means automatically assigned by the control
            chartAreaMACD.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartAreaMACD.AxisX.IntervalType = DateTimeIntervalType.Days;
            chartAreaMACD.AxisX.LabelStyle.IsStaggered = true;
            //chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            chartAreaMACD.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm";
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd";
            //chartArea.AxisX.LabelStyle.Angle = 45;
            chartAreaMACD.AxisX.IsLabelAutoFit = true;
            chartAreaMACD.CursorX.IsUserEnabled = true;
            chartAreaMACD.CursorX.IsUserSelectionEnabled = true;
            chartAreaMACD.CursorX.Interval = 0;
            chartAreaMACD.CursorX.IntervalOffset = 0;
            chartAreaMACD.CursorX.IntervalType = DateTimeIntervalType.Hours;
            chartAreaMACD.AxisX.ScaleView.Zoomable = true;
            chartAreaMACD.AxisX.ScrollBar.IsPositionedInside = false;

            chartAreaMACD.AxisY.ScaleBreakStyle.Enabled = true;
            chartAreaMACD.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 47;
            chartAreaMACD.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            chartAreaMACD.AxisY.ScaleBreakStyle.Spacing = 10;
            chartAreaMACD.AxisY.ScaleBreakStyle.LineColor = Color.Black;
            chartAreaMACD.AxisY.ScaleBreakStyle.LineWidth = 1;
            chartAreaMACD.CursorY.IsUserEnabled = true;
            chartAreaMACD.CursorY.IsUserSelectionEnabled = true;
            chartAreaMACD.CursorY.Interval = 0;
            chartAreaMACD.CursorY.IntervalOffset = 0;
            chartAreaMACD.CursorY.IntervalType = DateTimeIntervalType.Hours;

            chartAreaMACD.AxisY.ScaleView.Zoomable = true;

            formChart.ChartAreas.Add(chartAreaMACD);

            marketMin = data.AsEnumerable().Min(x => x.macd);
            marketMax = data.AsEnumerable().Max(x => x.macd);
            padding = (marketMax - marketMin) / 100 * 50;
            chartAreaMACD.AxisY.Minimum = marketMin - padding;
            chartAreaMACD.AxisY.Maximum = marketMax + padding;


            // SERIES
            formChart.Series.Clear();

            Series priceSeries = new Series();
            priceSeries.Points.Clear();
            priceSeries.ChartType = SeriesChartType.Candlestick;
            priceSeries.BorderWidth = 1;
            priceSeries.ShadowOffset = 0;
            priceSeries.IsVisibleInLegend = true;
            priceSeries.IsValueShownAsLabel = false;
            priceSeries.Color = Color.MediumPurple;
            priceSeries.XValueType = ChartValueType.DateTime;
            priceSeries.MarkerStyle = MarkerStyle.None;
            priceSeries.MarkerSize = 5;
            priceSeries["PriceUpColor"] = "Green";
            priceSeries["PriceDownColor"] = "Red";
            //priceSeries.ChartArea = "Price";
            // This gets rid of the gaps in the data:
            priceSeries.IsXValueIndexed = true;
            formChart.Series.Add(priceSeries);

            Series sma200Series = new Series();
            sma200Series.Points.Clear();
            sma200Series.ChartType = SeriesChartType.Line;
            priceSeries.IsValueShownAsLabel = false;
            sma200Series.Color = Color.Red;
            sma200Series.YValueType = ChartValueType.Double;
            sma200Series.XValueType = ChartValueType.DateTime;
            priceSeries.ChartArea = "ChartAreaPrice";
            // This gets rid of the gaps in the data:
            sma200Series.IsXValueIndexed = true;
            formChart.Series.Add(sma200Series);

            Series signalSeries = new Series();
            signalSeries.ChartType = SeriesChartType.Line;
            signalSeries.IsValueShownAsLabel = false;
            signalSeries.Color = Color.Blue;
            signalSeries.YValueType = ChartValueType.Double;
            signalSeries.XValueType = ChartValueType.DateTime;
            signalSeries.Points.Clear();
            signalSeries.ChartArea = "ChartAreaMACD";
            signalSeries.IsXValueIndexed = true;
            formChart.Series.Add(signalSeries);

            Series MACDseries = new Series();
            MACDseries.ChartType = SeriesChartType.Line;
            MACDseries.IsValueShownAsLabel = false;
            MACDseries.Color = Color.Magenta;
            MACDseries.YValueType = ChartValueType.Double;
            MACDseries.XValueType = ChartValueType.DateTime;
            MACDseries.Points.Clear();
            MACDseries.ChartArea = "ChartAreaMACD";
            MACDseries.IsXValueIndexed = true;
            formChart.Series.Add(MACDseries);

            Series histseries = new Series();
            histseries.ChartType = SeriesChartType.Column;
            histseries.IsValueShownAsLabel = false;
            histseries.Color = Color.Gray;
            histseries.YValueType = ChartValueType.Double;
            histseries.XValueType = ChartValueType.DateTime;
            histseries.Points.Clear();
            histseries.ChartArea = "ChartAreaMACD";
            histseries.IsXValueIndexed = true;
            formChart.Series.Add(histseries);



            foreach (CandleMACD row in data)
            {
                priceSeries.Points.AddXY(row.date_time, row.price_high, row.price_low, row.price_open, row.price_close);
                sma200Series.Points.AddXY(row.date_time, row.sma200);
                signalSeries.Points.AddXY(row.date_time, row.signal);
                MACDseries.Points.AddXY(row.date_time, row.macd);
                histseries.Points.AddXY(row.date_time, row.hist);
            }


        }

        public void ShowPriceActionAndTradeSignalsOLD(List<Candle> data, List<TradeSignal> tradeSetups)
        {
            if (data.Count() <= 0)
            {
                return;
            }

            // CHART AREA
            formChart.ChartAreas.Clear();

            ChartArea chartAreaPrice = new ChartArea("ChartAreaPrice");
            chartAreaPrice.AxisY.Interval = 20;
            //chartArea.AxisY.LabelStyle.Format = "C";
            chartAreaPrice.BackColor = Color.FromArgb(64, 165, 191, 228);
            chartAreaPrice.BackGradientStyle = GradientStyle.TopBottom;
            chartAreaPrice.BackSecondaryColor = Color.White;
            chartAreaPrice.BorderColor = Color.FromArgb(255, 255, 0, 255);
            chartAreaPrice.ShadowColor = Color.Transparent;
            chartAreaPrice.AxisX.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaPrice.AxisY.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaPrice.AxisX.LineWidth = 2;
            chartAreaPrice.AxisY.LineWidth = 2;
            chartAreaPrice.AxisY.Title = "Price";
            chartAreaPrice.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaPrice.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaPrice.AxisX.MajorGrid.LineWidth = 1;
            chartAreaPrice.AxisY.MajorGrid.LineWidth = 1;
            chartAreaPrice.AxisX.MajorTickMark.Enabled = false;
            chartAreaPrice.AxisY.MajorTickMark.Enabled = false;
            chartAreaPrice.AxisX.MajorGrid.Enabled = false;
            chartAreaPrice.AxisY.MajorGrid.Enabled = true;
            chartAreaPrice.AxisX.Interval = 0; //Set to 0 means automatically assigned by the control
            chartAreaPrice.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartAreaPrice.AxisX.IntervalType = DateTimeIntervalType.Days;
            chartAreaPrice.AxisX.LabelStyle.IsStaggered = true;
            //chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            chartAreaPrice.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm";
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd";
            //chartArea.AxisX.LabelStyle.Angle = 45;
            chartAreaPrice.AxisX.IsLabelAutoFit = true;
            chartAreaPrice.CursorX.IsUserEnabled = true;
            chartAreaPrice.CursorX.IsUserSelectionEnabled = true;
            chartAreaPrice.CursorX.Interval = 0;
            chartAreaPrice.CursorX.IntervalOffset = 0;
            chartAreaPrice.CursorX.IntervalType = DateTimeIntervalType.Hours;
            chartAreaPrice.AxisX.ScaleView.Zoomable = true;
            chartAreaPrice.AxisX.ScrollBar.IsPositionedInside = false;

            chartAreaPrice.AxisY.ScaleBreakStyle.Enabled = true;
            chartAreaPrice.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 47;
            chartAreaPrice.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            chartAreaPrice.AxisY.ScaleBreakStyle.Spacing = 10;
            chartAreaPrice.AxisY.ScaleBreakStyle.LineColor = Color.Black;
            chartAreaPrice.AxisY.ScaleBreakStyle.LineWidth = 1;
            chartAreaPrice.CursorY.IsUserEnabled = true;
            chartAreaPrice.CursorY.IsUserSelectionEnabled = true;
            chartAreaPrice.CursorY.Interval = 0;
            chartAreaPrice.CursorY.IntervalOffset = 0;
            chartAreaPrice.CursorY.IntervalType = DateTimeIntervalType.Hours;

            chartAreaPrice.AxisY.ScaleView.Zoomable = true;

            double marketMin = data.AsEnumerable().Min(x => x.price_low);
            double marketMax = data.AsEnumerable().Max(x => x.price_high);
            double padding = (marketMax - marketMin) / 100 * 50;
            chartAreaPrice.AxisY.Minimum = marketMin - padding;
            chartAreaPrice.AxisY.Maximum = marketMax + padding;

            formChart.ChartAreas.Add(chartAreaPrice);


            ChartArea chartAreaMACD = new ChartArea("ChartAreaMACD");
            chartAreaMACD.AlignWithChartArea = "ChartAreaPrice";
            chartAreaMACD.AxisY.Interval = 20;
            //chartArea.AxisY.LabelStyle.Format = "C";
            chartAreaMACD.BackColor = Color.FromArgb(64, 165, 191, 228);
            chartAreaMACD.BackGradientStyle = GradientStyle.TopBottom;
            chartAreaMACD.BackSecondaryColor = Color.White;
            chartAreaMACD.BorderColor = Color.FromArgb(255, 255, 0, 255);
            chartAreaMACD.ShadowColor = Color.Transparent;
            chartAreaMACD.AxisX.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaMACD.AxisY.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaMACD.AxisX.LineWidth = 2;
            chartAreaMACD.AxisY.LineWidth = 2;
            chartAreaMACD.AxisY.Title = "MACD";
            chartAreaMACD.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaMACD.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaMACD.AxisX.MajorGrid.LineWidth = 1;
            chartAreaMACD.AxisY.MajorGrid.LineWidth = 1;
            chartAreaMACD.AxisX.MajorTickMark.Enabled = false;
            chartAreaMACD.AxisY.MajorTickMark.Enabled = true;
            chartAreaMACD.AxisY.LabelStyle.Format = "0.00";
            //chartAreaMACD.AxisX.MajorGrid.Enabled = false;
            //chartAreaMACD.AxisY.MajorGrid.Enabled = false;
            chartAreaMACD.AxisX.Interval = 0; //Set to 0 means automatically assigned by the control
            chartAreaMACD.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartAreaMACD.AxisX.IntervalType = DateTimeIntervalType.Days;
            chartAreaMACD.AxisX.LabelStyle.IsStaggered = true;
            //chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            chartAreaMACD.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm";
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd";
            //chartArea.AxisX.LabelStyle.Angle = 45;
            chartAreaMACD.AxisX.IsLabelAutoFit = true;
            chartAreaMACD.CursorX.IsUserEnabled = true;
            chartAreaMACD.CursorX.IsUserSelectionEnabled = true;
            chartAreaMACD.CursorX.Interval = 0;
            chartAreaMACD.CursorX.IntervalOffset = 0;
            chartAreaMACD.CursorX.IntervalType = DateTimeIntervalType.Hours;
            chartAreaMACD.AxisX.ScaleView.Zoomable = true;
            chartAreaMACD.AxisX.ScrollBar.IsPositionedInside = false;

            chartAreaMACD.AxisY.ScaleBreakStyle.Enabled = true;
            chartAreaMACD.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 47;
            chartAreaMACD.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            chartAreaMACD.AxisY.ScaleBreakStyle.Spacing = 10;
            chartAreaMACD.AxisY.ScaleBreakStyle.LineColor = Color.Black;
            chartAreaMACD.AxisY.ScaleBreakStyle.LineWidth = 1;
            chartAreaMACD.CursorY.IsUserEnabled = true;
            chartAreaMACD.CursorY.IsUserSelectionEnabled = true;
            chartAreaMACD.CursorY.Interval = 0;
            chartAreaMACD.CursorY.IntervalOffset = 0;
            chartAreaMACD.CursorY.IntervalType = DateTimeIntervalType.Hours;

            chartAreaMACD.AxisY.ScaleView.Zoomable = true;

            formChart.ChartAreas.Add(chartAreaMACD);

            //marketMin = data.AsEnumerable().Min(x => x.macd);
            //marketMax = data.AsEnumerable().Max(x => x.macd);
            //padding = (marketMax - marketMin) / 100 * 50;
            //chartAreaMACD.AxisY.Minimum = marketMin - padding;
            //chartAreaMACD.AxisY.Maximum = marketMax + padding;


            // SERIES
            formChart.Series.Clear();

            Series priceSeries = new Series();
            priceSeries.Points.Clear();
            priceSeries.ChartType = SeriesChartType.Candlestick;
            priceSeries.BorderWidth = 1;
            priceSeries.ShadowOffset = 0;
            priceSeries.IsVisibleInLegend = true;
            priceSeries.IsValueShownAsLabel = false;
            priceSeries.Color = Color.MediumPurple;
            priceSeries.XValueType = ChartValueType.DateTime;
            priceSeries.MarkerStyle = MarkerStyle.None;
            priceSeries.MarkerSize = 5;
            priceSeries["PriceUpColor"] = "Green";
            priceSeries["PriceDownColor"] = "Red";
            //priceSeries.ChartArea = "Price";
            // This gets rid of the gaps in the data:
            priceSeries.IsXValueIndexed = true;
            formChart.Series.Add(priceSeries);

            // TREND
            Series sma200Series = new Series();
            sma200Series.Points.Clear();
            sma200Series.ChartType = SeriesChartType.Line;
            priceSeries.IsValueShownAsLabel = false;
            sma200Series.Color = Color.Red;
            sma200Series.YValueType = ChartValueType.Double;
            sma200Series.XValueType = ChartValueType.DateTime;
            priceSeries.ChartArea = "ChartAreaPrice";
            // This gets rid of the gaps in the data:
            sma200Series.IsXValueIndexed = true;
            formChart.Series.Add(sma200Series);

            // MACD
            Series signalSeries = new Series();
            signalSeries.ChartType = SeriesChartType.Line;
            signalSeries.IsValueShownAsLabel = false;
            signalSeries.Color = Color.Blue;
            signalSeries.YValueType = ChartValueType.Double;
            signalSeries.XValueType = ChartValueType.DateTime;
            signalSeries.Points.Clear();
            signalSeries.ChartArea = "ChartAreaMACD";
            signalSeries.IsXValueIndexed = true;
            formChart.Series.Add(signalSeries);

            Series MACDseries = new Series();
            MACDseries.ChartType = SeriesChartType.Line;
            MACDseries.IsValueShownAsLabel = false;
            MACDseries.Color = Color.Magenta;
            MACDseries.YValueType = ChartValueType.Double;
            MACDseries.XValueType = ChartValueType.DateTime;
            MACDseries.Points.Clear();
            MACDseries.ChartArea = "ChartAreaMACD";
            MACDseries.IsXValueIndexed = true;
            formChart.Series.Add(MACDseries);

            Series histseries = new Series();
            histseries.ChartType = SeriesChartType.Column;
            histseries.IsValueShownAsLabel = false;
            histseries.Color = Color.Gray;
            histseries.YValueType = ChartValueType.Double;
            histseries.XValueType = ChartValueType.DateTime;
            histseries.Points.Clear();
            histseries.ChartArea = "ChartAreaMACD";
            histseries.IsXValueIndexed = true;
            formChart.Series.Add(histseries);

            // TRADE SIGNALS
            Series entrySignalSeries = new Series();
            entrySignalSeries.ChartType = SeriesChartType.Point;
            entrySignalSeries.IsValueShownAsLabel = false;
            //signalBuy.Font = new Font("Arial", 40);
            entrySignalSeries.MarkerSize = 10;
            entrySignalSeries.Color = Color.Cyan;
            entrySignalSeries.YValueType = ChartValueType.Double;
            entrySignalSeries.XValueType = ChartValueType.DateTime;
            entrySignalSeries.Points.Clear();
            entrySignalSeries.ChartArea = "ChartAreaPrice";
            entrySignalSeries.IsXValueIndexed = true;
            formChart.Series.Add(entrySignalSeries);

            Series StopLossSeries = new Series();
            StopLossSeries.ChartType = SeriesChartType.Point;
            StopLossSeries.IsValueShownAsLabel = false;
            //signalBuy.Font = new Font("Arial", 40);
            StopLossSeries.MarkerSize = 10;
            StopLossSeries.Color = Color.Brown;
            StopLossSeries.YValueType = ChartValueType.Double;
            StopLossSeries.XValueType = ChartValueType.DateTime;
            StopLossSeries.Points.Clear();
            StopLossSeries.ChartArea = "ChartAreaPrice";
            StopLossSeries.IsXValueIndexed = true;
            formChart.Series.Add(StopLossSeries);

            Series ProfitSeries = new Series();
            ProfitSeries.ChartType = SeriesChartType.Point;
            ProfitSeries.IsValueShownAsLabel = false;
            //signalBuy.Font = new Font("Arial", 40);
            ProfitSeries.MarkerSize = 10;
            ProfitSeries.Color = Color.Purple;
            ProfitSeries.YValueType = ChartValueType.Double;
            ProfitSeries.XValueType = ChartValueType.DateTime;
            ProfitSeries.Points.Clear();
            ProfitSeries.ChartArea = "ChartAreaPrice";
            ProfitSeries.IsXValueIndexed = true;
            formChart.Series.Add(ProfitSeries);


            foreach (Candle row in data)
            {
                DateTime theTime = row.date_time;

                priceSeries.Points.AddXY(theTime, row.price_high, row.price_low, row.price_open, row.price_close);

                // TODO: Derive indicators on the fly
                //sma200Series.Points.AddXY(theTime, row.sma200);
                //signalSeries.Points.AddXY(theTime, row.signal);
                //MACDseries.Points.AddXY(theTime, row.macd);
                //histseries.Points.AddXY(theTime, row.hist);

                TradeSignal tradeSignal = tradeSetups.FirstOrDefault(x => x.getOpenDate() == theTime);
                if (tradeSignal != null)
                {
                    entrySignalSeries.Points.AddXY(theTime, tradeSignal.getEntry());
                    StopLossSeries.Points.AddXY(theTime, tradeSignal.getStopLoss());
                    ProfitSeries.Points.AddXY(theTime, tradeSignal.getProfitLevel());
                    //tradeSignalSeries.Points.AddXY(theTime, 0);
                }
                else
                {
                    entrySignalSeries.Points.AddXY(theTime, row.price_close - 30);
                    StopLossSeries.Points.AddXY(theTime, row.price_close - 30);
                    ProfitSeries.Points.AddXY(theTime, row.price_close - 30);
                }
            }
        }

        public void ShowPriceActionAndTradeSignals(ChartData data, List<TradeSignal> tradeSetups)
        {
            if (data.GetPriceAction() != null && data.GetPriceAction().Count() <= 0)
            {
                return;
            }

            List<Candle> priceAction = data.GetPriceAction();


            // CHART AREA
            formChart.ChartAreas.Clear();

            SetupPriceActionGraph(priceAction);

            if (data.GetIndicators() != null)
            {
                Dictionary<EnumIndicator, bool> indicators = data.GetIndicators();
                if (indicators.Keys.Contains(EnumIndicator.MA200))
                {

                }

                if (indicators.Keys.Contains(EnumIndicator.MACD))
                {

                }
            }

            SetupMACDGraph(priceAction);





            // TREND
            Series sma200Series = new Series();
            sma200Series.Points.Clear();
            sma200Series.ChartType = SeriesChartType.Line;
            seriesPrice.IsValueShownAsLabel = false;
            sma200Series.Color = Color.Red;
            sma200Series.YValueType = ChartValueType.Double;
            sma200Series.XValueType = ChartValueType.DateTime;
            seriesPrice.ChartArea = "ChartAreaPrice";
            // This gets rid of the gaps in the data:
            sma200Series.IsXValueIndexed = true;
            formChart.Series.Add(sma200Series);

            // MACD
            Series signalSeries = new Series();
            signalSeries.ChartType = SeriesChartType.Line;
            signalSeries.IsValueShownAsLabel = false;
            signalSeries.Color = Color.Blue;
            signalSeries.YValueType = ChartValueType.Double;
            signalSeries.XValueType = ChartValueType.DateTime;
            signalSeries.Points.Clear();
            signalSeries.ChartArea = "ChartAreaMACD";
            signalSeries.IsXValueIndexed = true;
            formChart.Series.Add(signalSeries);

            Series MACDseries = new Series();
            MACDseries.ChartType = SeriesChartType.Line;
            MACDseries.IsValueShownAsLabel = false;
            MACDseries.Color = Color.Magenta;
            MACDseries.YValueType = ChartValueType.Double;
            MACDseries.XValueType = ChartValueType.DateTime;
            MACDseries.Points.Clear();
            MACDseries.ChartArea = "ChartAreaMACD";
            MACDseries.IsXValueIndexed = true;
            formChart.Series.Add(MACDseries);

            Series histseries = new Series();
            histseries.ChartType = SeriesChartType.Column;
            histseries.IsValueShownAsLabel = false;
            histseries.Color = Color.Gray;
            histseries.YValueType = ChartValueType.Double;
            histseries.XValueType = ChartValueType.DateTime;
            histseries.Points.Clear();
            histseries.ChartArea = "ChartAreaMACD";
            histseries.IsXValueIndexed = true;
            formChart.Series.Add(histseries);

            // TRADE SIGNALS
            Series entrySignalSeries = new Series();
            entrySignalSeries.ChartType = SeriesChartType.Point;
            entrySignalSeries.IsValueShownAsLabel = false;
            //signalBuy.Font = new Font("Arial", 40);
            entrySignalSeries.MarkerSize = 10;
            entrySignalSeries.Color = Color.Cyan;
            entrySignalSeries.YValueType = ChartValueType.Double;
            entrySignalSeries.XValueType = ChartValueType.DateTime;
            entrySignalSeries.Points.Clear();
            entrySignalSeries.ChartArea = "ChartAreaPrice";
            entrySignalSeries.IsXValueIndexed = true;
            formChart.Series.Add(entrySignalSeries);

            Series StopLossSeries = new Series();
            StopLossSeries.ChartType = SeriesChartType.Point;
            StopLossSeries.IsValueShownAsLabel = false;
            //signalBuy.Font = new Font("Arial", 40);
            StopLossSeries.MarkerSize = 10;
            StopLossSeries.Color = Color.Brown;
            StopLossSeries.YValueType = ChartValueType.Double;
            StopLossSeries.XValueType = ChartValueType.DateTime;
            StopLossSeries.Points.Clear();
            StopLossSeries.ChartArea = "ChartAreaPrice";
            StopLossSeries.IsXValueIndexed = true;
            formChart.Series.Add(StopLossSeries);

            Series ProfitSeries = new Series();
            ProfitSeries.ChartType = SeriesChartType.Point;
            ProfitSeries.IsValueShownAsLabel = false;
            //signalBuy.Font = new Font("Arial", 40);
            ProfitSeries.MarkerSize = 10;
            ProfitSeries.Color = Color.Purple;
            ProfitSeries.YValueType = ChartValueType.Double;
            ProfitSeries.XValueType = ChartValueType.DateTime;
            ProfitSeries.Points.Clear();
            ProfitSeries.ChartArea = "ChartAreaPrice";
            ProfitSeries.IsXValueIndexed = true;
            formChart.Series.Add(ProfitSeries);


            foreach (Candle row in priceAction)
            {
                DateTime theTime = row.date_time;

                seriesPrice.Points.AddXY(theTime, row.price_high, row.price_low, row.price_open, row.price_close);

                // TODO: Derive indicators on the fly
                //sma200Series.Points.AddXY(theTime, row.sma200);
                //signalSeries.Points.AddXY(theTime, row.signal);
                //MACDseries.Points.AddXY(theTime, row.macd);
                //histseries.Points.AddXY(theTime, row.hist);

                TradeSignal tradeSignal = tradeSetups.FirstOrDefault(x => x.getOpenDate() == theTime);
                if (tradeSignal != null)
                {
                    entrySignalSeries.Points.AddXY(theTime, tradeSignal.getEntry());
                    StopLossSeries.Points.AddXY(theTime, tradeSignal.getStopLoss());
                    ProfitSeries.Points.AddXY(theTime, tradeSignal.getProfitLevel());
                    //tradeSignalSeries.Points.AddXY(theTime, 0);
                }
                else
                {
                    entrySignalSeries.Points.AddXY(theTime, row.price_close - 30);
                    StopLossSeries.Points.AddXY(theTime, row.price_close - 30);
                    ProfitSeries.Points.AddXY(theTime, row.price_close - 30);
                }
            }
        }

        public void ClearData()
        {
            formChart.Series.Clear();
        }

        // Helpers
        private void SetupPriceActionGraph(List<Candle> priceAction)
        {
            ChartArea chartAreaPrice = new ChartArea("ChartAreaPrice");
            chartAreaPrice.AxisY.Interval = 20;
            //chartArea.AxisY.LabelStyle.Format = "C";
            chartAreaPrice.BackColor = Color.FromArgb(64, 165, 191, 228);
            chartAreaPrice.BackGradientStyle = GradientStyle.TopBottom;
            chartAreaPrice.BackSecondaryColor = Color.White;
            chartAreaPrice.BorderColor = Color.FromArgb(255, 255, 0, 255);
            chartAreaPrice.ShadowColor = Color.Transparent;
            chartAreaPrice.AxisX.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaPrice.AxisY.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaPrice.AxisX.LineWidth = 2;
            chartAreaPrice.AxisY.LineWidth = 2;
            chartAreaPrice.AxisY.Title = "Price";
            chartAreaPrice.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaPrice.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaPrice.AxisX.MajorGrid.LineWidth = 1;
            chartAreaPrice.AxisY.MajorGrid.LineWidth = 1;
            chartAreaPrice.AxisX.MajorTickMark.Enabled = false;
            chartAreaPrice.AxisY.MajorTickMark.Enabled = false;
            chartAreaPrice.AxisX.MajorGrid.Enabled = false;
            chartAreaPrice.AxisY.MajorGrid.Enabled = true;
            chartAreaPrice.AxisX.Interval = 0; //Set to 0 means automatically assigned by the control
            chartAreaPrice.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartAreaPrice.AxisX.IntervalType = DateTimeIntervalType.Days;
            chartAreaPrice.AxisX.LabelStyle.IsStaggered = true;
            //chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            chartAreaPrice.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm";
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd";
            //chartArea.AxisX.LabelStyle.Angle = 45;
            chartAreaPrice.AxisX.IsLabelAutoFit = true;
            chartAreaPrice.CursorX.IsUserEnabled = true;
            chartAreaPrice.CursorX.IsUserSelectionEnabled = true;
            chartAreaPrice.CursorX.Interval = 0;
            chartAreaPrice.CursorX.IntervalOffset = 0;
            chartAreaPrice.CursorX.IntervalType = DateTimeIntervalType.Hours;
            chartAreaPrice.AxisX.ScaleView.Zoomable = true;
            chartAreaPrice.AxisX.ScrollBar.IsPositionedInside = false;

            chartAreaPrice.AxisY.ScaleBreakStyle.Enabled = true;
            chartAreaPrice.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 47;
            chartAreaPrice.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            chartAreaPrice.AxisY.ScaleBreakStyle.Spacing = 10;
            chartAreaPrice.AxisY.ScaleBreakStyle.LineColor = Color.Black;
            chartAreaPrice.AxisY.ScaleBreakStyle.LineWidth = 1;
            chartAreaPrice.CursorY.IsUserEnabled = true;
            chartAreaPrice.CursorY.IsUserSelectionEnabled = true;
            chartAreaPrice.CursorY.Interval = 0;
            chartAreaPrice.CursorY.IntervalOffset = 0;
            chartAreaPrice.CursorY.IntervalType = DateTimeIntervalType.Hours;

            chartAreaPrice.AxisY.ScaleView.Zoomable = true;

            double marketMin = priceAction.AsEnumerable().Min(x => x.price_low);
            double marketMax = priceAction.AsEnumerable().Max(x => x.price_high);
            double padding = (marketMax - marketMin) / 100 * 50;
            chartAreaPrice.AxisY.Minimum = marketMin - padding;
            chartAreaPrice.AxisY.Maximum = marketMax + padding;

            formChart.ChartAreas.Add(chartAreaPrice);

            // SERIES
            formChart.Series.Clear();

            seriesPrice.Points.Clear();
            seriesPrice.ChartType = SeriesChartType.Candlestick;
            seriesPrice.BorderWidth = 1;
            seriesPrice.ShadowOffset = 0;
            seriesPrice.IsVisibleInLegend = true;
            seriesPrice.IsValueShownAsLabel = false;
            seriesPrice.Color = Color.MediumPurple;
            seriesPrice.XValueType = ChartValueType.DateTime;
            seriesPrice.MarkerStyle = MarkerStyle.None;
            seriesPrice.MarkerSize = 5;
            seriesPrice["PriceUpColor"] = "Green";
            seriesPrice["PriceDownColor"] = "Red";
            //priceSeries.ChartArea = "Price";
            // This gets rid of the gaps in the data:
            seriesPrice.IsXValueIndexed = true;
            formChart.Series.Add(seriesPrice);

        }

        private void SetupMACDGraph(List<Candle> priceAction)
        {
            ChartArea chartAreaMACD = new ChartArea("ChartAreaMACD");
            chartAreaMACD.AlignWithChartArea = "ChartAreaPrice";
            chartAreaMACD.AxisY.Interval = 20;
            //chartArea.AxisY.LabelStyle.Format = "C";
            chartAreaMACD.BackColor = Color.FromArgb(64, 165, 191, 228);
            chartAreaMACD.BackGradientStyle = GradientStyle.TopBottom;
            chartAreaMACD.BackSecondaryColor = Color.White;
            chartAreaMACD.BorderColor = Color.FromArgb(255, 255, 0, 255);
            chartAreaMACD.ShadowColor = Color.Transparent;
            chartAreaMACD.AxisX.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaMACD.AxisY.LineColor = Color.FromArgb(255, 0, 0, 255);
            chartAreaMACD.AxisX.LineWidth = 2;
            chartAreaMACD.AxisY.LineWidth = 2;
            chartAreaMACD.AxisY.Title = "MACD";
            chartAreaMACD.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaMACD.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartAreaMACD.AxisX.MajorGrid.LineWidth = 1;
            chartAreaMACD.AxisY.MajorGrid.LineWidth = 1;
            chartAreaMACD.AxisX.MajorTickMark.Enabled = false;
            chartAreaMACD.AxisY.MajorTickMark.Enabled = true;
            chartAreaMACD.AxisY.LabelStyle.Format = "0.00";
            //chartAreaMACD.AxisX.MajorGrid.Enabled = false;
            //chartAreaMACD.AxisY.MajorGrid.Enabled = false;
            chartAreaMACD.AxisX.Interval = 0; //Set to 0 means automatically assigned by the control
            chartAreaMACD.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartAreaMACD.AxisX.IntervalType = DateTimeIntervalType.Days;
            chartAreaMACD.AxisX.LabelStyle.IsStaggered = true;
            //chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            chartAreaMACD.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm";
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd";
            //chartArea.AxisX.LabelStyle.Angle = 45;
            chartAreaMACD.AxisX.IsLabelAutoFit = true;
            chartAreaMACD.CursorX.IsUserEnabled = true;
            chartAreaMACD.CursorX.IsUserSelectionEnabled = true;
            chartAreaMACD.CursorX.Interval = 0;
            chartAreaMACD.CursorX.IntervalOffset = 0;
            chartAreaMACD.CursorX.IntervalType = DateTimeIntervalType.Hours;
            chartAreaMACD.AxisX.ScaleView.Zoomable = true;
            chartAreaMACD.AxisX.ScrollBar.IsPositionedInside = false;

            chartAreaMACD.AxisY.ScaleBreakStyle.Enabled = true;
            chartAreaMACD.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 47;
            chartAreaMACD.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            chartAreaMACD.AxisY.ScaleBreakStyle.Spacing = 10;
            chartAreaMACD.AxisY.ScaleBreakStyle.LineColor = Color.Black;
            chartAreaMACD.AxisY.ScaleBreakStyle.LineWidth = 1;
            chartAreaMACD.CursorY.IsUserEnabled = true;
            chartAreaMACD.CursorY.IsUserSelectionEnabled = true;
            chartAreaMACD.CursorY.Interval = 0;
            chartAreaMACD.CursorY.IntervalOffset = 0;
            chartAreaMACD.CursorY.IntervalType = DateTimeIntervalType.Hours;

            chartAreaMACD.AxisY.ScaleView.Zoomable = true;

            formChart.ChartAreas.Add(chartAreaMACD);

            //marketMin = data.AsEnumerable().Min(x => x.macd);
            //marketMax = data.AsEnumerable().Max(x => x.macd);
            //padding = (marketMax - marketMin) / 100 * 50;
            //chartAreaMACD.AxisY.Minimum = marketMin - padding;
            //chartAreaMACD.AxisY.Maximum = marketMax + padding;

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////






        private void SetSeries()
        {
            formChart.Series.Clear();
            //Set the line type
            seriesPrice.ChartType = SeriesChartType.Candlestick;
            //Line width
            seriesPrice.BorderWidth = 1;
            //Shadow width
            seriesPrice.ShadowOffset = 0;
            //Whether to display in the legend collection Legends
            seriesPrice.IsVisibleInLegend = true;
            //Whether there is data display on the data point on the line
            seriesPrice.IsValueShownAsLabel = false;
            //Line colour
            seriesPrice.Color = Color.MediumPurple;
            //Set the display type of the curve X axis
            seriesPrice.XValueType = ChartValueType.DateTime;
            //Set the type of data point
            seriesPrice.MarkerStyle = MarkerStyle.None;
            //The size of the line data point
            seriesPrice.MarkerSize = 5;

            seriesPrice["PriceUpColor"] = "Green";
            seriesPrice["PriceDownColor"] = "Red";
            // This gets rid of the gaps in the data:
            seriesPrice.IsXValueIndexed = true;

            formChart.Series.Add(seriesPrice);

            // SMA
            sma50Price.ChartType = SeriesChartType.Line;
            sma50Price.IsValueShownAsLabel = false;
            sma50Price.Color = Color.Black;
            sma50Price.YValueType = ChartValueType.Double;
            sma50Price.XValueType = ChartValueType.DateTime;
            sma50Price.Points.Clear();
            sma50Price.ChartArea = "Market";
            formChart.Series.Add(sma50Price);

            ema50signal.ChartType = SeriesChartType.Line;
            ema50signal.IsValueShownAsLabel = false;
            ema50signal.Color = Color.Green;
            ema50signal.YValueType = ChartValueType.Double;
            ema50signal.XValueType = ChartValueType.DateTime;
            ema50signal.Points.Clear();
            ema50signal.ChartArea = "Market";
            ema50signal.IsXValueIndexed = true;

            formChart.Series.Add(ema50signal);

            // MACD
            signal.ChartType = SeriesChartType.Line;
            signal.IsValueShownAsLabel = false;
            signal.Color = Color.Blue;
            signal.YValueType = ChartValueType.Double;
            signal.XValueType = ChartValueType.DateTime;
            signal.Points.Clear();
            signal.ChartArea = "MACD";
            signal.IsXValueIndexed = true;
            formChart.Series.Add(signal);

            MACD.ChartType = SeriesChartType.Line;
            MACD.IsValueShownAsLabel = false;
            MACD.Color = Color.Magenta;
            MACD.YValueType = ChartValueType.Double;
            MACD.XValueType = ChartValueType.DateTime;
            MACD.Points.Clear();
            MACD.ChartArea = "MACD";
            MACD.IsXValueIndexed = true;
            formChart.Series.Add(MACD);

            HIST.ChartType = SeriesChartType.Column;
            HIST.IsValueShownAsLabel = false;
            HIST.Color = Color.Gray;
            HIST.YValueType = ChartValueType.Double;
            HIST.XValueType = ChartValueType.DateTime;
            HIST.Points.Clear();
            HIST.ChartArea = "MACD";
            HIST.IsXValueIndexed = true;
            formChart.Series.Add(HIST);

            // TREND
            sma200trend.ChartType = SeriesChartType.Line;
            sma200trend.IsValueShownAsLabel = false;
            sma200trend.Color = Color.Red;
            sma200trend.YValueType = ChartValueType.Double;
            sma200trend.XValueType = ChartValueType.DateTime;
            sma200trend.Points.Clear();
            sma200trend.ChartArea = "Market";
            sma200trend.IsXValueIndexed = true;
            formChart.Series.Add(sma200trend);

            // BUY SIGNAL
            signalBuy.ChartType = SeriesChartType.Point;
            signalBuy.IsValueShownAsLabel = false;
            //signalBuy.Font = new Font("Arial", 40);
            signalBuy.MarkerSize = 10;
            signalBuy.Color = Color.Magenta;
            signalBuy.YValueType = ChartValueType.Double;
            signalBuy.XValueType = ChartValueType.DateTime;
            signalBuy.Points.Clear();
            signalBuy.ChartArea = "Market";
            signalBuy.IsXValueIndexed = true;
            formChart.Series.Add(signalBuy);

            // BUY SIGNAL
            signalStop.ChartType = SeriesChartType.Point;
            signalStop.IsValueShownAsLabel = false;
            //signalStop.Font = new Font("Arial", 40);
            signalStop.MarkerSize = 10;
            signalStop.Color = Color.Black;
            signalStop.YValueType = ChartValueType.Double;
            signalStop.XValueType = ChartValueType.DateTime;
            signalStop.Points.Clear();
            signalStop.ChartArea = "Market";
            signalStop.IsXValueIndexed = true;
            formChart.Series.Add(signalStop);

            // BUY SIGNAL
            signalProfit.ChartType = SeriesChartType.Point;
            signalProfit.IsValueShownAsLabel = false;
            //signalProfit.Font = new Font("Arial", 40);
            signalProfit.MarkerSize = 10;
            signalProfit.Color = Color.Purple;
            signalProfit.YValueType = ChartValueType.Double;
            signalProfit.XValueType = ChartValueType.DateTime;
            signalProfit.Points.Clear();
            signalProfit.ChartArea = "Market";
            signalProfit.IsXValueIndexed = true;
            formChart.Series.Add(signalProfit);

            //Line 3: Upper limit horizontal line
            //Series seriesMax = new Series("Max");
            //seriesMax.ChartType = SeriesChartType.Line;
            //seriesMax.BorderWidth = 1;
            //seriesMax.ShadowOffset = 0;
            //seriesMax.IsVisibleInLegend = true;
            //seriesMax.IsValueShownAsLabel = false;
            //seriesMax.Color = Color.Red;
            //seriesMax.XValueType = ChartValueType.DateTime;
            //seriesMax.MarkerStyle = MarkerStyle.None;
            //myChart.Series.Add(seriesMax);

        }

        public void UpdateData(DataRow dataRow)
        {
            dataTableMarket.Rows.Add(dataRow);
            seriesPrice.Points.AddXY(
                dataRow["TheTime"],
                dataRow["High"],
                dataRow["Low"],
                dataRow["Open"],
                dataRow["Close"]);

            // Re-calibrate minimums and maximums for axis range
            //chartAreaMarket.AxisX.Minimum = Convert.ToDateTime(dataTableMarket.Rows[0]["TheTime"]).ToOADate();
            //chartArea.AxisX.Minimum = (DateTime.Now).AddMinutes(-5).ToOADate();

            double marketMin = dataTableMarket.AsEnumerable().Min(col => col.Field<double>("Low"));
            double marketMax = dataTableMarket.AsEnumerable().Max(col => col.Field<double>("High"));

            double padding = (marketMax - marketMin) / 100 * 50;
            //chartAreaMarket.AxisY.Minimum = marketMin - padding;
            //chartAreaMarket.AxisY.Maximum = marketMax + padding;
            //chartArea.AxisY.Minimum = 124;
            //chartArea.AxisY.Maximum = 126;
            int currentIndex = dataTableMarket.Rows.Count - 1;

            //seriesPrice.Points.Clear();

            // MA50
            double newClose = (double)dataRow["Close"];
            sma50.advance(newClose);
            //ema50.advance(newClose);
            ema10.advance(newClose);
            sma200.advance(newClose);


            //Start drawing lines
            //foreach (DataRow dr in dataTableMarket.Rows)
            //{
            //    seriesPrice.Points.AddXY(
            //    dr["TheTime"],
            //    dr["High"],
            //    dr["Low"],
            //    dr["Open"],
            //    dr["Close"]);
            //}

            if (ema10.getSampleFull())
            {
                //sma50Price.Points.AddXY(dataTableMarket.Rows[currentIndex]["TheTime"], sma50.getAverage());
                //ema50signal.Points.AddXY(dataTableMarket.Rows[currentIndex]["TheTime"], ema10.getAverage());
            }

            if (sma200.getSampleFull())
            {
                //sma50Price.Points.AddXY(dataTableMarket.Rows[currentIndex]["TheTime"], sma50.getAverage());
                sma200trend.Points.AddXY(dataTableMarket.Rows[currentIndex]["TheTime"], sma200.getAverage());
            }


            // MACD Chart Area //////////////////////////////////////////////////////////////////////////////////////////////
            //chartAreaMACD.AxisX.Minimum = Convert.ToDateTime(dataTableMarket.Rows[0]["TheTime"]).ToOADate();


            //ema12.advance(newClose);
            //ema26.advance(newClose);
            //double ave12 = ema12.getAverage();
            //double ave26 = ema26.getAverage();

            //double macdLine = ave12 - ave26;
            //ema9.advance(macdLine);

            //chartAreaMACD.AxisY.Minimum = -6;
            //chartAreaMACD.AxisY.Maximum = 6;


            // TODO: Change to hourly or daily data. Fix MACD calculation

            //if (ema9.getSampleFull())
            //{
            //    ema9signal.Points.AddXY(dataTableMarket.Rows[currentIndex]["TheTime"], ema9.getAverage());
            //    MACD.Points.AddXY(dataTableMarket.Rows[currentIndex]["TheTime"], macdLine);
            //}
        }



        public void SetData()
        {
            if (dataTableMarket.Rows.Count > 0)
            {
                seriesPrice.Points.Clear();
                sma200trend.Points.Clear();
                MACD.Points.Clear();
                signal.Points.Clear();
                signalBuy.Points.Clear();
                HIST.Points.Clear();

                double marketMin = dataTableMarket.AsEnumerable().Min(col => col.Field<double>("Low"));
                double marketMax = dataTableMarket.AsEnumerable().Max(col => col.Field<double>("High"));
                double padding = (marketMax - marketMin) / 100 * 50;
                chartAreaMarket.AxisY.Minimum = marketMin - padding;
                chartAreaMarket.AxisY.Maximum = marketMax + padding;

                foreach (DataRow dr in dataTableMarket.Rows)
                {
                    seriesPrice.Points.AddXY(dr["TheTime"], dr["High"], dr["Low"], dr["Open"], dr["Close"]);
                    sma200trend.Points.AddXY(dr["TheTime"], dr["200sma"]);

                    MACD.Points.AddXY(dr["TheTime"], dr["MACD"]);
                    signal.Points.AddXY(dr["TheTime"], dr["Signal"]);
                    signalBuy.Points.AddXY(dr["TheTime"], dr["BUY"]);
                    HIST.Points.AddXY(dr["TheTime"], dr["HIST"]);

                    //seriesMin.Points.AddXY(dr["TheTime"], 15); //Set the offline to 15
                    //seriesMax.Points.AddXY(dr["TheTime"], 30); //Set the upper limit to 30
                }

            }



        }

        public void SetData(IEnumerable<KeyValuePair<DateTime, TimeSeriesData>> initialData)
        {
            for (int n = 0; n < initialData.Count(); n++)
            {
                KeyValuePair<DateTime, TimeSeriesData> candle = initialData.ElementAt(n);
                DataRow dr = dataTableMarket.NewRow();

                // Convert to double
                double High = Convert.ToDouble(candle.Value.High);
                double Low = Convert.ToDouble(candle.Value.Low);
                double Open = Convert.ToDouble(candle.Value.Open);
                double Close = Convert.ToDouble(candle.Value.Close);

                dr["TheTime"] = candle.Key;
                dr["High"] = High;
                dr["Low"] = Low;
                dr["Open"] = Open;
                dr["Close"] = Close;
                dataTableMarket.Rows.Add(dr);
            }

            // Re-calibrate minimums and maximums for axis range
            chartAreaMarket.AxisX.Minimum = Convert.ToDateTime(dataTableMarket.Rows[0]["TheTime"]).ToOADate();
            //chartArea.AxisX.Minimum = (DateTime.Now).AddMinutes(-5).ToOADate();

            double min = dataTableMarket.AsEnumerable().Min(col => col.Field<double>("Low"));
            double max = dataTableMarket.AsEnumerable().Max(col => col.Field<double>("High"));
            double padding = (max - min) / 100 * 5;
            chartAreaMarket.AxisY.Minimum = min - padding;
            chartAreaMarket.AxisY.Maximum = max + padding;


            // MIN AND MAX FROM DICTIONARY ////////////////////////////////////////////////////
            //string[] lows = priceAction.Values.Select(x => x.Low).ToArray();
            //double yMin = Array.ConvertAll(lows, Double.Parse).Min();

            //string[] highs = priceAction.Values.Select(x => x.High).ToArray();
            //double[] highsDouble = Array.ConvertAll(highs, Double.Parse);
            ////double yMax = Array.ConvertAll(highs, Double.Parse).Max();
            //double yMax = highsDouble.Max();

            //chartArea.AxisY.Minimum = yMin;
            //chartArea.AxisY.Maximum = yMax;

            //Set the minimum value of the X axis to the X coordinate value of the first point
            //chartArea.AxisY.Minimum = 110;
            //chartArea.AxisY.Maximum = 135;
            ///////////////////////////////////////////////////////////////////////////////////


            seriesPrice.Points.Clear();

            //Start drawing lines
            foreach (DataRow dr in dataTableMarket.Rows)
            {
                seriesPrice.Points.AddXY(dr["TheTime"], dr["High"], dr["Low"], dr["Open"], dr["Close"]);

                //seriesMin.Points.AddXY(dr["TheTime"], 15); //Set the offline to 15
                //seriesMax.Points.AddXY(dr["TheTime"], 30); //Set the upper limit to 30
            }
        }

        public void CreateSnapshot(DataTable data, DateTime date)
        {
            //KeyValuePair<DateTime, TimeSeriesData> startData = rewindDictionary(data, date, 5);
            //KeyValuePair<DateTime, TimeSeriesData> endDateData = fastForwardDictionary(data, date, 5);

            DataRow startRow = rewindDataTable(data, date, 50);
            DataRow endRow = fastForwardDataTable(data, date, 50);

            // Create subset of data table to display on graph
            DataTable snapshot = data.AsEnumerable()
                .Where(x => x.Field<DateTime>("TheTime").Subtract(startRow.Field<DateTime>("TheTime")).TotalMilliseconds >= 0
                && x.Field<DateTime>("TheTime").Subtract(endRow.Field<DateTime>("TheTime")).TotalMilliseconds <= 0)
                .Select(x => x).CopyToDataTable();
            //DataTable snapshot = data;

            seriesPrice.Points.Clear();
            sma200trend.Points.Clear();
            MACD.Points.Clear();
            signal.Points.Clear();
            signalBuy.Points.Clear();
            HIST.Points.Clear();

            double marketMin = snapshot.AsEnumerable().Min(col => col.Field<double>("Low"));
            double marketMax = snapshot.AsEnumerable().Max(col => col.Field<double>("High"));


            double padding = (marketMax - marketMin) / 100 * 50;
            //double padding = 30;
            //padding = 0;
            chartAreaMarket.AxisY.Minimum = marketMin - padding;
            chartAreaMarket.AxisY.Maximum = marketMax + padding;

            try
            {
                double macdMin = snapshot.AsEnumerable().Min(col => col.Field<double>("MACD"));
                double macdMax = snapshot.AsEnumerable().Max(col => col.Field<double>("MACD"));
                padding = (macdMax - macdMin) / 100 * 50;

                //double macdMin = -1;
                //double macdMax = 1;
                chartAreaMACD.AxisY.Minimum = macdMin - padding;
                chartAreaMACD.AxisY.Maximum = macdMax + padding;
            }
            catch (Exception)
            {
            }

            for (int index = 0; index < snapshot.Rows.Count; index++)
            {
                DateTime TheTime = snapshot.Rows[index].Field<DateTime>("TheTime");
                double High = snapshot.Rows[index].Field<double>("High");
                double Low = snapshot.Rows[index].Field<double>("Low");
                double Open = snapshot.Rows[index].Field<double>("Open");
                double Close = snapshot.Rows[index].Field<double>("Close");
                seriesPrice.Points.AddXY(TheTime, High, Low, Open, Close);

                double MACD = snapshot.Rows[index].Field<double>("MACD");
                double Signal = snapshot.Rows[index].Field<double>("Signal");
                double Hist = snapshot.Rows[index].Field<double>("HIST");
                this.MACD.Points.AddXY(TheTime, MACD);
                signal.Points.AddXY(TheTime, Signal);
                HIST.Points.AddXY(TheTime, Hist);

                double sma200 = snapshot.Rows[index].Field<double>("Trend");
                sma200trend.Points.AddXY(TheTime, sma200);

                try
                {
                    double Entry = snapshot.Rows[index].Field<double>("Entry");
                    double StopLoss = snapshot.Rows[index].Field<double>("StopLoss");
                    double Profit = snapshot.Rows[index].Field<double>("Profit");
                    signalBuy.Points.AddXY(TheTime, Entry);
                    signalStop.Points.AddXY(TheTime, StopLoss);
                    signalProfit.Points.AddXY(TheTime, Profit);
                }
                catch (Exception ex)
                {
                    // DO NOTHING. THERE WILL BE GAPS IN THE TABLE WHERE THERE ARE NO TRADE SIGNALS
                }

            }
        }


        private DataRow rewindDataTable(DataTable data, DateTime currentDate, int distance)
        {
            // Rewind dictionary
            DateTime lowerLimit = data.AsEnumerable().Min(col => col.Field<DateTime>("TheTime"));
            DataRow currentRow = data.AsEnumerable().Where(dr => dr.Field<DateTime>("TheTime") == currentDate).First();
            DateTime rewindTime = currentDate;
            int rewindCounter = 0;
            bool rewound = false;
            while (!rewound)
            {
                DateTime currentTime = currentRow.Field<DateTime>("TheTime");
                // Check if we're at the start of the dict and stop early
                if (currentTime.Subtract(lowerLimit).TotalMinutes <= 0)
                {
                    return currentRow;
                }
                rewindTime = rewindTime.AddMinutes(-5);
                if (data.AsEnumerable().Where(dr => dr.Field<DateTime>("TheTime") == rewindTime).Count() > 0)
                {
                    currentRow = data.AsEnumerable().Where(dr => dr.Field<DateTime>("TheTime") == rewindTime).First();
                    rewindCounter++;
                }
                if (rewindCounter == distance)
                {
                    rewound = true;
                }
            }

            return currentRow;
        }

        private DataRow fastForwardDataTable(DataTable data, DateTime currentDate, int distance)
        {
            // Rewind dictionary
            DateTime upperLimit = data.AsEnumerable().Max(col => col.Field<DateTime>("TheTime"));
            DataRow currentRow = data.AsEnumerable().Where(dr => dr.Field<DateTime>("TheTime") == currentDate).First();
            DateTime fastForwardTime = currentDate;
            int fastForwardCounter = 0;
            bool fastForwarded = false;
            while (!fastForwarded)
            {
                DateTime currentTime = currentRow.Field<DateTime>("TheTime");
                // Check if we're at the start of the dict and stop early
                if (currentTime.Subtract(upperLimit).TotalMinutes >= 0)
                {
                    return currentRow;
                }
                fastForwardTime = fastForwardTime.AddMinutes(5);
                if (data.AsEnumerable().Where(dr => dr.Field<DateTime>("TheTime") == fastForwardTime).Count() > 0)
                {
                    currentRow = data.AsEnumerable().Where(dr => dr.Field<DateTime>("TheTime") == fastForwardTime).First();
                    fastForwardCounter++;
                }
                if (fastForwardCounter == distance)
                {
                    fastForwarded = true;
                }
            }

            return currentRow;
        }

        private KeyValuePair<DateTime, TimeSeriesData> rewindDictionary(Dictionary<DateTime, TimeSeriesData> data, DateTime currentDate, int distance)
        {
            // Rewind dictionary
            DateTime startTime = DateTime.Now;
            TimeSeriesData StartData = data.Values.First();
            DateTime rewindTime = currentDate;
            int rewindCounter = 0;
            bool rewound = false;
            while (!rewound)
            {
                // Check if we're at the start of the dict and stop early
                if (rewindTime.Subtract(data.First().Key).TotalMinutes <= 0)
                {
                    rewindTime = data.First().Key;
                    return new KeyValuePair<DateTime, TimeSeriesData>(startTime, StartData);
                }
                rewindTime = rewindTime.AddMinutes(-5);
                if (data.ContainsKey(rewindTime))
                {
                    data.TryGetValue(rewindTime, out StartData);
                    startTime = rewindTime;
                    rewindCounter++;
                }
                if (rewindCounter == distance)
                {
                    rewound = true;
                }
            }

            return new KeyValuePair<DateTime, TimeSeriesData>(startTime, StartData);
        }

        private KeyValuePair<DateTime, TimeSeriesData> fastForwardDictionary(Dictionary<DateTime, TimeSeriesData> data, DateTime currentDate, int distance)
        {
            // Rewind dictionary
            DateTime endTime = DateTime.Now;
            TimeSeriesData endData = new TimeSeriesData();
            DateTime fastForwardTime = currentDate;
            int fastForwardCounter = 0;
            bool fastForwarded = false;
            while (!fastForwarded)
            {
                // Check if we're at the end of the dict and stop early
                if (data.Last().Key.Subtract(fastForwardTime).TotalMinutes <= 0)
                {
                    fastForwardTime = data.Last().Key;
                    return new KeyValuePair<DateTime, TimeSeriesData>(endTime, endData);
                }
                fastForwardTime = fastForwardTime.AddMinutes(5);
                if (data.ContainsKey(fastForwardTime))
                {
                    data.TryGetValue(fastForwardTime, out endData);
                    endTime = fastForwardTime;
                    fastForwardCounter++;
                }
                if (fastForwardCounter == distance)
                {
                    fastForwarded = true;
                }
            }

            return new KeyValuePair<DateTime, TimeSeriesData>(endTime, endData);
        }


        public void SetNewData()
        {
            seriesPrice.Points.Clear();
            //Set the minimum value of the X axis to the X coordinate value of the first point
            chartAreaMarket.AxisX.Minimum = (DateTime.Now).AddMinutes(-5).ToOADate();

            seriesPrice.Points.AddXY(DateTime.Now, 128, 125, 126, 127);
            seriesPrice.Points.AddXY(DateTime.Now.AddMinutes(5), 128, 125, 126, 127);
            seriesPrice.Points.AddXY(DateTime.Now.AddMinutes(10), 128, 125, 126, 127);
            seriesPrice.Points.AddXY(DateTime.Now.AddMinutes(15), 128, 125, 126, 127);
            seriesPrice.Points.AddXY(DateTime.Now.AddMinutes(20), 128, 125, 126, 127);


            //chartArea.RecalculateAxesScale();

        }

    }
}
