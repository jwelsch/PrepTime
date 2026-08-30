using System;
using System.Linq;
using PrepTime.Properties;
using System.Collections.Generic;

namespace PrepTime
{
   /// <summary>
   /// Provides the interaction between the UI and the data model.
   /// </summary>
   internal class Controller
   {
      ///<summary>
      /// Creates an object of type Controller.
      /// </summary>
      public Controller()
      {
      }

      /// <summary>
      /// Gets the begin time.
      /// </summary>
      /// <returns>The begin time.</returns>
      public DateTime GetBeginTime()
      {
         return Program.DataModel.EndTime - this.CalculateTotalTaskTime();
      }

      /// <summary>
      /// Sets the end time.
      /// </summary>
      /// <param name="time">The end time.</param>
      public void SetEndTime( DateTime time )
      {
         Program.DataModel.EndTime = time;
         this.CalculateTaskTime();
      }

      /// <summary>
      /// Adds a dish to prepare.
      /// </summary>
      /// <param name="name">Name of the dish.</param>
      /// <returns>The unique ID of the dish.</returns>
      public IDish DishAdd( string name )
      {
         var dish = new Dish( name );
         Program.DataModel.Dishes.Add( dish );

         return dish;
      }

      /// <summary>
      /// Deletes the dish.
      /// </summary>
      /// <param name="id">The unique ID of the dish to remove.</param>
      /// <returns>True if the dish can be deleted, false otherwise.</returns>
      public bool DishDelete( int id )
      {
         var dish = this.DishFind( id );

         if ( dish == null )
         {
            return false;
         }

         if ( Program.DataModel.Dishes.Count == 1 )
         {
            return false;
         }

         foreach ( var task in dish.Tasks )
         {
            Program.DataModel.DependencyGraph.RemoveNode( task );
         }

         Program.DataModel.Dishes.Remove( dish );

         return true;
      }

      /// <summary>
      /// Changes the name of the dish with the specified ID.
      /// </summary>
      /// <param name="id">ID of the dish to change.</param>
      /// <param name="newName">New name of the dish.</param>
      public void DishChangeName( int id, string newName )
      {
         var dish = this.DishFind( id );

         if ( dish == null )
         {
            throw new ArgumentException( Resources.ErrorNoMatchingDishID, "id" );
         }

         dish.Name = newName;
      }

      /// <summary>
      /// Checks if a dish with the same name exists.
      /// </summary>
      /// <param name="name">Name of dish to check.</param>
      /// <returns>True if there is a dish with the specified name, false otherwise.</returns>
      public bool DishNameExists( string name )
      {
         foreach ( var dish in Program.DataModel.Dishes )
         {
            if ( dish.Name == name )
            {
               return true;
            }
         }

         return false;
      }

      /// <summary>
      /// Finds the dish with the specified ID.
      /// </summary>
      /// <param name="id">ID of the dish to find.</param>
      /// <returns>The Dish object with that has a matching ID or null if no match was found.</returns>
      private Dish DishFind( int id )
      {
         foreach ( var dish in Program.DataModel.Dishes )
         {
            if ( dish.ID == id )
            {
               return (Dish) dish;
            }
         }

         return null;
      }

      /// <summary>
      /// Adds a task to a dish.
      /// </summary>
      /// <param name="dishID">Unique ID of the dish that is getting the task added to it.</param>
      /// <param name="description">Description of the task.</param>
      /// <param name="interval">Interval of the task.</param>
      /// <returns>The unique ID of the task.</returns>
      public ITask TaskAdd( int dishID, string description, TimeSpan interval )
      {
         var dish = this.DishFind( dishID );

         if ( dish == null )
         {
            throw new ArgumentException( Resources.ErrorNoMatchingDishID, "dishID" );
         }

         var task = new Task( description, interval, dish.ID );
         task.BeginOffset = task.Interval;

         if ( dish.Tasks.Count > 0 )
         {
            var sortedTaskIDs = this.DishGetSortedTasks( dish.ID );
            Program.DataModel.DependencyGraph.AddNode( task.ID, new int[] { sortedTaskIDs[sortedTaskIDs.Length - 1] } );
         }
         else
         {
            Program.DataModel.DependencyGraph.AddNode( task.ID );
         }

         dish.Tasks.Add( task );

         this.CalculateTaskTime();

         return task;
      }

