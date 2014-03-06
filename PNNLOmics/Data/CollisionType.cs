using System;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Type of MS/MS fragmentation techniques.
    /// </summary>
    public enum CollisionType
    {
        CID = 0,
        ECD,
        ETD,
        HCD,
        HID,        
        None,
        Other        
    }
}