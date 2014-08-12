using System;
using System.Collections.ObjectModel;

namespace PrepTime
{
   /// <summary>
   /// Represents an object in the data model.
   /// </summary>
   public class Entity : IEntity
   {
      /// <summary>
      /// Gets the identifier of the Dish.
      /// </summary>
      public int ID
      {
         get;
         private set;
      }

      ///<summary>
      /// Creates an object of type Entity.
      /// </summary>
      /// <param name="sequentialNumberKey">Key to the sequential number generator for the object.</param>
      public Entity( object sequentialNumberKey )
      {
         this.ID = SequentialNumber.Next( sequentialNumberKey );
      }

      ///<summary>
      /// Creates an object of type Entity.
      /// </summary>
      /// <param name="id">ID of the entity.</param>
      public Entity( int id )
      {
         this.ID = id;
      }

      ///<summary>
      /// Creates an object of type Entity.
      /// </summary>
      public Entity()
      {
         this.ID = SequentialNumber.Next( typeof( Entity ) );
      }
   }

   #region EntityCollection

   /// <summary>
   /// Collection of entity objects.
   /// </summary>
   public class EntityCollection : BaseCollection<Entity>
   {
   }

   #endregion
}
