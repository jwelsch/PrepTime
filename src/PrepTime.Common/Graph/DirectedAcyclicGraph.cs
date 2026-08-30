using System;
using System.Collections.Generic;

namespace PrepTime.Common.Graph
{
    public interface IDirectedAcyclicGraph
    {
        IList<IVertex> SortedVertices { get; }

        bool TryAddVertex(IVertex vertex);

        void AddVertex(IVertex vertex);

        bool RemoveVertex(IVertex vertex);
    }

    /// <summary>
    /// Represents a directed acyclic graph.
    /// </summary>
    public class DirectedAcyclicGraph : IDirectedAcyclicGraph
    {
        private readonly ITopologicalSorter _topologicalSorter;

        private IList<IVertex> _vertices = new List<IVertex>();

        public IList<IVertex> SortedVertices => _vertices;

        public DirectedAcyclicGraph(ITopologicalSorter topologicalSorter)
        {
            _topologicalSorter = topologicalSorter;
        }

        public bool TryAddVertex(IVertex vertex)
        {
            if (_vertices.Contains(vertex))
            {
                return false;
            }

            _vertices.Add(vertex);

            if (!_topologicalSorter.TryDepthFirst(ref _vertices))
            {
                _vertices.Remove(vertex);
                return false;
            }

            return true;
        }

        public void AddVertex(IVertex vertex)
        {
            if (!TryAddVertex(vertex))
            {
                throw new InvalidOperationException($"Adding vertex would create a circular dependency.");
            }
        }

        public bool RemoveVertex(IVertex vertex)
        {
            return _vertices.Remove(vertex);
        }
    }
}
