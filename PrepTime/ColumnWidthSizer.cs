using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PrepTime
{
   /// <summary>
   /// Contains methods for setting the widths of columns.
   /// </summary>
   public class ColumnWidthSizer
   {
      /// <summary>
      /// ListView containing the columns to size.
      /// </summary>
      private ListView listView;

      /// <summary>
      /// True to ensure that there is horizontal space for a vertical scroll bar, false otherwise.
      /// </summary>
      private bool accommodateScrollBar;

      /// <summary>
      /// The indexes of the columns to size.
      /// </summary>
      private int[] indexesToSize;

      /// <summary>
      /// Determines whether or not to ignore a resize event in the list view.
      /// </summary>
      private bool ignoreResize = false;

      /// <summary>
      /// Creates an object of type ColumnWidthSizer.
      /// </summary>
      /// <param name="listView">ListView containing the columns to size.</param>
      /// <param name="accommodateScrollBar">True to ensure that there is horizontal space for a vertical scroll bar, false otherwise.</param>
      public ColumnWidthSizer( ListView listView, bool accommodateScrollBar )
      {
         // Validate input.
         if ( listView == null )
         {
            throw new ArgumentNullException( "listView" );
         }

         // Remember the parameters.
         this.listView = listView;
         this.accommodateScrollBar = accommodateScrollBar;

         // Register for the list view's resize event.
         this.listView.Resize += delegate( object sender, EventArgs e )
         {
            if ( !this.ignoreResize )
            {
               this.SizeNow();
            }
         };

         // Register for the list view's column width changing event.
         this.listView.ColumnWidthChanging += delegate( object sender, ColumnWidthChangingEventArgs e )
         {
            // Do not process resize events while the column width is changing.
            this.ignoreResize = true;
         };

         // Register for the list view's column width changing event.
         this.listView.ColumnWidthChanged += delegate( object sender, ColumnWidthChangedEventArgs e )
         {
            // The column width is done changing, so resume processing resize events.
            this.ignoreResize = false;
         };
      }

      /// <summary>
      /// Creates an object of type ColumnWidthSizer.
      /// </summary>
      /// <param name="listView">ListView containing the columns to size.</param>
      /// <param name="accommodateScrollBar">True to ensure that there is horizontal space for a vertical scroll bar, false otherwise.</param>
      /// <param name="indexesToSize">The indexes of the columns to size.</param>
      public ColumnWidthSizer( ListView listView, bool accommodateScrollBar, int[] indexesToSize )
         : this( listView, accommodateScrollBar )
      {
         // Validate input.
         if ( indexesToSize == null )
         {
            throw new ArgumentNullException( "indexesToSize" );
         }

         this.indexesToSize = indexesToSize;
      }

      /// <summary>
      /// Adjust the widths of the columns now.
      /// </summary>
      public void SizeNow()
      {
         if ( this.indexesToSize == null )
         {
            ColumnWidthSizer.AllocateHorizontalSpace( this.listView, this.accommodateScrollBar );
         }
         else
         {
            ColumnWidthSizer.AllocateHorizontalSpace( this.listView, this.accommodateScrollBar, this.indexesToSize );
         }
      }

      /// <summary>
      /// Evenly allocates the available horizontal space amongst all the columns in the ListView.
      /// </summary>
      /// <param name="listView">ListView containing the columns to size.</param>
      /// <param name="accommodateScrollBar">True to ensure that there is horizontal space for a vertical
      /// scroll bar, false otherwise.</param>
      public static void AllocateHorizontalSpace( ListView listView, bool accommodateScrollBar )
      {
         // Validate input.
         if ( listView == null )
         {
            throw new ArgumentNullException( "listView" );
         }

         // Do nothing if no columns are in the ListView.
         if ( listView.Columns.Count <= 0 )
         {
            return;
         }

         //
         // Create an array of all the column indexes since this method distributes
         // the available width evenly amongst all columns.
         //

         var columns = new int[listView.Columns.Count];

         for ( var i = 0; i < columns.Length; i++ )
         {
            columns[i] = i;
         }

         // Size the columns.
         ColumnWidthSizer.AllocateHorizontalSpace( listView, accommodateScrollBar, columns );
      }

      /// <summary>
      /// Evenly allocates the available horizontal space amongst specified columns in the ListView.
      /// </summary>
      /// <param name="listView">ListView containing the columns to size.</param>
      /// <param name="accommodateScrollBar">True to ensure that there is horizontal space for a vertical
      /// scroll bar, false otherwise.</param>
      /// <param name="indexesToSize">The indexes of the columns to size.</param>
      public static void AllocateHorizontalSpace( ListView listView, bool accommodateScrollBar, int[] indexesToSize )
      {
         // Validate input.
         if ( listView == null )
         {
            throw new ArgumentNullException( "listView" );
         }

         // Do nothing if no columns are in the ListView.
         if ( listView.Columns.Count <= 0 )
         {
            return;
         }

         // The assumed width of the ListView used in width calculations.
         var listViewWidth = listView.ClientSize.Width;

         // Container for the columns that will be allocated horizontal space.
         var columns = new List<ColumnHeader>();

         // Check if there are any columns to size.
         if ( ( indexesToSize != null ) && ( indexesToSize.Length > 0 ) )
         {
            // Two tasks need to be done:
            // 1: Remove the widths of the columns NOT to be sized from the available horizontal space.
            // 2: Add the columns that will be resized to a collection so they can be resized separately.
            for ( var i = 0; i < listView.Columns.Count; i++ )
            {
               // Controls whether the width of the column is excluded from the available horizontal space
               // used in the sizing calculations.
               var excludeWidth = true;

               // Attempt to match the column index to the column indexes that are supposed to be resized.
               for ( var j = 0; j < indexesToSize.Length; j++ )
               {
                  // Check if the index is one that is supposed to be resized.
                  if ( indexesToSize[j] == i )
                  {
                     // Add the column to be resized to the separate collection.
                     columns.Add( listView.Columns[i] );

                     // Do not exclude this column's width from the available horizontal space.
                     excludeWidth = false;
                     break;
                  }
               }

               // If the column's width is not to be included in the available horizontal space,
               // exclude it from the assumed width.
               if ( excludeWidth )
               {
                  listViewWidth -= listView.Columns[i].Width;
               }
            }
         }

         // Handle the vertical scroll bar, if any.
         listViewWidth += ColumnWidthSizer.AdjustListViewWidthForScrollBar( listView, accommodateScrollBar );

         // Do not let the ListView redraw itself while its columns are being resized.
         listView.BeginUpdate();

         try
         {
            // Size the columns.
            ColumnWidthSizer.DistributeWidth( columns.ToArray(), listViewWidth );
         }
         finally
         {
            // Allow the ListView to update itself now the the columns have been resized.
            listView.EndUpdate();
         }
      }

      /// <summary>
      /// Returns an adjustment to apply to the assumed ListView width based on how a vertical
      /// scroll bar should be handled.
      /// </summary>
      /// <param name="listView">ListView whose assumed width will be given an adjustment for.</param>
      /// <param name="accommodateScrollBar">Whether or not to accommodate a vertical scroll bar.</param>
      /// <returns>An adjustment to apply to an assumed ListView width in pixels.</returns>
      private static int AdjustListViewWidthForScrollBar( ListView listView, bool accommodateScrollBar )
      {
         var widthDelta = 0;

         // If a scroll bar is visible, the client width of the ListView already takes it into account,
         // so no special handling is needed.
         // Check if the caller wants to leave space for a vertical scroll bar.
         if ( !ScrollBarInfo.IsVerticalScrollBarVisible( listView ) && accommodateScrollBar )
         {
            // Reduce the assumed width of the ListView to accommodate the width of a vertical scroll bar.
            widthDelta -= ScrollBarInfo.VerticalScrollBarWidth();
         }

         return widthDelta;
      }

      /// <summary>
      /// Distribute the given width amongst the specified columns.
      /// </summary>
      /// <param name="columns">Columns to distribute the width among.</param>
      /// <param name="assumedListViewWidth">The width of the ListView to use in the calculations.</param>
      private static void DistributeWidth( ColumnHeader[] columns, int assumedListViewWidth )
      {
         // Validate input.
         if ( columns == null )
         {
            throw new ArgumentNullException( "columns" );
         }

         // Do nothing if no columns are to be operated on.
         if ( columns.Length == 0 )
         {
            return;
         }

         // Check that the ListView is wide enough to display the columns.
         if ( assumedListViewWidth <= 0 )
         {
            throw new ArgumentException( "The ListView is not wide enough to display the columns.", "listView" );
         }

         // Calculate how many pixels will be left after trying to evenly distribute the width of the
         // ListView among all the columns.
         var remainder = assumedListViewWidth % columns.Length;

         // Set the widths of all the columns.
         for ( var i = 0; i < columns.Length; i++ )
         {
            // Evenly allocate space for each column.  Keep adding an extra pixel until all the extra pixels
            // have been distributed.
            columns[i].Width = ( assumedListViewWidth / columns.Length ) + ( ( remainder > 0 ) ? 1 : 0 );

            // Reduce the remaining pixels, if any, by 1.
            if ( remainder > 0 )
            {
               remainder--;
            }
         }
      }
   }
}
