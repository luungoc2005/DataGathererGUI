﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using DataHelper.Attributes;

namespace DataGatherer.Models
{
    #region "4 Jan 2017"
    //public class DailyPrice
    //{
    //    [JsonProperty("StockCode")]
    //    public string StockCode { get; set; }

    //    [JsonProperty("CloseDate")]
    //    public DateTime CloseDate { get; set; }

    //    [Feature]
    //    public double DaysFromNow
    //    {
    //        get
    //        {
    //            return (DateTime.Now - CloseDate).TotalDays;
    //        }
    //    }

    //    #region "DayOfWeek"

    //    public int DayOfWeek
    //    {
    //        get
    //        {
    //            return Convert.ToInt32(CloseDate.DayOfWeek);
    //        }
    //    }

    //    [Feature]
    //    public double IsMonday
    //    {
    //        get
    //        {
    //            return (CloseDate.DayOfWeek == System.DayOfWeek.Monday) ? 1.0 : 0.0;
    //        }
    //    }

    //    [Feature]
    //    public double IsTuesday
    //    {
    //        get
    //        {
    //            return (CloseDate.DayOfWeek == System.DayOfWeek.Tuesday) ? 1.0 : 0.0;
    //        }
    //    }

    //    [Feature]
    //    public double IsWednesday
    //    {
    //        get
    //        {
    //            return (CloseDate.DayOfWeek == System.DayOfWeek.Wednesday) ? 1.0 : 0.0;
    //        }
    //    }
    //    [Feature]
    //    public double IsThursday
    //    {
    //        get
    //        {
    //            return (CloseDate.DayOfWeek == System.DayOfWeek.Thursday) ? 1.0 : 0.0;
    //        }
    //    }
    //    [Feature]
    //    public double IsFriday
    //    {
    //        get
    //        {
    //            return (CloseDate.DayOfWeek == System.DayOfWeek.Friday) ? 1.0 : 0.0;
    //        }
    //    }
    //    #endregion

    //    [Feature]
    //    [JsonProperty("KLCPLH")]
    //    public double KLCPLH { get; set; }

    //    [Feature]
    //    [JsonProperty("KLCPNY")]
    //    public double KLCPNY { get; set; }

    //    [Feature]
    //    [JsonProperty("PriorClosePrice")]
    //    public double PriorClosePrice { get; set; }

    //    [Feature]
    //    [JsonProperty("CeilingPrice")]
    //    public double CeilingPrice { get; set; }

    //    [Feature]
    //    [JsonProperty("FloorPrice")]
    //    public double FloorPrice { get; set; }

    //    [Feature]
    //    [JsonProperty("TradingVolume")]
    //    public double TradingVolume { get; set; }

    //    [Feature]
    //    [JsonProperty("TotalVal")]
    //    public long TotalVal { get; set; }

    //    [Feature]
    //    [JsonProperty("CapitalLevel")]
    //    public double CapitalLevel { get; set; }

    //    [JsonProperty("Highest")]
    //    public double Highest { get; set; }

    //    [JsonProperty("Lowest")]
    //    public double Lowest { get; set; }

    //    [JsonProperty("OpenPrice")]
    //    public double OpenPrice { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("ClosePrice")]
    //    public double ClosePrice { get; set; }

    //    private double _profit = double.NaN;

    //    [Label]
    //    public double Profit
    //    {
    //        get
    //        {
    //            if (_profit.Equals(double.NaN)) _profit = (ClosePrice / PriorClosePrice) - 1;
    //            return _profit;
    //        }
    //        set
    //        {
    //            _profit = value;
    //        }
    //    }

    //    public double ProfitPretified
    //    {
    //        get
    //        {
    //            return Math.Round(Profit * 100, 3);
    //        }
    //    }

    //    public double Ranking { get; set; }

    //    //[Feature] - removed as of 1/1
    //    [JsonProperty("AvrPrice")]
    //    public double AvrPrice { get; set; }

    //    //[Feature] - removed as of 12/28
    //    [JsonProperty("Oscillate")]
    //    public double Oscillate { get; set; }

    //    //[Feature] - removed as of 1/1
    //    [JsonProperty("PercentOscillate")]
    //    public double PercentOscillate { get; set; }

