using System.Collections.Generic;
using System.Linq;

namespace PrepTime.Common.Graph
{
    public interface IVisitMap
    {
        bool HasNotPermanent();

        bool FindFirstUnmarked(out IVisitRecord? visitRecord);

        bool FindVisitRecord(IVertex vertex, out IVisitRecord? visitRecord);
    }

    public class VisitMap : IVisitMap
    {
        private readonly List<IVisitRecord> _map;

        public VisitMap(IEnumerable<IVertex> vertices)
        {
            _map = vertices.Select(i => (IVisitRecord)new VisitRecord(i)).ToList();
        }

        public bool HasNotPermanent()
        {
            return _map.Any(i => !i.IsPermanent);
        }

        public bool FindFirstUnmarked(out IVisitRecord? visitRecord)
        {
            visitRecord = _map.FirstOrDefault(i => i.IsUnmarked);

            return visitRecord != null;
        }

        public bool FindVisitRecord(IVertex vertex, out IVisitRecord? visitRecord)
        {
            visitRecord = _map.FirstOrDefault(i => i.Vertex.Equals(vertex));

            return visitRecord != null;
        }
    }
}
