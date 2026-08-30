using System;
using PrepTime.Properties;

namespace PrepTime
{
   /// <summary>
   /// Represents an type of action action that can be performed on a Dish.
   /// </summary>
   internal enum DishActionType
   {
      Add,
      Edit,
      Delete
   }

   /// <summary>
   /// Represents an action that can be performed on a Dish.
   /// </summary>
   internal class DishActionItem
   {
      /// <summary>
      /// Gets the type of action.
      /// </summary>
      public DishActionType Type
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets the text of the action.
      /// </summary>
      public string Text
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets or sets the action to call for the item.
      /// </summary>
      public Action Action
      {
         get;
         private set;
      }

      /// <summary>
      /// Creates an object of type DishActionItem.
      /// </summary>
      /// <param name="type">Type of action.</param>
      /// <param name="text">Text of the action.</param>
      /// <param name="action">Action to call for the item.</param>
      protected DishActionItem( DishActionType type, string text, Action action )
      {
         this.Type = type;
         this.Text = text;
         this.Action = action;
      }

      /// <summary>
      /// Returns a string that represents the current object.
      /// </summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         return this.Text;
      }
   }

   /// <summary>
   /// Represents an add action that can be performed on a Dish.
   /// </summary>
   internal class AddDishActionItem : DishActionItem
   {
      ///<summary>
      /// Creates an object of type AddDishActionItem.
      /// </summary>
      /// <param name="action">Action to call for the item.</param>
      public AddDishActionItem( Action action )
         : base( DishActionType.Add, Resources.DishActionAdd, action )
      {
      }
   }

   /// <summary>
   /// Represents an edit action that can be performed on a Dish.
   /// </summary>
   internal class EditDishActionItem : DishActionItem
   {
      ///<summary>
      /// Creates an object of type EditDishActionItem.
      /// </summary>
      /// <param name="action">Action to call for the item.</param>
      public EditDishActionItem( Action action )
         : base( DishActionType.Edit, Resources.DishActionEdit, action )
      {
      }
   }

   /// <summary>
   /// Represents a delete action that can be performed on a Dish.
   /// </summary>
   internal class DeleteDishActionItem : DishActionItem
   {
      ///<summary>
      /// Creates an object of type DeleteDishActionItem.
      /// </summary>
      /// <param name="action">Action to call for the item.</param>
      public DeleteDishActionItem( Action action )
         : base( DishActionType.Delete, Resources.DishActionDelete, action )
      {
      }
   }
}
