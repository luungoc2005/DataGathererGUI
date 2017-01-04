using DataGatherer.Models;
using DataGathererGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Accord.Neuro;
using System.IO;

namespace DataGathererGUI
{
    public static class Global
    {
        private static List<string> _stockCodes = new List<string>();
        public static List<DailyPrice> DataList { get; set; } = new List<DailyPrice>();
        public static List<string> StockCodes
        {
            get
            {
                if (_stockCodes.Count == 0)
                {
                    _stockCodes = DataLoader.GetStockCodes();
                }

                return _stockCodes;
            }
        }

        public static double[][] inputs;
        public static double[][] outputs;

        public static double[][] testingInputs;
        public static double[][] testingOutputs;

        private static PropertyInfo[] features;

        public static int FeaturesCount
        {
            get
            {
                if (features == null || features.Length == 0) features = DataHelper.DataHelper.GetFeatures(typeof(DailyPrice));
                return features.Length;
            }
        }

        private static ActivationNetwork _model;
        public static ActivationNetwork Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                if (_model != null)
                {
                    UpdateData();
                }
            }
        }

        public static void UpdateData()
        {
            if (DataList.Count > 0)
            {
                // initialize the data sets
                object[] trainingList;
                object[] testingList;

                DataHelper.DataHelper.SliceData(DataList.ToArray(), 0.8, out trainingList, out testingList);

                inputs = DataHelper.DataHelper.GetInputArray(trainingList);
                outputs = DataHelper.DataHelper.GetOutputArray(trainingList);

                testingInputs = DataHelper.DataHelper.GetInputArray(testingList);
                testingOutputs = DataHelper.DataHelper.GetOutputArray(testingList);
            }
        }

        private static string _modelFile = String.Empty;
        public static string ModelFile
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_modelFile) || !File.Exists(_modelFile))
                {
                    string path = $"{Directory.GetCurrentDirectory()}\\models";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    _modelFile = $"{path}\\model.dat";
                }
                return _modelFile;
            }
            set
            {
                if (File.Exists(value))
                {
                    _modelFile = value;
                    using (var fs = File.OpenRead(Global.ModelFile))
                    {
                        Model = (ActivationNetwork)Network.Load(fs);
                    }
                }
            }
        }

        public static string GetRecommendFile
        {
            get
            {
                string path = $"{Directory.GetCurrentDirectory()}\\recommend";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return $"{path}\\{Utils.GetUnixTime(DateTime.Now)}.txt";
            }
        }

        public static DateTime GetLatestRecommendDate()
        {
            string path = $"{Directory.GetCurrentDirectory()}\\recommend";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var dateList = new List<DateTime>();
            foreach (var filePath in Directory.EnumerateFiles(path))
            {
                var date = Utils.UnixToDateTime(Path.GetFileNameWithoutExtension(filePath));
                dateList.Add(date);
            }
            if (dateList.Count == 0)
            {
                return new DateTime(1970, 1, 1);
            }
            else
            {
                return dateList.OrderByDescending(x => x).FirstOrDefault();
            }
        }
    }
}
