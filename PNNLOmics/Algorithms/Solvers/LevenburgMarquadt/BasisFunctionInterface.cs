namespace PNNLOmics.Algorithms.Solvers.LevenburgMarquadt
{
    /// <summary>
    /// interface for strategy design pattern
    /// </summary>
    public interface IBasisFunctionInterface
    {
        void FunctionDelegate(double[] c, double[] x, ref double functionResult, object obj);
    }
}
