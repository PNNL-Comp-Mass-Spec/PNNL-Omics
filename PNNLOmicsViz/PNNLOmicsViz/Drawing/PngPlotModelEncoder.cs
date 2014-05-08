using OxyPlot;
using OxyPlot.Wpf;
using PNNLOmics.Annotations;
using System.IO;
using System.Windows.Media.Imaging;

namespace PNNLOmicsViz.Drawing
{
    [UsedImplicitly]
    public sealed class PngPlotModelEncoder : IPlotModelEncoder<BitmapSource>
    {             
        /// <summary>
        /// Converts the plot model into an SVG string.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BitmapSource CreateImage(PlotModel model)
        {                      
            var pngExporter = new PngExporter();                            
            return pngExporter.ExportToBitmap(model);            
        }

        public void SaveImage(PlotModel model, string path)
        {
            using (var stream = File.Create(@"m:\asdfasdfa.png"))
            {
                var pngExporter = new PngExporter();
                pngExporter.Export(model, stream);                
            }
        }
    }
}