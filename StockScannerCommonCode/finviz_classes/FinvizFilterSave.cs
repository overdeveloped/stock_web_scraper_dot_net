using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScannerCommonCode
{
    public class FinvizFilterSave
    {
        public string saveName { get; set; }
        public int exchange { get; set; }
        public int marketCap { get; set; }
        public int earningsDate { get; set; }
        public int targetPrice { get; set; }
        public int index { get; set; }
        public int dividendYield { get; set; }
        public int averageVolume { get; set; }
        public int ipoDate { get; set; }
        public int sector { get; set; }
        public int floatShort { get; set; }
        public int relativeVolume { get; set; }
        public int sharesOutstanding { get; set; }
        public int industry { get; set; }
        public int analystRecommends { get; set; }
        public int currentVolume { get; set; }
        public int _float { get; set; }
        public int country { get; set; }
        public int optionShort { get; set; }
        public int price { get; set; }
        public Dictionary<string, string> translations { get; set; }

        // Translations
        // Column 1
        private string[] translationExchange { get; set; }
        private string[] translationMarketCap { get; set; }
        private string[] translationEarningsDate { get; set; }
        private string[] translationTargetPrice { get; set; }
        // Column 2
        private string[] translationIndex { get; set; }
        private string[] translationDividendYield { get; set; }
        private string[] translationAverageVolume { get; set; }
        private string[] translationIpoDate { get; set; }
        // Column 3
        private string[] translationSector { get; set; }
        private string[] translationFloatShort { get; set; }
        private string[] translationRelativeVolume { get; set; }
        private string[] translationSharesOutstanding { get; set; }
        // Column 4
        private string[] translationIndustry { get; set; }
        private string[] translationAnalystRecomends { get; set; }
        private string[] translationCurrentVolume { get; set; }
        private string[] translationFloat { get; set; }

        // Column 5
        private string[] translationCountry { get; set; }
        private string[] translationOptionShort { get; set; }
        private string[] translationPrice { get; set; }


        public FinvizFilterSave()
        {
            this.translations = new Dictionary<string, string>();
            SetupTranslations();
        }

        public FinvizFilterSave(string saveName, int exchange, int marketCap, int earningsDate, int targetPrice, int index, int dividendYield, int averageVolume, int secter, int floatShare, int relativeVolume, int sharesOutstanding, int industry, int analystRecommends, int currentValue, int @float, int country, int optionShort, int price, int ipoDate)
        {
            this.saveName = saveName;
            this.exchange = exchange;
            this.marketCap = marketCap;
            this.earningsDate = earningsDate;
            this.targetPrice = targetPrice;
            this.index = index;
            this.dividendYield = dividendYield;
            this.averageVolume = averageVolume;
            this.sector = secter;
            this.floatShort = floatShare;
            this.relativeVolume = relativeVolume;
            this.sharesOutstanding = sharesOutstanding;
            this.industry = industry;
            this.analystRecommends = analystRecommends;
            this.currentVolume = currentValue;
            this._float = @float;
            this.country = country;
            this.optionShort = optionShort;
            this.price = price;
            this.ipoDate = ipoDate;

            SetupTranslations();
        }

        private void SetupTranslations()
        {
            // Copy URL paramerters from the website:
            // https://finviz.com/screener.ashx?v=111&f=fa_div_o10

            // Column 1
            translations.Add("Any", "");
            translations.Add("AMEX", "exch_amex");
            translations.Add("NASDAQ", "exch_nasd");
            translations.Add("NYSE", "exch_nyse");
            
            translations.Add("Any", "");
            translations.Add("Mega ($200bln and more)", "cap_mega");
            translations.Add("Large ($10bln to $200bln)", "cap_large");
            translations.Add("Mid ($2bln to $10bln)", "cap_mid");
            translations.Add("Small ($300mln to $2bln)", "cap_small");
            translations.Add("Micro ($50mln to $300mln)", "cap_micro");
            translations.Add("Nano (under $50mln)", "cap_nano");
            translations.Add("+Large (over $10bln)", "cap_largeover");
            translations.Add("+Mid (over $2bln)", "cap_midover");
            translations.Add("+Small (over $300mln)", "cap_smallover");
            translations.Add("+Micro (over $50mln)", "cap_microover");
            translations.Add("-Large (under $200bln)", "cap_largeunder");
            translations.Add("-Mid (under $10bln)", "cap_midunder");
            translations.Add("-Small (under $2bln)", "cap_smallunder");
            translations.Add("-Micro (under $300mln)", "cap_microunder");
            translations.Add("Custom (Elite only)", "custom doesn't work");

            translations.Add("Any", "");
            translations.Add("Today", "earningsdate_today");
            translations.Add("Today Before Market Open", "earningsdate_todaybefore");
            translations.Add("Today After Market Close", "earningsdate_todayafter");
            translations.Add("Tomorrow", "earningsdate_tomorrow");
            translations.Add("Tomorrow Before Market Open", "earningsdate_tomorrowbefore");
            translations.Add("Tomorrow After Market Close", "earningsdate_tomorrowafter");
            translations.Add("Yesterday", "earningsdate_yesterday");
            translations.Add("Yesterday Before Market Open", "earningsdate_yesterdaybefore");
            translations.Add("Yesterday After Market Close", "earningsdate_yesterdayafter");
            translations.Add("Next 5 Days", "earningsdate_nextdays5");
            translations.Add("Previous 5 Days", "earningsdate_prevdays5");
            translations.Add("This Week", "earningsdate_thisweek");
            translations.Add("Next Week", "earningsdate_nextweek");
            translations.Add("Previous Week", "earningsdate_prevweek");
            translations.Add("This Month", "earningsdate_thismonth");
            translations.Add("Custom (Elite only)", "custom doesn't work");

            translations.Add("Any", "");
            translations.Add("targetprice_a50", "50% Above Price");
            translations.Add("targetprice_a40", "40% Above Price");
            translations.Add("targetprice_a30", "30% Above Price");
            translations.Add("targetprice_a20", "20% Above Price");
            translations.Add("targetprice_a10", "10% Above Price");
            translations.Add("targetprice_a5", "5% Above Price");
            translations.Add("targetprice_above", "Above Price");
            translations.Add("targetprice_below", "Below Price");
            translations.Add("targetprice_b5", "5% Below Price");
            translations.Add("targetprice_b10", "10% Below Price");
            translations.Add("targetprice_b20", "20% Below Price");
            translations.Add("targetprice_b30", "30% Below Price");
            translations.Add("targetprice_b40", "40% Below Price");
            translations.Add("targetprice_b50", "50% Below Price");


            translationExchange = new string[]
            {
                "Any",
                "exch_amex",
                "exch_nasd",
                "exch_nyse"
            };

            translationMarketCap = new string[]
            {
                "Any",
                "cap_mega",
                "cap_large",
                "cap_mid",
                "cap_small",
                "cap_micro",
                "cap_nano",
                "cap_largeover",
                "cap_midover",
                "cap_smallover",
                "cap_microover",
                "cap_largeunder",
                "cap_midunder",
                "cap_smallunder",
                "cap_microunder",
                "custom doesn't work"
            };

            translationEarningsDate = new string[]
            {
                "Any",
                "earningsdate_today",
                "earningsdate_todaybefore",
                "earningsdate_todayafter",
                "earningsdate_tomorrow",
                "earningsdate_tomorrowbefore",
                "earningsdate_tomorrowafter",
                "earningsdate_yesterday",
                "earningsdate_yesterdaybefore",
                "earningsdate_yesterdayafter",
                "earningsdate_nextdays5",
                "earningsdate_prevdays5",
                "earningsdate_thisweek",
                "earningsdate_nextweek",
                "earningsdate_prevweek",
                "earningsdate_thismonth",
                "custom doesn't work"
            };

            translationTargetPrice = new string[]
            {
                "Any",
                "targetprice_a50",
                "targetprice_a40",
                "targetprice_a30",
                "targetprice_a20",
                "targetprice_a10",
                "targetprice_a5",
                "targetprice_b50"
            };

            // Column 2
            translations.Add("Any", "");
            translations.Add("S&P 500", "idx_sp500");
            translations.Add("DJIA", "idx_dji");

            translations.Add("Any", "");
            translations.Add("None (0%)", "fa_div_none");
            translations.Add("Positive (>0%)", "fa_div_pos");
            translations.Add("High (>5%)", "fa_div_high");
            translations.Add("Very High (>10%)", "fa_div_veryhigh");
            translations.Add("Over 1%", "fa_div_o1");
            translations.Add("Over 2%", "fa_div_o2");
            translations.Add("Over 3%", "fa_div_o3");
            translations.Add("Over 4%", "fa_div_o4");
            translations.Add("Over 5%", "fa_div_o5");
            translations.Add("Over 6%", "fa_div_o6");
            translations.Add("Over 7%", "fa_div_o7");
            translations.Add("Over 8%", "fa_div_o8");
            translations.Add("Over 9%", "fa_div_o9");
            translations.Add("Over 10%", "fa_div_o10");
            translations.Add("Custom (Elite only)", "custom doesn't work");

            translations.Add("Any", "");
            translations.Add("Under 50K", "sh_avgvol_u50");
            translations.Add("Under 100K", "sh_avgvol_u100");
            translations.Add("Under 500K", "sh_avgvol_u500");
            translations.Add("Under 750K", "sh_avgvol_u750");
            translations.Add("Under 1M", "sh_avgvol_u1000");
            translations.Add("Over 50K", "sh_avgvol_o50");
            translations.Add("Over 100K", "sh_avgvol_o100");
            translations.Add("Over 200K", "sh_avgvol_o200");
            translations.Add("Over 300K", "sh_avgvol_o300");
            translations.Add("Over 400K", "sh_avgvol_o400");
            translations.Add("Over 500K", "sh_avgvol_o500");
            translations.Add("Over 750K", "sh_avgvol_o750");
            translations.Add("Over 1M", "sh_avgvol_o1000");
            translations.Add("Over 2M", "sh_avgvol_o2000");
            translations.Add("100K to 500K", "sh_avgvol_100to500");
            translations.Add("100K to 1M", "sh_avgvol_100to1000");
            translations.Add("500K to 1M", "sh_avgvol_500to1000");
            translations.Add("500K to 10M", "sh_avgvol_500to10000");
            translations.Add("Custom (Elite only)", "custom doesn't work");

            translations.Add("Any", "");
            translations.Add("Today", "ipodate_today");
            translations.Add("Yesterday", "ipodate_yesterday");
            translations.Add("In the last week", "ipodate_prevweek");
            translations.Add("In the last month", "ipodate_prevmonth");
            translations.Add("In the last quarter", "ipodate_prevquarter");
            translations.Add("In the last year", "ipodate_prevyear");
            translations.Add("In the last 2 years", "ipodate_prev2yrs");
            translations.Add("In the last 3 years", "ipodate_prev3yrs");
            translations.Add("In the last 5 years", "ipodate_prev5yrs");
            translations.Add("More than a year ago", "ipodate_more1");
            translations.Add("More than 5 years ago", "ipodate_more5");
            translations.Add("More than 10 years ago", "ipodate_more10");
            translations.Add("More than 15 years ago", "ipodate_more15");
            translations.Add("More than 20 years ago", "ipodate_more20");
            translations.Add("More than 25 years ago", "ipodate_more25");
            translations.Add("Custom (Elite only)", "custom doesn't work");


            translationIndex = new string[]
            {
                "Any",
                "idx_sp500",
                "idx_dji"
            };

            translationDividendYield = new string[]
            {
                "Any",
                "fa_div_none",
                "fa_div_pos",
                "fa_div_high",
                "fa_div_veryhigh",
                "fa_div_o1",
                "fa_div_o2",
                "fa_div_o3",
                "fa_div_o4",
                "fa_div_o5",
                "fa_div_o6",
                "fa_div_o7",
                "fa_div_o8",
                "fa_div_o9",
                "fa_div_o10",
                "custom doesn't work"
            };

            translationAverageVolume = new string[]
            {
                "Any",
                "sh_avgvol_u50",
                "sh_avgvol_u100",
                "sh_avgvol_u500",
                "sh_avgvol_u750",
                "sh_avgvol_u1000",
                "sh_avgvol_o50",
                "sh_avgvol_o100",
                "sh_avgvol_o200",
                "sh_avgvol_o300",
                "sh_avgvol_o400",
                "sh_avgvol_o500",
                "sh_avgvol_o750",
                "sh_avgvol_o1000",
                "sh_avgvol_o2000",
                "sh_avgvol_100to500",
                "sh_avgvol_100to1000",
                "sh_avgvol_500to1000",
                "sh_avgvol_500to10000",
                "custom doesn't work"
            };

            translationIpoDate = new string[]
            {
                "Any",
                "ipodate_today",
                "ipodate_yesterday",
                "ipodate_prevweek",
                "ipodate_prevmonth",
                "ipodate_prevquarter",
                "ipodate_prevyear",
                "ipodate_prev2yrs",
                "ipodate_prev3yrs",
                "ipodate_prev5yrs",
                "ipodate_more1",
                "ipodate_more5",
                "ipodate_more10",
                "ipodate_more15",
                "ipodate_more20",
                "ipodate_more25",
                "custom doesn't work"
            };

            // Column 3
            translations.Add("Any", "");
            translations.Add("Basic Materials", "sec_basicmaterials");
            translations.Add("Communication Services", "sec_communicationservices");
            translations.Add("Consumer Cyclical", "sec_consumercyclical");
            translations.Add("Consumer Defensive", "sec_consumerdefensive");
            translations.Add("Energy", "sec_energy");
            translations.Add("Financial", "sec_financial");
            translations.Add("Healthcare", "sec_healthcare");
            translations.Add("Industrials", "sec_industrials");
            translations.Add("Real Estate", "sec_realestate");
            translations.Add("Technology", "sec_technology");
            translations.Add("Utilities", "sec_utilities");
            translations.Add("Custom (Elite only)", "custom doesn't work");

            translations.Add("Any", "");
            translations.Add("Low (<5%)", "sh_short_low");
            translations.Add("High (>20%)", "sh_short_high");
            translations.Add("Under 5%", "sh_short_u5");
            translations.Add("Under 10%", "sh_short_u10");
            translations.Add("Under 15%", "sh_short_u15");
            translations.Add("Under 20%", "sh_short_u20");
            translations.Add("Under 25%", "sh_short_u25");
            translations.Add("Under 30%", "sh_short_u30");
            translations.Add("Over 5%", "sh_short_o5");
            translations.Add("Over 10%", "sh_short_o10");
            translations.Add("Over 15%", "sh_short_o15");
            translations.Add("Over 20%", "sh_short_o20");
            translations.Add("Over 25%", "sh_short_o25");
            translations.Add("Over 30%", "sh_short_o30");
            translations.Add("Custom (Elite only)", "custom doesn't work");

            translations.Add("Any", "");
            translations.Add("Over 10", "sh_relvol_o10");
            translations.Add("Over 5", "sh_relvol_o5");
            translations.Add("Over 3", "sh_relvol_o3");
            translations.Add("Over 2", "sh_relvol_o2");
            translations.Add("Over 1.5", "sh_relvol_o1.5");
            translations.Add("Over 1", "sh_relvol_o1");
            translations.Add("Over 0.75", "sh_relvol_o0.75");
            translations.Add("Over 0.5", "sh_relvol_o0.5");
            translations.Add("Over 0.25", "sh_relvol_o0.25");
            translations.Add("Under 2", "sh_relvol_u2");
            translations.Add("Under 1.5", "sh_relvol_u1.5");
            translations.Add("Under 1", "sh_relvol_u1");
            translations.Add("Under 0.75", "sh_relvol_u0.75");
            translations.Add("Under 0.5", "sh_relvol_u0.5");
            translations.Add("Under 0.25", "sh_relvol_u0.25");
            translations.Add("Under 0.1", "sh_relvol_u0.1");
            translations.Add("Custom (Elite only)", "dddddd");

            translations.Add("Any", "");
            translations.Add("Under 1M", "sh_outstanding_u1");
            translations.Add("Under 5M", "sh_outstanding_u5");
            translations.Add("Under 10M", "sh_outstanding_u10");
            translations.Add("Under 20M", "sh_outstanding_u20");
            translations.Add("Under 50M", "sh_outstanding_u50");
            translations.Add("Under 100M", "sh_outstanding_u100");
            translations.Add("Over 1M", "sh_outstanding_o1");
            translations.Add("Over 2M", "sh_outstanding_o2");
            translations.Add("Over 5M", "sh_outstanding_o5");
            translations.Add("Over 10M", "sh_outstanding_o10");
            translations.Add("Over 20M", "sh_outstanding_o20");
            translations.Add("Over 50M", "sh_outstanding_o50");
            translations.Add("Over 100M", "sh_outstanding_o100");
            translations.Add("Over 200M", "sh_outstanding_o200");
            translations.Add("Over 500M", "sh_outstanding_o500");
            translations.Add("Over 1000M", "sh_outstanding_o1000");
            translations.Add("Custom (Elite only)", "custom doesn't work");

            translationSector = new string[]
            {
                "Any",
                "sec_basicmaterials",
                "sec_communicationservices",
                "sec_consumercyclical",
                "sec_consumerdefensive",
                "sec_energy",
                "sec_financial",
                "sec_healthcare",
                "sec_industrials",
                "sec_realestate",
                "sec_technology",
                "sec_utilities",
                "custom doesn't work"
            };

            translationFloatShort = new string[]
            {
                "Any",
                "sh_short_low",
                "sh_short_high",
                "sh_short_u5",
                "sh_short_u10",
                "sh_short_u15",
                "sh_short_u20",
                "sh_short_u25",
                "sh_short_u30",
                "sh_short_o5",
                "sh_short_o10",
                "sh_short_o15",
                "sh_short_o20",
                "sh_short_o25",
                "sh_short_o30",
                "custom doesn't work"
            };

            translationRelativeVolume = new string[]
            {
                "Any",
                "sh_relvol_o10",
                "sh_relvol_o5",
                "sh_relvol_o3",
                "sh_relvol_o2",
                "sh_relvol_o1.5",
                "sh_relvol_o1",
                "sh_relvol_o0.75",
                "sh_relvol_o0.5",
                "sh_relvol_o0.25",
                "sh_relvol_u2",
                "sh_relvol_u1.5",
                "sh_relvol_u1",
                "sh_relvol_u0.75",
                "sh_relvol_u0.5",
                "sh_relvol_u0.25",
                "sh_relvol_u0.1",
                "custom doesn't work"
            };

            translationSharesOutstanding = new string[]
            {
                "Any",
                "sh_outstanding_u1",
                "sh_outstanding_u5",
                "sh_outstanding_u10",
                "sh_outstanding_u20",
                "sh_outstanding_u50",
                "sh_outstanding_u100",
                "sh_outstanding_o1",
                "sh_outstanding_o2",
                "sh_outstanding_o5",
                "sh_outstanding_o10",
                "sh_outstanding_o20",
                "sh_outstanding_o50",
                "sh_outstanding_o100",
                "sh_outstanding_o200",
                "sh_outstanding_o500",
                "sh_outstanding_o1000",
                "custom doesn't work"
            };

            // Column 4
            translations.Add("Any", "ind_stocksonly");
            translations.Add("Stocks only (ex-Funds)", "ind_exchangetradedfund");
            translations.Add("Exchange Traded Fund", "ind_advertisingagencies");
            translations.Add("Advertising Agencies", "ind_aerospacedefense");
            translations.Add("Aerospace &amp; Defense", "ind_agriculturalinputs");
            translations.Add("Agricultural Inputs", "ind_airlines");
            translations.Add("Airlines", "ind_airportsairservices");
            translations.Add("Airports &amp; Air Services", "ind_aluminum");
            translations.Add("Aluminum", "ind_apparelmanufacturing");
            translations.Add("Apparel Manufacturing", "ind_apparelretail");
            translations.Add("Apparel Retail", "ind_assetmanagement");
            translations.Add("Asset Management", "ind_autoparts");
            translations.Add("Auto Manufacturers", "ind_autotruckdealerships");
            translations.Add("Auto Parts", "ind_banksdiversified");
            translations.Add("Auto &amp; Truck Dealerships", "ind_banksregional");
            translations.Add("Banks - Diversified", "ind_banksregional");
            translations.Add("Banks - Regional", "ind_beveragesbrewers");
            translations.Add("Beverages - Brewers", "ind_beveragesbrewers");
            translations.Add("Beverages - Non-Alcoholic", "ind_beveragesnonalcoholic");
            translations.Add("Beverages - Wineries &amp; Distilleries", "ind_beverageswineriesdistilleries");
            translations.Add("Biotechnology", "ind_biotechnology");
            translations.Add("Broadcasting", "ind_broadcasting");
            translations.Add("Building Materials", "ind_buildingmaterials");
            translations.Add("Building Products &amp; Equipment", "ind_buildingproductsequipment");
            translations.Add("Business Equipment &amp; Supplies", "ind_businessequipmentsupplies");
            translations.Add("Capital Markets", "ind_capitalmarkets");
            translations.Add("Chemicals", "ind_chemicals");
            translations.Add("Closed-End Fund - Debt", "ind_closedendfunddebt");
            translations.Add("Closed-End Fund - Equity", "ind_closedendfundequity");
            translations.Add("Closed-End Fund - Foreign", "ind_closedendfundforeign");
            translations.Add("Coking Coal", "ind_cokingcoal");
            translations.Add("Communication Equipment", "ind_communicationequipment");
            translations.Add("Computer Hardware", "ind_computerhardware");
            translations.Add("Confectioners", "ind_confectioners");
            translations.Add("Conglomerates", "ind_conglomerates");
            translations.Add("Consulting Services", "ind_consultingservices");
            translations.Add("Consumer Electronics", "ind_consumerelectronics");
            translations.Add("Copper", "ind_copper");
            translations.Add("Credit Services", "ind_creditservices");
            translations.Add("Department Stores", "ind_departmentstores");
            translations.Add("Diagnostics &amp; Research", "ind_diagnosticsresearch");
            translations.Add("Discount Stores", "ind_discountstores");
            translations.Add("Drug Manufacturers - General", "ind_drugmanufacturersgeneral");
            translations.Add("Drug Manufacturers - Specialty &amp; Generic", "ind_drugmanufacturersspecialtygeneric");
            translations.Add("Education &amp; Training Services", "ind_educationtrainingservices");
            translations.Add("Electrical Equipment &amp; Parts", "ind_electricalequipmentparts");
            translations.Add("Electronic Components", "ind_electroniccomponents");
            translations.Add("Electronic Gaming &amp; Multimedia", "ind_electronicgamingmultimedia");
            translations.Add("Electronics &amp; Computer Distribution", "ind_electronicscomputerdistribution");
            translations.Add("Engineering &amp; Construction", "ind_engineeringconstruction");
            translations.Add("Entertainment", "ind_entertainment");
            translations.Add("Exchange Traded Fund", "ind_exchangetradedfund");
            translations.Add("Farm &amp; Heavy Construction Machinery", "ind_farmheavyconstructionmachinery");
            translations.Add("Farm Products", "ind_farmproducts");
            translations.Add("Financial Conglomerates", "ind_financialconglomerates");
            translations.Add("Financial Data &amp; Stock Exchanges", "ind_financialdatastockexchanges");
            translations.Add("Food Distribution", "ind_fooddistribution");
            translations.Add("Footwear &amp; Accessories", "ind_footwearaccessories");
            translations.Add("Furnishings, Fixtures &amp; Appliances", "ind_furnishingsfixturesappliances");
            translations.Add("Gambling", "ind_gambling");
            translations.Add("Gold", "ind_gold");
            translations.Add("Grocery Stores", "ind_grocerystores");
            translations.Add("Healthcare Plans", "ind_healthcareplans");
            translations.Add("Health Information Services", "ind_healthinformationservices");
            translations.Add("Home Improvement Retail", "ind_homeimprovementretail");
            translations.Add("Household &amp; Personal Products", "ind_householdpersonalproducts");
            translations.Add("Industrial Distribution", "ind_industrialdistribution");
            translations.Add("Information Technology Services", "ind_informationtechnologyservices");
            translations.Add("Infrastructure Operations", "ind_infrastructureoperations");
            translations.Add("Insurance Brokers", "ind_insurancebrokers");
            translations.Add("Insurance - Diversified", "ind_insurancediversified");
            translations.Add("Insurance - Life", "ind_insurancelife");
            translations.Add("Insurance - Property &amp; Casualty", "ind_insurancepropertycasualty");
            translations.Add("Insurance - Reinsurance", "ind_insurancereinsurance");
            translations.Add("Insurance - Specialty", "ind_insurancespecialty");
            translations.Add("Integrated Freight &amp; Logistics", "ind_integratedfreightlogistics");
            translations.Add("Internet Content &amp; Information", "ind_internetcontentinformation");
            translations.Add("Internet Retail", "ind_internetretail");
            translations.Add("Leisure", "ind_leisure");
            translations.Add("Lodging", "ind_lodging");
            translations.Add("Lumber &amp; Wood Production", "ind_lumberwoodproduction");
            translations.Add("Luxury Goods", "ind_luxurygoods");
            translations.Add("Marine Shipping", "ind_marineshipping");
            translations.Add("Medical Care Facilities", "ind_medicalcarefacilities");
            translations.Add("Medical Devices", "ind_medicaldevices");
            translations.Add("Medical Distribution", "ind_medicaldistribution");
            translations.Add("Medical Instruments &amp; Supplies", "ind_medicalinstrumentssupplies");
            translations.Add("Metal Fabrication", "ind_metalfabrication");
            translations.Add("Mortgage Finance", "ind_mortgagefinance");
            translations.Add("Oil &amp; Gas Drilling", "ind_oilgasdrilling");
            translations.Add("Oil &amp; Gas E&amp;P", "ind_oilgasep");
            translations.Add("Oil &amp; Gas Equipment &amp; Services", "ind_oilgasequipmentservices");
            translations.Add("Oil &amp; Gas Integrated", "ind_oilgasintegrated");
            translations.Add("Oil &amp; Gas Midstream", "ind_oilgasmidstream");
            translations.Add("Oil &amp; Gas Refining &amp; Marketing", "ind_oilgasrefiningmarketing");
            translations.Add("Other Industrial Metals &amp; Mining", "ind_otherindustrialmetalsmining");
            translations.Add("Other Precious Metals &amp; Mining", "ind_otherpreciousmetalsmining");
            translations.Add("Packaged Foods", "ind_packagedfoods");
            translations.Add("Packaging &amp; Containers", "ind_packagingcontainers");
            translations.Add("Paper &amp; Paper Products", "ind_paperpaperproducts");
            translations.Add("Personal Services", "ind_personalservices");
            translations.Add("Pharmaceutical Retailers", "ind_pharmaceuticalretailers");
            translations.Add("Pollution &amp; Treatment Controls", "ind_pollutiontreatmentcontrols");
            translations.Add("Publishing", "ind_publishing");
            translations.Add("Railroads", "ind_railroads");
            translations.Add("Real Estate - Development", "ind_realestatedevelopment");
            translations.Add("Real Estate - Diversified", "ind_realestatediversified");
            translations.Add("Real Estate Services", "ind_realestateservices");
            translations.Add("Recreational Vehicles", "ind_recreationalvehicles");
            translations.Add("REIT - Diversified", "ind_reitdiversified");
            translations.Add("REIT - Healthcare Facilities", "ind_reithealthcarefacilities");
            translations.Add("REIT - Hotel &amp; Motel", "ind_reithotelmotel");
            translations.Add("REIT - Industrial", "ind_reitindustrial");
            translations.Add("REIT - Mortgage", "ind_reitmortgage");
            translations.Add("REIT - Office", "ind_reitoffice");
            translations.Add("REIT - Residential", "ind_reitresidential");
            translations.Add("REIT - Retail", "ind_reitretail");
            translations.Add("REIT - Specialty", "ind_reitspecialty");
            translations.Add("Rental &amp; Leasing Services", "ind_rentalleasingservices");
            translations.Add("Residential Construction", "ind_residentialconstruction");
            translations.Add("Resorts &amp; Casinos", "ind_resortscasinos");
            translations.Add("Restaurants", "ind_restaurants");
            translations.Add("Scientific &amp; Technical Instruments", "ind_scientifictechnicalinstruments");
            translations.Add("Security &amp; Protection Services", "ind_securityprotectionservices");
            translations.Add("Semiconductor Equipment &amp; Materials", "ind_semiconductorequipmentmaterials");
            translations.Add("Semiconductors", "ind_semiconductors");
            translations.Add("Shell Companies", "ind_shellcompanies");
            translations.Add("Silver", "ind_silver");
            translations.Add("Software - Application", "ind_softwareapplication");
            translations.Add("Software - Infrastructure", "ind_softwareinfrastructure");
            translations.Add("Solar", "ind_solar");
            translations.Add("Specialty Business Services", "ind_specialtybusinessservices");
            translations.Add("Specialty Chemicals", "ind_specialtychemicals");
            translations.Add("Specialty Industrial Machinery", "ind_specialtyindustrialmachinery");
            translations.Add("Specialty Retail", "ind_specialtyretail");
            translations.Add("Staffing &amp; Employment Services", "ind_staffingemploymentservices");
            translations.Add("Steel", "ind_steel");
            translations.Add("Telecom Services", "ind_telecomservices");
            translations.Add("Textile Manufacturing", "ind_textilemanufacturing");
            translations.Add("Thermal Coal", "ind_thermalcoal");
            translations.Add("Tobacco", "ind_tobacco");
            translations.Add("Tools &amp; Accessories", "ind_toolsaccessories");
            translations.Add("Travel Services", "ind_travelservices");
            translations.Add("Trucking", "ind_trucking");
            translations.Add("Uranium", "ind_uranium");
            translations.Add("Utilities - Diversified", "ind_utilitiesdiversified");
            translations.Add("Utilities - Independent Power Producers", "ind_utilitiesindependentpowerproducers");
            translations.Add("Utilities - Regulated Electric", "ind_utilitiesregulatedelectric");
            translations.Add("Utilities - Regulated Gas", "ind_utilitiesregulatedgas");
            translations.Add("Utilities - Regulated Water", "ind_utilitiesregulatedwater");
            translations.Add("Utilities - Renewable", "ind_utilitiesrenewable");
            translations.Add("Waste Management", "ind_wastemanagement");
            translations.Add("Custom (Elite only)", "custom doesn't work");

            translations.Add("Any", "");
            translations.Add("Strong Buy (1)", "an_recom_strongbuy");
            translations.Add("Buy or better", "an_recom_buybetter");
            translations.Add("Buy", "an_recom_buy");
            translations.Add("Hold or better", "an_recom_holdbetter");
            translations.Add("Hold", "an_recom_hold");
            translations.Add("Hold or worse", "an_recom_holdworse");
            translations.Add("Sell", "an_recom_sell");
            translations.Add("Sell or worse", "an_recom_sellworse");
            translations.Add("Strong Sell (5)", "an_recom_strongsell");
            translations.Add("Custom (Elite only)", "custom doesn't work");

            translations.Add("Any", "");
            translations.Add("Cusdddddy", "Under 50K");
            translations.Add("Cusdddddy", "Under 100K");
            translations.Add("Cusdddddy", "Under 500K");
            translations.Add("Cusdddddy", "Under 750K");
            translations.Add("Cusdddddy", "Under 1M");
            translations.Add("Cusdddddy", "Over 0");
            translations.Add("Cusdddddy", "Over 50K");
            translations.Add("Cusdddddy", "Over 100K");
            translations.Add("Cusdddddy", "Over 200K");
            translations.Add("Cusdddddy", "Over 300K");
            translations.Add("Cusdddddy", "Over 400K");
            translations.Add("Cusdddddy", "Over 500K");
            translations.Add("Cusdddddy", "Over 750K");
            translations.Add("Cusdddddy", "Over 1M");
            translations.Add("Cusdddddy", "Over 2M");
            translations.Add("Cusdddddy", "Over 5M");
            translations.Add("Cusdddddy", "Over 10M");
            translations.Add("Cusdddddy", "Over 20M");
            translations.Add("Custom (Elite only)", "Cusdddddy");

            translations.Add("Any", "");
            translations.Add("Under 50K", "sh_curvol_u50");
            translations.Add("Under 100K", "sh_curvol_u100");
            translations.Add("Under 500K", "sh_curvol_u500");
            translations.Add("Under 750K", "sh_curvol_u750");
            translations.Add("Under 1M", "sh_curvol_u1000");
            translations.Add("Over 0", "sh_curvol_o0");
            translations.Add("Over 50K", "sh_curvol_o50");
            translations.Add("Over 100K", "sh_curvol_o100");
            translations.Add("Over 200K", "sh_curvol_o200");
            translations.Add("Over 300K", "sh_curvol_o300");
            translations.Add("Over 400K", "sh_curvol_o400");
            translations.Add("Over 500K", "sh_curvol_o500");
            translations.Add("Over 750K", "sh_curvol_o750");
            translations.Add("Over 1M", "sh_curvol_o1000");
            translations.Add("Over 2M", "sh_curvol_o2000");
            translations.Add("Over 5M", "sh_curvol_o5000");
            translations.Add("Over 10M", "sh_curvol_o10000");
            translations.Add("Over 20M", "sh_curvol_o20000");
            translations.Add("Custom (Elite only)", "Cusdddddy");

            translations.Add("Any", "");
            translations.Add("Under 1M", "sh_float_u1");
            translations.Add("Under 5M", "sh_float_u5");
            translations.Add("Under 10M", "sh_float_u10");
            translations.Add("Under 20M", "sh_float_u20");
            translations.Add("Under 50M", "sh_float_u50");
            translations.Add("Under 100M", "sh_float_u100");
            translations.Add("Over 1M", "sh_float_o1");
            translations.Add("Over 2M", "sh_float_o2");
            translations.Add("Over 5M", "sh_float_o5");
            translations.Add("Over 10M", "sh_float_o10");
            translations.Add("Over 20M", "sh_float_o20");
            translations.Add("Over 50M", "sh_float_o50");
            translations.Add("Over 100M", "sh_float_o100");
            translations.Add("Over 200M", "sh_float_o200");
            translations.Add("Over 500M", "sh_float_o500");
            translations.Add("Over 1000M", "sh_float_o1000");
            translations.Add("Custom (Elite only)", "custom doesn't work");


            translationIndustry = new string[]
            {
                "Any",
                "ind_stocksonly",
                "ind_exchangetradedfund",
                "ind_advertisingagencies",
                "ind_aerospacedefense",
                "ind_agriculturalinputs",
                "ind_airlines",
                "ind_airportsairservices",
                "ind_aluminum",
                "ind_apparelmanufacturing",
                "ind_apparelretail",
                "ind_assetmanagement",
                "ind_autoparts",
                "ind_autotruckdealerships",
                "ind_banksdiversified",
                "ind_banksregional",
                "ind_banksregional",
                "ind_beveragesbrewers",
                "ind_beveragesbrewers",
                "ind_beveragesnonalcoholic",
                "ind_beverageswineriesdistilleries",
                "ind_biotechnology",
                "ind_broadcasting",
                "ind_buildingmaterials",
                "ind_buildingproductsequipment",
                "ind_businessequipmentsupplies",
                "ind_capitalmarkets",
                "ind_chemicals",
                "ind_closedendfunddebt",
                "ind_closedendfundequity",
                "ind_closedendfundforeign",
                "ind_cokingcoal",
                "ind_communicationequipment",
                "ind_computerhardware",
                "ind_confectioners",
                "ind_conglomerates",
                "ind_consultingservices",
                "ind_consumerelectronics",
                "ind_copper",
                "ind_creditservices",
                "ind_departmentstores",
                "ind_diagnosticsresearch",
                "ind_discountstores",
                "ind_drugmanufacturersgeneral",
                "ind_drugmanufacturersspecialtygeneric",
                "ind_educationtrainingservices",
                "ind_electricalequipmentparts",
                "ind_electroniccomponents",
                "ind_electronicgamingmultimedia",
                "ind_electronicscomputerdistribution",
                "ind_engineeringconstruction",
                "ind_entertainment",
                "ind_exchangetradedfund",
                "ind_farmheavyconstructionmachinery",
                "ind_farmproducts",
                "ind_financialconglomerates",
                "ind_financialdatastockexchanges",
                "ind_fooddistribution",
                "ind_footwearaccessories",
                "ind_furnishingsfixturesappliances",
                "ind_gambling",
                "ind_gold",
                "ind_grocerystores",
                "ind_healthcareplans",
                "ind_healthinformationservices",
                "ind_homeimprovementretail",
                "ind_householdpersonalproducts",
                "ind_industrialdistribution",
                "ind_informationtechnologyservices",
                "ind_infrastructureoperations",
                "ind_insurancebrokers",
                "ind_insurancediversified",
                "ind_insurancelife",
                "ind_insurancepropertycasualty",
                "ind_insurancereinsurance",
                "ind_insurancespecialty",
                "ind_integratedfreightlogistics",
                "ind_internetcontentinformation",
                "ind_internetretail",
                "ind_leisure",
                "ind_lodging",
                "ind_lumberwoodproduction",
                "ind_luxurygoods",
                "ind_marineshipping",
                "ind_medicalcarefacilities",
                "ind_medicaldevices",
                "ind_medicaldistribution",
                "ind_medicalinstrumentssupplies",
                "ind_metalfabrication",
                "ind_mortgagefinance",
                "ind_oilgasdrilling",
                "ind_oilgasep",
                "ind_oilgasequipmentservices",
                "ind_oilgasintegrated",
                "ind_oilgasmidstream",
                "ind_oilgasrefiningmarketing",
                "ind_otherindustrialmetalsmining",
                "ind_otherpreciousmetalsmining",
                "ind_packagedfoods",
                "ind_packagingcontainers",
                "ind_paperpaperproducts",
                "ind_personalservices",
                "ind_pharmaceuticalretailers",
                "ind_pollutiontreatmentcontrols",
                "ind_publishing",
                "ind_railroads",
                "ind_realestatedevelopment",
                "ind_realestatediversified",
                "ind_realestateservices",
                "ind_recreationalvehicles",
                "ind_reitdiversified",
                "ind_reithealthcarefacilities",
                "ind_reithotelmotel",
                "ind_reitindustrial",
                "ind_reitmortgage",
                "ind_reitoffice",
                "ind_reitresidential",
                "ind_reitretail",
                "ind_reitspecialty",
                "ind_rentalleasingservices",
                "ind_residentialconstruction",
                "ind_resortscasinos",
                "ind_restaurants",
                "ind_scientifictechnicalinstruments",
                "ind_securityprotectionservices",
                "ind_semiconductorequipmentmaterials",
                "ind_semiconductors",
                "ind_shellcompanies",
                "ind_silver",
                "ind_softwareapplication",
                "ind_softwareinfrastructure",
                "ind_solar",
                "ind_specialtybusinessservices",
                "ind_specialtychemicals",
                "ind_specialtyindustrialmachinery",
                "ind_specialtyretail",
                "ind_staffingemploymentservices",
                "ind_steel",
                "ind_telecomservices",
                "ind_textilemanufacturing",
                "ind_thermalcoal",
                "ind_tobacco",
                "ind_toolsaccessories",
                "ind_travelservices",
                "ind_trucking",
                "ind_uranium",
                "ind_utilitiesdiversified",
                "ind_utilitiesindependentpowerproducers",
                "ind_utilitiesregulatedelectric",
                "ind_utilitiesregulatedgas",
                "ind_utilitiesregulatedwater",
                "ind_utilitiesrenewable",
                "ind_wastemanagement",
                "custom doesn't work"
            };

            translationAnalystRecomends = new string[]
            {
                "Any",
                "an_recom_strongbuy",
                "an_recom_buybetter",
                "an_recom_buy",
                "an_recom_holdbetter",
                "an_recom_hold",
                "an_recom_holdworse",
                "an_recom_sell",
                "an_recom_sellworse",
                "an_recom_strongsell",
                "custom doesn't work"
            };

            translationCurrentVolume = new string[]
            {
                "Any",
                "sh_curvol_u50",
                "sh_curvol_u100",
                "sh_curvol_u500",
                "sh_curvol_u750",
                "sh_curvol_u1000",
                "sh_curvol_o0",
                "sh_curvol_o50",
                "sh_curvol_o100",
                "sh_curvol_o200",
                "sh_curvol_o300",
                "sh_curvol_o400",
                "sh_curvol_o500",
                "sh_curvol_o750",
                "sh_curvol_o1000",
                "sh_curvol_o2000",
                "sh_curvol_o5000",
                "sh_curvol_o10000",
                "sh_curvol_o20000",
                "custom doesn't work"
            };

            translationFloat = new string[]
            {
                "Any",
                "sh_float_u1",
                "sh_float_u5",
                "sh_float_u10",
                "sh_float_u20",
                "sh_float_u50",
                "sh_float_u100",
                "sh_float_o1",
                "sh_float_o2",
                "sh_float_o5",
                "sh_float_o10",
                "sh_float_o20",
                "sh_float_o50",
                "sh_float_o100",
                "sh_float_o200",
                "sh_float_o500",
                "sh_float_o1000",
                "custom doesn't work"

            };

            // Column 5
            translations.Add("Any", "");
            translations.Add("USA", "geo_usa");
            translations.Add("Foreign (ex-USA)", "geo_notusa");
            translations.Add("Asia", "geo_asia");
            translations.Add("Europe", "geo_europe");
            translations.Add("Latin America", "geo_latinamerica");
            translations.Add("BRIC", "geo_bric");
            translations.Add("Argentina", "geo_argentina");
            translations.Add("Australia", "geo_australia");
            translations.Add("Bahamas", "geo_bahamas");
            translations.Add("Belgium", "geo_belgium");
            translations.Add("BeNeLux", "geo_benelux");
            translations.Add("Bermuda", "geo_bermuda");
            translations.Add("Brazil", "geo_brazil");
            translations.Add("Canada", "geo_canada");
            translations.Add("Cayman Islands", "geo_caymanislands");
            translations.Add("Chile", "geo_chile");
            translations.Add("China", "geo_china");
            translations.Add("China & Hong Kong", "geo_chinahongkong");
            translations.Add("Colombia", "geo_colombia");
            translations.Add("Cyprus", "geo_cyprus");
            translations.Add("Denmark", "geo_denmark");
            translations.Add("Finland", "geo_finland");
            translations.Add("France", "geo_france");
            translations.Add("Germany", "geo_germany");
            translations.Add("Greece", "geo_greece");
            translations.Add("Hong Kong", "geo_hongkong");
            translations.Add("Hungary", "geo_hungary");
            translations.Add("Iceland", "geo_iceland");
            translations.Add("India", "geo_india");
            translations.Add("Indonesia", "geo_indonesia");
            translations.Add("Ireland", "geo_ireland");
            translations.Add("Israel", "geo_israel");
            translations.Add("Italy", "geo_italy");
            translations.Add("Japan", "geo_japan");
            translations.Add("Kazakhstan", "geo_kazakhstan");
            translations.Add("Luxembourg", "geo_luxembourg");
            translations.Add("Malaysia", "geo_malaysia");
            translations.Add("Malta", "geo_malta");
            translations.Add("Mexico", "geo_mexico");
            translations.Add("Monaco", "geo_monaco");
            translations.Add("Netherlands", "geo_netherlands");
            translations.Add("New Zealand", "geo_newzealand");
            translations.Add("Norway", "geo_norway");
            translations.Add("Panama", "geo_panama");
            translations.Add("Peru", "geo_peru");
            translations.Add("Philippines", "geo_philippines");
            translations.Add("Portugal", "geo_portugal");
            translations.Add("Russia", "geo_russia");
            translations.Add("Singapore", "geo_singapore");
            translations.Add("South Africa", "geo_southafrica");
            translations.Add("South Korea", "geo_southkorea");
            translations.Add("Spain", "geo_spain");
            translations.Add("Sweden", "geo_sweden");
            translations.Add("Switzerland", "geo_switzerland");
            translations.Add("Taiwan", "geo_taiwan");
            translations.Add("Turkey", "geo_turkey");
            translations.Add("United Arab Emirates", "geo_unitedarabemirates");
            translations.Add("United Kingdom", "geo_unitedkingdom");
            translations.Add("Uruguay", "geo_uruguay");
            translations.Add("Custom (Elite only)", "custom doesn't work");

            translations.Add("Any", "");
            translations.Add("Optionable", "sh_opt_option");
            translations.Add("Shortable", "sh_opt_short");
            translations.Add("Optionable and shortable", "sh_opt_optionshort");
            
            translations.Add("Any", "");
            translations.Add("Under $1", "sh_price_u1");
            translations.Add("Under $2", "sh_price_u2");
            translations.Add("Under $3", "sh_price_u3");
            translations.Add("Under $4", "sh_price_u4");
            translations.Add("Under $5", "sh_price_u5");
            translations.Add("Under $7", "sh_price_u6");
            translations.Add("Under $10", "sh_price_u7");
            translations.Add("Under $15", "sh_price_u10");
            translations.Add("Under $20", "sh_price_u15");
            translations.Add("Under $30", "sh_price_u20");
            translations.Add("Under $40", "sh_price_u30");
            translations.Add("Under $50", "sh_price_u40");
            translations.Add("Over $1", "sh_price_u50");
            translations.Add("Over $2", "sh_price_o1");
            translations.Add("Over $3", "sh_price_o2");
            translations.Add("Over $4", "sh_price_o3");
            translations.Add("Over $5", "sh_price_o4");
            translations.Add("Over $6", "sh_price_o5");
            translations.Add("Over $7", "sh_price_o6");
            translations.Add("Over $10", "sh_price_o7");
            translations.Add("Over $15", "sh_price_o10");
            translations.Add("Over $20", "sh_price_o15");
            translations.Add("Over $30", "sh_price_o20");
            translations.Add("Over $40", "sh_price_o30");
            translations.Add("Over $50", "sh_price_o40");
            translations.Add("Over $60", "sh_price_o50");
            translations.Add("Over $70", "sh_price_o60");
            translations.Add("Over $80", "sh_price_o70");
            translations.Add("Over $90", "sh_price_o80");
            translations.Add("Over $100", "sh_price_o90");
            translations.Add("$1 to $5", "sh_price_o100");
            translations.Add("$1 to $10", "sh_price_1to5");
            translations.Add("$1 to $20", "sh_price_1to10");
            translations.Add("$5 to $10", "sh_price_1to20");
            translations.Add("$5 to $20", "sh_price_5to10");
            translations.Add("$5 to $50", "sh_price_5to20");
            translations.Add("$10 to $20", "sh_price_5to50");
            translations.Add("$10 to $50", "sh_price_10to20");
            translations.Add("$20 to $50", "sh_price_20to50");
            translations.Add("$50 to $100", "sh_price_500to100");


            translationCountry = new string[]
            {
                "Any",
                "geo_usa",
                "geo_notusa",
                "geo_asia",
                "geo_europe",
                "geo_latinamerica",
                "geo_bric",
                "geo_argentina",
                "geo_australia",
                "geo_bahamas",
                "geo_belgium",
                "geo_benelux",
                "geo_bermuda",
                "geo_brazil",
                "geo_canada",
                "geo_caymanislands",
                "geo_chile",
                "geo_china",
                "geo_chinahongkong",
                "geo_colombia",
                "geo_cyprus",
                "geo_denmark",
                "geo_finland",
                "geo_france",
                "geo_germany",
                "geo_greece",
                "geo_hongkong",
                "geo_hungary",
                "geo_iceland",
                "geo_india",
                "geo_indonesia",
                "geo_ireland",
                "geo_israel",
                "geo_italy",
                "geo_japan",
                "geo_kazakhstan",
                "geo_luxembourg",
                "geo_malaysia",
                "geo_malta",
                "geo_mexico",
                "geo_monaco",
                "geo_netherlands",
                "geo_newzealand",
                "geo_norway",
                "geo_panama",
                "geo_peru",
                "geo_philippines",
                "geo_portugal",
                "geo_russia",
                "geo_singapore",
                "geo_southafrica",
                "geo_southkorea",
                "geo_spain",
                "geo_sweden",
                "geo_switzerland",
                "geo_taiwan",
                "geo_turkey",
                "geo_unitedarabemirates",
                "geo_unitedkingdom",
                "geo_uruguay",
                "custom doesn't work"
            };

            translationOptionShort = new string[]
            {
                "Any",
                "sh_opt_option",
                "sh_opt_short",
                "sh_opt_optionshort"
            };

            translationPrice = new string[]
            {
                "Any",
                "sh_price_u1",
                "sh_price_u2",
                "sh_price_u3",
                "sh_price_u4",
                "sh_price_u5",
                "sh_price_u6",
                "sh_price_u7",
                "sh_price_u10",
                "sh_price_u15",
                "sh_price_u20",
                "sh_price_u30",
                "sh_price_u40",
                "sh_price_u50",
                "sh_price_o1",
                "sh_price_o2",
                "sh_price_o3",
                "sh_price_o4",
                "sh_price_o5",
                "sh_price_o6",
                "sh_price_o7",
                "sh_price_o10",
                "sh_price_o15",
                "sh_price_o20",
                "sh_price_o30",
                "sh_price_o40",
                "sh_price_o50",
                "sh_price_o60",
                "sh_price_o70",
                "sh_price_o80",
                "sh_price_o90",
                "sh_price_o100",
                "sh_price_1to5",
                "sh_price_1to10",
                "sh_price_1to20",
                "sh_price_5to10",
                "sh_price_5to20",
                "sh_price_5to50",
                "sh_price_10to20",
                "sh_price_20to50",
                "sh_price_500to100",
                "custom doesn't work"
            };

        }

        public string GetSaveName()
        {
            return saveName;
        }

        public void SetSaveName(string value)
        {
            saveName = value;
        }

        public int GetExchange()
        {
            return exchange;
        }

        public void SetExchange(int value)
        {
            exchange = value;
        }

        public int GetMarketCap()
        {
            return marketCap;
        }

        public void SetMarketCap(int value)
        {
            marketCap = value;
        }

        public int GetEarningsDate()
        {
            return earningsDate;
        }

        public void SetEarningsDate(int value)
        {
            earningsDate = value;
        }

        public int GetTargetPrice()
        {
            return targetPrice;
        }

        public void SetTargetPrice(int value)
        {
            targetPrice = value;
        }

        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int value)
        {
            index = value;
        }

        public int GetDividendYield()
        {
            return dividendYield;
        }

        public void SetDividendYield(int value)
        {
            dividendYield = value;
        }

        public int GetAverageVolume()
        {
            return averageVolume;
        }

        public void SetAverageVolume(int value)
        {
            averageVolume = value;
        }

        public int GetIPODate()
        {
            return ipoDate;
        }

        public void SetIPODate(int value)
        {
            ipoDate = value;
        }

        public int GetSecter()
        {
            return sector;
        }

        public void SetSecter(int value)
        {
            sector = value;
        }

        public int GetFloatShare()
        {
            return floatShort;
        }

        public void SetFloatShare(int value)
        {
            floatShort = value;
        }

        public int GetRelativeVolume()
        {
            return relativeVolume;
        }

        public void SetRelativeVolume(int value)
        {
            relativeVolume = value;
        }

        public int GetSharesOutstanding()
        {
            return sharesOutstanding;
        }

        public void SetSharesOutstanding(int value)
        {
            sharesOutstanding = value;
        }

        public int GetIndustry()
        {
            return industry;
        }

        public void SetIndustry(int value)
        {
            industry = value;
        }

        public int GetAnalystRecommends()
        {
            return analystRecommends;
        }

        public void SetAnalystRecommends(int value)
        {
            analystRecommends = value;
        }

        public int GetCurrentValue()
        {
            return currentVolume;
        }

        public void SetCurrentValue(int value)
        {
            currentVolume = value;
        }

        public int GetFloat()
        {
            return _float;
        }

        public void SetFloat(int value)
        {
            _float = value;
        }

        public int GetCountry()
        {
            return country;
        }

        public void SetCountry(int value)
        {
            country = value;
        }

        public int GetOptionShort()
        {
            return optionShort;
        }

        public void SetOptionShort(int value)
        {
            optionShort = value;
        }

        public int GetPrice()
        {
            return price;
        }

        public void SetPrice(int value)
        {
            price = value;
        }
    }
}
