using System;
using System.Windows.Forms;
using PrepTime.Properties;
using System.Diagnostics;

namespace PrepTime
{
   /// <summary>
   /// Displays message boxes customized for the application.
   /// </summary>
   internal static class PrepTimeMessageBox
   {
      /// <summary>
      /// Gets the parent window.
      /// </summary>
      /// <returns>An interface to the parent window.</returns>
      private static IWin32Window GetParentWindow()
      {
         return new Win32Window( Program.MainForm.Handle );
      }

      /// <summary>
      /// Displays a message box.
      /// </summary>
      /// <param name="buttons">Button(s) to put on the message box.</param>
      /// <param name="icon">Icon to put on the message box.</param>
      /// <param name="text">Text to show.</param>
      /// <param name="args">Arguments with which to format the text.</param>
      /// <returns>The dialog result from the message box.</returns>
      private static DialogResult Show( MessageBoxButtons buttons, MessageBoxIcon icon, string text, params object[] args )
      {
         return MessageBox.Show( PrepTimeMessageBox.GetParentWindow(), String.Format( text, args ), Resources.ApplicationName, buttons, icon );
      }

      /// <summary>
      /// Displays a message box.
      /// </summary>
      /// <param name="buttons">Button(s) to put on the message box.</param>
      /// <param name="icon">Icon to put on the message box.</param>
      /// <param name="text">Text to show.</param>
      /// <param name="title">Form title.</param>
      /// <param name="args">Arguments with which to format the text.</param>
      /// <returns>The dialog result from the message box.</returns>
      private static DialogResult Show( MessageBoxButtons buttons, MessageBoxIcon icon, string text, string title, params object[] args )
      {
         return MessageBox.Show( PrepTimeMessageBox.GetParentWindow(), String.Format( text, args ), title, buttons, icon );
      }

      /// <summary>
      /// Displays a message box with an OK button.
      /// </summary>
      /// <param name="buttons">Button(s) to put on the message box.</param>
      /// <param name="icon">Icon to put on the message box.</param>
      /// <param name="text">Text to show.</param>
      /// <param name="args">Arguments with which to format the text.</param>
      /// <returns>The dialog result from the message box.</returns>
      public static DialogResult Show( string text, params object[] args )
      {
         return PrepTimeMessageBox.Show( MessageBoxButtons.OK, MessageBoxIcon.None, text, args );
      }

      /// <summary>
      /// Displays a message box with an OK button and an Error icon.
      /// </summary>
      /// <param name="text">Text to show.</param>
      /// <param name="args">Arguments with which to format the text.</param>
      /// <returns>The dialog result from the message box.</returns>
      public static DialogResult ShowError( string text, params object[] args )
      {
         return PrepTimeMessageBox.Show( MessageBoxButtons.OK, MessageBoxIcon.Error, text, String.Format( Resources.ErrorFormTitle, Resources.ApplicationName ), args );
      }

      /// <summary>
      /// Displays a message box with an OK button and an Information icon.
      /// </summary>
      /// <param name="text">Text to show.</param>
      /// <param name="args">Arguments with which to format the text.</param>
      /// <returns>The dialog result from the message box.</returns>
      public static DialogResult ShowInformation( string text, params object[] args )
      {
         return PrepTimeMessageBox.Show( MessageBoxButtons.OK, MessageBoxIcon.Information, text, args );
      }

      /// <summary>
      /// Displays a message box with an OK button and a Warning icon.
      /// </summary>
      /// <param name="text">Text to show.</param>
      /// <param name="args">Arguments with which to format the text.</param>
      /// <returns>The dialog result from the message box.</returns>
      public static DialogResult ShowWarning( string text, params object[] args )
      {
         return PrepTimeMessageBox.Show( MessageBoxButtons.OK, MessageBoxIcon.Warning, text, args );
      }

      /// <summary>
      /// Displays a message box with Yes and No buttons and a Question icon.
      /// </summary>
      /// <param name="text">Text to show.</param>
      /// <param name="args">Arguments with which to format the text.</param>
      /// <returns>The dialog result from the message box.</returns>
      public static DialogResult ShowQuestion( string text, params object[] args )
      {
         return PrepTimeMessageBox.Show( MessageBoxButtons.YesNo, MessageBoxIcon.Question, text, args );
      }

      /// <summary>
      /// Displays a message box with Yes, No, and Cancel buttons and a Question icon.
      /// </summary>
      /// <param name="text">Text to show.</param>
      /// <param name="args">Arguments with which to format the text.</param>
      /// <returns>The dialog result from the message box.</returns>
      public static DialogResult ShowQuestionCancel( string text, params object[] args )
      {
         return PrepTimeMessageBox.Show( MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, text, args );
      }
   }
}
