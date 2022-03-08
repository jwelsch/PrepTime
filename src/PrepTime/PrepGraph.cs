using System;
using System.Collections.Generic;
using PrepTime.Properties;
using Newtonsoft.Json;

namespace PrepTime
{
   /// <summary>
   /// Directed acyclic graph that shows the order of steps necessary to complete the preparation.
   /// </summary>
   public class PrepGraph
   {
      #region Node

      /// <summary>
      /// A node in the graph.
      /// </summary>
      public class Node
      {
         /// <summary>
         /// Gets the ID of the entity that the node represents.
         /// </summary>
         public int ID
         {
            get;
            private set;
         }

         /// <summary>
         /// Gets the dependencies of the node.
         /// </summary>
         public Dictionary<int, Node> Dependencies
         {
            get;
            private set;
         }

         ///<summary>
         /// Creates an object of type Node.
         /// </summary>
         /// <param name="id">ID of the entity that the node represents.</param>
         public Node( int id )
         {
            this.Dependencies = new Dictionary<int, Node>();
            this.ID = id;
         }

         /// <summary>
         /// Returns an array of IDs that include the node itself and its immediate dependencies.
         /// </summary>
         /// <returns>Array of IDs.</returns>
         public int[] FirstOrderIDs()
         {
            var ids = new int[this.Dependencies.Count + 1];

            ids[0] = this.ID;

            var i = 1;
            foreach ( var dependency in this.Dependencies )
            {
               ids[i++] = dependency.Value.ID;
            }

            return ids;
         }

         /// <summary>
         /// Forms a group out of the node and its dependency.
         /// </summary>
         /// <returns>Array of nodes that form a group.</returns>
         public Node[] FormGroup()
         {
            var group = new Node[this.Dependencies.Count + 1];

            group[0] = this;

            var i = 1;
            foreach ( var dependency in this.Dependencies )
            {
               group[i++] = dependency.Value;
            }

            return group;
         }
      }

      #endregion

      /// <summary>
      /// Gets all nodes in the graph.
      /// </summary>
      public Dictionary<int, Node> Nodes
      {
         get;
         private set;
      }

      private int[] sortedEntityIDs;
      /// <summary>
      /// Gets the topologically sorted entity IDs.
      /// </summary>
      [JsonIgnore]
      public int[] SortedEntityIDs
      {
         get { return this.sortedEntityIDs; }
      }

      ///<summary>
      /// Creates an object of type PrepGraph.
      /// </summary>
      public PrepGraph()
      {
         this.Nodes = new Dictionary<int, Node>();
      }

      /// <summary>
      /// Creates an object of type PrepGraph.
      /// </summary>
      /// <param name="graph">Graph to copy from.</param>
      public PrepGraph( PrepGraph graph )
         : this()
      {
         foreach ( var node in graph.Nodes.Values )
         {
            this.AddNode( node.ID );
         }

         foreach ( var node in this.Nodes.Values )
         {
            var dependencies = graph.Nodes[node.ID].Dependencies;

            foreach ( var dependency in dependencies )
            {
               node.Dependencies.Add( dependency.Key, dependency.Value );
            }
         }
      }

      /// <summary>
      /// Finds the node with the specified ID.
      /// </summary>
      /// <param name="entityID">ID of the node to find.</param>
      /// <returns>Node with matching ID.</returns>
      private Node FindNode( int entityID )
      {
         Node node = null;

         if ( !this.Nodes.TryGetValue( entityID, out node ) )
         {
            throw new ArgumentException( Resources.ErrorNoMatchingEntityID, "entityID" );
         }

         return node;
      }

      /// <summary>
      /// Adds an entity to the graph.
      /// </summary>
      /// <param name="id">ID of entity to add.</param>
      public void AddNode( int id )
      {
         this.AddNode( id, new int[0] );
      }

