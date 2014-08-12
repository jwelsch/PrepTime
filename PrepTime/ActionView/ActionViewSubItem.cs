using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace PrepTime
{
   /// <summary>
   /// Represents an item that executes an action in an ActionView.
   /// </summary>
   public partial class ActionViewItem : ListViewItem
   {
      #region Activate Delegate

      /// <summary>
      /// Arguments passed when an sub item is activated.
      /// </summary>
      public class ActivateEventArgs
      {
         private ActionView container;
         /// <summary>
         /// Gets the ActionView that contains the sub item.
         /// </summary>
         public ActionView Container
         {
            get { return this.container; }
         }

         private Rectangle bounds;
         /// <summary>
         /// Gets the bounds of the sub item that was clicked.
         /// </summary>
         public Rectangle Bounds
         {
            get { return this.bounds; }
         }

         private object data;
         /// <summary>
         /// User specified data passed to the activate handler.
         /// </summary>
         public object Data
         {
            get { return this.data; }
         }

         /// <summary>
         /// Creates an object of type ActivateEventArgs.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         /// <param name="bounds">Bounds of the sub item that was clicked.</param>
         /// <param name="data">User specified data passed to the activate handler.</param>
         public ActivateEventArgs( ActionView container, Rectangle bounds, object data )
         {
            this.container = container;
            this.bounds = bounds;
            this.data = data;
         }

         /// <summary>
         /// Creates an object of type ActivateEventArgs.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         /// <param name="bounds">Bounds of the sub item that was clicked.</param>
         public ActivateEventArgs( ActionView container, Rectangle bounds )
            : this( container, bounds, null )
         {
         }
      }

      /// <summary>
      /// Delegate that is called to handle an activation event.
      /// </summary>
      /// <param name="sender">Caller.</param>
      /// <param name="e">Event arguments.</param>
      public delegate void ActivateHandler( object sender, ActivateEventArgs e );

      #endregion

      #region Deactivate Delegate

      /// <summary>
      /// Arguments passed when an sub item is deactivated.
      /// </summary>
      public class DeactivateEventArgs
      {
         private ActionView container;
         /// <summary>
         /// Gets the ActionView that contains the sub item.
         /// </summary>
         public ActionView Container
         {
            get { return this.container; }
         }

         private object data;
         /// <summary>
         /// User specified data passed to the activate handler.
         /// </summary>
         public object Data
         {
            get { return this.data; }
         }

         /// <summary>
         /// Creates an object of type DeactivateEventArgs.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         /// <param name="data">User specified data passed to the activate handler.</param>
         public DeactivateEventArgs( ActionView container, object data )
         {
            this.container = container;
            this.data = data;
         }

         /// <summary>
         /// Creates an object of type DeactivateEventArgs.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         public DeactivateEventArgs( ActionView container )
            : this( container, null )
         {
         }
      }

      /// <summary>
      /// Delegate that is called to handle a deactivation event.
      /// </summary>
      /// <param name="sender">Caller.</param>
      /// <param name="e">Event arguments.</param>
      public delegate void DeactivateHandler( object sender, DeactivateEventArgs e );

      #endregion

      /// <summary>
      /// Represents a sub item that executes an action in an ActionView.
      /// </summary>
      public class ActionViewSubItem : ListViewItem.ListViewSubItem
      {
         /// <summary>
         /// Fires when the sub item is activated.
         /// </summary>
         public event ActivateHandler Activated;

         /// <summary>
         /// Fires when the sub item is deactivated.
         /// </summary>
         public event DeactivateHandler Deactivated;

         private bool awaitingDeactivation;
         /// <summary>
         /// Gets whether or not the subitem is awaiting deactivation.
         /// </summary>
         public bool AwaitingDeactivation
         {
            get { return this.awaitingDeactivation; }
            protected set { this.awaitingDeactivation = value; }
         }

         /// <summary>
         /// Activates the sub item.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         /// <param name="bounds">Bounding rectangle of the sub item.</param>
         public void Activate( ActionView container, Rectangle bounds )
         {
            this.Activate( container, bounds, null );
         }

         /// <summary>
         /// Activates the sub item.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         /// <param name="bounds">Bounding rectangle of the sub item.</param>
         /// <param name="data">Data to pass to the deactivation handler.</param>
         public void Activate( ActionView container, Rectangle bounds, object data )
         {
            this.OnActivate( container, bounds, data );
         }

         /// <summary>
         /// Executes a custom activation action for the sub item.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         /// <param name="bounds">Bounding rectangle of the sub item.</param>
         /// <param name="data">Data to pass to the deactivation handler.</param>
         protected virtual void OnActivate( ActionView container, Rectangle bounds, object data )
         {
            if ( this.Activated != null )
            {
               this.Activated.Invoke( this, new ActivateEventArgs( container, bounds, data ) );
            }
         }

         /// <summary>
         /// Deactivates the sub item.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         public void Deactivate( ActionView container )
         {
            this.Deactivate( container, null );
         }

         /// <summary>
         /// Deactivates the sub item.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         /// <param name="data">Data to pass to the deactivation handler.</param>
         public void Deactivate( ActionView container, object data )
         {
            this.OnDeactivate( container, data );
         }

         /// <summary>
         /// Executes a custom deactivation action for the sub item.
         /// </summary>
         /// <param name="container">ActionView that contains the sub item.</param>
         /// <param name="data">Data to pass to the deactivation handler.</param>
         protected virtual void OnDeactivate( ActionView container, object data )
         {
            if ( this.Deactivated != null )
            {
               this.Deactivated.Invoke( this, new DeactivateEventArgs( container, data ) );
            }
         }

         /// <summary>
         /// Gets the parent ActionView of the ActionViewSubItem.
         /// </summary>
         /// <returns>ActionView that is the parent of the ActionViewSubItem.</returns>
         protected ActionView GetParentActionView()
         {
            var type = typeof( ListViewItem.ListViewSubItem );
            var fieldInfo = type.GetField( "owner", BindingFlags.NonPublic | BindingFlags.Instance );

            if ( fieldInfo == null )
            {
               throw new Exception( "Unable to get ListViewSubItem owner field." );
            }

            var listViewItem = (ListViewItem) fieldInfo.GetValue( this );

            if ( listViewItem == null )
            {
               throw new Exception( "ListViewSubItem is not in a ListViewItem." );
            }

            return (ActionView) listViewItem.ListView;
         }
      }
   }
}
