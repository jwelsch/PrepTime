using System;

namespace PrepTime
{
   /// <summary>
   /// Interface that represents a task to be done to a dish.
   /// </summary>
   public interface ITask : IEntity
   {
      /// <summary>
      /// Gets the description of the task.
      /// </summary>
      string Description
      {
         get;
         set;
      }

      /// <summary>
      /// Gets the interval of the task.
      /// </summary>
      TimeSpan Interval
      {
         get;
         set;
      }

      /// <summary>
      /// Gets the time to begin the task.
      /// </summary>
      DateTime BeginTime
      {
         get;
      }

      /// <summary>
      /// Gets the ID of the dish that owns the task.
      /// </summary>
      int DishID
      {
         get;
      }
   }
}
