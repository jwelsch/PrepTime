using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PrepTime
{
   /// <summary>
   /// Create an instance of objectType, based properties in the JSON object
   /// </summary>
   /// <param name="objectType">type of object expected</param>
   /// <param name="jObject">contents of JSON object that will be deserialized</param>
   /// <returns></returns>
   public class IDishJsonCreationConverter : JsonCreationConverter<IDish>
   {
      private List<Dish> dishList;

      public IDishJsonCreationConverter( List<Dish> dishList )
      {
         this.dishList = dishList;
      }

      /// <summary>
      /// Create an instance of objectType, based properties in the JSON object.
      /// </summary>
      /// <param name="objectType">type of object expected.</param>
      /// <param name="jObject">contents of JSON object that will be deserialized.</param>
      /// <returns>Object of type T.</returns>
      protected override IDish Create( Type objectType, JObject jObject )
      {
         if ( this.FieldExists( "$ref", jObject ) )
         {
            throw new Exception( "Attempted to deserialize $ref." );
         }

         var id = (int) jObject["ID"];

         foreach ( var dish in this.dishList )
         {
            if ( id == dish.ID )
            {
               return dish;
            }
         }

         var newDish = new Dish( id, (string) jObject["Name"] );
         this.dishList.Add( newDish );

         return newDish;
      }
   }
}
