using DataGatherer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System.Diagnostics;

namespace DataGathererGUI
{
    public partial class Form1 : Form
    {
        private object syncLock = new object();
        private bool IsTraining { get; set; } = false;

        IEnumerable<DailyPrice> StocksList { get; set; }
        IEnumerable<DailyPrice> LatestStocks { get; set; }
        List<DailyPrice> _predictList = new List<DailyPrice>();
        List<ListBox> _stockListBox = new List<ListBox>();

        private struct TrainingProgress
        {
            public int epochs { get; set; }
            public double error { get; set; }
            public long timeElapsed { get; set; }
        }
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load the data

            //toolStripProgressBar1.Visible = true;
            //toolStripStatusLabel1.Text = "Loading";
            //var progressHandler = new Progress<int>(x => toolStripProgressBar1.Value = x);
            //var loadTask = DataLoader.ImportData(progressHandler);
            //loadTask.GetAwaiter().OnCompleted(
            //    () =>
            //    {
            //        toolStripStatusLabel1.Text = $"Imported {Global.DataList.Count} items";
            //        toolStripProgressBar1.Visible = false;
            //        UpdateList();
            //    });
            //await loadTask;
            DataLoader.ImportData();
            StocksList = Global.DataList.
                GroupBy(x => x.StockCode).
                Select(x => x.FirstOrDefault());

            var latestDate = Global.DataList.OrderByDescending(d => d.CloseDate).Take(1).FirstOrDefault().CloseDate;
            LatestStocks = Global.DataList.Where(x => (x.CloseDate == latestDate));
            _stockListBox.Add(listBox1);
            _stockListBox.Add(listBox4);
            UpdateList();
            comboBox1.SelectedIndex = 1;
            LoadModel(Global.ModelFile);
            txNodes.Text = Math.Ceiling(((double)Global.FeaturesCount + 1) / 2).ToString();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            toolStripProgressBar1.Visible = true;
            toolStripStatusLabel1.Text = "Downloading";
            var progressHandler = new Progress<int>(x => toolStripProgressBar1.Value = x);
            var downloadTask = DataLoader.GetDataFromDate(DateTime.Now, progressHandler, checkBox2.Checked);
            downloadTask.GetAwaiter().OnCompleted(
                () => 
                {
                    toolStripStatusLabel1.Text = "Ready";
                    toolStripProgressBar1.Visible = false;
                    UpdateList();
                    Global.UpdateData();

                    var latestDate = Global.DataList.OrderByDescending(d => d.CloseDate).Take(1).FirstOrDefault().CloseDate;
                    LatestStocks = Global.DataList.Where(x => (x.CloseDate == latestDate));
                });
            await downloadTask;
        }

        private void UpdateList(string filter = "", ListBox listControl = null)
        {
            Global.inputs = DataHelper.DataHelper.GetInputArray(Global.DataList);
            Global.outputs = DataHelper.DataHelper.GetOutputArray(Global.DataList);
            lbPoints.Text = Global.inputs.Length.ToString();
            lbFeatures.Text = Global.FeaturesCount.ToString();

            //DateTime latestDate = (from dates in Global.DataList
            //                       orderby dates.CloseDate descending
            //                       select dates.CloseDate).FirstOrDefault();

            if (String.IsNullOrWhiteSpace(filter))
            {
                foreach (var item in _stockListBox)
                {
                    item.DataSource = StocksList.ToList();
                    item.DisplayMember = "StockCode";
                }
            }
            else
            {
                if (listControl != null)
                {
                    listControl.DataSource = StocksList.
                        Where(x => (filter == "" || x.StockCode.ToLower().StartsWith(filter))).
                        ToList();
                }
            }
        }

        private void AddListItems(IEnumerable<DailyPrice> data, ListView list)
        {
            list.Items.Clear();
            var _data = data.ToList();
            var _buffer = new List<ListViewItem>();
            foreach (var item in _data.OrderByDescending(x => x.Ranking))
            {
                _buffer.Add(new ListViewItem(new string[]
                    {
                        item.Ranking.ToString(),
                        item.StockCode,
                        item.ClosePrice.ToString(),
                        item.ProfitPretified.ToString(),
                        item.URL
                    }
                )
                {
                    Tag = item
                });
            }
            list.Items.AddRange(_buffer.ToArray());
            list.Tag = _buffer; // Clear list data
        }

