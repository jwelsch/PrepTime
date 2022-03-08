using System;
using System.Collections.Generic;
using PrepTime.Common.Models;

namespace PrepTime.Common.Graph
{
    public interface IPrepGraph
    {
        int[] SortedEntityIDs { get; }
        Dictionary<int, IPrepGraphNode> Nodes { get; }
        void AddDependencies(int dependentID, IEnumerable<IEntity> dependencies);
        void AddDependencies(int dependentID, IEnumerable<int> dependencyIDs);
        void AddNode(int id);
        void AddNode(int id, IEnumerable<int> dependencies);
        void DebugDump();
        int[] GetDependencies(int entityID);
        int[] GetDependents(int entityID);
        int[][] GroupedTopologicalSort();
        void RemoveDependencies(int dependentID, IEnumerable<IEntity> dependencies);
        void RemoveDependencies(int dependentID, IEnumerable<int> dependencyIDs);
        void RemoveNode(IEntity entity);
        int[] TopologicalSort();
        bool TryAddDependencies(int dependentID, IEnumerable<IEntity> dependencies);
        bool TryAddDependencies(int dependentID, IEnumerable<int> dependencyIDs);
        bool TryTopologicalSort(out int[] sortedList);
    }

    /// <summary>
    /// Directed acyclic graph that shows the order of steps necessary to complete the preparation.
    /// </summary>
    public class PrepGraph : IPrepGraph
    {
        /// <summary>
        /// Gets the topologically sorted entity IDs.
        /// </summary>
        public int[] SortedEntityIDs { get; private set; } = Array.Empty<int>();

        /// <summary>
        /// Gets all nodes in the graph.
        /// </summary>
        public Dictionary<int, IPrepGraphNode> Nodes { get; } = new();

        ///<summary>
        /// Creates an object of type PrepGraph.
        /// </summary>
        public PrepGraph()
        {
        }

        /// <summary>
        /// Creates an object of type PrepGraph.
        /// </summary>
        /// <param name="graph">Graph to copy from.</param>
        public PrepGraph(IPrepGraph graph)
        {
            foreach (var node in graph.Nodes.Values)
            {
                AddNode(node.ID);
            }

            foreach (var node in Nodes.Values)
            {
                var dependencies = graph.Nodes[node.ID].Dependencies;

                foreach (var dependency in dependencies)
                {
                    node.Dependencies.Add(dependency.Key, dependency.Value);
                }
            }
        }

        /// <summary>
        /// Finds the node with the specified ID.
        /// </summary>
        /// <param name="entityID">ID of the node to find.</param>
        /// <returns>Node with matching ID.</returns>
        private IPrepGraphNode FindNode(int entityID)
        {
            return Nodes.TryGetValue(entityID, out var node)
                    ? node
                    : throw new ArgumentException(Resources.ErrorNoMatchingEntityID, nameof(entityID));
        }

        /// <summary>
        /// Adds an entity to the graph.
        /// </summary>
        /// <param name="id">ID of entity to add.</param>
        public void AddNode(int id)
        {
            AddNode(id, new int[0]);
        }

        /// <summary>
        /// Adds an entity to the graph.
        /// </summary>
        /// <param name="id">ID of entity to add.</param>
        /// <param name="dependencies">The IDs of entity dependencies.</param>
        public void AddNode(int id, IEnumerable<int> dependencies)
        {
            var node = new PrepGraphNode(id);

            foreach (var dependency in dependencies)
            {
                if (!Nodes.ContainsKey(dependency))
                {
                    throw new DependencyDoesNotExistException(string.Format(Resources.ErrorDependencyDoesNotExist, dependency.GetType()));
                }

                //var entry = Nodes[dependency];
                node.Dependencies.Add(dependency, new PrepGraphNode(dependency));
            }

            Nodes.Add(node.ID, node);

            try
            {
                SortedEntityIDs = TopologicalSort();
            }
            catch (CircularDependencyException)
            {
                Nodes.Remove(node.ID);

                throw;
            }
        }

        /// <summary>
        /// Removes an entity from the graph.
        /// </summary>
        /// <param name="entity">Entity to remove.</param>
        public void RemoveNode(IEntity entity)
        {
            Nodes.Remove(entity.ID);

            foreach (var node in Nodes.Values)
            {
                if (node.Dependencies.ContainsKey(entity.ID))
                {
                    node.Dependencies.Remove(entity.ID);
                }
            }

            SortedEntityIDs = TopologicalSort();
        }

        /// <summary>
        /// Adds dependencies to a node.
        /// </summary>
        /// <param name="dependentID">ID of the the node to add dependencies to.</param>
        /// <param name="dependencies">Dependencies to add.</param>
        public void AddDependencies(int dependentID, IEnumerable<IEntity> dependencies)
        {
            if (!TryAddDependencies(dependentID, dependencies))
            {
                throw new CircularDependencyException();
            }
        }

        /// <summary>
        /// Adds dependencies to a node.
        /// </summary>
        /// <param name="dependentID">ID of the the node to add dependencies to.</param>
        /// <param name="dependencyIDs">IDs of dependencies to add.</param>
        public void AddDependencies(int dependentID, IEnumerable<int> dependencyIDs)
        {
            if (!TryAddDependencies(dependentID, dependencyIDs))
            {
                throw new CircularDependencyException();
            }
        }

        /// <summary>
        /// Adds dependencies to a node.
        /// </summary>
        /// <param name="dependentID">ID of the the node to add dependencies to.</param>
        /// <param name="dependencies">Dependencies to add.</param>
        /// <returns>True if the dependency was added, false otherwise.</returns>
        public bool TryAddDependencies(int dependentID, IEnumerable<IEntity> dependencies)
        {
            var ids = new List<int>();

            foreach (var dependency in dependencies)
            {
                ids.Add(dependency.ID);
            }

            return TryAddDependencies(dependentID, ids);
        }

