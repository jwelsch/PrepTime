namespace PrepTime.Common.Graph
{
    public interface IVisitRecord
    {
        IVertex Vertex { get; }

        bool IsUnmarked { get; }

        bool IsTemporary { get; }

        bool IsPermanent { get; }

        void MarkTemporary();

        void MarkPermanent();
    }

    public class VisitRecord : IVisitRecord
    {
        /// <summary>
        /// null  => unvisited
        /// false => temporary
        /// true  => permanent
        /// </summary>
        private bool? _mark = null;

        public IVertex Vertex { get; }

        public bool IsUnmarked => !_mark.HasValue;

        public bool IsTemporary => _mark.HasValue && !_mark.Value;

        public bool IsPermanent => _mark.HasValue && _mark.Value;

        public void MarkTemporary()
        {
            _mark = false;
        }

        public void MarkPermanent()
        {
            _mark = true;
        }

        public VisitRecord(IVertex vertex)
        {
            Vertex = vertex;
        }
    }
}
