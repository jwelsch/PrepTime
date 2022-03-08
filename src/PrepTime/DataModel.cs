using System;

namespace PrepTime
{
   /// <summary>
   /// Data model.
   /// </summary>
   public class DataModel
   {
      /// <summary>
      /// Gets the version of the data model.
      /// </summary>
      public Version Version
      {
         get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; }
      }

      /// <summary>
      /// Gets the begin time.
      /// </summary>
      public DateTime BeginTime
      {
         get;
         set;
      }

      /// <summary>
      /// Gets the end time.
      /// </summary>
      public DateTime EndTime
      {
         get;
         set;
      }

      /// <summary>
      /// Gets the dependency graph.
      /// </summary>
      public PrepGraph DependencyGraph
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets the collection dishes.
      /// </summary>
      public DishCollection Dishes
      {
         get;
         private set;
      }

      /// <summary>
      /// Creates an object of type DataModel.
      /// </summary>
      public DataModel()
      {
         this.EndTime = DateTime.Now;
         this.BeginTime = this.EndTime;
         this.DependencyGraph = new PrepGraph();
         this.Dishes = new DishCollection();
      }

      public void DebugDump()
      {
         System.Diagnostics.Trace.WriteLine( String.Format( "BeginTime: {0}", this.BeginTime.ToString( "hh:mm tt MMM dd, yyyy" ) ) );
         System.Diagnostics.Trace.WriteLine( String.Format( "EndTime: {0}", this.EndTime.ToString( "hh:mm tt MMM dd, yyyy" ) ) );

         System.Diagnostics.Trace.WriteLine( String.Format( "Dishes" ) );
         foreach ( var dish in this.Dishes )
         {
            System.Diagnostics.Trace.WriteLine( String.Format( "  ID: {0}", dish.ID ) );
            System.Diagnostics.Trace.WriteLine( String.Format( "  Name: {0}", dish.Name ) );
            System.Diagnostics.Trace.WriteLine( String.Format( "  Tasks" ) );

            foreach ( var task in dish.Tasks )
            {
               System.Diagnostics.Trace.WriteLine( String.Format( "    ID: {0}", task.ID ) );
               System.Diagnostics.Trace.WriteLine( String.Format( "    Description: {0}", task.Description ) );
               System.Diagnostics.Trace.WriteLine( String.Format( "    Interval: {0}", task.Interval ) );
               System.Diagnostics.Trace.WriteLine( String.Format( "    BeginTime: {0}", task.BeginTime ) );
            }
         }

         System.Diagnostics.Trace.WriteLine( String.Format( "Dependencies" ) );
         foreach ( var node in this.DependencyGraph.Nodes )
         {
            System.Diagnostics.Trace.WriteLine( String.Format( "  ID: {0}", node.Value.ID ) );

            foreach ( var dependency in node.Value.Dependencies )
            {
               System.Diagnostics.Trace.WriteLine( String.Format( "    ID: {0}", dependency.Value.ID ) );
            }
         }
      }
   }
}