    //    [Feature]
    //    [JsonProperty("YearLow")]
    //    public double YearLow { get; set; }

    //    [Feature]
    //    [JsonProperty("YearHigh")]
    //    public double YearHigh { get; set; }

    //    [Feature]
    //    [JsonProperty("AvgVol")]
    //    public double AvgVol { get; set; }

    //    [Feature]
    //    [JsonProperty("BuyRedundancy")]
    //    public double BuyRedundancy { get; set; }

    //    [Feature]
    //    [JsonProperty("SellRedundancy")]
    //    public double SellRedundancy { get; set; }

    //    [Feature]
    //    [JsonProperty("ForeignRoom")]
    //    public double ForeignRoom { get; set; }

    //    [Feature]
    //    [JsonProperty("Dividend")]
    //    public double Dividend { get; set; }

    //    [Feature]
    //    [JsonProperty("Yield")]
    //    public double Yield { get; set; }

    //    [Feature]
    //    [JsonProperty("Beta")]
    //    public double Beta { get; set; }

    //    [Feature]
    //    [JsonProperty("EPS")]
    //    public double EPS { get; set; }

    //    [Feature]
    //    [JsonProperty("PE")]
    //    public double PE { get; set; }

    //    [Feature]
    //    [JsonProperty("FwPE")]
    //    public double FwPE { get; set; }

    //    [Feature]
    //    [JsonProperty("BVPS")]
    //    public double BVPS { get; set; }

    //    [Feature]
    //    [JsonProperty("PB")]
    //    public double PB { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("TotalRoom")]
    //    public double TotalRoom { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("CurrRoom")]
    //    public double CurrRoom { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("RemainRoom")]
    //    public double RemainRoom { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("ForeignBuyVolume")]
    //    public double ForeignBuyVolume { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("F_BuyVal")]
    //    public double FBuyVal { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("F_SellVol")]
    //    public double FSellVol { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("F_SellVal")]
    //    public double FSellVal { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("F_BuyPutVol")]
    //    public double FBuyPutVol { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("F_BuyPutVal")]
    //    public double FBuyPutVal { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("F_SellPutVol")]
    //    public double FSellPutVol { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("F_SellPutVal")]
    //    public double FSellPutVal { get; set; }

    //    [Feature]
    //    [JsonProperty("MarketStatus")]
    //    public double MarketStatus { get; set; }

    //    [JsonProperty("ColorId")]
    //    public double ColorId { get; set; }

    //    [JsonProperty("CloseDateView")]
    //    public string CloseDateView { get; set; }

    //    [JsonProperty("TotalTime")]
    //    public string TotalTime { get; set; }

    //    [JsonProperty("URL")]
    //    public string URL { get; set; }
    //}
    #endregion

    #region "5 Jan 2017"
    //public class DailyPrice
    //{
    //    [JsonProperty("StockCode")]
    //    public string StockCode { get; set; }

    //    [JsonProperty("CloseDate")]
    //    public DateTime CloseDate { get; set; }

    //    [Feature]
    //    public double DaysFromNow
    //    {
    //        get
    //        {
    //            return (DateTime.Now - CloseDate).TotalDays;
    //        }
    //    }

    //    #region "DayOfWeek"

    //    public int DayOfWeek
    //    {
    //        get
    //        {
    //            return Convert.ToInt32(CloseDate.DayOfWeek);
    //        }
    //    }

    //    [Feature]
    //    public double IsMonday
    //    {
    //        get
    //        {
    //            return (CloseDate.DayOfWeek == System.DayOfWeek.Monday) ? 1.0 : 0.0;
    //        }
    //    }

    //    [Feature]
    //    public double IsTuesday
    //    {
    //        get
    //        {
    //            return (CloseDate.DayOfWeek == System.DayOfWeek.Tuesday) ? 1.0 : 0.0;
    //        }
    //    }

    //    [Feature]
    //    public double IsWednesday
    //    {
    //        get
    //        {
    //            return (CloseDate.DayOfWeek == System.DayOfWeek.Wednesday) ? 1.0 : 0.0;
    //        }
    //    }
    //    [Feature]
    //    public double IsThursday
    //    {
    //        get
    //        {
    //            return (CloseDate.DayOfWeek == System.DayOfWeek.Thursday) ? 1.0 : 0.0;
    //        }
    //    }
    //    [Feature]
    //    public double IsFriday
    //    {
    //        get
    //        {
    //            return (CloseDate.DayOfWeek == System.DayOfWeek.Friday) ? 1.0 : 0.0;
    //        }
    //    }
    //    #endregion

