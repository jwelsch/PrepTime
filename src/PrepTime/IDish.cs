using System;

namespace PrepTime
{
   /// <summary>
   /// Interface that represents a dish to prepare.
   /// </summary>
   public interface IDish : IEntity
   {
      /// <summary>
      /// Gets or sets the name of the dish.
      /// </summary>
      string Name
      {
         get;
         set;
      }

      /// <summary>
      /// Gets the collection of tasks.
      /// </summary>
      TaskCollection Tasks
      {
         get;
      }
   }
}
