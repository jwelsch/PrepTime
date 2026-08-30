using System;
using System.Collections.Generic;

namespace PrepTime.Common.Graph
{
    public interface ITopologicalSorter
    {
        bool TryDepthFirst(ref IList<IVertex> vertices);

        bool TryDepthFirst(IList<IVertex> unsortedVertices, out IList<IVertex> sortedVertices);
    }

    public class TopologicalSorter : ITopologicalSorter
    {
        public bool TryDepthFirst(ref IList<IVertex> vertices)
        {
            var result = TryDepthFirst(vertices, out IList<IVertex> sortedVertices);

            if (result)
            {
                vertices = sortedVertices;
            }

            return result;
        }

        public bool TryDepthFirst(IList<IVertex> unsortedVertices, out IList<IVertex> sortedVertices)
        {
            //
            // Depth-first search method
            // https://en.wikipedia.org/wiki/Topological_sorting
            //

            sortedVertices = new List<IVertex>();

            if (unsortedVertices.Count == 0)
            {
                return false;
            }

            var visitMap = new VisitMap(unsortedVertices);

            while (visitMap.HasNotPermanent()
                   && visitMap.FindFirstUnmarked(out IVisitRecord? visitRecord))
            {
                if (visitRecord == null)
                {
                    throw new InvalidOperationException($"Found unmarked visit record, but it was null.");
                }

                if (!Visit(visitMap, sortedVertices, visitRecord))
                {
                    return false;
                }
            }

            return true;
        }

        private bool Visit(IVisitMap visitMap, IList<IVertex> sortedVertices, IVisitRecord visitRecord)
        {
            if (visitRecord.IsPermanent)
            {
                return true;
            }

            if (visitRecord.IsTemporary)
            {
                // Clear to signal circular dependency to caller.
                sortedVertices.Clear();
                return false;
            }

            visitRecord.MarkTemporary();

            for (var i = 0; i < visitRecord.Vertex.Edges.Count; i++)
            {
                var found = visitMap.FindVisitRecord(visitRecord.Vertex.Edges[i], out IVisitRecord? edgeVisitRecord);

                if (!found || edgeVisitRecord == null)
                {
                    throw new InvalidOperationException($"Error attempting to find vertex edge visit record - found: {found}; edgeVisitRecord: {(edgeVisitRecord == null ? "<null>" : "<not null>")}");
                }

                if (!Visit(visitMap, sortedVertices, edgeVisitRecord))
                {
                    if (sortedVertices.Count == 0)
                    {
                        // Return the vertex with the circular dependency.
                        sortedVertices.Add(visitRecord.Vertex);
                    }
                    return false;
                }
            }

            visitRecord.MarkPermanent();

            sortedVertices.Insert(0, visitRecord.Vertex);

            return true;
        }
    }
}
