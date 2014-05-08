using System.IO;
using OxyPlot;
using PNNLOmics.Annotations;

namespace PNNLOmicsViz.Drawing
{
    /// <summary>
    ///     Creates an SVN image from the plot model provided.
    /// </summary>
    [UsedImplicitly]
    public sealed class SvgBasicEncoder : IPlotModelEncoder<string>
    {
        private const int DEFAULT_WIDTH = 800;
        private const int DEFAULT_HEIGHT = 600;

        public SvgBasicEncoder()
            : this(DEFAULT_WIDTH, DEFAULT_HEIGHT)
        {
        }

        public SvgBasicEncoder(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        /// <summary>
        ///     Converts the plot model into an SVG string.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string CreateImage(PlotModel model)
        {
            string svgString = model.ToSvg(Width, Height, true);

            return svgString;
        }

        public void SaveImage(PlotModel model, string path)
        {
            string image = CreateImage(model);
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.Write(image);
            }
        }
    }
}