using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Extensions
{
    public static class MsnExtensions
    {
        public static UMCLight GetParentUmc(this MSSpectra spectrum)
        {
            return spectrum?.ParentFeature?.GetParentUmc();
        }
    }
}