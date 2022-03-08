using System;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
//using JmwSoftware.Validation;

namespace PrepTime
{
   #region BalloonAlreadyShowingException

   /// <summary>
   /// Exception thrown when a balloon tip cannot be shown because there is already one being shown.
   /// </summary>
   [global::System.Serializable]
   public class BalloonAlreadyShowingException : Exception
   {
      /// <summary>
      /// Constructs a BalloonAlreadyShowingException object.
      /// </summary>
      public BalloonAlreadyShowingException()
      {
      }

      /// <summary>
      /// Constructs a BalloonAlreadyShowingException object.
      /// </summary>
      /// <param name="message">The message associated with the exception.</param>
      public BalloonAlreadyShowingException( string message )
         : base( message )
      {
      }

      /// <summary>
      /// Constructs a BalloonAlreadyShowingException object.
      /// </summary>
      /// <param name="message">The message associated with the exception.</param>
      /// <param name="innerException">Another exception associated with the BalloonAlreadyShowingException.</param>
      public BalloonAlreadyShowingException( string message, Exception innerException )
         : base( message, innerException )
      {
      }

      /// <summary>
      /// Constructs a BalloonAlreadyShowingException object.
      /// </summary>
      /// <param name="info">The serialization information.</param>
      /// <param name="context">The streaming context.</param>
      protected BalloonAlreadyShowingException( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context )
         : base( info, context )
      {
      }
   }
   
   #endregion

   /// <summary>
   /// Displays a balloon tip for a TextBox.
   /// </summary>
   public static class TextBoxBalloonTip
   {
      private const Int32 EM_SHOWBALLOONTIP = 0x1503;
      private const Int32 EM_HIDEBALLOONTIP = 0x1504;

      /// <summary>
      /// Kind of icon shown in a balloon tip.
      /// </summary>
      public enum Icon
      {
         None = 0,
         Information,
         Warning,
         Error
      }

      /// <summary>
      /// Struct containing information on what a balloon tip displays.
      /// </summary>
      [StructLayout( LayoutKind.Sequential, CharSet = CharSet.Auto )]
      private struct EDITBALLOONTIP
      {
         public Int32 cbStruct;
         public string pszTitle;
         public string pszText;
         public TextBoxBalloonTip.Icon ttiIcon;
      }

      [DllImport( "user32.dll", SetLastError = false )]
      private static extern bool SendMessage( IntPtr hWnd, Int32 msg, IntPtr wParam, IntPtr lParam );

      //private IntPtr textBoxHandle;
      ///// <summary>
      ///// Gets or sets the handle of the TextBox that the balloon will display over.
      ///// </summary>
      //public IntPtr TextBoxHandle
      //{
      //   get { return this.textBoxHandle; }
      //   set { this.textBoxHandle = value; }
      //}

      //private string title;
      ///// <summary>
      ///// Gets or sets the title text of the balloon tip.
      ///// </summary>
      //public string Title
      //{
      //   get { return this.title; }
      //   set { this.title = value; }
      //}

      //private string body;
      ///// <summary>
      ///// Gets or sets the body text of the balloon tip.
      ///// </summary>
      //public string Body
      //{
      //   get { return this.body; }
      //   set { this.body = value; }
      //}

      //private EditBalloonTipIcon icon;
      ///// <summary>
      ///// Gets or sets the icon displayed in the balloon tip.
      ///// </summary>
      //public EditBalloonTipIcon Icon
      //{
      //   get { return this.icon; }
      //   set { this.icon = value; }
      //}

      ///// <summary>
      ///// Creates an object of type TextBoxBalloonTip.
      ///// </summary>
      //public TextBoxBalloonTip()
      //{
      //}

