using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScannerCommonCode
{
    public class FinvizFilter
    {
        private string RootUrl;
        private string FullUrl;
        private FinvizFilterSave filterSave;
        public List<Dictionary<string, string>> translations;

        // Individual Fields
        public Dictionary<string, string> TranslationExchange { get; set; }
        public Dictionary<string, string> TranslationMarketCap { get; set; }
        public Dictionary<string, string> TranslationEarningsDate { get; set; }
        public Dictionary<string, string> TranslationTargetPrice { get; set; }

        public Dictionary<string, string> TranslationIndex { get; set; }
        public Dictionary<string, string> TranslationDividendYield { get; set; }
        public Dictionary<string, string> TranslationAverageVolume { get; set; }
        public Dictionary<string, string> TranslationIPODate { get; set; }

        public Dictionary<string, string> TranslationSector { get; set; }
        public Dictionary<string, string> TranslationFloatShort { get; set; }
        public Dictionary<string, string> TranslationRelativeVolume { get; set; }
        public Dictionary<string, string> TranslationSharesOutstanding { get; set; }

        public Dictionary<string, string> TranslationIndustry { get; set; }
        public Dictionary<string, string> TranslationAnalystRecom { get; set; }
        public Dictionary<string, string> TranslationCurrentVolume { get; set; }
        public Dictionary<string, string> TranslationFloat { get; set; }

        public Dictionary<string, string> TranslationCountry { get; set; }
        public Dictionary<string, string> TranslationOptionShort { get; set; }
        public Dictionary<string, string> TranslationPrice { get; set; }


        // Individual Fields
        public string Exchange { get; set; }
        public string MarketCap { get; set; }
        public string EarningsDate { get; set; }
        public string TargetPrice { get; set; }

        public string Index { get; set; }
        public string DividendYield { get; set; }
        public string AverageVolume { get; set; }
        public string IPODate { get; set; }

        public string Sector { get; set; }
        public string FloatShort { get; set; }
        public string RelativeVolume { get; set; }
        public string SharesOutstanding { get; set; }

        public string Industry { get; set; }
        public string AnalystRecom { get; set; }
        public string CurrentVolume { get; set; }
        public string Float { get; set; }

        public string Country { get; set; }
        public string OptionShort { get; set; }
        public string Price { get; set; }

        public FinvizFilter()
        {
            translations = new List<Dictionary<string, string>>();
            SetupTranslations();
        }

        public string GetFullUrl()
        {
            return this.FullUrl;
        }

        public string BuildUrl()
        {
            // Start URL
            RootUrl = "https://finviz.com/screener.ashx?v=111";
            StringBuilder argumentsSB = new StringBuilder();

            string root = this.RootUrl;
            string args = "&f=";
            string urlArgument = "";

            // Column 1
            if (this.TranslationExchange.TryGetValue(this.Exchange, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationMarketCap.TryGetValue(this.MarketCap, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationEarningsDate.TryGetValue(this.EarningsDate, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationTargetPrice.TryGetValue(this.TargetPrice, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }

            // Column 2
            if (this.TranslationIndex.TryGetValue(this.Index, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationDividendYield.TryGetValue(this.DividendYield, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationAverageVolume.TryGetValue(this.AverageVolume, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationIPODate.TryGetValue(this.IPODate, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }

            // Column 3
            if (this.TranslationSector.TryGetValue(this.Sector, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationFloatShort.TryGetValue(this.FloatShort, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationRelativeVolume.TryGetValue(this.RelativeVolume, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationSharesOutstanding.TryGetValue(this.SharesOutstanding, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }

            // Column 4
            if (this.TranslationIndustry.TryGetValue(this.Industry, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationAnalystRecom.TryGetValue(this.AnalystRecom, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationCurrentVolume.TryGetValue(this.CurrentVolume, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationFloat.TryGetValue(this.Float, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }

            // Column 5
            if (this.TranslationCountry.TryGetValue(this.Country, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationOptionShort.TryGetValue(this.OptionShort, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }
            if (this.TranslationPrice.TryGetValue(this.Price, out urlArgument))
            {
                if (argumentsSB.Length > 0) argumentsSB.Append(",");
                argumentsSB.Append(urlArgument);
            }




            //foreach (var translation in translations)
            //{
            //    for (int i = 0; i < filters.Length; i++)
            //    {
            //        if (translation.Keys.Contains(filters[i]))
            //        {
            //            if (!firstRun) argumentsSB.Append(",");
            //            argumentsSB.Append(translation[filters[i]]);
            //            firstRun = false;
            //        }
            //    }
            //}

            string fullUrl = "";
            if (argumentsSB.Length > 0)
            {
                fullUrl = root + args + argumentsSB.ToString();
            }
            else
            {
                fullUrl = root;
            }

            this.FullUrl = fullUrl;

            return fullUrl;
        }

        private void SetupTranslations()
        {
            // Copy URL paramerters from the website:
            // https://finviz.com/screener.ashx?v=111&f=fa_div_o10

            // Column 1
            this.TranslationExchange = new Dictionary<string, string>
            {
                { "AMEX", "exch_amex" },
                { "NASDAQ", "exch_nasd" },
                { "NYSE", "exch_nyse" }
            };

            this.TranslationMarketCap = new Dictionary<string, string>
            {
                { "Mega ($200bln and more)", "cap_mega" },
                { "Large ($10bln to $200bln)", "cap_large" },
                { "Mid ($2bln to $10bln)", "cap_mid" },
                { "Small ($300mln to $2bln)", "cap_small" },
                { "Micro ($50mln to $300mln)", "cap_micro" },
                { "Nano (under $50mln)", "cap_nano" },
                { "+Large (over $10bln)", "cap_largeover" },
                { "+Mid (over $2bln)", "cap_midover" },
                { "+Small (over $300mln)", "cap_smallover" },
                { "+Micro (over $50mln)", "cap_microover" },
                { "-Large (under $200bln)", "cap_largeunder" },
                { "-Mid (under $10bln)", "cap_midunder" },
                { "-Small (under $2bln)", "cap_smallunder" },
                { "-Micro (under $300mln)", "cap_microunder" }
            };

            this.TranslationEarningsDate = new Dictionary<string, string>
            {
                { "Today", "earningsdate_today" },
                { "Today Before Market Open", "earningsdate_todaybefore" },
                { "Today After Market Close", "earningsdate_todayafter" },
                { "Tomorrow", "earningsdate_tomorrow" },
                { "Tomorrow Before Market Open", "earningsdate_tomorrowbefore" },
                { "Tomorrow After Market Close", "earningsdate_tomorrowafter" },
                { "Yesterday", "earningsdate_yesterday" },
                { "Yesterday Before Market Open", "earningsdate_yesterdaybefore" },
                { "Yesterday After Market Close", "earningsdate_yesterdayafter" },
                { "Next 5 Days", "earningsdate_nextdays5" },
                { "Previous 5 Days", "earningsdate_prevdays5" },
                { "This Week", "earningsdate_thisweek" },
                { "Next Week", "earningsdate_nextweek" },
                { "Previous Week", "earningsdate_prevweek" },
                { "This Month", "earningsdate_thismonth" }
            };

            this.TranslationTargetPrice = new Dictionary<string, string>
            {
                { "50% Above Price", "targetprice_a50" },
                { "40% Above Price", "targetprice_a40" },
                { "30% Above Price", "targetprice_a30" },
                { "20% Above Price", "targetprice_a20" },
                { "10% Above Price", "targetprice_a10" },
                { "5% Above Price", "targetprice_a5" },
                { "Above Price", "targetprice_above" },
                { "Below Price", "targetprice_below" },
                { "5% Below Price" , "targetprice_b5" },
                { "10% Below Price", "targetprice_b10" },
                { "20% Below Price", "targetprice_b20" },
                { "30% Below Price", "targetprice_b30" },
                { "40% Below Price", "targetprice_b40" },
                { "50% Below Price", "targetprice_b50" }
            };


            // Column 2
            this.TranslationIndex = new Dictionary<string, string>
            {
                { "S&P 500", "idx_sp500" },
                { "DJIA", "idx_dji" }
            };

            this.TranslationDividendYield = new Dictionary<string, string>
            {
                { "None (0%)", "fa_div_none" },
                { "Positive (>0%)", "fa_div_pos" },
                { "High (>5%)", "fa_div_high" },
                { "Very High (>10%)", "fa_div_veryhigh" },
                { "Over 1%", "fa_div_o1" },
                { "Over 2%", "fa_div_o2" },
                { "Over 3%", "fa_div_o3" },
                { "Over 4%", "fa_div_o4" },
                { "Over 5%", "fa_div_o5" },
                { "Over 6%", "fa_div_o6" },
                { "Over 7%", "fa_div_o7" },
                { "Over 8%", "fa_div_o8" },
                { "Over 9%", "fa_div_o9" },
                { "Over 10%", "fa_div_o10" }
            };

            this.TranslationAverageVolume = new Dictionary<string, string>
            {
                { "Under 50K", "sh_avgvol_u50" },
                { "Under 100K", "sh_avgvol_u100" },
                { "Under 500K", "sh_avgvol_u500" },
                { "Under 750K", "sh_avgvol_u750" },
                { "Under 1M", "sh_avgvol_u1000" },
                { "Over 50K", "sh_avgvol_o50" },
                { "Over 100K", "sh_avgvol_o100" },
                { "Over 200K", "sh_avgvol_o200" },
                { "Over 300K", "sh_avgvol_o300" },
                { "Over 400K", "sh_avgvol_o400" },
                { "Over 500K", "sh_avgvol_o500" },
                { "Over 750K", "sh_avgvol_o750" },
                { "Over 1M", "sh_avgvol_o1000" },
                { "Over 2M", "sh_avgvol_o2000" },
                { "100K to 500K", "sh_avgvol_100to500" },
                { "100K to 1M", "sh_avgvol_100to1000" },
                { "500K to 1M", "sh_avgvol_500to1000" },
                { "500K to 10M", "sh_avgvol_500to10000" },
                { "Custom (Elite only)", "custom doesn't work" }
            };

            this.TranslationIPODate = new Dictionary<string, string>
            {
                {"Today", "ipodate_today"},
                {"Yesterday", "ipodate_yesterday"},
                {"In the last week", "ipodate_prevweek"},
                {"In the last month", "ipodate_prevmonth"},
                {"In the last quarter", "ipodate_prevquarter"},
                {"In the last year", "ipodate_prevyear"},
                {"In the last 2 years", "ipodate_prev2yrs"},
                {"In the last 3 years", "ipodate_prev3yrs"},
                {"In the last 5 years", "ipodate_prev5yrs"},
                {"More than a year ago", "ipodate_more1"},
                {"More than 5 years ago", "ipodate_more5"},
                {"More than 10 years ago", "ipodate_more10"},
                {"More than 15 years ago", "ipodate_more15"},
                {"More than 20 years ago", "ipodate_more20"},
                {"More than 25 years ago", "ipodate_more25"}
            };


            // Column 3
            this.TranslationSector = new Dictionary<string, string>
            {
                {"Basic Materials", "sec_basicmaterials"},
                {"Communication Services", "sec_communicationservices"},
                {"Consumer Cyclical", "sec_consumercyclical"},
                {"Consumer Defensive", "sec_consumerdefensive"},
                {"Energy", "sec_energy"},
                {"Financial", "sec_financial"},
                {"Healthcare", "sec_healthcare"},
                {"Industrials", "sec_industrials"},
                {"Real Estate", "sec_realestate"},
                {"Technology", "sec_technology"},
                {"Utilities", "sec_utilities"}
            };

            this.TranslationFloatShort = new Dictionary<string, string>
            {
                {"Low (<5%)", "sh_short_low"},
                {"High (>20%)", "sh_short_high"},
                {"Under 5%", "sh_short_u5"},
                {"Under 10%", "sh_short_u10"},
                {"Under 15%", "sh_short_u15"},
                {"Under 20%", "sh_short_u20"},
                {"Under 25%", "sh_short_u25"},
                {"Under 30%", "sh_short_u30"},
                {"Over 5%", "sh_short_o5"},
                {"Over 10%", "sh_short_o10"},
                {"Over 15%", "sh_short_o15"},
                {"Over 20%", "sh_short_o20"},
                {"Over 25%", "sh_short_o25"},
                {"Over 30%", "sh_short_o30"}
            };

            this.TranslationRelativeVolume = new Dictionary<string, string>
            {
                {"Over 10", "sh_relvol_o10"},
                {"Over 5", "sh_relvol_o5"},
                {"Over 3", "sh_relvol_o3"},
                {"Over 2", "sh_relvol_o2"},
                {"Over 1.5", "sh_relvol_o1.5"},
                {"Over 1", "sh_relvol_o1"},
                {"Over 0.75", "sh_relvol_o0.75"},
                {"Over 0.5", "sh_relvol_o0.5"},
                {"Over 0.25", "sh_relvol_o0.25"},
                {"Under 2", "sh_relvol_u2"},
                {"Under 1.5", "sh_relvol_u1.5"},
                {"Under 1", "sh_relvol_u1"},
                {"Under 0.75", "sh_relvol_u0.75"},
                {"Under 0.5", "sh_relvol_u0.5"},
                {"Under 0.25", "sh_relvol_u0.25"},
                {"Under 0.1", "sh_relvol_u0.1"}
            };

            this.TranslationSharesOutstanding = new Dictionary<string, string>
            {
                {"Under 1M", "sh_outstanding_u1"},
                {"Under 5M", "sh_outstanding_u5"},
                {"Under 10M", "sh_outstanding_u10"},
                {"Under 20M", "sh_outstanding_u20"},
                {"Under 50M", "sh_outstanding_u50"},
                {"Under 100M", "sh_outstanding_u100"},
                {"Over 1M", "sh_outstanding_o1"},
                {"Over 2M", "sh_outstanding_o2"},
                {"Over 5M", "sh_outstanding_o5"},
                {"Over 10M", "sh_outstanding_o10"},
                {"Over 20M", "sh_outstanding_o20"},
                {"Over 50M", "sh_outstanding_o50"},
                {"Over 100M", "sh_outstanding_o100"},
                {"Over 200M", "sh_outstanding_o200"},
                {"Over 500M", "sh_outstanding_o500"},
                {"Over 1000M", "sh_outstanding_o1000"}
            };


            // Column 4
            this.TranslationIndustry = new Dictionary<string, string>
            {
                { "Stocks only (ex-Funds)",                          "ind_stocksonly"},
                { "Advertising Agencies",                            "ind_advertisingagencies"},
                { "Aerospace & Defense",                             "ind_aerospacedefense"},
                { "Agricultural Inputs",                             "ind_agriculturalinputs"},
                { "Airlines",                                        "ind_airlines"},
                { "Airports & Air Services",                         "ind_airportsairservices"},
                { "Aluminum",                                        "ind_aluminum"},
                { "Apparel Manufacturing",                           "ind_apparelmanufacturing"},
                { "Apparel Retail",                                  "ind_apparelretail"},
                { "Asset Management",                                "ind_assetmanagement"},
                { "Auto Manufacturers",                              "ind_automanufacturers"},
                { "Auto Parts",                                      "ind_autoparts"},
                { "Auto & Truck Dealerships",                        "ind_autotruckdealerships"},
                { "Banks - Diversified",                             "ind_banksdiversified"},
                { "Banks - Regional",                                "ind_banksregional"},
                { "Beverages - Brewers",                             "ind_beveragesbrewers"},
                { "Beverages - Non-Alcoholic",                       "ind_beveragesnonalcoholic"},
                { "Beverages - Wineries & Distilleries",             "ind_beverageswineriesdistilleries"},
                { "Biotechnology",                                   "ind_biotechnology"},
                { "Broadcasting",                                    "ind_broadcasting"},
                { "Building Materials",                              "ind_buildingmaterials"},
                { "Building Products & Equipment",                   "ind_buildingproductsequipment"},
                { "Business Equipment & Supplies",                   "ind_businessequipmentsupplies"},
                { "Capital Markets",                                 "ind_capitalmarkets"},
                { "Chemicals",                                       "ind_chemicals"},
                { "Closed-End Fund - Debt",                          "ind_closedendfunddebt"},
                { "Closed-End Fund - Equity",                        "ind_closedendfundequity"},
                { "Closed-End Fund - Foreign",                       "ind_closedendfundforeign"},
                { "Coking Coal",                                     "ind_cokingcoal"},
                { "Communication Equipment",                         "ind_communicationequipment"},
                { "Computer Hardware",                               "ind_computerhardware"},
                { "Confectioners",                                   "ind_confectioners"},
                { "Conglomerates",                                   "ind_conglomerates"},
                { "Consulting Services",                             "ind_consultingservices"},
                { "Consumer Electronics",                            "ind_consumerelectronics"},
                { "Copper",                                          "ind_copper"},
                { "Credit Services",                                 "ind_creditservices"},
                { "Department Stores",                               "ind_departmentstores"},
                { "Diagnostics & Research",                          "ind_diagnosticsresearch"},
                { "Discount Stores",                                 "ind_discountstores"},
                { "Drug Manufacturers - General",                    "ind_drugmanufacturersgeneral"},
                { "Drug Manufacturers - Specialty & Generic",        "ind_drugmanufacturersspecialtygeneric"},
                { "Education & Training Services",                   "ind_educationtrainingservices"},
                { "Electrical Equipment & Parts",                    "ind_electricalequipmentparts"},
                { "Electronic Components",                           "ind_electroniccomponents"},
                { "Electronic Gaming & Multimedia",                  "ind_electronicgamingmultimedia"},
                { "Electronics & Computer Distribution",             "ind_electronicscomputerdistribution"},
                { "Engineering & Construction",                      "ind_engineeringconstruction"},
                { "Entertainment",                                   "ind_entertainment"},
                { "Exchange Traded Fund",                            "ind_exchangetradedfund"},
                { "Farm & Heavy Construction Machinery",             "ind_farmheavyconstructionmachinery"},
                { "Farm Products",                                   "ind_farmproducts"},
                { "Financial Conglomerates",                         "ind_financialconglomerates"},
                { "Financial Data & Stock Exchanges",                "ind_financialdatastockexchanges"},
                { "Food Distribution",                               "ind_fooddistribution"},
                { "Footwear & Accessories",                          "ind_footwearaccessories"},
                { "Furnishings, Fixtures & Appliances",              "ind_furnishingsfixturesappliances"},
                { "Gambling",                                        "ind_gambling"},
                { "Gold",                                            "ind_gold"},
                { "Grocery Stores",                                  "ind_grocerystores"},
                { "Healthcare Plans",                                "ind_healthcareplans"},
                { "Health Information Services",                     "ind_healthinformationservices"},
                { "Home Improvement Retail",                         "ind_homeimprovementretail"},
                { "Household & Personal Products",                   "ind_householdpersonalproducts"},
                { "Industrial Distribution",                         "ind_industrialdistribution"},
                { "Information Technology Services",                 "ind_informationtechnologyservices"},
                { "Infrastructure Operations",                       "ind_infrastructureoperations"},
                { "Insurance Brokers",                               "ind_insurancebrokers"},
                { "Insurance - Diversified",                         "ind_insurancediversified"},
                { "Insurance - Life",                                "ind_insurancelife"},
                { "Insurance - Property & Casualty",                 "ind_insurancepropertycasualty"},
                { "Insurance - Reinsurance",                         "ind_insurancereinsurance"},
                { "Insurance - Specialty",                           "ind_insurancespecialty"},
                { "Integrated Freight & Logistics",                  "ind_integratedfreightlogistics"},
                { "Internet Content & Information",                  "ind_internetcontentinformation"},
                { "Internet Retail",                                 "ind_internetretail"},
                { "Leisure",                                         "ind_leisure"},
                { "Lodging",                                         "ind_lodging"},
                { "Lumber & Wood Production",                        "ind_lumberwoodproduction"},
                { "Luxury Goods",                                    "ind_luxurygoods"},
                { "Marine Shipping",                                 "ind_marineshipping"},
                { "Medical Care Facilities",                         "ind_medicalcarefacilities"},
                { "Medical Devices",                                 "ind_medicaldevices"},
                { "Medical Distribution",                            "ind_medicaldistribution"},
                { "Medical Instruments & Supplies",                  "ind_medicalinstrumentssupplies"},
                { "Metal Fabrication",                               "ind_metalfabrication"},
                { "Mortgage Finance",                                "ind_mortgagefinance"},
                { "Oil & Gas Drilling",                              "ind_oilgasdrilling"},
                { "Oil & Gas E&P",                                   "ind_oilgasep"},
                { "Oil & Gas Equipment & Services",                  "ind_oilgasequipmentservices"},
                { "Oil & Gas Integrated",                            "ind_oilgasintegrated"},
                { "Oil & Gas Midstream",                             "ind_oilgasmidstream"},
                { "Oil & Gas Refining & Marketing",                  "ind_oilgasrefiningmarketing"},
                { "Other Industrial Metals & Mining",                "ind_otherindustrialmetalsmining"},
                { "Other Precious Metals & Mining",                  "ind_otherpreciousmetalsmining"},
                { "Packaged Foods",                                  "ind_packagedfoods"},
                { "Packaging & Containers",                          "ind_packagingcontainers"},
                { "Paper & Paper Products",                          "ind_paperpaperproducts"},
                { "Personal Services",                               "ind_personalservices"},
                { "Pharmaceutical Retailers",                        "ind_pharmaceuticalretailers"},
                { "Pollution & Treatment Controls",                  "ind_pollutiontreatmentcontrols"},
                { "Publishing",                                      "ind_publishing"},
                { "Railroads",                                       "ind_railroads"},
                { "Real Estate - Development",                       "ind_realestatedevelopment"},
                { "Real Estate - Diversified",                       "ind_realestatediversified"},
                { "Real Estate Services",                            "ind_realestateservices"},
                { "Recreational Vehicles",                           "ind_recreationalvehicles"},
                { "REIT - Diversified",                              "ind_reitdiversified"},
                { "REIT - Healthcare Facilities",                    "ind_reithealthcarefacilities"},
                { "REIT - Hotel & Motel",                            "ind_reithotelmotel"},
                { "REIT - Industrial",                               "ind_reitindustrial"},
                { "REIT - Mortgage",                                 "ind_reitmortgage"},
                { "REIT - Office",                                   "ind_reitoffice"},
                { "REIT - Residential",                              "ind_reitresidential"},
                { "REIT - Retail",                                   "ind_reitretail"},
                { "REIT - Specialty",                                "ind_reitspecialty"},
                { "Rental & Leasing Services",                       "ind_rentalleasingservices"},
                { "Residential Construction",                        "ind_residentialconstruction"},
                { "Resorts & Casinos",                               "ind_resortscasinos"},
                { "Restaurants",                                     "ind_restaurants"},
                { "Scientific & Technical Instruments",              "ind_scientifictechnicalinstruments"},
                { "Security & Protection Services",                  "ind_securityprotectionservices"},
                { "Semiconductor Equipment & Materials",             "ind_semiconductorequipmentmaterials"},
                { "Semiconductors",                                  "ind_semiconductors"},
                { "Shell Companies",                                 "ind_shellcompanies"},
                { "Silver",                                          "ind_silver"},
                { "Software - Application",                          "ind_softwareapplication"},
                { "Software - Infrastructure",                       "ind_softwareinfrastructure"},
                { "Solar",                                           "ind_solar"},
                { "Specialty Business Services",                     "ind_specialtybusinessservices"},
                { "Specialty Chemicals",                             "ind_specialtychemicals"},
                { "Specialty Industrial Machinery",                  "ind_specialtyindustrialmachinery"},
                { "Specialty Retail",                                "ind_specialtyretail"},
                { "Staffing & Employment Services",                  "ind_staffingemploymentservices"},
                { "Steel",                                           "ind_steel"},
                { "Telecom Services",                                "ind_telecomservices"},
                { "Textile Manufacturing",                           "ind_textilemanufacturing"},
                { "Thermal Coal",                                    "ind_thermalcoal"},
                { "Tobacco",                                         "ind_tobacco"},
                { "Tools & Accessories",                             "ind_toolsaccessories"},
                { "Travel Services",                                 "ind_travelservices"},
                { "Trucking",                                        "ind_trucking"},
                { "Uranium",                                         "ind_uranium"},
                { "Utilities - Diversified",                         "ind_utilitiesdiversified"},
                { "Utilities - Independent Power Producers",         "ind_utilitiesindependentpowerproducers"},
                { "Utilities - Regulated Electric",                  "ind_utilitiesregulatedelectric"},
                { "Utilities - Regulated Gas",                       "ind_utilitiesregulatedgas"},
                { "Utilities - Regulated Water",                     "ind_utilitiesregulatedwater"},
                { "Utilities - Renewable",                           "ind_utilitiesrenewable"},
                { "Waste Management",                                "ind_wastemanagement"}
            };

            this.TranslationAnalystRecom = new Dictionary<string, string>
            {
                {"Strong Buy (1)", "an_recom_strongbuy"},
                {"Buy or better", "an_recom_buybetter"},
                {"Buy", "an_recom_buy"},
                {"Hold or better", "an_recom_holdbetter"},
                {"Hold", "an_recom_hold"},
                {"Hold or worse", "an_recom_holdworse"},
                {"Sell", "an_recom_sell"},
                {"Sell or worse", "an_recom_sellworse"},
                {"Strong Sell (5)", "an_recom_strongsell"}
            };

            this.TranslationCurrentVolume = new Dictionary<string, string>
            {
                {"Under 50K", "sh_curvol_u50"},
                {"Under 100K", "sh_curvol_u100"},
                {"Under 500K", "sh_curvol_u500"},
                {"Under 750K", "sh_curvol_u750"},
                {"Under 1M", "sh_curvol_u1000"},
                {"Over 0", "sh_curvol_o0"},
                {"Over 50K", "sh_curvol_o50"},
                {"Over 100K", "sh_curvol_o100"},
                {"Over 200K", "sh_curvol_o200"},
                {"Over 300K", "sh_curvol_o300"},
                {"Over 400K", "sh_curvol_o400"},
                {"Over 500K", "sh_curvol_o500"},
                {"Over 750K", "sh_curvol_o750"},
                {"Over 1M", "sh_curvol_o1000"},
                {"Over 2M", "sh_curvol_o2000"},
                {"Over 5M", "sh_curvol_o5000"},
                {"Over 10M", "sh_curvol_o10000"},
                {"Over 20M", "sh_curvol_o20000"}
            };

            this.TranslationFloat = new Dictionary<string, string>
            {
                {"Under 1M", "sh_float_u1"},
                {"Under 5M", "sh_float_u5"},
                {"Under 10M", "sh_float_u10"},
                {"Under 20M", "sh_float_u20"},
                {"Under 50M", "sh_float_u50"},
                {"Under 100M", "sh_float_u100"},
                {"Over 1M", "sh_float_o1"},
                {"Over 2M", "sh_float_o2"},
                {"Over 5M", "sh_float_o5"},
                {"Over 10M", "sh_float_o10"},
                {"Over 20M", "sh_float_o20"},
                {"Over 50M", "sh_float_o50"},
                {"Over 100M", "sh_float_o100"},
                {"Over 200M", "sh_float_o200"},
                {"Over 500M", "sh_float_o500"},
                {"Over 1000M", "sh_float_o1000"}
            };

            // Column 5
            this.TranslationCountry = new Dictionary<string, string>
            {
                {"USA", "geo_usa"},
                {"Foreign (ex-USA)", "geo_notusa"},
                {"Asia", "geo_asia"},
                {"Europe", "geo_europe"},
                {"Latin America", "geo_latinamerica"},
                {"BRIC", "geo_bric"},
                {"Argentina", "geo_argentina"},
                {"Australia", "geo_australia"},
                {"Bahamas", "geo_bahamas"},
                {"Belgium", "geo_belgium"},
                {"BeNeLux", "geo_benelux"},
                {"Bermuda", "geo_bermuda"},
                {"Brazil", "geo_brazil"},
                {"Canada", "geo_canada"},
                {"Cayman Islands", "geo_caymanislands"},
                {"Chile", "geo_chile"},
                {"China", "geo_china"},
                {"China & Hong Kong", "geo_chinahongkong"},
                {"Colombia", "geo_colombia"},
                {"Cyprus", "geo_cyprus"},
                {"Denmark", "geo_denmark"},
                {"Finland", "geo_finland"},
                {"France", "geo_france"},
                {"Germany", "geo_germany"},
                {"Greece", "geo_greece"},
                {"Hong Kong", "geo_hongkong"},
                {"Hungary", "geo_hungary"},
                {"Iceland", "geo_iceland"},
                {"India", "geo_india"},
                {"Indonesia", "geo_indonesia"},
                {"Ireland", "geo_ireland"},
                {"Israel", "geo_israel"},
                {"Italy", "geo_italy"},
                {"Japan", "geo_japan"},
                {"Kazakhstan", "geo_kazakhstan"},
                {"Luxembourg", "geo_luxembourg"},
                {"Malaysia", "geo_malaysia"},
                {"Malta", "geo_malta"},
                {"Mexico", "geo_mexico"},
                {"Monaco", "geo_monaco"},
                {"Netherlands", "geo_netherlands"},
                {"New Zealand", "geo_newzealand"},
                {"Norway", "geo_norway"},
                {"Panama", "geo_panama"},
                {"Peru", "geo_peru"},
                {"Philippines", "geo_philippines"},
                {"Portugal", "geo_portugal"},
                {"Russia", "geo_russia"},
                {"Singapore", "geo_singapore"},
                {"South Africa", "geo_southafrica"},
                {"South Korea", "geo_southkorea"},
                {"Spain", "geo_spain"},
                {"Sweden", "geo_sweden"},
                {"Switzerland", "geo_switzerland"},
                {"Taiwan", "geo_taiwan"},
                {"Turkey", "geo_turkey"},
                {"United Arab Emirates", "geo_unitedarabemirates"},
                {"United Kingdom", "geo_unitedkingdom"},
                {"Uruguay", "geo_uruguay"}
            };

            this.TranslationOptionShort = new Dictionary<string, string>
            {
                {"Optionable", "sh_opt_option"},
                {"Shortable", "sh_opt_short"},
                {"Optionable and shortable", "sh_opt_optionshort"}
            };

            this.TranslationPrice = new Dictionary<string, string>
            {
                {"Under $1", "sh_price_u1"},
                {"Under $2", "sh_price_u2"},
                {"Under $3", "sh_price_u3"},
                {"Under $4", "sh_price_u4"},
                {"Under $5", "sh_price_u5"},
                {"Under $7", "sh_price_u6"},
                {"Under $10", "sh_price_u7"},
                {"Under $15", "sh_price_u10"},
                {"Under $20", "sh_price_u15"},
                {"Under $30", "sh_price_u20"},
                {"Under $40", "sh_price_u30"},
                {"Under $50", "sh_price_u40"},
                {"Over $1", "sh_price_u50"},
                {"Over $2", "sh_price_o1"},
                {"Over $3", "sh_price_o2"},
                {"Over $4", "sh_price_o3"},
                {"Over $5", "sh_price_o4"},
                {"Over $6", "sh_price_o5"},
                {"Over $7", "sh_price_o6"},
                {"Over $10", "sh_price_o7"},
                {"Over $15", "sh_price_o10"},
                {"Over $20", "sh_price_o15"},
                {"Over $30", "sh_price_o20"},
                {"Over $40", "sh_price_o30"},
                {"Over $50", "sh_price_o40"},
                {"Over $60", "sh_price_o50"},
                {"Over $70", "sh_price_o60"},
                {"Over $80", "sh_price_o70"},
                {"Over $90", "sh_price_o80"},
                {"Over $100", "sh_price_o90"},
                {"$1 to $5", "sh_price_o100"},
                {"$1 to $10", "sh_price_1to5"},
                {"$1 to $20", "sh_price_1to10"},
                {"$5 to $10", "sh_price_1to20"},
                {"$5 to $20", "sh_price_5to10"},
                {"$5 to $50", "sh_price_5to20"},
                {"$10 to $20", "sh_price_5to50"},
                {"$10 to $50", "sh_price_10to20"},
                {"$20 to $50", "sh_price_20to50"},
                {"$50 to $100", "sh_price_500to100"}
            };
        }
    }
}
