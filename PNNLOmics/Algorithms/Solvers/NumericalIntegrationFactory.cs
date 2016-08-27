namespace PNNLOmics.Algorithms.Solvers
{
    /// <summary>
    /// Factory for creating numerical integration objects.
    /// </summary>
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Solvers.LevenburgMarquadt")]
    public class NumericalIntegrationFactory
    {
        /// Creates a numerical integration object based on the specified function.
        /// </summary>
        /// <param name="functionChoice"></param>
        /// <returns></returns>
        public static NumericalIntegrationBase CreateIntegrator(NumericalIntegrationEnum functionChoice)
        {
            //default
            NumericalIntegrationBase solver = null;
            
            switch (functionChoice)
            {
                case NumericalIntegrationEnum.Trapezoidal:
                    solver = new TrapezoidIntegration();
                    break;   
                default:
                    solver = null;
                    break;
            }
            return solver;
        }
    }

    
}
