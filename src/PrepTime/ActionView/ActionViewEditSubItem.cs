using System;
using System.Drawing;
using System.Windows.Forms;

namespace PrepTime
{
   /// <summary>
   /// Represents an item that executes an action in an ActionView.
   /// </summary>
   public partial class ActionViewItem : ListViewItem
   {
      /// <summary>
      /// Describes the action to take regarding saving the information in
      /// the control upon deactivation.
      /// </summary>
      private enum SaveAction
      {
         /// <summary>
         /// Save the data in the editing control.
         /// </summary>
         Save,

         /// <summary>
         /// Discard the data in the editing control.
         /// </summary>
         Discard
      }

      /// <summary>
      /// Represents a sub item that allows in place editing of itself when activated.
      /// </summary>
      public abstract class ActionViewEditSubItem<T> : ActionViewSubItem
         where T : Control
      {
         private T editControl;
         /// <summary>
         /// Gets the control that edits the sub item.
         /// </summary>
         protected T EditControl
         {
            get { return this.editControl; }
         }

         /// <summary>
         /// Creates an object of type ActionViewEditSubItem.
         /// </summary>
         public ActionViewEditSubItem()
         {
         }

         /// <summary>
         /// Executes a custom activation action for the sub item.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         /// <param name="bounds">Bounding rectangle of the sub item.</param>
         /// <param name="data">Data to pass to the deactivation handler.</param>
         protected override void OnActivate( ActionView container, Rectangle bounds, object data )
         {
            // Create control and load with data.
            this.editControl = this.LoadControl();

            if ( this.editControl != null )
            {
               // Register control with parent.
               container.Controls.Add( this.editControl );

               this.AwaitingDeactivation = true;

               // Show control.
               this.editControl.SuspendLayout();
               this.editControl.Visible = true;
               this.editControl.Location = bounds.Location;
               this.editControl.Size = bounds.Size;
               this.editControl.ResumeLayout();
               this.editControl.LostFocus += new EventHandler( editControl_LostFocus );
               this.editControl.KeyUp += new KeyEventHandler( editControl_KeyUp );
               this.editControl.BringToFront();
               this.editControl.Select();
            }

            base.OnActivate( container, bounds, data );
         }

         /// <summary>
         /// Executes a custom deactivation action for the sub item.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         /// <param name="data">Data to pass to the deactivation handler.</param>
         protected override void OnDeactivate( ActionView container, object data )
         {
            this.AwaitingDeactivation = false;

            var saveEdit = true;

            if ( data != null )
            {
               saveEdit = ( ( (SaveAction) data ) == SaveAction.Save );
            }

            this.HideEditControl( saveEdit );

            base.OnDeactivate( container, data );
         }

         /// <summary>
         /// Hides the item editing control.
         /// </summary>
         /// <param name="saveEdit">True to save the edit, false otherwise.</param>
         private void HideEditControl( bool saveEdit )
         {
            if ( this.editControl != null )
            {
               this.editControl.LostFocus -= new EventHandler( editControl_LostFocus );
               this.editControl.KeyUp -= new KeyEventHandler( editControl_KeyUp );
               this.editControl.Visible = false;

               if ( saveEdit )
               {
                  this.SaveControl( this.editControl );
               }

               var parent = this.editControl.Parent;
               parent.Controls.Remove( editControl );

               this.editControl.Dispose();
               this.editControl = null;

               parent.Focus();
            }
         }

         private void editControl_LostFocus( object sender, EventArgs e )
         {
            var control = (Control) sender;

            // If a child of the control that lost focus now has the focus, do not hide the control.
            if ( !control.ContainsFocus )
            {
               //this.HideEditControl( true );

               var container = this.GetParentActionView();
               container.DeactivateSubItem( this, SaveAction.Save );
            }
         }

         private void editControl_KeyUp( object sender, KeyEventArgs e )
         {
            if ( e.KeyCode == Keys.Enter )
            {
               var container = this.GetParentActionView();
               container.DeactivateSubItem( this, SaveAction.Save );
               //this.HideEditControl( true );
            }
            else if ( e.KeyCode == Keys.Escape )
            {
               var container = this.GetParentActionView();
               container.DeactivateSubItem( this, SaveAction.Discard );
               //this.HideEditControl( false );
            }
         }

         /// <summary>
         /// Loads the control from a sub class.
         /// </summary>
         /// <returns>The control to display.</returns>
         protected abstract T LoadControl();

         /// <summary>
         /// Saves the control.
         /// </summary>
         /// <param name="editControl">Control that edited the sub item.</param>
         protected abstract void SaveControl( T editControl );
      }
   }
}
