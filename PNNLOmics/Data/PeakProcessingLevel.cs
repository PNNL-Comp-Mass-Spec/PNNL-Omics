namespace PNNLOmics.Data
{
    /// <summary>
    /// Type of Peak Processing applied to the corresponding peak list.
    /// </summary>
    public enum PeakProcessingLevel
    {
        None,
        BackgroundSubtracted,
        Centroided,
        Thresholded,
        Deisotoped,
        Filtered,
        Other
    }
}
