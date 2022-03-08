using System.Collections.Generic;

namespace PrepTime.Common.Graph
{
    public interface IVertex
    {
        IList<IVertex> Edges { get; }
    }

    public interface IVertex<T> : IVertex
    {
        T? Data { get; }
    }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Vertex<T> : IVertex<T>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public IList<IVertex> Edges { get; }

        public T? Data { get; }

        public Vertex(T? data, params IVertex[] edges)
            : this(data, new List<IVertex>(edges))
        {
        }

        public Vertex(T? data, IList<IVertex> edges)
        {
            Data = data;
            Edges = edges;
        }

        public Vertex(T? data)
            : this(data, new List<IVertex>())
        {
        }

        //public override bool Equals(object? obj)
        //{
        //    if (obj == null
        //        || obj is not IVertex<T> vertex
        //        || Edges.Count != vertex.Edges.Count)
        //    {
        //        return false;
        //    }

        //    foreach (var objEdge in Edges)
        //    {
        //        var match = false;

        //        foreach (var vertexEdge in vertex.Edges)
        //        {
        //            if (objEdge.Equals(vertexEdge))
        //            {
        //                match = true;
        //                break;
        //            }
        //        }

        //        if (!match)
        //        {
        //            return false;
        //        }
        //    }

        //    if (Data == null)
        //    {
        //        return vertex.Data == null;
        //    }

        //    return Data.Equals(vertex.Data);
        //}
    }
}