      /// <summary>
      /// Changes the description of the task with the specified ID.
      /// </summary>
      /// <param name="id">ID of the task to change.</param>
      /// <param name="newName">New description of the task.</param>
      public void TaskChangeDescription( int id, string newDescription )
      {
         var task = this.TaskFind( id );

         if ( task == null )
         {
            throw new ArgumentException( Resources.ErrorNoMatchingTaskID, "id" );
         }

         task.Description = newDescription;
      }

      /// <summary>
      /// Changes the interval of the task with the specified ID.
      /// </summary>
      /// <param name="id">ID of the task to change.</param>
      /// <param name="newInterval">New interval of the task.</param>
      public void TaskChangeInterval( int id, TimeSpan newInterval )
      {
         var task = this.TaskFind( id );

         if ( task == null )
         {
            throw new ArgumentException( Resources.ErrorNoMatchingTaskID, "id" );
         }

         task.Interval = newInterval;
         task.BeginOffset = newInterval;

         this.CalculateTaskTime();
      }

      /// <summary>
      /// Updates the given task with the given dependencies.
      /// </summary>
      /// <param name="dependentID">ID of the dependent task.</param>
      /// <param name="dependencyIDs">IDs of the task's dependencies.</param>
      public void TaskUpdateDependencies( int dependentID, IEnumerable<int> dependencyIDs )
      {
         var dependentTask = this.TaskFind( dependentID );

         if ( dependentTask == null )
         {
            throw new ArgumentException( Resources.ErrorNoMatchingTaskID, "dependentID" );
         }

         var dependencies = Program.DataModel.DependencyGraph.GetDependencies( dependentID );
         var found = false;
         var idsToAdd = new List<int>();
         var idsToRemove = new List<int>();

         foreach ( var entityID in dependencies )
         {
            found = false;

            foreach ( var dependencyID in dependencyIDs )
            {
               if ( entityID == dependencyID )
               {
                  found = true;
                  break;
               }
            }

            if ( !found )
            {
               idsToRemove.Add( entityID );
            }
         }

         foreach ( var dependencyID in dependencyIDs )
         {
            found = false;

            foreach ( var entityID in dependencies )
            {
               if ( dependencyID == entityID )
               {
                  found = true;
                  break;
               }
            }

            if ( !found )
            {
               idsToAdd.Add( dependencyID );
            }
         }

         Program.DataModel.DependencyGraph.AddDependencies( dependentID, idsToAdd );
         Program.DataModel.DependencyGraph.RemoveDependencies( dependentID, idsToRemove );

         this.CalculateTaskTime();
      }

      /// <summary>
      /// Finds the task with the specified ID.
      /// </summary>
      /// <param name="id">ID of the task to find.</param>
      /// <returns>The Task object with that has a matching ID or null if no match was found.</returns>
      private Task TaskFind( int id )
      {
         foreach ( var dish in Program.DataModel.Dishes )
         {
            foreach ( var task in dish.Tasks )
            {
               if ( task.ID == id )
               {
                  return (Task) task;
               }
            }
         }

         return null;
      }

      /// <summary>
      /// Calculates the total amount of time needed by all tasks.
      /// </summary>
      /// <returns>Total amount of time needed by all tasks.</returns>
      public TimeSpan CalculateTotalTaskTime()
      {
         var longest = TimeSpan.Zero;

         foreach ( var dish in Program.DataModel.Dishes )
         {
            var total = TimeSpan.Zero;

            foreach ( var task in dish.Tasks )
            {
               total += task.Interval;
            }

            if ( total > longest )
            {
               longest = total;
            }
         }

         return longest;
      }