    //    [Feature]
    //    [JsonProperty("KLCPLH")]
    //    public double KLCPLH { get; set; }

    //    //[Feature]
    //    [JsonProperty("KLCPNY")]
    //    public double KLCPNY { get; set; }

    //    [Feature]
    //    [JsonProperty("PriorClosePrice")]
    //    public double PriorClosePrice { get; set; }

    //    //[Feature]
    //    [JsonProperty("CeilingPrice")]
    //    public double CeilingPrice { get; set; }

    //    //[Feature]
    //    [JsonProperty("FloorPrice")]
    //    public double FloorPrice { get; set; }

    //    [Feature]
    //    [JsonProperty("TradingVolume")]
    //    public double TradingVolume { get; set; }

    //    //[Feature]
    //    [JsonProperty("TotalVal")]
    //    public long TotalVal { get; set; }

    //    [Feature]
    //    [JsonProperty("CapitalLevel")]
    //    public double CapitalLevel { get; set; }

    //    [JsonProperty("Highest")]
    //    public double Highest { get; set; }

    //    [JsonProperty("Lowest")]
    //    public double Lowest { get; set; }

    //    [JsonProperty("OpenPrice")]
    //    public double OpenPrice { get; set; }

    //    //[Feature] // - added as of 12/30
    //    [JsonProperty("ClosePrice")]
    //    public double ClosePrice { get; set; }

    //    private double _profit = double.NaN;

    //    [Label]
    //    public double Profit
    //    {
    //        get
    //        {
    //            if (_profit.Equals(double.NaN)) _profit = (ClosePrice / PriorClosePrice) - 1;
    //            return _profit;
    //        }
    //        set
    //        {
    //            _profit = value;
    //        }
    //    }

    //    public double ProfitPretified
    //    {
    //        get
    //        {
    //            return Math.Round(Profit * 100, 3);
    //        }
    //    }

    //    public double Ranking { get; set; }

    //    //[Feature] - removed as of 1/1
    //    [JsonProperty("AvrPrice")]
    //    public double AvrPrice { get; set; }

    //    //[Feature] - removed as of 12/28
    //    [JsonProperty("Oscillate")]
    //    public double Oscillate { get; set; }

    //    //[Feature] - removed as of 1/1
    //    [JsonProperty("PercentOscillate")]
    //    public double PercentOscillate { get; set; }

    //    [Feature]
    //    [JsonProperty("YearLow")]
    //    public double YearLow { get; set; }

    //    [Feature]
    //    [JsonProperty("YearHigh")]
    //    public double YearHigh { get; set; }

    //    [Feature]
    //    [JsonProperty("AvgVol")]
    //    public double AvgVol { get; set; }

    //    [Feature]
    //    [JsonProperty("BuyRedundancy")]
    //    public double BuyRedundancy { get; set; }

    //    [Feature]
    //    [JsonProperty("SellRedundancy")]
    //    public double SellRedundancy { get; set; }

    //    [Feature]
    //    [JsonProperty("ForeignRoom")]
    //    public double ForeignRoom { get; set; }

    //    [Feature]
    //    [JsonProperty("Dividend")]
    //    public double Dividend { get; set; }

    //    [Feature]
    //    [JsonProperty("Yield")]
    //    public double Yield { get; set; }

    //    [Feature]
    //    [JsonProperty("Beta")]
    //    public double Beta { get; set; }

    //    [Feature]
    //    [JsonProperty("EPS")]
    //    public double EPS { get; set; }

    //    [Feature]
    //    [JsonProperty("PE")]
    //    public double PE { get; set; }

    //    [Feature]
    //    [JsonProperty("FwPE")]
    //    public double FwPE { get; set; }

    //    [Feature]
    //    [JsonProperty("BVPS")]
    //    public double BVPS { get; set; }

    //    [Feature]
    //    [JsonProperty("PB")]
    //    public double PB { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("TotalRoom")]
    //    public double TotalRoom { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("CurrRoom")]
    //    public double CurrRoom { get; set; }