      /// <summary>
      /// Shows a balloon tip.
      /// </summary>
      /// <param name="textBoxHandle">Handle of the TextBox that the balloon will be displayed over.</param>
      /// <param name="body">Body text of the balloon tip.</param>
      /// <param name="title">Title text of the balloon tip.</param>
      /// <param name="icon">Icon displayed on the balloon tip.</param>
      public static void Show( IntPtr textBoxHandle, string body, string title, Icon icon )
      {
         //StringCheck.String( body, false, "body" );
         //StringCheck.String( title, false, "title" );

         var balloonTip = new EDITBALLOONTIP();

         // Fill in the structure's fields.
         balloonTip.cbStruct = Marshal.SizeOf( balloonTip );
         balloonTip.pszText = body;
         balloonTip.pszTitle = title;
         balloonTip.ttiIcon = icon;

         var lParam = IntPtr.Zero;

         try
         {
            // Allocate memory to store the structure and copy it to an IntPtr.
            lParam = Marshal.AllocHGlobal( balloonTip.cbStruct );
            Marshal.StructureToPtr( balloonTip, lParam, false );

            // Tell the TextBox to show the tip.
            if ( !SendMessage( textBoxHandle, EM_SHOWBALLOONTIP, IntPtr.Zero, lParam ) )
            {
               throw new Win32Exception( "SendMessage() failed to display a TextBox balloon tip." );
            }
         }
         finally
         {
            if ( lParam != IntPtr.Zero )
            {
               // Free the allocated memory.
               Marshal.FreeHGlobal( lParam );
            }
         }
      }

      /// <summary>
      /// Shows a balloon tip.
      /// </summary>
      /// <param name="textBox">TextBox that the balloon will be displayed over.</param>
      /// <param name="body">Body text of the balloon tip.</param>
      /// <param name="title">Title text of the balloon tip.</param>
      /// <param name="icon">Icon displayed on the balloon tip.</param>
      public static void Show( TextBox textBox, string body, string title, Icon icon )
      {
         TextBoxBalloonTip.Show( textBox.Handle, body, title, icon );
      }

      /// <summary>
      /// Shows a balloon tip.
      /// </summary>
      /// <param name="textBoxHandle">Handle of the TextBox that the balloon will be displayed over.</param>
      /// <param name="body">Body text of the balloon tip.</param>
      /// <param name="title">Title text of the balloon tip.</param>
      public static void Show( IntPtr textBoxHandle, string body, string title )
      {
         TextBoxBalloonTip.Show( textBoxHandle, body, title, TextBoxBalloonTip.Icon.None );
      }

      /// <summary>
      /// Shows a balloon tip.
      /// </summary>
      /// <param name="textBox">TextBox that the balloon will be displayed over.</param>
      /// <param name="body">Body text of the balloon tip.</param>
      /// <param name="title">Title text of the balloon tip.</param>
      public static void Show( TextBox textBox, string body, string title )
      {
         TextBoxBalloonTip.Show( textBox.Handle, body, title );
      }

      /// <summary>
      /// Shows a balloon tip.
      /// </summary>
      /// <param name="textBoxHandle">Handle of the TextBox that the balloon will be displayed over.</param>
      /// <param name="body">Body text of the balloon tip.</param>
      public static void Show( IntPtr textBoxHandle, string body )
      {
         TextBoxBalloonTip.Show( textBoxHandle, body, string.Empty, TextBoxBalloonTip.Icon.None );
      }

      /// <summary>
      /// Shows a balloon tip.
      /// </summary>
      /// <param name="textBox">TextBox that the balloon will be displayed over.</param>
      /// <param name="body">Body text of the balloon tip.</param>
      public static void Show( TextBox textBox, string body )
      {
         TextBoxBalloonTip.Show( textBox.Handle, body, string.Empty, TextBoxBalloonTip.Icon.None );
      }

      /// <summary>
      /// Hides a balloon tip that is being shown.
      /// </summary>
      /// <param name="textBoxHandle">Handle of the TextBox that the balloon is displayed over.</param>
      public static void Hide( IntPtr textBoxHandle )
      {
         if ( !TextBoxBalloonTip.SendMessage( textBoxHandle, EM_HIDEBALLOONTIP, IntPtr.Zero, IntPtr.Zero ) )
         {
            throw new Win32Exception( "SendMessage() failed to hide a TextBox balloon tip." );
         }
      }

      /// <summary>
      /// Hides a balloon tip that is being shown.
      /// </summary>
      /// <param name="textBoxHandle">TextBox that the balloon is displayed over.</param>
      public static void Hide( TextBox textBox )
      {
         TextBoxBalloonTip.Hide( textBox.Handle );
      }
   }
}
