using System;
using System.Windows.Forms;

namespace PrepTime
{
   internal static class Program
   {
      /// <summary>
      /// Gets the data model for the application.
      /// </summary>
      public static DataModel DataModel
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets the controller for the application.
      /// </summary>
      public static Controller Controller
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets the main form for the application.
      /// </summary>
      public static MainForm MainForm
      {
         get;
         private set;
      }

      /// <summary>
      /// Static constructor.
      /// </summary>
      static Program()
      {
         Program.CreateNewDataModel();
      }

      /// <summary>
      /// Creates a new data model.
      /// </summary>
      public static void CreateNewDataModel()
      {
         SequentialNumber.SetNextValue( typeof( Entity ), 0 );

         Program.DataModel = new DataModel();
         Program.Controller = new Controller();
         Program.Controller.DishAdd( "Dish" );
      }

      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      private static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault( false );
         Program.MainForm = new MainForm();
         Application.Run( Program.MainForm );
      }
   }
}