    //    //[Feature] // - added as of 12/30
    //    [JsonProperty("RemainRoom")]
    //    public double RemainRoom { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("ForeignBuyVolume")]
    //    public double ForeignBuyVolume { get; set; }

    //    //[Feature] // - added as of 12/30
    //    [JsonProperty("F_BuyVal")]
    //    public double FBuyVal { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("F_SellVol")]
    //    public double FSellVol { get; set; }

    //    //[Feature] // - added as of 12/30
    //    [JsonProperty("F_SellVal")]
    //    public double FSellVal { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("F_BuyPutVol")]
    //    public double FBuyPutVol { get; set; }

    //    //[Feature] // - added as of 12/30
    //    [JsonProperty("F_BuyPutVal")]
    //    public double FBuyPutVal { get; set; }

    //    [Feature] // - added as of 12/30
    //    [JsonProperty("F_SellPutVol")]
    //    public double FSellPutVol { get; set; }

    //    //[Feature] // - added as of 12/30
    //    [JsonProperty("F_SellPutVal")]
    //    public double FSellPutVal { get; set; }

    //    //[Feature]
    //    [JsonProperty("MarketStatus")]
    //    public double MarketStatus { get; set; }

    //    [JsonProperty("ColorId")]
    //    public double ColorId { get; set; }

    //    [JsonProperty("CloseDateView")]
    //    public string CloseDateView { get; set; }

    //    [JsonProperty("TotalTime")]
    //    public string TotalTime { get; set; }

    //    [JsonProperty("URL")]
    //    public string URL { get; set; }
    //}
    #endregion

    #region "Current"
    public class DailyPrice
    {
        [JsonProperty("StockCode")]
        public string StockCode { get; set; }

        [JsonProperty("CloseDate")]
        public DateTime CloseDate { get; set; }

        [Feature]
        public double DaysFromNow
        {
            get
            {
                return (DateTime.Now - CloseDate).TotalDays;
            }
        }

        #region "DayOfWeek"

        public int DayOfWeek
        {
            get
            {
                return Convert.ToInt32(CloseDate.DayOfWeek);
            }
        }

        [Feature]
        public double IsMonday
        {
            get
            {
                return (CloseDate.DayOfWeek == System.DayOfWeek.Monday) ? 1.0 : 0.0;
            }
        }

        [Feature]
        public double IsTuesday
        {
            get
            {
                return (CloseDate.DayOfWeek == System.DayOfWeek.Tuesday) ? 1.0 : 0.0;
            }
        }

        [Feature]
        public double IsWednesday
        {
            get
            {
                return (CloseDate.DayOfWeek == System.DayOfWeek.Wednesday) ? 1.0 : 0.0;
            }
        }
        [Feature]
        public double IsThursday
        {
            get
            {
                return (CloseDate.DayOfWeek == System.DayOfWeek.Thursday) ? 1.0 : 0.0;
            }
        }
        [Feature]
        public double IsFriday
        {
            get
            {
                return (CloseDate.DayOfWeek == System.DayOfWeek.Friday) ? 1.0 : 0.0;
            }
        }
        #endregion

        [Feature]
        [JsonProperty("KLCPLH")]
        public double KLCPLH { get; set; }

        //[Feature]
        [JsonProperty("KLCPNY")]
        public double KLCPNY { get; set; }

        [Feature]
        [JsonProperty("PriorClosePrice")]
        public double PriorClosePrice { get; set; }

        //[Feature]
        [JsonProperty("CeilingPrice")]
        public double CeilingPrice { get; set; }

        //[Feature]
        [JsonProperty("FloorPrice")]
        public double FloorPrice { get; set; }

        [Feature]
        [JsonProperty("TradingVolume")]
        public double TradingVolume { get; set; }

        //[Feature]
        [JsonProperty("TotalVal")]
        public long TotalVal { get; set; }

        [Feature]
        [JsonProperty("CapitalLevel")]
        public double CapitalLevel { get; set; }

        [JsonProperty("Highest")]
        public double Highest { get; set; }

        [JsonProperty("Lowest")]
        public double Lowest { get; set; }

