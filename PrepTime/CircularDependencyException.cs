using System;

namespace PrepTime
{
   /// <summary>
   /// Thrown when a circular dependency has been found.
   /// </summary>
   [Serializable]
   public class CircularDependencyException : Exception
   {
      /// <summary>
      /// Constructs an object of type CircularDependencyException.
      /// </summary>
      public CircularDependencyException()
         : this( String.Empty )
      {
      }

      /// <summary>
      /// Constructs an object of type CircularDependencyException.
      /// </summary>
      /// <param name="message">The message associated with the exception.</param>
      public CircularDependencyException( string message )
         : this( message, null )
      {
      }

      /// <summary>
      /// Constructs an object of type CircularDependencyException.
      /// </summary>
      /// <param name="message">The message associated with the exception.</param>
      /// <param name="innerException">Another exception associated with the CircularDependencyException.</param>
      public CircularDependencyException( string message, Exception innerException )
         : base( message, innerException )
      {
      }

      /// <summary>
      /// Constructs a CircularDependencyException object.
      /// </summary>
      /// <param name="info">The serialization information.</param>
      /// <param name="context">The streaming context.</param>
      protected CircularDependencyException( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context )
         : base( info, context )
      {
      }
   }
}
