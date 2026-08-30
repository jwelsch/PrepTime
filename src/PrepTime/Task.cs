using System;
using Newtonsoft.Json;

namespace PrepTime
{
   /// <summary>
   /// Represents a task to be done to a dish.
   /// </summary>
   public class Task : Entity, ITask
   {
      /// <summary>
      /// Gets the description of the task.
      /// </summary>
      public string Description
      {
         get;
         set;
      }

      /// <summary>
      /// Gets the interval of the task.
      /// </summary>
      public TimeSpan Interval
      {
         get;
         set;
      }

      /// <summary>
      /// Gets the time to begin the task.
      /// </summary>
      public DateTime BeginTime
      {
         get;
         set;
      }

      /// <summary>
      /// Gets the ID of the dish that owns the task.
      /// </summary>
      public int DishID
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets or sets how soon the next task can begin after this one.
      /// </summary>
      public TimeSpan BeginOffset
      {
         get;
         set;
      }

      ///// <summary>
      ///// Gets or sets how soon the next task can end after this one.
      ///// </summary>
      //public TimeSpan EndOffset
      //{
      //   get;
      //   set;
      //}

      /// <summary>
      /// Creates an object of type Task.
      /// </summary>
      /// <param name="id">ID of the task.</param>
      /// <param name="description">The description of the task.</param>
      /// <param name="interval">The interval of the task.</param>
      /// <param name="dishID">ID of the dish that owns the task.</param>
      public Task( int id, string description, TimeSpan interval, int dishID )
         : base( (int) id )
      {
         this.Description = description;
         this.Interval = interval;
         this.DishID = dishID;
         //this.BeginOffset = this.Interval;
      }

      /// <summary>
      /// Creates an object of type Task.
      /// </summary>
      /// <param name="description">The description of the task.</param>
      /// <param name="interval">The interval of the task.</param>
      /// <param name="dishID">ID of the dish that owns the task.</param>
      public Task( string description, TimeSpan interval, int dishID )
         : base()
      {
         this.Description = description;
         this.Interval = interval;
         this.DishID = dishID;
      }

      /// <summary>
      /// Returns a string that represents the current object.
      /// </summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         return String.Format( "{0} [{1}] {2}", this.BeginTime.ToString( "hh:mm tt MMM dd, yyyy" ), this.Interval, this.Description );
      }
   }

   /// <summary>
   /// A collection of Task objects.
   /// </summary>
   public class TaskCollection : BaseCollection<ITask>
   {
      /// <summary>
      /// Gets the total time span for all the tasks in the collection.
      /// </summary>
      /// <returns>Total time.</returns>
      public TimeSpan GetTotalTimeLength()
      {
         var interval = new TimeSpan();

         foreach ( var task in this )
         {
            interval += task.Interval;
         }

         return interval;
      }
   }
}
