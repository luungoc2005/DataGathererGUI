using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGathererGUI
{
    public class Score
    {
        private double[] _data;
        private double[] _expected;

        public Score(double[] data, double[] expected)
        {
            if (data.Length != expected.Length)
            {
                //Error
                _data = data;
                _expected = data;
            }
            else
            {
                _data = data;
                _expected = expected;
            }
        }

        private double _MSE = double.NaN;
        public double MSE
        {
            get
            {
                if (_MSE.Equals(double.NaN))
                {
                    if (_data.Length == 0)
                        _MSE = 0;
                    else
                    {
                        double sum = 0;
                        for (int i = 0; i < _data.Length; i++)
                        {
                            sum += Math.Pow((_data[i] - _expected[i]), 2);
                        }
                        _MSE = sum / _data.Length;
                    }
                }
                return _MSE;
            }
        }

        public double RMSE
        {
            get
            {
                return Math.Sqrt(MSE);
            }
        }
    }
}
