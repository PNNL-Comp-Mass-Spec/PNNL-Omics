namespace PNNLOmics.Algorithms.Statistics
{
    public class HypothesisTestingFactory
    {
        public static IHypothesisTestingTwoSample CreateTests(HypothesisTests test)
        {
            IHypothesisTestingTwoSample newTest = null;

            switch (test)
            {
                case HypothesisTests.TTest:
                    newTest = new StudentTTest();
                    break;
                case HypothesisTests.MannWhitneyU:
                    newTest = new MannWhitneyTest();
                    break;
                case HypothesisTests.KolmogorovSmirnov:
                    newTest = new KolmogorovSmirnovTest();
                    break;
                case HypothesisTests.Wilcoxon:
                    newTest = new WilcoxonSignedRankAccord();
                    break;
                default:
                    break;
            }

            return newTest;
        }
    }

    public class NormalityTestingFactory
    {
        public static INormalityTest CreateTests(NormalityTests test)
        {
            INormalityTest newTest = null;

            switch (test)
            {
                case NormalityTests.JacqueBera:
                    newTest = new JacqueBeraNormalityTest();
                    break;
                default:
                    break;
            }

            return newTest;
        }
    }

    public enum HypothesisTests
    {
        TTest,        
        MannWhitneyU,
        Wilcoxon,
        KolmogorovSmirnov
    }

    public enum NormalityTests
    {
        JacqueBera
    }
}