      /// <summary>
      /// Adds an entity to the graph.
      /// </summary>
      /// <param name="id">ID of entity to add.</param>
      /// <param name="dependencies">The IDs of entity dependencies.</param>
      public void AddNode( int id, IEnumerable<int> dependencies )
      {
         var node = new Node( id );

         foreach ( var dependency in dependencies )
         {
            if ( !this.Nodes.ContainsKey( dependency ) )
            {
               throw new DependencyDoesNotExistException( String.Format( Resources.ErrorDependencyDoesNotExist, dependency.GetType() ) );
            }

            var entry = this.Nodes[dependency];
            node.Dependencies.Add( dependency, new Node( dependency ) );
         }

         this.Nodes.Add( node.ID, node );

         try
         {
            this.sortedEntityIDs = this.TopologicalSort();
         }
         catch ( CircularDependencyException )
         {
            this.Nodes.Remove( node.ID );

            throw;
         }
      }

      /// <summary>
      /// Removes an entity from the graph.
      /// </summary>
      /// <param name="entity">Entity to remove.</param>
      public void RemoveNode( IEntity entity )
      {
         this.Nodes.Remove( entity.ID );

         foreach ( var node in this.Nodes.Values )
         {
            if ( node.Dependencies.ContainsKey( entity.ID ) )
            {
               node.Dependencies.Remove( entity.ID );
            }
         }

         this.sortedEntityIDs = this.TopologicalSort();
      }

      /// <summary>
      /// Adds dependencies to a node.
      /// </summary>
      /// <param name="dependentID">ID of the the node to add dependencies to.</param>
      /// <param name="dependencies">Dependencies to add.</param>
      public void AddDependencies( int dependentID, IEnumerable<IEntity> dependencies )
      {
         if ( !this.TryAddDependencies( dependentID, dependencies ) )
         {
            throw new CircularDependencyException();
         }
      }

