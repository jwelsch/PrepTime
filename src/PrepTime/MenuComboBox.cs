using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace PrepTime
{
   /// <summary>
   /// A ComboBox that acts like a menu.
   /// </summary>
   [ProvideProperty( "MenuComboBox", typeof( ComboBox ) )]
   public class MenuComboBox : ComboBox
   {
      /// <summary>
      /// Gets or sets the menu text.
      /// </summary>
      private Object menuText;

      /// <summary>
      /// Tracks if the user selected an item (true) or if the user closed the drop down list by clicking elsewhere.
      /// </summary>
      private bool changeSubmitted = false;

      /// <summary>
      /// Event raised when a menu item is selected by the user.
      /// </summary>
      public event MenuComboBoxHandler MenuItemSelected;

      protected override void OnCreateControl()
      {
         this.DropDownStyle = ComboBoxStyle.DropDownList;
         base.OnCreateControl();
      }

      /// <summary>
      /// Raises the DropDown event.
      /// </summary>
      /// <param name="e">An EventArgs that contains the event data.</param>
      protected override void OnDropDown( EventArgs e )
      {
         //System.Diagnostics.Trace.WriteLine( "OnDropDown" );
         if ( this.Items.Count > 0 )
         {
            this.menuText = this.Items[0];
            this.Items.RemoveAt( 0 );
         }

         this.changeSubmitted = false;

         base.OnDropDown( e );
      }

      protected override void OnDropDownClosed( EventArgs e )
      {
         if ( this.changeSubmitted )
         {
            this.changeSubmitted = false;
         }
         else
         {
            if ( this.menuText != null )
            {
               this.Items.Insert( 0, this.menuText );
               this.SelectedIndex = 0;
            }
         }

         base.OnDropDownClosed( e );
      }

      /// <summary>
      /// Raises the SelectionChangeCommitted event.  This is only fired when a USER selects an item.
      /// </summary>
      /// <param name="e">An EventArgs that contains the event data.</param>
      protected override void OnSelectionChangeCommitted( EventArgs e )
      {
         //System.Diagnostics.Trace.WriteLine( "OnSelectionChangeCommitted" );

         var selectedMenuItem = this.SelectedItem;
         var selectedMenuItemIndex = this.SelectedIndex;

         if ( this.menuText != null )
         {
            this.Items.Insert( 0, this.menuText );
            this.SelectedIndex = 0;
         }

         this.changeSubmitted = true;

         if ( this.MenuItemSelected != null )
         {
            this.MenuItemSelected( this, new MenuComboBoxEventArgs( selectedMenuItem, selectedMenuItemIndex ) );
         }

         //base.OnSelectionChangeCommitted( e );
      }

      protected override void OnSelectedIndexChanged( EventArgs e )
      {
         //System.Diagnostics.Trace.WriteLine( "OnSelectedIndexChanged" );
         //base.OnSelectedIndexChanged( e );
      }

      protected override void OnSelectedItemChanged( EventArgs e )
      {
         //System.Diagnostics.Trace.WriteLine( "OnSelectedItemChanged" );
         //base.OnSelectedItemChanged( e );
      }

      protected override void OnSelectedValueChanged( EventArgs e )
      {
         //System.Diagnostics.Trace.WriteLine( "OnSelectedValueChanged" );
         //base.OnSelectedValueChanged( e );
      }

      protected override void WndProc( ref Message m )
      {
         base.WndProc( ref m );

         if ( m.Msg == 0x000F ) // WM_PAINT
         {
            if ( this.menuText != null )
            {
               if ( this.DroppedDown )
               {
                  //System.Diagnostics.Trace.WriteLine( "OnPaint" );
                  using ( var graphics = this.CreateGraphics() )
                  {
                     var size = graphics.MeasureString( this.menuText.ToString(), this.Font );
                     var displayArea = new RectangleF( new PointF( this.DisplayRectangle.Left + 2, this.DisplayRectangle.Top + 4 ), size );
                     var brush = SystemBrushes.WindowText; // new SolidBrush( this.ForeColor );
                     graphics.DrawString( this.menuText.ToString(), this.Font, brush, displayArea );
                  }
               }
            }
         }
      }
   }

   #region MenuComboBoxHandler/Event

   /// <summary>
   /// Arguments associated with an MenuComboBox event.
   /// </summary>
   public class MenuComboBoxEventArgs : EventArgs
   {
      /// <summary>
      /// Gets the menu item that was selected.
      /// </summary>
      public Object MenuItem
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets the menu item index that was selected.
      /// </summary>
      public int MenuItemIndex
      {
         get;
         private set;
      }

      ///<summary>
      /// Creates an object of type MenuComboBoxEventArgs.
      /// </summary>
      /// <param name="menuItem">Object that was selected.</param>
      /// <param name="menuItemIndex">Menu item index that was selected.</param>
      public MenuComboBoxEventArgs( Object menuItem, int menuItemIndex )
      {
         this.MenuItem = menuItem;
         this.MenuItemIndex = menuItemIndex;
      }
   }

   /// <summary>
   /// Prototype for a method called for an MenuComboBox event.
   /// </summary>
   /// <param name="sender">Object that triggered the event.</param>
   /// <param name="e">Arguments associated with the event.</param>
   public delegate void MenuComboBoxHandler( object sender, MenuComboBoxEventArgs e );

   #endregion
}
