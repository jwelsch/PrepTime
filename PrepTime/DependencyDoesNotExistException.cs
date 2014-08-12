using System;

namespace PrepTime
{
   /// <summary>
   /// Thrown when a dependency does not exist.
   /// </summary>
   [Serializable]
   internal class DependencyDoesNotExistException : Exception
   {
      /// <summary>
      /// Constructs an object of type DependencyDoesNotExistException.
      /// </summary>
      public DependencyDoesNotExistException()
      {
      }

      /// <summary>
      /// Constructs an object of type DependencyDoesNotExistException.
      /// </summary>
      /// <param name="message">The message associated with the exception.</param>
      public DependencyDoesNotExistException( string message )
         : this( message, null )
      {
      }

      /// <summary>
      /// Constructs an object of type DependencyDoesNotExistException.
      /// </summary>
      /// <param name="message">The message associated with the exception.</param>
      /// <param name="innerException">Another exception associated with the DependencyDoesNotExistException.</param>
      public DependencyDoesNotExistException( string message, Exception innerException )
         : base( message, innerException )
      {
      }

      /// <summary>
      /// Constructs a DependencyDoesNotExistException object.
      /// </summary>
      /// <param name="info">The serialization information.</param>
      /// <param name="context">The streaming context.</param>
      protected DependencyDoesNotExistException( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context )
         : base( info, context )
      {
      }
   }
}
