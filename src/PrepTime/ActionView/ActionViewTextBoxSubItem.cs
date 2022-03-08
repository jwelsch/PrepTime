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
      /// Represents a sub item that allows in place editing of itself with a TextBox when activated.
      /// </summary>
      public class ActionViewTextBoxSubItem : ActionViewEditSubItem<TextBox>
      {
         /// <summary>
         /// Creates an object of type ActionViewTextBoxSubItem.
         /// </summary>
         public ActionViewTextBoxSubItem()
         {
         }

         /// <summary>
         /// Creates an object of type ActionViewTextBoxSubItem.
         /// </summary>
         /// <param name="text">Text to display.</param>
         public ActionViewTextBoxSubItem( string text )
         {
            this.Text = text;
         }

         /// <summary>
         /// Loads the control from a sub class.
         /// </summary>
         /// <returns>The control to display.</returns>
         protected override TextBox LoadControl()
         {
            var control = new TextBox()
            {
               Text = this.Text,

               // Set these to allow processing of the ENTER key.
               // This will allow the ENTER press to be ignored so as to
               // not propagate it to the parent control.
               Multiline = true,
               AcceptsReturn = true
            };

            control.KeyDown += ( object sender, KeyEventArgs e ) =>
               {
                  // Swallow the ENTER key press otherwise a new line will be created in the text box.
                  if ( e.KeyCode == Keys.Enter )
                  {
                     e.SuppressKeyPress = true;
                  }
                  else if ( e.KeyCode == Keys.Escape )
                  {
                     e.SuppressKeyPress = true;
                  }
               };

            return control;
         }

         /// <summary>
         /// Saves the control.
         /// </summary>
         /// <param name="editControl">Control that edited the sub item.</param>
         protected override void SaveControl( TextBox editControl )
         {
            this.Text = editControl.Text;
         }
      }
   }
}
