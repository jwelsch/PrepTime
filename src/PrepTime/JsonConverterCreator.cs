using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PrepTime
{
   /// <summary>
   /// Base class that can be overridden to create types of of type T.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public abstract class JsonCreationConverter<T> : JsonConverter
   {
      /// <summary>
      /// Create an instance of objectType, based properties in the JSON object.
      /// </summary>
      /// <param name="objectType">type of object expected.</param>
      /// <param name="jObject">contents of JSON object that will be deserialized.</param>
      /// <returns>Object of type T.</returns>
      protected abstract T Create( Type objectType, JObject jObject );

      public override bool CanConvert( Type objectType )
      {
         return typeof( T ).IsAssignableFrom( objectType );
      }

      public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
      {
         // Load JObject from stream.
         var jObject = JObject.Load( reader );

         // Create target object based on JObject.
         var target = this.Create( objectType, jObject );

         // Populate the object properties.
         serializer.Populate( jObject.CreateReader(), target );

         return target;
      }

      public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
      {
         throw new NotImplementedException();
      }

      protected bool FieldExists( string fieldName, JObject jObject )
      {
         return jObject[fieldName] != null;
      }
   }
}