      /// <summary>
      /// Adds dependencies to a node.
      /// </summary>
      /// <param name="dependentID">ID of the the node to add dependencies to.</param>
      /// <param name="dependencyIDs">IDs of dependencies to add.</param>
      public void AddDependencies( int dependentID, IEnumerable<int> dependencyIDs )
      {
         if ( !this.TryAddDependencies( dependentID, dependencyIDs ) )
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
      public bool TryAddDependencies( int dependentID, IEnumerable<IEntity> dependencies )
      {
         var ids = new List<int>();

         foreach ( var dependency in dependencies )
         {
            ids.Add( dependency.ID );
         }

         return this.TryAddDependencies( dependentID, ids );
      }

      /// <summary>
      /// Adds dependencies to a node.
      /// </summary>
      /// <param name="dependentID">ID of the the node to add dependencies to.</param>
      /// <param name="dependencyIDs">IDs of dependencies to add.</param>
      /// <returns>True if the dependency was added, false otherwise.</returns>
      public bool TryAddDependencies( int dependentID, IEnumerable<int> dependencyIDs )
      {
         var dependentNode = this.FindNode( dependentID );

         foreach ( var dependencyID in dependencyIDs )
         {
            var independentNode = this.FindNode( dependencyID );

            dependentNode.Dependencies.Add( independentNode.ID, independentNode );
         }

         if ( !this.TryTopologicalSort( out this.sortedEntityIDs ) )
         {
            this.RemoveDependencies( dependentID, dependencyIDs );

            return false;
         }

         return true;
      }

      /// <summary>
      /// Removes dependencies from a node.
      /// </summary>
      /// <param name="dependentID">ID of the the node to remove dependencies from.</param>
      /// <param name="dependencies">Dependencies to remove.</param>
      public void RemoveDependencies( int dependentID, IEnumerable<IEntity> dependencies )
      {
         var dependentNode = this.FindNode( dependentID );

         foreach ( var dependency in dependencies )
         {
            dependentNode.Dependencies.Remove( dependency.ID );
         }

         this.sortedEntityIDs = this.TopologicalSort();
      }

      /// <summary>
      /// Removes dependencies from a node.
      /// </summary>
      /// <param name="dependentID">ID of the the node to remove dependencies from.</param>
      /// <param name="dependencyIDs">IDs of dependencies to remove.</param>
      public void RemoveDependencies( int dependentID, IEnumerable<int> dependencyIDs )
      {
         var dependentNode = this.FindNode( dependentID );

         foreach ( var dependencyID in dependencyIDs )
         {
            dependentNode.Dependencies.Remove( dependencyID );
         }

         this.sortedEntityIDs = this.TopologicalSort();
      }

      /// <summary>
      /// Performs a topological sort on the nodes.
      /// </summary>
      /// <param name="sortedList">A array of topologically sorted entity IDs.</param>
      /// <returns>True if the sort was successful, false otherwise.</returns>
      public bool TryTopologicalSort( out int[] sortedList )
      {
         var dag = new DirectedAcyclicGraph();
         return dag.TryTopologicalSort( this.Nodes.Values, out sortedList );
      }

      /// <summary>
      /// Performs a topological sort on the nodes.
      /// </summary>
      /// <returns>A array of topologically sorted entity IDs.</returns>
      public int[] TopologicalSort()
      {
         var dag = new DirectedAcyclicGraph();
         int[] sortedList = null;

         if ( !dag.TryTopologicalSort( this.Nodes.Values, out sortedList ) )
         {
            throw new CircularDependencyException();
         }

         return sortedList;
      }

      /// <summary>
      /// Topologically sorts groups of nodes disconnected.
      /// </summary>
      /// <returns>Groups of topologically sorted entity IDs.</returns>
      public int[][] GroupedTopologicalSort()
      {
         //System.Diagnostics.Trace.WriteLine( String.Format( "Before grouping" ) );
         //PrepGraph.DebugDump( this.Nodes.Values );

         var dag = new DirectedAcyclicGraph();
         var groups = dag.ConnectedGroups( this.Nodes.Values );

         var sortedGroups = new int[groups.Length][];

         var i = 0;
         foreach ( var group in groups )
         {
            //System.Diagnostics.Trace.WriteLine( String.Format( "After grouping" ) );
            //PrepGraph.DebugDump( group );

            sortedGroups[i++] = dag.TopologicalSort( group );
         }

         return sortedGroups;
      }

      /// <summary>
      /// Gets the dependencies of the given entity.
      /// </summary>
      /// <param name="entityID">ID of the entity.</param>
      /// <returns>Array of dependency IDs.</returns>
      public int[] GetDependencies( int entityID )
      {
         var node = this.FindNode( entityID );

         var entities = new int[node.Dependencies.Count];

         var index = 0;
         foreach ( var dependency in node.Dependencies.Values )
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
         PrepGraph.DebugDump( this.Nodes.Values );
      }

      /// <summary>
      /// Dumps debug information about the graph.
      /// </summary>
      public static void DebugDump( IEnumerable<PrepGraph.Node> graph )
      {
         foreach ( var node in graph )
         {
            System.Diagnostics.Trace.WriteLine( String.Format( "  Entity ID: {0}", node.ID ) );

            foreach ( var dependency in node.Dependencies )
            {
               System.Diagnostics.Trace.WriteLine( String.Format( "    Dependency ID: {0}", dependency.Value.ID ) );
            }
         }
      }

      /// <summary>
      /// Gets the IDs of the entities that are dependent on the given entity.
      /// </summary>
      /// <param name="entityID">ID that entities are dependent upon.</param>
      /// <returns>Dependent IDs.</returns>
      public int[] GetDependents( int entityID )
      {
         var dependents = new List<int>();

         foreach ( var node in this.Nodes )
         {
            if ( entityID == node.Value.ID )
            {
               continue;
            }

            foreach ( var dependency in node.Value.Dependencies )
            {
               if ( dependency.Value.ID == entityID )
               {
                  if ( !dependents.Contains( dependency.Value.ID ) )
                  {
                     dependents.Add( node.Value.ID );
                  }
               }
            }
         }

         return dependents.ToArray();
      }
   }
}
