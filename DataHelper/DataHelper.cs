using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataHelper.Attributes
{
    public class Feature : Attribute { }
    public class Label : Attribute { }
}

/// <summary>
/// This is a helper class for generating nice values of 
/// </summary>
namespace DataHelper
{
    public static class DataHelper
    {
        #region "Generic methods"
        /// <summary>
        /// Gets a list of properties for which an attribute is defined
        /// </summary>
        /// <param name="input">Type of the input object</param>
        /// <param name="attribute">The attribute to search for</param>
        /// <returns></returns>
        private static PropertyInfo[] GetPropertyList(Type input, Type attribute)
        {
            if (attribute.IsEquivalentTo(typeof(Attribute))) return null;
            IEnumerable<PropertyInfo> returnList;
            returnList = input.GetRuntimeProperties().Where(x =>
            {
                return x.IsDefined(attribute);
            });
            return returnList.ToArray();
        }

        /// <summary>
        /// Generic function to get an array of double values from list of input based on an attribute
        /// </summary>
        /// <param name="input">The input data</param>
        /// <param name="attribute">The attribute to search for</param>
        /// <returns></returns>
        private static double[][] GetDataArray(IEnumerable<object> input, Type attribute)
        {
            var inputArray = input.ToArray();
            if (inputArray.Length == 0)
            {
                return new double[0][];
            }
            else
            {
                var features = GetPropertyList(inputArray[0].GetType(), attribute);
                if (features.Length == 0) return new double[0][];
                var returnList = new List<double[]>();
                var attributesList = new List<double>();

                foreach (var point in input)
                {
                    foreach (var feature in features)
                    {
                        if (feature.PropertyType.IsArray)
                        {
                            attributesList.AddRange(GetPropertyArray(point, feature));
                        }
                        else if (feature.PropertyType.IsEnum)
                        {
                            attributesList.AddRange(GetPropertyEnum(point, feature));
                        }
                        else
                        {
                            attributesList.Add(GetPropertyValue(point, feature));
                        }
                    }
                    returnList.Add(attributesList.ToArray());
                    attributesList.Clear();
                }

                return returnList.ToArray();
            }
        }

        private static double GetPropertyValue(object input, PropertyInfo property)
        {
            try
            {
                return (double)Convert.ChangeType(property.GetValue(input, null), typeof(double));
            }
            catch
            {
                return double.NaN;
            }
        }

        private static double[] GetPropertyArray(object input, PropertyInfo property)
        {
            try
            {
                return ((object[])property.GetValue(input, null))
                    .Select(x => (double)Convert.ChangeType(x, typeof(double)))
                    .ToArray();
            }
            catch
            {
                return null;
            }
        }

        private static double[] GetPropertyEnum(object input, PropertyInfo property)
        {
            try
            {
                var value = property.GetValue(input, null);
                var enumValues = Enum.GetValues(value.GetType());
                var returnList = new List<double>();
                foreach (var item in enumValues)
                {
                    if (value.Equals(item))
                    {
                        returnList.Add(1d);
                    }
                    else
                    {
                        returnList.Add(0d);
                    }
                }
                return returnList.ToArray();
            }
            catch
            {
                return null;
            }
        }
        #endregion
        /// <summary>
        /// Gets list of properties marked with the Feature attribute
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetFeatures(Type input)
        {
            return GetPropertyList(input, typeof(Attributes.Feature));
        }

        /// <summary>
        /// Get list of properties marked with the Label attribute
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetLabels(Type input)
        {
            return GetPropertyList(input, typeof(Attributes.Label));
        }

        /// <summary>
        /// Clones an object by copying its editable properties
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static void CloneObject(object input, object output)
        {
            IEnumerable<PropertyInfo> returnList;
            returnList = input.GetType().GetRuntimeProperties().Where(x => (x.CanRead && x.CanWrite));
            
            foreach (var property in returnList)
            {
                property.SetValue(output, property.GetValue(input, null));
            }
        }

        /// <summary>
        /// Randomly slices data into testing slice and learning slice
        /// </summary>
        /// <param name="input"></param>
        /// <param name="learningRate">Proportion of training data, usually 0.5 to 0.8</param>
        /// <param name="learningSlice">Return value: learning slice</param>
        /// <param name="testingSlice">Return valu: testing slice</param>
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

        /// <summary>
        /// Gets all inputs from an array of objects
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[][] GetInputArray(IEnumerable<object> input)
        {
            return GetDataArray(input, typeof(Attributes.Feature));
        }

        /// <summary>
        /// Gets all outputs from an array of objects
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[][] GetOutputArray(IEnumerable<object> input)
        {
            return GetDataArray(input, typeof(Attributes.Label));
        }

        /// <summary>
        /// Count of properties with the "Feature" attribute
        /// </summary>
        /// <param name="input">Input type</param>
        /// <returns></returns>
        public static int GetFeaturesCount(Type input)
        {
            return GetFeatures(input).Length;
        }

        /// <summary>
        /// Count of properties with the "Label" attribute
        /// </summary>
        /// <param name="input">Input type</param>
        /// <returns></returns>
        public static int GetLabelsCount(Type input)
        {
            return GetLabels(input).Length;
        }
    }
}
