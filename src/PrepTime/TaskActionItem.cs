using System;
using PrepTime.Properties;

namespace PrepTime
{
   /// <summary>
   /// Represents an type of action action that can be performed on a task.
   /// </summary>
   internal enum TaskActionType
   {
      Edit,
      Delete,
      Dependencies,
      MoveUp,
      MoveDown
   }

   /// <summary>
   /// Represents an action that can be performed on a task.
   /// </summary>
   internal class TaskActionItem
   {
      /// <summary>
      /// Gets the type of action.
      /// </summary>
      public TaskActionType Type
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
      /// Creates an object of type TaskActionItem.
      /// </summary>
      /// <param name="type">Type of action.</param>
      /// <param name="text">Text of the action.</param>
      /// <param name="action">Action to call for the item.</param>
      protected TaskActionItem( TaskActionType type, string text, Action action )
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
   /// Represents an add action that can be performed on a Task.
   /// </summary>
   internal class DependenciesTaskActionItem : TaskActionItem
   {
      ///<summary>
      /// Creates an object of type DependenciesTaskActionItem.
      /// </summary>
      /// <param name="action">Action to call for the item.</param>
      public DependenciesTaskActionItem( Action action )
         : base( TaskActionType.Dependencies, Resources.TaskActionDependencies, action )
      {
      }
   }

   /// <summary>
   /// Represents an edit action that can be performed on a Task.
   /// </summary>
   internal class EditTaskActionItem : TaskActionItem
   {
      ///<summary>
      /// Creates an object of type EditTaskActionItem.
      /// </summary>
      /// <param name="action">Action to call for the item.</param>
      public EditTaskActionItem( Action action )
         : base( TaskActionType.Edit, Resources.TaskActionEdit, action )
      {
      }
   }

   /// <summary>
   /// Represents a delete action that can be performed on a Task.
   /// </summary>
   internal class DeleteTaskActionItem : TaskActionItem
   {
      ///<summary>
      /// Creates an object of type DeleteTaskActionItem.
      /// </summary>
      /// <param name="action">Action to call for the item.</param>
      public DeleteTaskActionItem( Action action )
         : base( TaskActionType.Delete, Resources.TaskActionDelete, action )
      {
      }
   }

   /// <summary>
   /// Represents a delete action that can be performed on a Task.
   /// </summary>
   internal class MoveUpTaskActionItem : TaskActionItem
   {
      ///<summary>
      /// Creates an object of type MoveUpTaskActionItem.
      /// </summary>
      /// <param name="action">Action to call for the item.</param>
      public MoveUpTaskActionItem( Action action )
         : base( TaskActionType.MoveUp, Resources.TaskActionMoveUp, action )
      {
      }
   }

   /// <summary>
   /// Represents a delete action that can be performed on a Task.
   /// </summary>
   internal class MoveDownTaskActionItem : TaskActionItem
   {
      ///<summary>
      /// Creates an object of type MoveDownTaskActionItem.
      /// </summary>
      /// <param name="action">Action to call for the item.</param>
      public MoveDownTaskActionItem( Action action )
         : base( TaskActionType.MoveDown, Resources.TaskActionMoveDown, action )
      {
      }
   }
}
