using System;
using System.Collections.ObjectModel;

namespace PrepTime
{
   /// <summary>
   /// Interface for a data model entity.
   /// </summary>
   public interface IEntity
   {
      /// <summary>
      /// Gets the identifier of the Dish.
      /// </summary>
      int ID
      {
         get;
      }
   }

   #region IEntityCollection

   /// <summary>
   /// Collection of entity interface objects.
   /// </summary>
   internal class IEntityCollection : BaseCollection<IEntity>
   {
   }

   #endregion
}
