using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace PrepTime
{
   /// <summary>
   /// Create an instance of objectType, based properties in the JSON object
   /// </summary>
   /// <param name="objectType">type of object expected</param>
   /// <param name="jObject">contents of JSON object that will be deserialized</param>
   /// <returns></returns>
   public class ITaskJsonCreationConverter : JsonCreationConverter<ITask>
   {
      private List<Dish> dishList;

      public ITaskJsonCreationConverter( List<Dish> dishList )
      {
         this.dishList = dishList;
      }

      /// <summary>
      /// Create an instance of objectType, based properties in the JSON object.
      /// </summary>
      /// <param name="objectType">type of object expected.</param>
      /// <param name="jObject">contents of JSON object that will be deserialized.</param>
      /// <returns>Object of type T.</returns>
      protected override ITask Create( Type objectType, JObject jObject )
      {
         var dishID = (int) jObject["DishID"];

         foreach ( var dish in this.dishList )
         {
            if ( dish.ID == dishID )
            {
               return (ITask) new Task( (int) jObject["ID"], (string) jObject["Description"], (TimeSpan) jObject["Interval"], dish.ID );
            }
         }

         throw new Exception( "Unknown parent dish in task." );
      }
   }
}
