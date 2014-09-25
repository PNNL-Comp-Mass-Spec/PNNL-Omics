using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Extensions
{
    public static class MsnExtensions
    {
        public static UMCLight GetParentUmc(this MSSpectra spectrum)
        {
            if (spectrum == null) return null;

            if (spectrum.ParentFeature != null)
            {
                return spectrum.ParentFeature.GetParentUmc();
            }
            return null;
        }
    }
}