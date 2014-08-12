using System;
using System.Text;
using PrepTime.Properties;

namespace PrepTime
{
   /// <summary>
   /// Contains methods for parsing an interval.
   /// </summary>
   public static class IntervalParser
   {
      /// <summary>
      /// Parses a string representation of an interval.
      /// </summary>
      /// <param name="intervalText">String representation of an interval.</param>
      /// <returns>TimeSpan object representing the interval.</returns>
      public static TimeSpan Parse( string intervalText )
      {
         var colon = false;
         var firstBuilder = new StringBuilder();
         var secondBuilder = new StringBuilder();

         foreach ( var c in intervalText )
         {
            if ( c == ':' )
            {
               if ( colon )
               {
                  throw new ArgumentException( Resources.ErrorIntervalStringMoreThanOneColon );
               }

               colon = true;
            }
            else if ( Char.IsDigit( c ) )
            {
               if ( colon )
               {
                  secondBuilder.Append( c );
               }
               else
               {
                  firstBuilder.Append( c );
               }
            }
            else
            {
               throw new ArgumentException( Resources.ErrorIntervalStringUnknownCharacter );
            }
         }

         var hours = 0;
         var minutes = 0;

         if ( colon )
         {
            hours = ( ( firstBuilder.ToString() == String.Empty ) ? 0 : Int32.Parse( firstBuilder.ToString() ) );
            minutes = ( ( secondBuilder.ToString() == String.Empty ) ? 0 : Int32.Parse( secondBuilder.ToString() ) );
         }
         else
         {
            minutes = ( ( firstBuilder.ToString() == String.Empty ) ? 0 : Int32.Parse( firstBuilder.ToString() ) );
         }

         return new TimeSpan( hours, minutes, 0 );
      }
   }
}
