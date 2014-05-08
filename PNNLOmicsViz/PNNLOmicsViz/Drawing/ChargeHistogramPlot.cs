using System.Collections.Generic;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace PNNLOmicsViz.Drawing
{
    public class ChargeHistogramPlot : PlotBase
    {
        protected CategoryAxis m_xAxis;

        public ChargeHistogramPlot(Dictionary<int, int> histogram, string name) :
            base(name)
        {
            var axis = new CategoryAxis(AxisPosition.Bottom)
            {
                MinorStep = 1,
                LabelField = "Charge States",
                AbsoluteMinimum = 0,
                GapWidth = 0
            };

            // Count axis
            var linearAxis = new LinearAxis(AxisPosition.Left, 0)
            {
                MaximumPadding = .15,
                AbsoluteMinimum = 1,
                Minimum = 0,
                MinimumPadding = 1
            };

            Model.IsLegendVisible = true;
            Model.Axes.Add(axis);
            Model.Axes.Add(linearAxis);

            // Add the data to the view model
            var data = new ColumnSeries
            {
                ValueField = "Value",
                StrokeThickness = 1,
                LabelFormatString = "{0}",
            };

            var colors = new ColorTypeIterator();
            foreach (int key in histogram.Keys)
            {
                axis.Labels.Add(key.ToString());
                int number = histogram[key];

                var column = new ColumnItem(number)
                {
                    Color = colors.GetColor(key)
                };
                data.Items.Add(column);
            }

            m_xAxis = axis;
            Model.Series.Add(data);
        }
    }
}