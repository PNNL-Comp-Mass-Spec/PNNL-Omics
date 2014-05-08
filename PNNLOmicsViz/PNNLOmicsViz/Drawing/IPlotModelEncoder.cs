using OxyPlot;
using PNNLOmics.Annotations;

namespace PNNLOmicsViz.Drawing
{
    public interface IPlotModelEncoder<T>
    {
        [UsedImplicitly]
        T CreateImage(PlotModel model);

        /// <summary>
        ///     Saves the model data to the path provided assuming format T.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="path"></param>
        void SaveImage(PlotModel model, string path);
    }
}