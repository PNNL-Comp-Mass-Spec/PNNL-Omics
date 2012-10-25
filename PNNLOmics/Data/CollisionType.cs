using System;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Type of MS/MS fragmentation techniques.
    /// </summary>
    public enum CollisionType
    {
        HID,
        CID,
        ETD,
        ECD,
        Other,
        None
    }
}