      /// <summary>
      /// Calculates the task begin time for all dishes.
      /// </summary>
      private void CalculateTaskTime()
      {
         var groups = Program.DataModel.DependencyGraph.GroupedTopologicalSort();
         var tasks = new List<Task>();

         foreach ( var group in groups )
         {
            var total = TimeSpan.Zero;
            var entities = this.FindEntities( group );

            for ( var i = entities.Length - 1; i >= 0; i-- )
            {
               var task = entities[i] as Task;

               if ( task != null )
               {
                  total += task.Interval;
                  task.BeginTime = Program.DataModel.EndTime - total;
                  tasks.Add( task );
               }
            }
         }

         //var comparer = new Comparer<Task>();
         //tasks.Sort( comparer );

         //System.Diagnostics.Trace.WriteLine( "Before offset adjustment." );
         //foreach ( var task in tasks )
         //{
         //   System.Diagnostics.Trace.WriteLine( String.Format( "Begin: {0}; Description: {1}", task.BeginTime, task.Description ) );
         //}

         //for ( var i = tasks.Count - 1; i >= 0; i-- )
         //{
         //   for ( var j = i - 1; j >= 0; j-- )
         //   {
         //      if ( tasks[i].DishID == tasks[j].DishID )
         //      {
         //         continue;
         //      }

         //      var beginOffsetTime = tasks[j].BeginTime + tasks[j].BeginOffset;

         //      if ( beginOffsetTime > tasks[i].BeginTime )
         //      {
         //         tasks[j].BeginTime -= beginOffsetTime - tasks[i].BeginTime;
         //      }
         //   }
         //}

         //tasks.Sort( comparer );
         //System.Diagnostics.Trace.WriteLine( "\nAfter offset adjustment." );
         //foreach ( var task in tasks )
         //{
         //   System.Diagnostics.Trace.WriteLine( String.Format( "Begin: {0}; Description: {1}", task.BeginTime, task.Description ) );
         //}
      }

      /// <summary>
      /// Deletes the task.
      /// </summary>
      /// <param name="taskID">ID of the task to delete.</param>
      internal void TaskDelete( int taskID )
      {
         var task = this.TaskFind( taskID );

         if ( task == null )
         {
            throw new ArgumentException( Resources.ErrorNoMatchingTaskID, "task" );
         }

         var dish = this.DishFind( task.DishID );
         dish.Tasks.Remove( task );
         Program.DataModel.DependencyGraph.RemoveNode( task );
      }

      /// <summary>
      /// Determines if a task can depend on the given independent ID.
      /// </summary>
      /// <param name="dependentTaskID">ID of the task.</param>
      /// <param name="independentID">ID of the independent entity.</param>
      /// <returns>True if the dependency can exist, false otherwise.</returns>
      internal bool TaskCanHaveDependency( int dependentTaskID, int independentID )
      {
         var dependentTask = this.TaskFind( dependentTaskID );

         if ( dependentTask == null )
         {
            throw new ArgumentException( Resources.ErrorNoMatchingTaskID, "dependentID" );
         }

         var independentTask = this.TaskFind( independentID );

         if ( independentTask == null )
         {
            var independentDish = this.DishFind( independentID );

            if ( independentDish == null )
            {
               throw new ArgumentException( Resources.ErrorNoMatchingEntityID, "independentID" );
            }

            if ( independentDish.ID == dependentTask.DishID )
            {
               return false;
            }
         }
         else
         {
            if ( independentTask.DishID == dependentTask.DishID )
            {
               return false;
            }

            var tempArray = new IEntity[] { independentTask };

            try
            {
               if ( !Program.DataModel.DependencyGraph.TryAddDependencies( dependentTaskID, tempArray ) )
               {
                  return false;
               }
            }
            finally
            {
               Program.DataModel.DependencyGraph.RemoveDependencies( dependentTaskID, tempArray );
            }
         }

         return true;
      }

      /// <summary>
      /// Finds the entity that has the given ID.
      /// </summary>
      /// <param name="id">ID to find.</param>
      /// <returns>Interface to the entity that has the given ID, or null if no match was found.</returns>
      public IEntity FindEntity( int id )
      {
         foreach ( var dish in Program.DataModel.Dishes )
         {
            if ( dish.ID == id )
            {
               return dish;
            }

            foreach ( var task in dish.Tasks )
            {
               if ( task.ID == id )
               {
                  return task;
               }
            }
         }

         return null;
      }

