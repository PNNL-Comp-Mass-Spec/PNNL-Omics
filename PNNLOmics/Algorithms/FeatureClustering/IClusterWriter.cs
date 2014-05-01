namespace PNNLOmics.Algorithms.FeatureClustering
{
    public interface IClusterWriter<T>
    {
        void WriteCluster(T cluster);
        void Close();
    }
}
