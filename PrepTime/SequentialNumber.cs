using System;
using System.Collections.Generic;

namespace PrepTime
{
   /// <summary>
   /// Gets a sequential set of numbers.
   /// </summary>
   internal class SequentialNumber
   {
      /// <summary>
      /// Tracks what numbers have been used.
      /// </summary>
      private int number = 0;

      /// <summary>
      /// Get the next sequential number.
      /// </summary>
      /// <returns>Next sequential number.</returns>
      public int Next()
      {
         return this.number++;
      }

      ///<summary>
      /// Creates an object of type SequentialNumber.
      /// </summary>
      public SequentialNumber()
      {
      }

      /// <summary>
      /// Tracks different SequentialNumber objects.
      /// </summary>
      private static Dictionary<object, SequentialNumber> map = new Dictionary<object,SequentialNumber>();

      /// <summary>
      /// Get the next sequential number for the object.
      /// </summary>
      /// <param name="key">Key to the sequential number.</param>
      /// <returns>Next sequential number.</returns>
      public static int Next( object key )
      {
         SequentialNumber sn = null;

         if ( !SequentialNumber.map.TryGetValue( key, out sn ) )
         {
            sn = new SequentialNumber();
            SequentialNumber.map.Add( key, sn );
         }

         return sn.Next();
      }

      /// <summary>
      /// Sets the next value in the sequence.
      /// </summary>
      /// <param name="key">Key to the sequential number.</param>
      /// <param name="value">Value to set.</param>
      public static void SetNextValue( object key, int value )
      {
         SequentialNumber sn = null;

         if ( !SequentialNumber.map.TryGetValue( key, out sn ) )
         {
            sn = new SequentialNumber();
            SequentialNumber.map.Add( key, sn );
         }

         sn.number = value;
      }
   }
}
