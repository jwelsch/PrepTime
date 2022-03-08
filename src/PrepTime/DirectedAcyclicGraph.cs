using System;
using System.Collections.Generic;

namespace PrepTime
{
   /// <summary>
   /// Represents a directed acyclic graph.
   /// </summary>
   internal class DirectedAcyclicGraph
   {
      /// <summary>
      /// The types of visits that can happen to a node.
      /// </summary>
      private enum VisitType
      {
         /// <summary>
         /// Not visited yet.
         /// </summary>
         None,

         /// <summary>
         /// Temporary visit.
         /// </summary>
         Temporary,

         /// <summary>
         /// Permanent visit.
         /// </summary>
         Permanent
      }

      /// <summary>
      /// Tracks visits of nodes.
      /// </summary>
      private Dictionary<int, VisitType> visits = null;

      /// <summary>
      /// Contains the sorted entities.
      /// </summary>
      private List<PrepGraph.Node> sorted = null;

      /// <summary>
      /// Performs a topological sort on the given graph.
      /// </summary>
      /// <param name="graph">Graph to sort.</param>
      /// <returns>Array of entity IDs that have been topologically sorted.</returns>
      public int[] TopologicalSort( ICollection<PrepGraph.Node> graph )
      {
         int[] sortedList = null;

         if ( !this.TryTopologicalSort( graph, out sortedList ) )
         {
            throw new CircularDependencyException();
         }

         return sortedList;
      }

      /// <summary>
      /// Performs a topological sort on the given graph.
      /// </summary>
      /// <param name="graph">Graph to sort.</param>
      /// <param name="sortedEntityIDs">List of topologically sorted entity IDs.</param>
      /// <returns>True if the sort was successful, false otherwise.</returns>
      public bool TryTopologicalSort( ICollection<PrepGraph.Node> graph, out int[] sortedEntityIDs )
      {
         this.visits = new Dictionary<int, VisitType>();
         this.sorted = new List<PrepGraph.Node>();
         var permanentVisits = 0;

         sortedEntityIDs = null;

         while ( graph.Count > permanentVisits )
         {
            foreach ( var node in graph )
            {
               if ( this.visits.ContainsKey( node.ID ) )
               {
                  continue;
               }

               if ( !this.Visit( node ) )
               {
                  return false;
               }
            }

            foreach ( var visit in this.visits.Values )
            {
               permanentVisits += ( visit == VisitType.Permanent ) ? 1 : 0;
            }
         }

         var sortedNodes = this.sorted.ToArray();
         sortedEntityIDs = new int[sortedNodes.Length];

         for ( var i = 0; i < sortedNodes.Length; i++ )
         {
            sortedEntityIDs[i] = sortedNodes[i].ID;
         }

         this.visits = null;
         this.sorted = null;

         return true;
      }

      /// <summary>
      /// Visits each node.
      /// </summary>
      /// <param name="node">Node to visit.</param>
      /// <returns>True to continue, false otherwise.</returns>
      private bool Visit( PrepGraph.Node node )
      {
         var visited = VisitType.None;

         if ( this.visits.TryGetValue( node.ID, out visited ) )
         {
            if ( visited == VisitType.Temporary )
            {
               return false;
            }
         }
         else
         {
            this.visits.Add( node.ID, VisitType.Temporary );

            foreach ( var dependency in node.Dependencies.Values )
            {
               if ( !this.Visit( dependency ) )
               {
                  return false;
               }
            }

            this.visits[node.ID] = VisitType.Permanent;

            this.sorted.Add( node );
         }

         return true;
      }

      /// <summary>
      /// Finds groups of connected nodes in the given graph.
      /// </summary>
      /// <param name="graph">Graph containing nodes.</param>
      /// <returns>Arrays of nodes that are connected.</returns>
      public PrepGraph.Node[][] ConnectedGroups( ICollection<PrepGraph.Node> graph )
      {
         var groups = new List<Dictionary<int, PrepGraph.Node>>();

         foreach ( var vertice in graph )
         {
            var firstOrderGroup = vertice.FormGroup();

            var intersected = false;

            foreach ( var group in groups )
            {
               if ( this.DoGroupsIntersect( group.Values, firstOrderGroup ) )
               {
                  intersected = true;
                  this.AddFirstOrderGroup( group, firstOrderGroup );
               }
            }

            if ( !intersected )
            {
               var group = new Dictionary<int, PrepGraph.Node>();
               this.AddFirstOrderGroup( group, firstOrderGroup );
               groups.Add( group );
            }

            groups = this.ConsolidateGroups( groups );
         }

         var output = new PrepGraph.Node[groups.Count][];

         for ( var i = 0; i < groups.Count; i++ )
         {
            output[i] = new PrepGraph.Node[groups[i].Values.Count];

            var j = 0;
            foreach ( var node in groups[i].Values )
            {
               output[i][j++] = node;
            }
         }

         return output;
      }

      /// <summary>
      /// Consolidates the list of groups.  Two groups are consolidated if they share at least one node.
      /// </summary>
      /// <param name="groups">List of groups to consolidate.</param>
      /// <returns>Consolidated list of groups.</returns>
      private List<Dictionary<int, PrepGraph.Node>> ConsolidateGroups( List<Dictionary<int, PrepGraph.Node>> groups )
      {
         var intersection = false;

         if ( groups.Count == 0 )
         {
            return new List<Dictionary<int, PrepGraph.Node>>();
         }
         else if ( groups.Count == 1 )
         {
            return groups;
         }

         do
         {
            for ( var i = 0; i < groups.Count; i++ )
            {
               if ( i + 1 >= groups.Count )
               {
                  intersection = false;
                  break;
               }

               intersection = this.DoGroupsIntersect( groups[i].Values, groups[i + 1].Values );
               if ( intersection )
               {
                  foreach ( var node in groups[i + 1] )
                  {
                     if ( !groups[i].ContainsKey( node.Value.ID ) )
                     {
                        groups[i].Add( node.Value.ID, node.Value );
                     }
                  }

                  groups.RemoveAt( i + 1 );
                  break;
               }
            }
         }
         while ( intersection );

         return groups;
      }

      /// <summary>
      /// Adds a first order collection from a node to a group of nodes.
      /// </summary>
      /// <param name="group">Group to add to.</param>
      /// <param name="nodes">First order collection to be added.</param>
      private void AddFirstOrderGroup( Dictionary<int, PrepGraph.Node> group, IEnumerable<PrepGraph.Node> nodes )
      {
         foreach ( var node in nodes )
         {
            if ( !group.ContainsKey( node.ID ) )
            {
               group.Add( node.ID, node );
            }
         }
      }

      private bool DoGroupsIntersect( IEnumerable<PrepGraph.Node> group1, IEnumerable<PrepGraph.Node> group2 )
      {
         foreach ( var vkp1 in group1 )
         {
            foreach ( var vkp2 in group2 )
            {
               if ( vkp1.ID == vkp2.ID )
               {
                  return true;
               }
            }
         }

         return false;
      }
   }
}
