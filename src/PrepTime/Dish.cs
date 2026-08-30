using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace PrepTime
{
   /// <summary>
   /// Represents a dish to prepare.
   /// </summary>
   public class Dish : Entity, IDish
   {
      /// <summary>
      /// Gets the name of the dish.
      /// </summary>
      public string Name
      {
         get;
         set;
      }

      /// <summary>
      /// Gets the collection of tasks.
      /// </summary>
      public TaskCollection Tasks
      {
         get;
         private set;
      }

      ///<summary>
      /// Creates an object of type Dish.
      /// </summary>
      /// <param name="id">ID of the dish.</param>
      /// <param name="name">The name of the dish.</param>
      public Dish( int id, string name )
         : base( id )
      {
         this.Tasks = new TaskCollection();
         this.Name = name;
      }

      ///<summary>
      /// Creates an object of type Dish.
      /// </summary>
      /// <param name="name">The name of the dish.</param>
      public Dish( string name )
         : base()
      {
         this.Tasks = new TaskCollection();
         this.Name = name;
      }

      /// <summary>
      /// Returns a string that represents the current object.
      /// </summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         return this.Name;
      }
   }

   /// <summary>
   /// A collection of Dish objects.
   /// </summary>
   public class DishCollection : BaseCollection<IDish>
   {
   }
}