        [JsonProperty("OpenPrice")]
        public double OpenPrice { get; set; }

        //[Feature] // - added as of 12/30
        [JsonProperty("ClosePrice")]
        public double ClosePrice { get; set; }

        private double _profit = double.NaN;

        [Label]
        public double Profit
        {
            get
            {
                if (_profit.Equals(double.NaN)) _profit = (ClosePrice / PriorClosePrice) - 1;
                return _profit;
            }
            set
            {
                _profit = value;
            }
        }

        public double ProfitPretified
        {
            get
            {
                return Math.Round(Profit * 100, 3);
            }
        }

        public double Ranking { get; set; }

        //[Feature] - removed as of 1/1
        [JsonProperty("AvrPrice")]
        public double AvrPrice { get; set; }

        //[Feature] - removed as of 12/28
        [JsonProperty("Oscillate")]
        public double Oscillate { get; set; }

        //[Feature] - removed as of 1/1
        [JsonProperty("PercentOscillate")]
        public double PercentOscillate { get; set; }

        [Feature]
        [JsonProperty("YearLow")]
        public double YearLow { get; set; }

        [Feature]
        [JsonProperty("YearHigh")]
        public double YearHigh { get; set; }

        [Feature]
        [JsonProperty("AvgVol")]
        public double AvgVol { get; set; }

        [Feature]
        [JsonProperty("BuyRedundancy")]
        public double BuyRedundancy { get; set; }

        [Feature]
        [JsonProperty("SellRedundancy")]
        public double SellRedundancy { get; set; }

        [Feature]
        [JsonProperty("ForeignRoom")]
        public double ForeignRoom { get; set; }

        [Feature]
        [JsonProperty("Dividend")]
        public double Dividend { get; set; }

        [Feature]
        [JsonProperty("Yield")]
        public double Yield { get; set; }

        [Feature]
        [JsonProperty("Beta")]
        public double Beta { get; set; }

        [Feature]
        [JsonProperty("EPS")]
        public double EPS { get; set; }

        [Feature]
        [JsonProperty("PE")]
        public double PE { get; set; }

        [Feature]
        [JsonProperty("FwPE")]
        public double FwPE { get; set; }

        [Feature]
        [JsonProperty("BVPS")]
        public double BVPS { get; set; }

        [Feature]
        [JsonProperty("PB")]
        public double PB { get; set; }

        [Feature] // - added as of 12/30
        [JsonProperty("TotalRoom")]
        public double TotalRoom { get; set; }

        [Feature] // - added as of 12/30
        [JsonProperty("CurrRoom")]
        public double CurrRoom { get; set; }

        //[Feature] // - added as of 12/30
        [JsonProperty("RemainRoom")]
        public double RemainRoom { get; set; }

        [Feature] // - added as of 12/30
        [JsonProperty("ForeignBuyVolume")]
        public double ForeignBuyVolume { get; set; }

        //[Feature] // - added as of 12/30
        [JsonProperty("F_BuyVal")]
        public double FBuyVal { get; set; }

        [Feature] // - added as of 12/30
        [JsonProperty("F_SellVol")]
        public double FSellVol { get; set; }

        //[Feature] // - added as of 12/30
        [JsonProperty("F_SellVal")]
        public double FSellVal { get; set; }

        [Feature] // - added as of 12/30
        [JsonProperty("F_BuyPutVol")]
        public double FBuyPutVol { get; set; }

        //[Feature] // - added as of 12/30
        [JsonProperty("F_BuyPutVal")]
        public double FBuyPutVal { get; set; }

        [Feature] // - added as of 12/30
        [JsonProperty("F_SellPutVol")]
        public double FSellPutVol { get; set; }

        //[Feature] // - added as of 12/30
        [JsonProperty("F_SellPutVal")]
        public double FSellPutVal { get; set; }

        //[Feature]
        [JsonProperty("MarketStatus")]
        public double MarketStatus { get; set; }

        [JsonProperty("ColorId")]
        public double ColorId { get; set; }

        [JsonProperty("CloseDateView")]
        public string CloseDateView { get; set; }

        [JsonProperty("TotalTime")]
        public string TotalTime { get; set; }

        [JsonProperty("URL")]
        public string URL { get; set; }
    }
    #endregion
}
