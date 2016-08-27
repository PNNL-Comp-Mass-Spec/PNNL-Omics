namespace PNNLOmics.Algorithms.Statistics
{
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Statistics")]
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
            }

            return newTest;
        }
    }
}