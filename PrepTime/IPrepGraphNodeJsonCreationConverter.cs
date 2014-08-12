using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PrepTime
{
   /// <summary>
   /// Create an instance of objectType, based properties in the JSON object
   /// </summary>
   /// <param name="objectType">type of object expected</param>
   /// <param name="jObject">contents of JSON object that will be deserialized</param>
   /// <returns></returns>
   public class IPrepGraphNodeJsonCreationConverter : JsonCreationConverter<PrepGraph.Node>
   {
      /// <summary>
      /// Create an instance of objectType, based properties in the JSON object.
      /// </summary>
      /// <param name="objectType">type of object expected.</param>
      /// <param name="jObject">contents of JSON object that will be deserialized.</param>
      /// <returns>Object of type T.</returns>
      protected override PrepGraph.Node Create( Type objectType, JObject jObject )
      {
         throw new NotImplementedException();
      }
   }
}