        /// <summary>
        /// Adds dependencies to a node.
        /// </summary>
        /// <param name="dependentID">ID of the the node to add dependencies to.</param>
        /// <param name="dependencyIDs">IDs of dependencies to add.</param>
        /// <returns>True if the dependency was added, false otherwise.</returns>
        public bool TryAddDependencies(int dependentID, IEnumerable<int> dependencyIDs)
        {
            var dependentNode = FindNode(dependentID);

            foreach (var dependencyID in dependencyIDs)
            {
                var independentNode = FindNode(dependencyID);

                dependentNode.Dependencies.Add(independentNode.ID, independentNode);
            }

            var success = TryTopologicalSort(out var sortedEntityIDs);
            SortedEntityIDs = sortedEntityIDs;

            if (!success)
            {
                RemoveDependencies(dependentID, dependencyIDs);
            }

            return success;
        }

        /// <summary>
        /// Removes dependencies from a node.
        /// </summary>
        /// <param name="dependentID">ID of the the node to remove dependencies from.</param>
        /// <param name="dependencies">Dependencies to remove.</param>
        public void RemoveDependencies(int dependentID, IEnumerable<IEntity> dependencies)
        {
            var dependentNode = FindNode(dependentID);

            foreach (var dependency in dependencies)
            {
                dependentNode.Dependencies.Remove(dependency.ID);
            }

            SortedEntityIDs = TopologicalSort();
        }

        /// <summary>
        /// Removes dependencies from a node.
        /// </summary>
        /// <param name="dependentID">ID of the the node to remove dependencies from.</param>
        /// <param name="dependencyIDs">IDs of dependencies to remove.</param>
        public void RemoveDependencies(int dependentID, IEnumerable<int> dependencyIDs)
        {
            var dependentNode = FindNode(dependentID);

            foreach (var dependencyID in dependencyIDs)
            {
                dependentNode.Dependencies.Remove(dependencyID);
            }

            SortedEntityIDs = TopologicalSort();
        }

        /// <summary>
        /// Performs a topological sort on the nodes.
        /// </summary>
        /// <param name="sortedList">A array of topologically sorted entity IDs.</param>
        /// <returns>True if the sort was successful, false otherwise.</returns>
        public bool TryTopologicalSort(out int[] sortedList)
        {
            //var dag = new DirectedAcyclicGraph();
            //return dag.TryTopologicalSort(Nodes.Values, out sortedList);
            sortedList = new int[0];
            return true;
        }

        /// <summary>
        /// Performs a topological sort on the nodes.
        /// </summary>
        /// <returns>A array of topologically sorted entity IDs.</returns>
        public int[] TopologicalSort()
        {
            //var dag = new DirectedAcyclicGraph();

            //if (!dag.TryTopologicalSort(Nodes.Values, out var sortedList))
            //{
            //    throw new CircularDependencyException();
            //}

            //return sortedList;
            return new int[0];
        }

        /// <summary>
        /// Topologically sorts groups of nodes disconnected.
        /// </summary>
        /// <returns>Groups of topologically sorted entity IDs.</returns>
        public int[][] GroupedTopologicalSort()
        {
            //System.Diagnostics.Trace.WriteLine( string.Format( "Before grouping" ) );
            //PrepGraph.DebugDump( Nodes.Values );

            //var dag = new DirectedAcyclicGraph();
            //var groups = dag.ConnectedGroups(Nodes.Values);

            //var sortedGroups = new int[groups.Length][];

            //var i = 0;
            //foreach (var group in groups)
            //{
            //    //System.Diagnostics.Trace.WriteLine( string.Format( "After grouping" ) );
            //    //PrepGraph.DebugDump( group );

            //    sortedGroups[i++] = dag.TopologicalSort(group);
            //}

            //return sortedGroups;

            return new int[0][];
        }

        /// <summary>
        /// Gets the dependencies of the given entity.
        /// </summary>
        /// <param name="entityID">ID of the entity.</param>
        /// <returns>Array of dependency IDs.</returns>
        public int[] GetDependencies(int entityID)
        {
            var node = FindNode(entityID);

            var entities = new int[node.Dependencies.Count];

            var index = 0;
            foreach (var dependency in node.Dependencies.Values)
            {
                entities[index++] = dependency.ID;
            }

            return entities;
        }

        /// <summary>
        /// Dumps debug information about the graph.
        /// </summary>
        public void DebugDump()
        {
            DebugDump(Nodes.Values);
        }

        /// <summary>
        /// Dumps debug information about the graph.
        /// </summary>
        public static void DebugDump(IEnumerable<IPrepGraphNode> graph)
        {
            foreach (var node in graph)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("  Entity ID: {0}", node.ID));

                foreach (var dependency in node.Dependencies)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("    Dependency ID: {0}", dependency.Value.ID));
                }
            }
        }

        /// <summary>
        /// Gets the IDs of the entities that are dependent on the given entity.
        /// </summary>
        /// <param name="entityID">ID that entities are dependent upon.</param>
        /// <returns>Dependent IDs.</returns>
        public int[] GetDependents(int entityID)
        {
            var dependents = new List<int>();

            foreach (var node in Nodes)
            {
                if (entityID == node.Value.ID)
                {
                    continue;
                }

                foreach (var dependency in node.Value.Dependencies)
                {
                    if (dependency.Value.ID == entityID)
                    {
                        if (!dependents.Contains(dependency.Value.ID))
                        {
                            dependents.Add(node.Value.ID);
                        }
                    }
                }
            }

            return dependents.ToArray();
        }
    }
}
