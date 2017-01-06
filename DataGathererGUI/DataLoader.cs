using DataGatherer;
using DataGatherer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace DataGathererGUI
{
    public static class DataLoader
    {
        public static void ImportData()
        {
            List<DailyPrice> dataList = new List<DailyPrice>();
            
            int count = 0, total = 0;
            if (Directory.Exists($"{Directory.GetCurrentDirectory()}\\data"))
            {
                string[] fileList = Directory.EnumerateFiles($"{Directory.GetCurrentDirectory()}\\data", "*").ToArray();
                total = fileList.Length;
                foreach (string filePath in fileList)
                {
                    using (var inStream = new StreamReader(File.OpenRead(filePath)))
                    {
                        //Console.WriteLine($"Importing from {filePath}");
                        while (!inStream.EndOfStream)
                        {
                            try
                            {
                                var import = JsonConvert.DeserializeObject<List<DailyPrice>>(inStream.ReadLine());

                                if (!(import == null) && import.Count > 0) dataList.Add(import[0]);
                                //if (!(import == null) && import.Count > 0 && import[0].MarketStatus != 0) System.Windows.Forms.MessageBox.Show("test");
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                    count += 1;
                }
            }
            else
            {
                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\data");
            }
            Console.WriteLine($"Imported {dataList.Count} entries");

            ProcessData(dataList);
            Global.DataList.AddRange(dataList);
        }

        public static async Task<DailyPrice> GetSingleStock(string stockCode)
        {
            string data = await WebMethods.WebGet($"http://finance.vietstock.vn/AjaxData/TradingResult/GetStockData.ashx?scode={stockCode}&_={Utils.GetUnixTime(DateTime.Now).ToString()}");
            if (data != null)
            {
                try
                {
                    var des = JsonConvert.DeserializeObject<List<DailyPrice>>(data);
                    if (des != null && des.Count > 0) return des[0];
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public static async Task GetDataFromDate(DateTime time, IProgress<int> progress, bool saveToFile = true)
        {
            List<DailyPrice> dataList = new List<DailyPrice>();
            int count = 0;

            var strFileName = $"{Directory.GetCurrentDirectory()}\\data\\{Utils.GetUnixTime(time).ToString()}.txt";
            Console.WriteLine($"Downloading data for {time.Date.ToString()}");
            using (var outStream = new StreamWriter(File.Open(strFileName, FileMode.OpenOrCreate)))
            {
                outStream.AutoFlush = true;

                if (Global.StockCodes.Count>0)
                {
                    for (int idx = 0; idx < Global.StockCodes.Count(); idx++)
                    {
                        var stockCode = Global.StockCodes[idx];

                        if (!String.IsNullOrWhiteSpace(stockCode))
                        {
                            string data = await WebMethods.WebGet($"http://finance.vietstock.vn/AjaxData/TradingResult/GetStockData.ashx?scode={stockCode}&_={Utils.GetUnixTime(time).ToString()}");

                            if (!String.IsNullOrWhiteSpace(data) && data != "[]")
                            {
                                outStream.WriteLine(data);

                                try
                                {
                                    var des = JsonConvert.DeserializeObject<List<DailyPrice>>(data);

                                    if (!(des == null) && des.Count > 0) dataList.Add(des[0]);
                                }
                                catch
                                {
                                    continue;
                                }
                            }

                            count += 1;
                            progress.Report((int)Math.Floor(((double)count / (double)Global.StockCodes.Count) * 100));
                            
                            // Console.Write($" \r{count}/{stockCodes.Count} - {stockCode}   ");
                        }
                    }
                }
            }
            if (!saveToFile) File.Delete(strFileName);
            ProcessData(dataList);
            Global.DataList.AddRange(dataList);
        }

        public static List<string> GetStockCodes()
        {
            List<string> stockCodes = new List<String>();

            using (var inStream = new StreamReader(File.OpenRead("input.txt")))
            {
                while (!inStream.EndOfStream)
                {
                    stockCodes.Add(inStream.ReadLine());
                }
            }

            return stockCodes;
        }

        public static List<DailyPrice> CalculateStdVar(IEnumerable<DailyPrice> inputList)
        {
            var returnList = new List<DailyPrice>();
            var averageList = new Dictionary<string, double>();

            foreach (var price in inputList)
            {
                if (!averageList.ContainsKey(price.StockCode))
                {
                    List<DailyPrice> stocks = inputList.Where(x => (x.StockCode == price.StockCode)).ToList();
                    var average = stocks.Sum(x => x.Profit) / stocks.Count;
                    var stdVar = Math.Sqrt(stocks.Select(x => Math.Pow(x.Profit - average, 2)).Sum() / stocks.Count);
                    returnList.AddRange(stocks.Select(x =>
                    {
                        var newPrice = x;
                        newPrice.Volatility = stdVar;
                        return newPrice;
                    }));
                }
            }

            return returnList;
        }

        public static List<DailyPrice> ProcessData(IEnumerable<DailyPrice> inputList)
        {
            var dictPrice = new Dictionary<DateTime, List<DailyPrice>>();
            var returnList = new List<DailyPrice>();

            foreach (var price in inputList)
            {
                if (!dictPrice.ContainsKey(price.CloseDate)) dictPrice.Add(price.CloseDate, new List<DailyPrice>());
                dictPrice[price.CloseDate].Add(price);
            }

            foreach (var dataList in dictPrice.Values)
            {
                if (dataList.Count > 1)
                {
                    dataList.Sort((x1, x2) => (x1.Profit.CompareTo(x2.Profit)));

                    for (int i = 0; i < dataList.Count; i++)
                    {
                        dataList[i].Ranking = i + 1;
                    }
                }

                returnList.AddRange(dataList);
            }

            returnList = CalculateStdVar(returnList);

            return returnList;
        }
    }
}
