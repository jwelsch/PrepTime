using System;
using System.Windows.Forms;

namespace PrepTime
{
   /// <summary>
   /// Exposes Win32 HWND handles.
   /// </summary>
   internal class Win32Window : IWin32Window
   {
      /// <summary>
      /// Handle to the window.
      /// </summary>
      private IntPtr handle = IntPtr.Zero;

      /// <summary>
      /// Creates an object of type Win32Window.
      /// </summary>
      /// <param name="handle">Handle to window.</param>
      public Win32Window( int handle )
         : this( new IntPtr( handle ) )
      {
      }

      /// <summary>
      /// Creates an object of type Win32Window.
      /// </summary>
      /// <param name="handle">Handle to window.</param>
      public Win32Window( IntPtr handle )
      {
         this.handle = handle;
      }

      #region IWin32Window Members

      /// <summary>
      /// Gets the handle to the window represented by the implementer.
      /// </summary>
      public IntPtr Handle
      {
         get { return this.handle; }
      }

      #endregion
   }
}
