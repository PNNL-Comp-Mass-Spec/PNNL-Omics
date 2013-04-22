using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Extensions
{
    public static class XYDataExtensions
    {
        /// <summary>
        /// Converts a list of XYZ data points to XY.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<XYData> ToXYData(this List<XYZData> data)
        {
            List<XYData> results = new List<XYData>();
            foreach (XYZData point in data)
            {
                results.Add(new XYData(point.X, point.Y));
            }
            return results;
        }
        
        /// <summary>
        /// Converts an XY Data List into a dictionary mapping the scan to the intensity. (X, Y)
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static Dictionary<int, long> CreateScanMap(this List<XYData> profile)
        {
            Dictionary<int, long> profileScanMap = new Dictionary<int, long>();

            // Converts the point structure into a map.
            foreach (XYData point in profile)
            {
                int scan        = Convert.ToInt32(point.X);
                long intensity  = Convert.ToInt64(point.Y);

                // Takes the max intensity for duplicate scans...
                if (profileScanMap.ContainsKey(scan))
                {
                    profileScanMap[scan] = Math.Max(intensity, profileScanMap[scan]);
                    continue;
                }
                profileScanMap.Add(scan, intensity);
            }

            return profileScanMap;
        }
    }
}
