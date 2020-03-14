using System.Collections.Generic;
using PNNLOmics.Algorithms.FeatureMatcher.Data;
using PNNLOmics.Annotations;
using PNNLOmics.Data.Features;
using PNNLOmics.Data.MassTags;

namespace PNNLOmics.Algorithms.FeatureMatcher
{
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.FeatureMatcher")]
    public class FeatureMatcherMassTag<TObserved, TTarget> :
        FeatureMatcher<TObserved, TTarget>               
        where TObserved : FeatureLight, new()
        where TTarget   : MassTagLight, new()
    {
        public FeatureMatcherMassTag(List<TObserved> observedFeatureList, List<TTarget> targetFeatureList, FeatureMatcherParameters matchParameters)
            : base(observedFeatureList, targetFeatureList, matchParameters)
        {
            
        }

        protected override void PerformStac(STACInformation stacInformation)
        {
            
            stacInformation.PerformStac(MatchList,
                MatchParameters.UserTolerances,
                MatchParameters.UseDriftTime,
                MatchParameters.UsePriors);
            
        }
    }
}