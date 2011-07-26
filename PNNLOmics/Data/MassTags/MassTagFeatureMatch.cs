using PNNLOmics.Data.Features;

namespace PNNLOmics.Data.MassTags
{
    /// <summary>
    /// Holds matches between a feature and a mass tag.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FeatureMatchLight<T, U>
        where T : FeatureLight
        where U : FeatureLight
    {
        public T Observed
        {
            get;
            set;
        }
        public U Target
        {
            get;
            set;
        }
    }
}
