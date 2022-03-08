using System.Collections.Generic;

namespace PrepTime.Common.Graph
{
    public interface IPrepGraphNode
    {
        int ID { get; }
        Dictionary<int, IPrepGraphNode> Dependencies { get; }
        int[] FirstOrderIDs();
        IPrepGraphNode[] FormGroup();
    }

    public class PrepGraphNode : IPrepGraphNode
    {
        /// <summary>
        /// Gets the ID of the entity that the node represents.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Gets the dependencies of the node.
        /// </summary>
        public Dictionary<int, IPrepGraphNode> Dependencies { get; } = new();

        ///<summary>
        /// Creates an object of type PrepGraphNode.
        /// </summary>
        /// <param name="id">ID of the entity that the node represents.</param>
        public PrepGraphNode(int id)
        {
            ID = id;
        }

        /// <summary>
        /// Returns an array of IDs that include the node itself and its immediate dependencies.
        /// </summary>
        /// <returns>Array of IDs.</returns>
        public int[] FirstOrderIDs()
        {
            var ids = new int[Dependencies.Count + 1];

            ids[0] = ID;

            var i = 1;
            foreach (var dependency in Dependencies)
            {
                ids[i++] = dependency.Value.ID;
            }

            return ids;
        }

        /// <summary>
        /// Forms a group out of the node and its dependency.
        /// </summary>
        /// <returns>Array of nodes that form a group.</returns>
        public IPrepGraphNode[] FormGroup()
        {
            var group = new IPrepGraphNode[Dependencies.Count + 1];

            group[0] = this;

            var i = 1;
            foreach (var dependency in Dependencies)
            {
                group[i++] = dependency.Value;
            }

            return group;
        }
    }
}