        private void FilterListItems(string filter, int columnIndex, ListView list)
        {
            if (list.Tag == null) list.Tag = list.Items;
          
            if (list.Tag != null)
            {
                var tagObject = list.Tag as IEnumerable<ListViewItem>;

                if (tagObject != null)
                {
                    var _tempList = new ListViewItem[tagObject.Count()];
                    //tagObject.CopyTo(_tempList, 0);
                    _tempList = tagObject.ToArray();
                    list.Items.Clear();
                    if (String.IsNullOrWhiteSpace(filter))
                    {
                        list.Items.AddRange(_tempList.ToArray());
                    }
                    else
                    {
                        list.Items.AddRange(_tempList
                            .Where(x => x.SubItems[columnIndex].Text.ToLower()
                                            .StartsWith(filter.ToLower()))
                            .ToArray());
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                IEnumerable<DailyPrice> priceForDate = Global.DataList
                    .Where(x => x.StockCode == ((DailyPrice)listBox1.SelectedItem).StockCode)
                    .OrderBy(x => x.CloseDate);
                
                listBox2.DataSource = priceForDate.ToList();
                chart1.Series[0].Points.DataBindY(priceForDate.Select(x => x.ClosePrice).ToArray());

                var firstItem = priceForDate.Select(x => x.ClosePrice).FirstOrDefault();
                var lastItem = priceForDate.Select(x => x.ClosePrice).LastOrDefault();
                lbStockSummary.Text = $"{priceForDate.Count()} days: Profit: {lastItem - firstItem} VND | {Math.Round((lastItem / firstItem - 1) * 100, 2)}%";

                listBox2.DisplayMember = "CloseDate";
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                propertyGrid1.SelectedObject = listBox2.SelectedItem;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateList(textBox1.Text.ToLower(), listBox1);
        }

        private void ResetTrainingButtons()
        {
            btnTrain.Enabled = (Global.Model != null);
            btnEvaluate.Enabled = (Global.Model != null);
            button5.Enabled = (Global.Model != null);
            btnPredict.Enabled = (Global.Model != null);
            button2.Enabled = (Global.Model != null);
            if (Global.Model != null)
            {
                var listNodes = Global.Model.Layers.Select(x => x.Neurons.Length.ToString()).ToArray();
                lbModelNodes.Text = String.Join("-", listNodes);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != String.Empty)
            {
                LoadModel(openFileDialog1.FileName);
            }
        }

        private void LoadModel(string fileName)
        {
            Global.ModelFile = openFileDialog1.FileName;
            ResetTrainingButtons();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (Global.Model != null)
            {
                Global.Model.Save(Global.ModelFile);
            }
        }

        private async void btnTrain_Click(object sender, EventArgs e)
        {
            var listError = new List<double>();
            if (IsTraining)
            {
                lock (syncLock)
                {
                    this.IsTraining = false;
                }
            }
            else
            {
                btnTrain.Text = "Pause Training";
                var progressHandler = new Progress<TrainingProgress>(x =>
                {
                    toolStripStatusLabel2.Text = 
                    $"Running epoch: {x.epochs} ({TimeSpan.FromMilliseconds(x.timeElapsed / Math.Max(x.epochs, 1)).TotalSeconds}s/epoch) - Current error: {x.error}. Time elapsed: {TimeSpan.FromMilliseconds(x.timeElapsed).ToString()}";
                    listError.Add(x.error);
                    if (listError.Count > 300) listError.RemoveAt(0);
                    chart3.Series[0].Points.DataBindY(listError.ToArray());
                });
                var trainingTask = TrainModel(progressHandler);
                trainingTask.GetAwaiter().OnCompleted(() =>
                {
                    toolStripStatusLabel2.Text = $"Ready. Current error: {GetCurrentScore()}";
                    btnTrain.Text = "Start Training";
                });
                await trainingTask;
            }
        }

        private async Task TrainModel(IProgress<TrainingProgress> progress)
        {
            if (Global.Model != null)
            {
                double learningRate = 1;
                double.TryParse(txLearnRate.Text, out learningRate);
                
                learningRate = Math.Max(learningRate, 0.1);

                //                BackPropagationLearning
                //              LevenbergMarquardtLearning
                //              ResilientBackpropagationLearning
                ISupervisedLearning teacher;

                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        teacher = new BackPropagationLearning(Global.Model);
                        ((BackPropagationLearning)teacher).LearningRate = learningRate;
                        break;
                    case 1:
                        teacher = new LevenbergMarquardtLearning(Global.Model, true);
                        ((LevenbergMarquardtLearning)teacher).LearningRate = learningRate;
                        break;
                    case 2:
                        teacher = new ResilientBackpropagationLearning(Global.Model);
                        ((ResilientBackpropagationLearning)teacher).LearningRate = learningRate;
                        break;
                    case 3:
                        teacher = new EvolutionaryLearning(Global.Model, 100);
                        break;
                    default:
                        teacher = new LevenbergMarquardtLearning(Global.Model, true);
                        ((LevenbergMarquardtLearning)teacher).LearningRate = learningRate;
                        break;
                }
                //var teacher = new ResilientBackpropagationLearning(Global.Model);

                this.IsTraining = true;

                bool _training = true;
                var sw = Stopwatch.StartNew();
                
                bool isKeepRunning = false;               

                await Task.Run(() => {
                    var retVal = new TrainingProgress();
                    retVal.error = double.PositiveInfinity;

                    while (_training)
                    {
                        lock (syncLock)
                        {
                            _training = this.IsTraining;
                            var error = teacher.RunEpoch(Global.inputs, Global.outputs) / Global.inputs.Length;
                            if (!isKeepRunning && (error > retVal.error))
                            {
                                if (MessageBox.Show("Training increases error. Continue?", 
                                    "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    isKeepRunning = true;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            retVal.error = error;
                        }

                        retVal.epochs++;
                        retVal.timeElapsed = sw.ElapsedMilliseconds;

                        //if (retVal.epochs % 50 == 0) // updates score every 50 epochs
                        //{
                        //    retVal.error = GetCurrentScore();
                        //}

                        progress.Report(retVal);
                    }
                });

                sw.Stop();
            }
        }

        private double GetCurrentScore()
        {
            lock (syncLock)
            {
                if (Global.Model == null) return 0;
                var score = new Score(Global.testingOutputs.Select(x => x.FirstOrDefault()).ToArray(),
                                    Global.testingInputs.Select(x => Global.Model.Compute(x).FirstOrDefault()).ToArray());
                return score.RMSE;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var result = true;
            if (Global.Model != null)
            {
                result = (MessageBox.Show("Current model is not saved. Continue?",
                    "Network not saved",
                    MessageBoxButtons.YesNo) == DialogResult.Yes);
                Global.ModelFile = null;
            }
            if (result)
            {
                string[] temp = txNodes.Text.Split('-');
                List<int> listNeurons = new List<int>();
                foreach (var str in temp)
                {
                    int neuronsCount = 1;
                    neuronsCount = Math.Max(neuronsCount, 1);
                    if (Int32.TryParse(str, out neuronsCount)) listNeurons.Add(neuronsCount);
                }
                listNeurons.Add(1); // last layer
                if (listNeurons.Count == 1) return;
                Global.Model = new ActivationNetwork(new SigmoidFunction(),
                    Global.FeaturesCount, listNeurons.ToArray());
                
                //Global.Model = new ActivationNetwork(new IdentityFunction(),
                //    Global.FeaturesCount, neuronsCount, 1);

                NguyenWidrow initializer = new NguyenWidrow(Global.Model);
                initializer.Randomize();
            }
            ResetTrainingButtons();
        }

        private void btnEvaluate_Click(object sender, EventArgs e)
        {
            if (Global.Model != null)
            {
                listView1.Items.Clear();
                double[] actual;
                double[] predicted;
                lock (syncLock)
                {
                    actual = Global.testingOutputs.Select(x => x.FirstOrDefault()).ToArray();
                    predicted = Global.testingInputs.Select(x => Global.Model.Compute(x).FirstOrDefault()).ToArray();

                }
                var score = new Score(actual, predicted);
                listBox3.Items.Add($"{DateTime.Now.ToShortTimeString()} - Score: {score.RMSE}");
                var tempList = new List<ListViewItem>();
                for (int i = 0; i < actual.Length; i++)
                {
                    var item = new ListViewItem(new string[] {
                        Math.Round(actual[i], 3).ToString(),
                        Math.Round(predicted[i], 3).ToString(),
                        $"{Math.Round((predicted[i] - actual[i]) / actual[i] * 100, 2).ToString()}%"
                    });
                    tempList.Add(item);
                }
                listView1.View = View.Details;
                listView1.Items.AddRange(tempList.ToArray());
            }
        }

        private DailyPrice PredictSingle(DailyPrice input, DateTime date, ActivationNetwork model = null)
        {
            var _model = model ?? Global.Model;
            
            _predictList = new List<DailyPrice>();
            var retVal = new DailyPrice();

            DataHelper.DataHelper.CloneObject(input, retVal);
            retVal.CloseDate = date;
            retVal.PriorClosePrice = retVal.ClosePrice;
            _predictList.Add(retVal);

            double[][] _input = DataHelper.DataHelper.GetInputArray(_predictList);
            var output = new double[1];

            output[0] = _model.Compute(_input[0]).FirstOrDefault();
            _predictList[0].Profit = output[0];
            _predictList[0].PriorClosePrice = _predictList[0].ClosePrice;
            _predictList[0].ClosePrice = Math.Round(_predictList[0].ClosePrice * (_predictList[0].Profit + 1));
            _predictList[0].AvrPrice = _predictList[0].ClosePrice;

            return _predictList[0];
        }

        private void btnPredict_Click(object sender, EventArgs e)
        {
            if (Global.Model != null)
            {
                var predictList = new List<DailyPrice>();
                var date = Utils.GetNextDay(DateTime.Now);
                foreach (var stock in LatestStocks)
                {
                    predictList.Add(PredictSingle(stock, date));
                }
                DataLoader.ProcessData(predictList);

                AddListItems(predictList, listView3);

                var retVal = MessageBox.Show($"Latest recommendation is from {Global.GetLatestRecommendDate().ToShortDateString()}\nDo you want to save the recommendation?",
                    "Save recommendation?", MessageBoxButtons.YesNo);
                if (retVal == DialogResult.Yes)
                {
                    using (var writer = new StreamWriter(File.Open(Global.GetRecommendFile, FileMode.OpenOrCreate)))
                    {
                        writer.AutoFlush = true;
                        writer.Write(string.Join(", \n", (from price in predictList
                                                          orderby price.Ranking descending
                                                          select $"#{price.Ranking}: {price.StockCode} - {price.ProfitPretified}")
                                             .ToArray()));
                    }
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                AddListItems(LatestStocks, listView2);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            FilterListItems(textBox2.Text, 1, listView2);
            FilterListItems(textBox2.Text, 1, listView3);
        }

        private async void listView3_DoubleClick(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                if (listView3.SelectedItems[0].SubItems.Count > 1)
                {
                    var selected = listView3.SelectedItems[0].SubItems[1].Text;
                    var selectedObj = listView3.SelectedItems[0].Tag as DailyPrice;

                    if (selectedObj != null)
                    {
                        label6.Text = $"Retrieving data for {selected}";
                        var test = await DataLoader.GetSingleStock(selected);

                        if (test != null)
                        {
                            MessageBox.Show($"Stock code: {selectedObj.StockCode}\n" +
                                $"Profit: {test.ProfitPretified} (predicted: {selectedObj.ProfitPretified})\n" +
                                $"Error: {Math.Abs(selectedObj.ProfitPretified - test.ProfitPretified)}\n" +
                                $"Price: {test.ClosePrice}\n" +
                                $"Last Close Price: {test.PriorClosePrice}\n" + 
                                $"Close Date: {test.CloseDate.ToLongDateString()}\n",
                                "Result");
                        }
                    }
                }
                label6.Text = "Ready";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Global.Model == null) return;

            double right = 0, wrong = 0, wrongNegative = 0, rightPositive = 0, approximateScore = 0;
            var dictPrice = new Dictionary<DateTime, Dictionary<string, DailyPrice>>();
            var dictPredict = new Dictionary<DateTime, Dictionary<string, DailyPrice>>();
            var returnList = new List<DailyPrice>();

            foreach (var price in Global.DataList)
            {
                if (!dictPrice.ContainsKey(price.CloseDate)) dictPrice.Add(price.CloseDate, new Dictionary<string, DailyPrice>());
                if (!dictPrice[price.CloseDate].ContainsKey(price.StockCode)) dictPrice[price.CloseDate].Add(price.StockCode, price);
            }

            var dateList = dictPrice.Keys.OrderBy(x => x).ToList();

            if (dateList.Count < 2) return;

            for (int i = 0; i < dateList.Count - 1; i++)
            {
                var currentDate = dateList[i];
                var nextDate = dateList[i + 1];
                if (!dictPredict.ContainsKey(currentDate)) dictPredict.Add(currentDate, new Dictionary<string, DailyPrice>());
                foreach (var stock in dictPrice[currentDate].Values)
                {
                    DailyPrice nextDayValue;
                    DailyPrice nextDayPredicted;
                    if (dictPrice[nextDate].TryGetValue(stock.StockCode, out nextDayValue))
                    {
                        nextDayPredicted = PredictSingle(stock, nextDate);

                        if (!dictPredict[currentDate].ContainsKey(nextDayPredicted.StockCode)) dictPredict[currentDate].Add(nextDayPredicted.StockCode, nextDayPredicted);
                        //if ((nextDayPredicted.Profit >= 0 && nextDayValue.Profit >= 0) ||
                        //    (nextDayPredicted.Profit < 0 && nextDayValue.Profit < 0))
                        //{
                        //    right += 1;
                        //    if (nextDayPredicted.Profit >= 0 && nextDayValue.Profit >= 0) rightPositive += 1;
                        //}
                        //else
                        //{
                        //    wrong += 1;
                        //    if (nextDayPredicted.Profit >= 0 && nextDayValue.Profit < 0) wrongNegative += 1;
                        //}
                        if (nextDayPredicted.Profit >= 0) // hardcoded threshold
                        {
                            approximateScore += (nextDayValue.Profit);
                            if (nextDayValue.Profit < 0)
                            {
                                wrong += 1;
                            }
                            else
                            {
                                right += 1;
                            }
                        }
                    }
                }
                //DataLoader.ProcessData(dictPredict[currentDate].Values);
            }
            double total = right + wrong;
            MessageBox.Show($"Correct guesses: {right} - {Math.Round((right/total) * 100, 3)}%\n" +
                $"Incorrect guesses: {wrong} - {Math.Round((wrong / total) * 100, 3)}%\n" +
                $"Won guesses: {rightPositive} - {Math.Round((rightPositive / total) * 100, 3)}%\n" +
                $"Loss guesses: {wrongNegative} - {Math.Round((wrongNegative / total) * 100, 3)}%\n" +
                $"Approximate Score: {approximateScore / dateList.Count}");
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            UpdateList(textBox3.Text.ToLower(), listBox4);
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Global.Model == null || listBox4.SelectedItem == null) return;

            var selected = ((DailyPrice)listBox4.SelectedItem).StockCode;
            toolStripStatusLabel3.Text = $"Predicting {((DailyPrice)listBox4.SelectedItem).StockCode}";
            IEnumerable<DailyPrice> priceForDate = Global.DataList
                .Where(x => x.StockCode == selected)
                .OrderBy(x => x.CloseDate);

            var currentList = priceForDate.ToList();
            chart2.Series[0].Points.DataBindY(priceForDate.Select(x => x.ClosePrice).ToArray());

            ActivationNetwork _model = null;
            //Training a new model for the stock
            if (checkBox1.Checked)
            {
                _model = new ActivationNetwork(new BipolarSigmoidFunction(2),
                    Global.FeaturesCount, 15, 1); //hardcoded neurons count
                var _iterations = 1000;
                var _teacher = new ResilientBackpropagationLearning(_model);
                var _initializer = new NguyenWidrow(_model);
                _initializer.Randomize();

                var _inputs = DataHelper.DataHelper.GetInputArray(currentList);
                var _outputs = DataHelper.DataHelper.GetOutputArray(currentList);

                for (int i = 0; i < _iterations; i++)
                {
                    var trainingError = _teacher.RunEpoch(_inputs, _outputs);
                    toolStripStatusLabel3.Text = $"Predicting {((DailyPrice)listBox4.SelectedItem).StockCode} {i}/{_iterations} | e={trainingError / _inputs.Length}";
                    Application.DoEvents();
                }
            }

            //end of training
            var firstItem = priceForDate.Select(x => x.ClosePrice).FirstOrDefault();
            var lastItem = priceForDate.Select(x => x.ClosePrice).LastOrDefault();
            var firstStock = priceForDate.FirstOrDefault();
            lbStockCurrent.Text = $"{selected} | {priceForDate.Count()} days: Profit: {lastItem - firstItem} VND | {Math.Round((lastItem / firstItem - 1) * 100, 2)}%\n" +
                $"Volatility: {Math.Round(firstStock.Volatility * 100, 2)}%\n" +
                $"URL: {firstStock.URL}";

            var listPredict = priceForDate.ToList();
            int dayCount = listPredict.Count;
            double error = 0.0;
            if (listPredict.Count > 1)
            {
                for (int i = 0; i < listPredict.Count - 1; i++)
                {
                    var previous = currentList[i];
                    var original = currentList[i + 1];
                    listPredict[i + 1] = PredictSingle(previous, Utils.GetNextDay(previous.CloseDate), _model);
                    error += Math.Sqrt(Math.Pow(listPredict[i + 1].Profit - original.Profit, 2));
                }
                error /= dayCount;
                var startIdx = listPredict.Count - 1;
                string predictedDetails = String.Empty;

                for (int i = 0; i < 5; i++)
                {
                    var newIdx = startIdx + i;
                    var previous = listPredict.LastOrDefault();
                    var predictValue = PredictSingle(previous, Utils.GetNextDay(previous.CloseDate));
                    listPredict.Add(predictValue);
                    predictedDetails += $"{predictValue.CloseDate} - {predictValue.ClosePrice} | {predictValue.ProfitPretified}%\n";
                }

                chart2.Series[1].Points.DataBindY(listPredict.Select(x => x.ClosePrice).ToArray());

                var firstPredict = listPredict[dayCount - 1].ClosePrice;
                var lastPredict = Math.Round(listPredict.Select(x => x.ClosePrice).LastOrDefault());

                lbForecast.Text = $"Average error: {error}\n" +
                                  $"Last predicted price: {lastPredict}\n" +
                                  $"Change from now: {lastPredict - firstPredict} VND | {Math.Round((lastPredict / firstPredict - 1) * 100, 2)}%\n\n" +
                                  predictedDetails;
            }

            //moving average (4 days) as baseline
            int numDays = 4;
            var averageList = new List<double>();
            for (int i = 0; i < listPredict.Count; i++)
            {
                if (i >= (numDays - 1))
                {
                    var average = 0.0d;
                    for (int a = 0; a < numDays; a++)
                    {
                        if (i < currentList.Count)
                        {
                            average += currentList[i - a].ClosePrice / numDays;
                        }
                        else
                        {
                            average += listPredict[i - a].ClosePrice / numDays;
                        }
                    }
                    averageList.Add(average);
                }
                else averageList.Add(double.NaN);
            }
            chart2.Series[2].Points.DataBindY(averageList.ToArray());

            toolStripStatusLabel3.Text = $"Ready";
            checkBox1.Checked = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                listBox4_SelectedIndexChanged(this, e);
            }
        }
    }
}
