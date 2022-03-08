using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PrepTime
{
   /// <summary>
   /// Serializes a meal.
   /// </summary>
   internal class MealFileSerializer
   {
      ///<summary>
      /// Creates an object of type MealFileSaver.
      /// </summary>
      public MealFileSerializer()
      {
      }

      /// <summary>
      /// Saves the data model to the specified file path.
      /// </summary>
      /// <param name="filePath">Path to the file that will store the data model.</param>
      public void Save( string filePath )
      {
         using ( var stream = new StreamWriter( filePath ) )
         {
            using ( var writer = new JsonTextWriter( stream ) )
            {
               var serializer = new JsonSerializer();
               serializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
               serializer.Formatting = Formatting.Indented;

               serializer.Serialize( writer, Program.DataModel );
            }
         }
         //System.Diagnostics.Trace.WriteLine( "Save" );
         //Program.DataModel.DebugDump();
      }

      /// <summary>
      /// Saves the data model to the specified file path.
      /// </summary>
      /// <param name="filePath">Path to the file that will store the data model.</param>
      public void Load( string filePath )
      {
         using ( var stream = new StreamReader( filePath ) )
         {
            using ( var reader = new JsonTextReader( stream ) )
            {
               var dishList = new List<Dish>();

               var serializer = new JsonSerializer();
               serializer.Converters.Add( new IDishJsonCreationConverter( dishList ) );
               serializer.Converters.Add( new ITaskJsonCreationConverter( dishList ) );
               var dataModel = serializer.Deserialize<DataModel>( reader );

               var programType = typeof( Program );
               var method = programType.GetMethod( "set_DataModel", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic );
               method.Invoke( null, new object[] { dataModel } );

               var max = 0;

               foreach ( var dish in Program.DataModel.Dishes )
               {
                  if ( dish.ID > max )
                  {
                     max = dish.ID;
                  }

                  foreach ( var task in dish.Tasks )
                  {
                     if ( task.ID > max )
                     {
                        max = task.ID;
                     }
                  }
               }

               //System.Diagnostics.Trace.WriteLine( String.Format( "Max ID: {0}", max ) );
               SequentialNumber.SetNextValue( typeof( Entity ), max + 1 );
            }
         }
      }
   }
}
