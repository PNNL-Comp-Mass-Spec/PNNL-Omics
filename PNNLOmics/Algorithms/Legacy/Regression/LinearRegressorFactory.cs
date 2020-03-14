namespace PNNLOmics.Algorithms.Regression
{
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Regression")]
    public static class LinearRegressorFactory
    {
        public static IRegressorAlgorithm<LinearRegressionResult> Create(RegressionType type)
        {
            switch (type)
            {
                case RegressionType.LinearEm:
                    return new LinearEmModel();
                case RegressionType.MixtureRegression:
                    return new MixtureModelEm();
                default:
                    return null;
            }
        }
    }
}
