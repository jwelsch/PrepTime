using System;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PrepTime
{
   /// <summary>
   /// Compares list view items.
   /// </summary>
   internal class TaskComparer : IComparer
   {
      #region IComparer Members

      /// <summary>
      /// Performs a case-sensitive comparison of two objects of the same type and returns a value indicating whether one is less than, equal to, or greater than the other.
      /// </summary>
      /// <param name="a">The first object to compare.</param>
      /// <param name="b">The second object to compare.</param>
      /// <returns>A signed integer that indicates the relative values of a and b.
      /// Less than zero: a is less than b
      /// Zero: a equals b
      /// Greater than zero: a is greater than b</returns>
      public int Compare( Object a, Object b )
      {
         var aTask = (ITask) ( (ListViewItem) a ).Tag;
         var bTask = (ITask) ( (ListViewItem) b ).Tag;

         if ( aTask.BeginTime < bTask.BeginTime )
         {
            return -1;
         }
         else if ( aTask.BeginTime > bTask.BeginTime )
         {
            return 1;
         }

         return 0;
      }

      #endregion

   }

   internal class Comparer<T> : IComparer<T>
      where T : ITask
   {
      #region IComparer<Task> Members

      public int Compare( T x, T y )
      {
         if ( x.BeginTime < y.BeginTime )
         {
            return -1;
         }
         else if ( x.BeginTime > y.BeginTime )
         {
            return 1;
         }

         return 0;
      }

      #endregion
   }
}
