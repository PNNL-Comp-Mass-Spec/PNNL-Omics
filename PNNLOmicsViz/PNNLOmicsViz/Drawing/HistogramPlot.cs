using System.Collections.Generic;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace PNNLOmicsViz.Drawing
{
    public sealed class HistogramPlot : PlotBase
    {
        private CategoryAxis m_xAxis;
        private LinearAxis m_yAxis;

        public HistogramPlot(Dictionary<double, int> histogram, string name) :
            base(name)
        {
            var axis = new CategoryAxis(AxisPosition.Bottom)
            {
                MinorStep = 1,
                LabelField = "Values",
                AbsoluteMinimum = 0,
                GapWidth = 0
            };

            // Count axis
            var linearAxis = new LinearAxis(AxisPosition.Left, 0)
            {
                AbsoluteMinimum = 0,
                MinimumPadding = 1,
                Minimum = 0
            };

            Model.Axes.Add(axis);
            Model.Axes.Add(linearAxis);


            // Add the data to the view model
            var data = new ColumnSeries
            {
                ValueField = "Value",
                LabelFormatString = "{0}"
            };

            foreach (double key in histogram.Keys)
            {
                axis.Labels.Add(key.ToString());
                int number = histogram[key];
                data.Items.Add(new ColumnItem(number));
            }

            m_xAxis = axis;
            m_yAxis = linearAxis;
            Model.Series.Add(data);
        }
    }
}