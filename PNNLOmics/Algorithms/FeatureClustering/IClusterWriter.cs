namespace PNNLOmics.Algorithms.FeatureClustering
{
	[System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Clustering")]
    public interface IClusterWriter<T>
    {
        void WriteCluster(T cluster);
        void Close();
    }
}
