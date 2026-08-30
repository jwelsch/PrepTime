using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace PrepTime
{
   /// <summary>
   /// ListBox that allows in-place editing of items.
   /// </summary>
   [ProvideProperty( "EditListBox", typeof( ListBox ) )]
   public class EditListBox : ListBox
   {
      /// <summary>
      /// TextBox that does in-place editing of items.
      /// </summary>
      private EditBox editBox;

      /// <summary>
      /// Event raised when the edit box is shown.
      /// </summary>
      public event EventHandler<EditBoxShownArgs> EditBoxShown;

      /// <summary>
      /// Event raised when the edit box is hidden.
      /// </summary>
      public event EventHandler<EditBoxHiddenArgs> EditBoxHidden;

      /// <summary>
      /// Creates an object of type EditListBox.
      /// </summary>
      public EditListBox()
      {
         this.MouseDoubleClick += new MouseEventHandler( EditListBox_MouseDoubleClick );
      }

      private void EditListBox_MouseDoubleClick( object sender, MouseEventArgs e )
      {
         if ( e.Button == System.Windows.Forms.MouseButtons.Left )
         {
            this.ShowEditBox( e.Location );
         }
      }

      protected override void OnHandleCreated( EventArgs e )
      {
         base.OnHandleCreated( e );

         this.editBox = new EditBox();
         this.editBox.Parent = this;// Program.MainForm;
         this.editBox.Visible = false;
         this.editBox.LostFocus += new EventHandler( editBox_LostFocus );
         this.editBox.ReturnKeyDown += ( returnKeyDownSender, returnKeyDownArgs ) =>
            {
               //System.Diagnostics.Trace.WriteLine( "ReturnKeyDown" );
               this.HideEditBox( EditBoxHideReason.ReturnKey );
            };
         this.editBox.EscapeKeyDown += ( escapeKeyDownSender, escapeKeyDownArgs ) =>
            {
               //System.Diagnostics.Trace.WriteLine( "EscapeKeyDown" );
               this.HideEditBox( EditBoxHideReason.EscapeKey );
            };
      }

      private void editBox_LostFocus( object sender, EventArgs e )
      {
         //System.Diagnostics.Trace.WriteLine( "LostFocus" );
         this.HideEditBox( EditBoxHideReason.LostFocus );
      }

      /// <summary>
      /// Shows the edit box.
      /// </summary>
      /// <param name="location">The location in the ListBox to show the edit point at.</param>
      private void ShowEditBox( Point location )
      {
         var index = this.IndexFromPoint( location );

         if ( ( index < 0 ) || ( index >= this.Items.Count ) )
         {
            return;
         }

         this.ShowEditBox( index );
      }

      /// <summary>
      /// Shows the edit box.
      /// </summary>
      /// <param name="index">The index of the item to edit.</param>
      private void ShowEditBox( int index )
      {
         if ( ( index < 0 ) || ( index >= this.Items.Count ) )
         {
            throw new ArgumentOutOfRangeException( "index", index, String.Empty );
         }

         if ( this.editBox.Visible )
         {
            return;
         }

         if ( this.EditBoxShown != null )
         {
            var args = new EditBoxShownArgs( this.SelectedItem, this.SelectedIndex, this.editBox.Text );
            this.EditBoxShown( this, args );

            if ( args.Cancel )
            {
               return;
            }
         }

         var itemRect = this.GetItemRectangle( index );

         //var screenPoint = this.PointToScreen( itemRect.Location );
         //var clientPoint = Program.MainForm.PointToClient( screenPoint );

         this.editBox.Text = this.Items[index].ToString();
         this.editBox.Location = new Point( itemRect.X, itemRect.Y );
         //this.editBox.Location = new Point( clientPoint.X, clientPoint.Y );
         this.editBox.Size = new Size( itemRect.Width - 3, this.editBox.Size.Height );
         this.editBox.SelectAll();
         this.editBox.BringToFront();
         this.editBox.Visible = true;
         this.editBox.Focus();
      }

      /// <summary>
      /// Hides the edit box.
      /// </summary>
      /// <param name="reason">The reason the edit box was hidden.</param>
      private void HideEditBox( EditBoxHideReason reason )
      {
         if ( !this.editBox.Visible )
         {
            return;
         }

         if ( this.EditBoxHidden != null )
         {
            var args = new EditBoxHiddenArgs( reason, this.SelectedItem, this.SelectedIndex, this.editBox.Text );
            this.EditBoxHidden( this, args );

            if ( args.Cancel )
            {
               this.editBox.Focus();
               return;
            }
         }

         // Setting Visible to false will cause the LostFocus event to fire.
         this.editBox.LostFocus -= this.editBox_LostFocus;
         this.editBox.Visible = false;
         this.editBox.LostFocus += this.editBox_LostFocus;

         this.RefreshItem( this.SelectedIndex );
      }

      /// <summary>
      /// Edits a specific item.
      /// </summary>
      /// <param name="index">Index of item to edit.</param>
      public void EditItem( int index )
      {
         this.ShowEditBox( index );
      }

      /// <summary>
      /// Gets the handle to the edit box.
      /// </summary>
      public IntPtr EditBoxHandle
      {
         get { return this.editBox.Handle; }
      }

      #region EditBox

      /// <summary>
      /// Represents the TextBox used to edit items in the ListBox.
      /// </summary>
      private class EditBox : TextBox
      {
         /// <summary>
         /// Event fired when the return key is pressed.
         /// </summary>
         public event EventHandler ReturnKeyDown;

         /// <summary>
         /// Event fired when the escape key is pressed.
         /// </summary>
         public event EventHandler EscapeKeyDown;

         ///<summary>
         /// Creates an object of type EditBox.
         /// </summary>
         public EditBox()
         {
         }

         public override bool PreProcessMessage( ref Message msg )
         {
            if ( msg.Msg == 0x0100 ) // WM_KEYDOWN
            {
               if ( msg.WParam.ToInt32() == 0x0D ) // VK_RETURN
               {
                  if ( this.ReturnKeyDown != null )
                  {
                     this.ReturnKeyDown( this, EventArgs.Empty );
                  }

                  return true;
               }
               else if ( msg.WParam.ToInt32() == 0x1B ) // VK_ESCAPE
               {
                  if ( this.EscapeKeyDown != null )
                  {
                     this.EscapeKeyDown( this, EventArgs.Empty );
                  }

                  return true;
               }
            }

            return base.PreProcessMessage( ref msg );
         }
      }

      #endregion
   }

   #region EditBoxActionArgs

   /// <summary>
   /// Arguments associated with showing the edit box.
   /// </summary>
   public class EditBoxShownArgs : EventArgs
   {
      /// <summary>
      /// Gets the item edited.
      /// </summary>
      public Object Item
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets the index of the item edited.
      /// </summary>
      public int ItemIndex
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets or sets the text in the edit box.
      /// </summary>
      public string EditBoxText
      {
         get;
         set;
      }

      /// <summary>
      /// Gets or sets whether or not to cancel the edit box action.
      /// </summary>
      public bool Cancel
      {
         get;
         set;
      }

      /// <summary>
      /// Creates an object of type EditBoxShownArgs.
      /// </summary>
      /// <param name="item">The item edited.</param>
      /// <param name="itemIndex">The index of the item edited.</param>
      /// <param name="editBoxText">Text in the edit box.</param>
      public EditBoxShownArgs( Object item, int itemIndex, string editBoxText )
      {
         this.Item = item;
         this.ItemIndex = itemIndex;
         this.EditBoxText = editBoxText;
      }

      /// <summary>
      /// Creates an object of type EditBoxShownArgs.
      /// </summary>
      /// <param name="item">The item edited.</param>
      /// <param name="itemIndex">The index of the item edited.</param>
      public EditBoxShownArgs( Object item, int itemIndex )
         : this( item, itemIndex, string.Empty )
      {
      }
   }

   /// <summary>
   /// Reasons the edit box could be hidden.
   /// </summary>
   public enum EditBoxHideReason
   {
      /// <summary>
      /// The return key was pressed.
      /// </summary>
      ReturnKey,

      /// <summary>
      /// The escape key was pressed.
      /// </summary>
      EscapeKey,

      /// <summary>
      /// The tab key was pressed.
      /// </summary>
      //TabKey,

      /// <summary>
      /// The edit box lost focus.
      /// </summary>
      LostFocus
   }

   /// <summary>
   /// Arguments associated with hiding the edit box.
   /// </summary>
   public class EditBoxHiddenArgs : EditBoxShownArgs
   {
      /// <summary>
      /// Gets the reason the edit box was hidden.
      /// </summary>
      public EditBoxHideReason Reason
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets or sets the text in the edit box.
      /// </summary>
      public new string EditBoxText
      {
         get;
         private set;
      }

      ///<summary>
      /// Creates an object of type EditBoxHiddenArgs.
      /// </summary>
      /// <param name="reason">The reason the edit box was hidden.</param>
      /// <param name="item">The item edited.</param>
      /// <param name="itemIndex">The index of the item edited.</param>
      /// <param name="editBoxText">Text in the edit box.</param>
      public EditBoxHiddenArgs( EditBoxHideReason reason, Object item, int itemIndex, string editBoxText )
         : base( item, itemIndex )
      {
         this.EditBoxText = editBoxText;
         this.Reason = reason;
      }
   }

   #endregion
}
