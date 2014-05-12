#region

using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

#endregion

namespace PNNLOmicsViz.Drawing
{
    /// <summary>
    ///     Creates a histogram from the dictionary provided
    /// </summary>
    public sealed class HistogramPlot : PlotBase
    {
        public HistogramPlot(Dictionary<double, int> histogram, string name) :
            base(name)
        {
            var axis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                MinorStep = 1,
                LabelField = "Values",
                AbsoluteMinimum = 0,
                GapWidth = 0
            };

            // Count axis
            var linearAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                AbsoluteMinimum = 0,
                MinimumPadding = 1,
                Minimum = 0
            };

            Model.Axes.Add(axis);
            Model.Axes.Add(linearAxis);

            // Add the data to the view model
            var data = new ColumnSeries
            {
                FillColor = OxyColors.Red,
                StrokeColor = OxyColors.Black,
                StrokeThickness = 2
            };

            foreach (var key in histogram.Keys.OrderBy(x => x))
            {
                axis.Labels.Add(key.ToString());
                var number = histogram[key];
                data.Items.Add(new ColumnItem(number));
            }

            Model.Series.Add(data);
        }
    }
}