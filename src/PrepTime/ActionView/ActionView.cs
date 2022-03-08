using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PrepTime
{
   /// <summary>
   /// A ListView-like control that forwards user actions to each cell.
   /// </summary>
   [ProvideProperty( "ActionView", typeof( ListView ) )]
   public class ActionView : ListView
   {
      // Windows messages.
      private const int WM_HSCROLL = 0x114;
      private const int WM_VSCROLL = 0x115;
      private const int WM_SIZE = 0x05;
      private const int WM_NOTIFY = 0x4E;
      private const int WM_MOUSEWHEEL = 0x020A;

      // ListView messages.
      private const int LVM_FIRST = 0x1000;
      private const int LVM_GETCOLUMNORDERARRAY = ( LVM_FIRST + 59 );

      [DllImport( "user32.dll", CharSet = CharSet.Auto )]
      private static extern IntPtr SendMessage( IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam );

      /// <summary>
      /// Automatically sizes columns.
      /// </summary>
      private ColumnWidthSizer columnSizer;

      /// <summary>
      /// Gets or sets whether to use the built-in ColumnWidthSizer or not.
      /// </summary>
      public bool UseColumnWidthSizer
      {
         get { return ( this.columnSizer != null ); }
         set
         {
            if ( value )
            {
               if ( this.columnSizer == null )
               {
                  this.columnSizer = new ColumnWidthSizer( this, false );
               }

               this.columnSizer.SizeNow();
            }
            else
            {
               if ( this.columnSizer != null )
               {
                  this.columnSizer = null;
               }
            }
         }
      }

      /// <summary>
      /// Gets or sets whether a double click activates a cell.  The default is false
      /// and a single click will activate a cell.
      /// </summary>
      public bool DoubleClickActivate
      {
         get;
         set;
      }

      /// <summary>
      /// The sub item that is currently activated.  Null if none are activated.
      /// </summary>
      private ActionViewItem.ActionViewSubItem activatedSubItem;

      /// <summary>
      /// Gets whether or not any subitems are awaiting deactivation.
      /// </summary>
      public bool AreSubItemsAwaitingDeactivation
      {
         get { return ( ( this.activatedSubItem != null ) && ( this.activatedSubItem.AwaitingDeactivation ) ); }
      }

      /// <summary>
      /// Creates an object of type ActionView.
      /// </summary>
      public ActionView()
      {
         this.MouseClick += new MouseEventHandler( ActionView_MouseClick );
         this.MouseDoubleClick += new MouseEventHandler( ActionView_MouseDoubleClick );
      }

      protected override void WndProc( ref Message m )
      {
         // Deactivate the activated sub item when these messages are received.
         if (
            ( m.Msg == ActionView.WM_HSCROLL )
            || ( m.Msg == ActionView.WM_VSCROLL )
            || ( m.Msg == ActionView.WM_SIZE )
            || ( m.Msg == ActionView.WM_MOUSEWHEEL )
            )
         {
            if ( this.activatedSubItem != null )
            {
               this.activatedSubItem.Deactivate( this );
               this.activatedSubItem = null;

               // Set the focus back to this control so child windows do not steal it during deactivation.
               this.Focus();
            }
         }

         base.WndProc( ref m );
      }

      private void ActionView_MouseClick( object sender, MouseEventArgs e )
      {
         if ( !this.DoubleClickActivate )
         {
            this.ActivateSubItem( e.Location );
         }
      }

      private void ActionView_MouseDoubleClick( object sender, MouseEventArgs e )
      {
         if ( this.DoubleClickActivate )
         {
            this.ActivateSubItem( e.Location );
         }
      }

      /// <summary>
      /// Activates the sub item at the specified location.
      /// </summary>
      /// <param name="location">Location that an activation event was received.</param>
      private void ActivateSubItem( Point location )
      {
         var info = this.HitTest( location );

         if ( info == null )
         {
            return;
         }

         var subItem = info.SubItem as ActionViewItem.ActionViewSubItem;

         if ( subItem != null )
         {
            int subItemIndex = 0;

            for ( int i = 0; i < info.Item.SubItems.Count; i++ )
            {
               if ( subItem == info.Item.SubItems[i] )
               {
                  subItemIndex = i;
                  break;
               }
            }

            this.ActivateSubItem( info.Item, subItemIndex, null );
         }
      }

      /// <summary>
      /// Activates a sub item.
      /// </summary>
      /// <param name="item">ListViewItem that contains the sub item.</param>
      /// <param name="subItemIndex">Zero-based index of the sub item to activate.</param>
      public void ActivateSubItem( ListViewItem item, int subItemIndex )
      {
         this.ActivateSubItem( item, subItemIndex, null );
      }

      /// <summary>
      /// Activates a sub item.
      /// </summary>
      /// <param name="item">ListViewItem that contains the sub item.</param>
      /// <param name="subItemIndex">Zero-based index of the sub item to activate.</param>
      /// <param name="data">Data to pass to the activation handler.</param>
      public void ActivateSubItem( ListViewItem item, int subItemIndex, object data )
      {
         var subItem = item.SubItems[subItemIndex] as ActionViewItem.ActionViewSubItem;

         if ( subItem != null )
         {
            // Need to get the column order in case they were rearranged.
            var order = this.ColumnOrder();

            int width = this.Columns[order[subItemIndex]].Width;
            int height = subItem.Bounds.Height;

            var bounds = new Rectangle( subItem.Bounds.Location, new Size( width, height ) );

            this.activatedSubItem = subItem;
            subItem.Activate( this, bounds, data );
         }
      }

      /// <summary>
      /// Deactivates a sub item.
      /// </summary>
      /// <param name="subItem">Sub item to deactivate.</param>
      public void DeactivateSubItem( ActionViewItem.ActionViewSubItem subItem )
      {
         if ( this.activatedSubItem == subItem )
         {
            this.DeactivateSubItem( subItem, null );
         }
      }

      /// <summary>
      /// Deactivates a sub item.
      /// </summary>
      /// <param name="subItem">Sub item to deactivate.</param>
      /// <param name="data">Data to pass to the deactivation handler.</param>
      public void DeactivateSubItem( ActionViewItem.ActionViewSubItem subItem, object data )
      {
         if ( this.activatedSubItem == subItem )
         {
            this.activatedSubItem.Deactivate( this, data );
            this.activatedSubItem = null;
         }
      }

      /// <summary>
      /// Deactivates a sub item.
      /// </summary>
      /// <param name="item">ListViewItem that contains the sub item.</param>
      /// <param name="subItemIndex">Zero-based index of the sub item to deactivate.</param>
      public void DeactivateSubItem( ListViewItem item, int subItemIndex )
      {
         this.DeactivateSubItem( item, subItemIndex, null );
      }

      /// <summary>
      /// Deactivates a sub item.
      /// </summary>
      /// <param name="item">ListViewItem that contains the sub item.</param>
      /// <param name="subItemIndex">Zero-based index of the sub item to deactivate.</param>
      /// <param name="data">Data to pass to the deactivation handler.</param>
      public void DeactivateSubItem( ListViewItem item, int subItemIndex, object data )
      {
         var subItem = item.SubItems[subItemIndex] as ActionViewItem.ActionViewSubItem;

         if ( subItem != null )
         {
            this.DeactivateSubItem( subItem, data );
         }
      }

      /// <summary>
      /// Gets the order of the columns in the ActionView.
      /// </summary>
      /// <returns>The order of the columns.</returns>
      public int[] ColumnOrder()
      {
         var lParam = Marshal.AllocHGlobal( Marshal.SizeOf( typeof( int ) ) * this.Columns.Count );
         var wParam = new IntPtr( this.Columns.Count );

         var res = ActionView.SendMessage( this.Handle, (UInt32) ActionView.LVM_GETCOLUMNORDERARRAY, wParam, lParam );

         if ( res.ToInt32() == 0 )
         {
            throw new Win32Exception( Marshal.GetLastWin32Error() );
         }

         var order = new int[this.Columns.Count];
         Marshal.Copy( lParam, order, 0, this.Columns.Count );
         Marshal.FreeHGlobal( lParam );

         return order;
      }
   }
}
