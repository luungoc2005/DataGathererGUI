using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataHelper.Attributes;
using System.Threading.Tasks;

namespace DataHelper
{
    public static class DataHelper
    {
        /// <summary>
        /// Gets list of properties marked with Feature attribute
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetFeatures(Type input)
        {
            IEnumerable<PropertyInfo> returnList;
            returnList = input.GetRuntimeProperties().Where(x =>
            {
                return x.IsDefined(typeof(Feature));
            });
            return returnList.ToArray();
        }

        public static PropertyInfo[] GetLabels(Type input)
        {
            IEnumerable<PropertyInfo> returnList;
            returnList = input.GetRuntimeProperties().Where(x =>
            {
                return x.IsDefined(typeof(Label));
            });
            return returnList.ToArray();
        }

        public static void CloneObject(object input, object output)
        {
            IEnumerable<PropertyInfo> returnList;
            returnList = input.GetType().GetRuntimeProperties().Where(x => (x.CanRead && x.CanWrite));
            
            foreach (var property in returnList)
            {
                property.SetValue(output, property.GetValue(input, null));
            }
        }

        public static void SliceData(object[] input, double learningRate, out object[] learningSlice, out object[] testingSlice)
        {
            if (input.Length == 0 || learningRate <= 0 || learningRate >= 1)
            {
                learningSlice = input;
                testingSlice = new double[0][];
            }
            else
            {
                var rand = new Random();
                var _input = new List<object>();
                var _learningSlice = new List<object>();
                var learningCount = Math.Max((int)Math.Floor(input.Length * learningRate), 1);

                _input.AddRange(input);

                for (int i = 0; i < learningCount; i++)
                {
                    var idx = rand.Next(_input.Count - 1);
                    _learningSlice.Add(_input[idx]);
                    _input.RemoveAt(idx);
                }

                //what remains would be the testing slice

                learningSlice = _learningSlice.ToArray();
                testingSlice = _input.ToArray();
            }
        }

        public static double[][] GetInputArray(IEnumerable<object> input)
        {
            var inputArray = input.ToArray();
            if (inputArray.Length == 0)
            {
                return new double[0][];
            }
            else
            {
                var features = GetFeatures(inputArray[0].GetType());
                if (features.Length == 0) return new double[0][];

                var dataList = input.Select(point =>
                {
                    return features.Select(feature =>
                    {
                        return GetPropertyValue(point, feature);
                    }).ToArray();
                });

                return dataList.ToArray();
            }
        }

        public static double[][] GetOutputArray(IEnumerable<object> input)
        {
            var inputArray = input.ToArray();
            if (inputArray.Length == 0)
            {
                return new double[0][];
            }
            else
            {
                var features = GetLabels(inputArray[0].GetType());
                if (features.Length == 0) return new double[0][];

                var dataList = input.Select(point =>
                {
                    return features.Select(feature =>
                    {
                        return GetPropertyValue(point, feature);
                    }).ToArray();
                });

                return dataList.ToArray();
            }
        }

        public static double GetPropertyValue(object input, PropertyInfo property)
        {
            try
            {
                return (double)Convert.ChangeType(property.GetValue(input, null), typeof(double));
            }
            catch
            {
                return 0d;
            }
        }
    }
}
