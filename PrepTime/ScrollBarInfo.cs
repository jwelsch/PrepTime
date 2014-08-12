using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PrepTime
{
   /// <summary>
   /// Contains methods to assist when dealing with scroll bars.
   /// </summary>
   public static class ScrollBarInfo
   {
      private const int GWL_STYLE = ( -16 );
      private const int WS_HSCROLL = 0x100000;
      private const int WS_VSCROLL = 0x200000;

      [DllImport( "user32.dll", CharSet = CharSet.Auto )]
      private static extern int GetWindowLong( IntPtr hWnd, int nIndex );

      /// <summary>
      /// Checks a control to see if it has a visible vertical scroll bar.
      /// </summary>
      /// <param name="control">Control to check.</param>
      /// <returns>True if the control has a visible vertical scroll bar, false otherwise.</returns>
      public static bool IsVerticalScrollBarVisible( Control control )
      {
         if ( !control.IsHandleCreated )
         {
            return false;
         }

         return ( ScrollBarInfo.GetWindowLong( control.Handle, ScrollBarInfo.GWL_STYLE ) & ScrollBarInfo.WS_VSCROLL ) != 0;
      }

      /// <summary>
      /// Checks a control to see if it has a visible horizontal scroll bar.
      /// </summary>
      /// <param name="control">Control to check.</param>
      /// <returns>True if the control has a visible horizontal scroll bar, false otherwise.</returns>
      public static bool IsHorizontalScrollBarVisible( Control control )
      {
         if ( !control.IsHandleCreated )
         {
            return false;
         }

         return ( ScrollBarInfo.GetWindowLong( control.Handle, ScrollBarInfo.GWL_STYLE ) & ScrollBarInfo.WS_HSCROLL ) != 0;
      }

      /// <summary>
      /// Gets the width of a vertical scroll bar.
      /// </summary>
      /// <returns>Width in pixels.</returns>
      public static int VerticalScrollBarWidth()
      {
         return SystemInformation.VerticalScrollBarWidth;
      }

      /// <summary>
      /// Gets the height of a horizontal scroll bar.
      /// </summary>
      /// <returns>Height in pixels.</returns>
      public static int HorizontalScrollBarHeight()
      {
         return SystemInformation.HorizontalScrollBarHeight;
      }
   }
}
