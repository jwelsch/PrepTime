using System;
using System.Text;

namespace PrepTime
{
   public static class TimeSpanToString
   {
      public static string Format( TimeSpan interval )
      {
         var hours = ( interval.Days * 24 ) + interval.Hours;

         return String.Format( "{0:D2}:{1:D2}", hours, interval.Minutes );
      }
   }
}
