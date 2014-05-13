#region

using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
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
            Model.PlotMargins = new OxyThickness(double.NaN, 40, 40, double.NaN);

            var axis = new CategoryAxis            
            {
                Position = AxisPosition.Bottom,                                              
                AbsoluteMinimum = 0,
                GapWidth = 0,     
                Angle =  -90,
                IsAxisVisible = false
            };
            Model.Axes.Add(axis);

            // Count axis
            var linearAxis = new LinearAxis
            {
                AbsoluteMinimum = 0,
                Position = AxisPosition.Bottom,   
            };
            Model.Axes.Add(linearAxis);

            // Count axis
            var linearAxis2 = new LinearAxis
            {
                Position = AxisPosition.Left,
                AbsoluteMinimum = 0,
                MinimumPadding = 1,
                Minimum = 0
            };
            Model.Axes.Add(linearAxis2);


            // Add the data to the view model
            var data = new ColumnSeries
            {
                FillColor = OxyColors.Red,
                StrokeColor = OxyColors.Black,
                ValueField = "Value",
                StrokeThickness = 2
            };
            var keys = histogram.Keys.OrderBy(x => x);

            var count = 0;
            foreach (var key in keys)
            {
                var keyValue = key.ToString();

                axis.Labels.Add(keyValue);
                axis.ActualLabels.Add(keyValue);

                var number = histogram[key];
                data.Items.Add(new ColumnItem(number));
            }

            Model.Series.Add(data);
        }
    }
}