using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{


    /// <summary>
    /// Encapsulates an Edge between two vertices.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Edge<T> : IComparable<Edge<T>>
        where T : FeatureLight, new()
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="distance"></param>
        public Edge(int id, double length, T featureA, T featureB)
        {
            ID = id;
            Length = length;
            VertexA = featureA;
            VertexB = featureB;
        }
        /// <summary>
        /// Gets the ID of the edge.
        /// </summary>
        public int ID
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the length of the edge.
        /// </summary>
        public double Length
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets a vertex of the edge.
        /// </summary>
        public T VertexA
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets a vertex of the edge.
        /// </summary>
        public T VertexB
        {
            get;
            private set;
        }
        public override string ToString()
        {

            return string.Format("{0},{1},{2},{3},{4},{5}", ID,
                                                                        VertexA.ID,
                                                                        VertexA.GroupID,
                                                                        VertexB.ID,
                                                                        VertexB.GroupID,
                                                                        Length);
        }

        #region IComparable<Edge<T>> Members
        /// <summary>
        /// Compares the other's edge length to this.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Edge<T> other)
        {
            return Length.CompareTo(other.Length);
        }

        #endregion
    }
}