      /// <summary>
      /// Finds the entities that have the given IDs.
      /// </summary>
      /// <param name="ids">IDs to find.</param>
      /// <returns>Entities that has the given IDs.</returns>
      public IEntity[] FindEntities( int[] ids )
      {
         var entities = new List<IEntity>();
         var found = false;

         foreach ( var id in ids )
         {
            found = false;

            foreach ( var dish in Program.DataModel.Dishes )
            {
               if ( dish.ID == id )
               {
                  entities.Add( dish );
                  found = true;
                  break;
               }

               if ( !found )
               {
                  foreach ( var task in dish.Tasks )
                  {
                     if ( task.ID == id )
                     {
                        entities.Add( task );
                        found = true;
                        break;
                     }
                  }

                  if ( found )
                  {
                     break;
                  }
               }
            }
         }

         //foreach ( var dish in Program.DataModel.Dishes )
         //{
         //   foreach ( var id in ids )
         //   {
         //      if ( dish.ID == id )
         //      {
         //         entities.Add( dish );
         //         break;
         //      }
         //   }

         //   foreach ( var task in dish.Tasks )
         //   {
         //      foreach ( var id in ids )
         //      {
         //         if ( task.ID == id )
         //         {
         //            entities.Add( task );
         //            break;
         //         }
         //      }
         //   }
         //}

         return entities.ToArray();
      }

      /// <summary>
      /// Swaps the positions of two tasks.
      /// </summary>
      /// <param name="task1">First task to swap.</param>
      /// <param name="task2">Second task to swap.</param>
      public void SwapTasks( ITask task1, ITask task2 )
      {
         if ( task1.DishID != task2.DishID )
         {
            throw new Exception( Resources.ErrorTasksNotFromSameDish );
         }

         var task1Dependencies = Program.DataModel.DependencyGraph.GetDependencies( task1.ID );
         var task2Dependencies = Program.DataModel.DependencyGraph.GetDependencies( task2.ID );

         var task1Dependents = Program.DataModel.DependencyGraph.GetDependents( task1.ID );
         var task2Dependents = Program.DataModel.DependencyGraph.GetDependents( task2.ID );

         Program.DataModel.DependencyGraph.RemoveDependencies( task1.ID, task1Dependencies );
         Program.DataModel.DependencyGraph.RemoveDependencies( task2.ID, task2Dependencies );

         for ( var i = 0; i < task1Dependencies.Length; i++ )
         {
            if ( task1Dependencies[i] == task2.ID )
            {
               task1Dependencies[i] = task1.ID;
            }
         }

         for ( var i = 0; i < task2Dependencies.Length; i++ )
         {
            if ( task2Dependencies[i] == task1.ID )
            {
               task2Dependencies[i] = task2.ID;
            }
         }

         var task1DishDependent = -1;
         var task2DishDependent = -1;

         foreach ( var dependent in task1Dependents )
         {
            var task = this.TaskFind( dependent );
            if ( task.DishID == task1.DishID )
            {
               Program.DataModel.DependencyGraph.RemoveDependencies( dependent, new int[] { task1.ID } );
               task1DishDependent = dependent;
               break;
            }
         }

         foreach ( var dependent in task2Dependents )
         {
            var task = this.TaskFind( dependent );
            if ( task.DishID == task2.DishID )
            {
               Program.DataModel.DependencyGraph.RemoveDependencies( dependent, new int[] { task2.ID } );
               task2DishDependent = dependent;
               break;
            }
         }

         this.TaskUpdateDependencies( task1.ID, task2Dependencies );
         this.TaskUpdateDependencies( task2.ID, task1Dependencies );

         if ( ( task1DishDependent != -1 ) && ( task1DishDependent != task2.ID ) )
         {
            Program.DataModel.DependencyGraph.AddDependencies( task1DishDependent, new int[] { task2.ID } );
         }

         if ( ( task2DishDependent != -1 ) && ( task2DishDependent != task1.ID ) )
         {
            Program.DataModel.DependencyGraph.AddDependencies( task2DishDependent, new int[] { task1.ID } );
         }

         this.CalculateTaskTime();
      }

      /// <summary>
      /// Gets the sorted tasks of the specified dish.
      /// </summary>
      /// <param name="id">ID of the dish.</param>
      /// <returns>Topologically sorted tasks.</returns>
      public int[] DishGetSortedTasks( int id )
      {
         var groups = Program.DataModel.DependencyGraph.GroupedTopologicalSort();

         foreach ( var group in groups )
         {
            foreach ( var taskID in group )
            {
               var task = this.TaskFind( taskID );

               if ( task.DishID == id )
               {
                  return group;
               }
               else
               {
                  continue;
               }
            }
         }

         return null;
      }
   }
}