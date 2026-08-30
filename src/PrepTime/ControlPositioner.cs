using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PrepTime
{
   #region FixedOffsets enum

   /// <summary>
   /// Bit flags indicating which side of the object will remain fixed relative to
   /// the offset to its parent.
   /// </summary>
   [Flags]
   public enum FixedOffsets
   {
      /// <summary>
      /// Do not fix any offets.
      /// </summary>
      None = 0x0000,

      /// <summary>
      /// Fix the top offset.
      /// </summary>
      Top = 0x0001,

      /// <summary>
      /// Fix the bottom offset.
      /// </summary>
      Bottom = 0x0002,

      /// <summary>
      /// Fix the left offset.
      /// </summary>
      Left = 0x0004,

      /// <summary>
      /// Fix the right offset.
      /// </summary>
      Right = 0x0008,

      /// <summary>
      /// Fix all offsets.
      /// </summary>
      All = Top | Bottom | Left | Right,

      /// <summary>
      /// Fix left and right sides.
      /// </summary>
      LeftRight = Left | Right,

      /// <summary>
      /// Fix top and bottom sides.
      /// </summary>
      TopBottom = Top | Bottom,

      /// <summary>
      /// Fix top and left sides.
      /// </summary>
      TopLeft = Top | Left,

      /// <summary>
      /// Fix top and right sides.
      /// </summary>
      TopRight = Top | Right,

      /// <summary>
      /// Fix bottom and left sides.
      /// </summary>
      BottomLeft = Bottom | Left,

      /// <summary>
      /// Fix bottom and left sides.
      /// </summary>
      BottomRight = Bottom | Right,

      /// <summary>
      /// Fix top, left, and right sides.
      /// </summary>
      TopLeftRight = Top | Left | Right,

      /// <summary>
      /// Fix top, bottom, and left sides.
      /// </summary>
      TopBottomLeft = Top | Bottom | Left,

      /// <summary>
      /// Fix bottom, left, and right sides.
      /// </summary>
      BottomLeftRight = Bottom | Left | Right,

      /// <summary>
      /// Fix top, bottom, and right sides.
      /// </summary>
      TopBottomRight = Top | Bottom | Right
   }

   #endregion

   /// <summary>
   /// Manages the correct position and size of a group of controls relative to a common parent.
   /// </summary>
   public class ControlPositionManager
   {
      /// <summary>
      /// The default value of the auto position field.
      /// </summary>
      private const bool DefaultAutoPosition = false;

      /// <summary>
      /// Control that is a common parent to all children.
      /// </summary>
      private Control parent;

      private bool autoPosition = ControlPositionManager.DefaultAutoPosition;
      /// <summary>
      /// Gets or sets whether to automatically position the child controls when the parent is resized.
      /// Defaults to false.
      /// </summary>
      public bool AutoPosition
      {
         get { return this.autoPosition; }
         set
         {
            // Unsubscribe the the parent's resize event if there is a parent and the
            // event was subscribed to already and the new value is to unsubscribe.
            if ( ( this.parent != null ) && this.autoPosition && !value )
            {
               this.parent.Resize -= new EventHandler( OnParentResize );
            }

            this.autoPosition = value;

            // Subscribe to the parent's resize event if there is a parent and the
            // the new value is to subscribe.
            if ( ( this.parent != null ) && this.autoPosition )
            {
               this.parent.Resize += new EventHandler( OnParentResize );
            }
         }
      }

      /// <summary>
      /// List of controls to manage.
      /// </summary>
      private List<ControlPositioner> controls = new List<ControlPositioner>();

      /// <summary>
      /// Creates an object of type ControlPositionManager.
      /// </summary>
      public ControlPositionManager()
      {
      }

      /// <summary>
      /// Creates an object of type ControlPositionManager.
      /// </summary>
      /// <param name="parent">Control that is a common parent to all children.</param>
      public ControlPositionManager( Control parent )
         : this( parent, ControlPositionManager.DefaultAutoPosition )
      {
      }

      /// <summary>
      /// Creates an object of type ControlPositionManager.
      /// </summary>
      /// <param name="parent">Control that is a common parent to all children.</param>
      /// <param name="autoPosition">True to automatically position the child controls when the parent is
      /// resized, false otherwise.</param>
      public ControlPositionManager( Control parent, bool autoPosition )
      {
         this.SetParent( parent, autoPosition );
      }

      /// <summary>
      /// Creates an object of type ControlPositionManager.
      /// </summary>
      /// <param name="parent">Control that is a common parent to all children.</param>
      /// <param name="childPositioners">Array containing ControlPositioners to manage.</param>
      /// <param name="autoPosition">True to automatically position the child controls when the parent is
      /// resized, false otherwise.</param>
      public ControlPositionManager( Control parent, ControlPositioner[] childPositioners, bool autoPosition )
      {
         this.SetParent( parent, autoPosition );

         foreach ( var child in childPositioners )
         {
            this.AddChild( child );
         }
      }

      /// <summary>
      /// Sets the parent control.
      /// </summary>
      /// <param name="parent">Control that is a common parent to all children.</param>
      public void SetParent( Control parent )
      {
         this.SetParent( parent, this.AutoPosition );
      }

      /// <summary>
      /// Sets the parent control.
      /// </summary>
      /// <param name="parent">Control that is a common parent to all children.</param>
      /// <param name="autoPosition">True to automatically position the child controls when the parent is
      /// resized, false otherwise.</param>
      public void SetParent( Control parent, bool autoPosition )
      {
         // Do not allow a null parent.
         if ( parent == null )
         {
            throw new ArgumentNullException();
         }

         // If the current parent control is not null, things must be done before
         // another parent can be set.
         if ( this.parent != null )
         {
            if ( parent != this.parent )
            {
               // Make sure that the parent control is not being changed if there are child
               // controls that have already been added.  All child controls must have the
               // same parent.
               if ( this.controls.Count > 0 )
               {
                  throw new InvalidOperationException( "Cannot change parents after adding child controls." );
               }

               // Unsubscribe to the parent is changing and this object is subscribed to
               // the parent's resize event.
               if ( this.autoPosition )
               {
                  this.parent.Resize -= new EventHandler( OnParentResize );
               }
            }
         }

         this.parent = parent;
         this.AutoPosition = autoPosition;
      }

      /// <summary>
      /// Called when the parent is resized.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      void OnParentResize( object sender, EventArgs e )
      {
         // Reposition the controls.
         this.Reposition();
      }

      /// <summary>
      /// Adds a child control to be managed.
      /// </summary>
      /// <param name="child">Child, of the parent control, to manage.</param>
      /// <param name="offsets">Offsets from the child's edges to the parent's edges that will remain fixed.</param>
      public void AddChild( Control child, FixedOffsets offsets )
      {
         this.AddChild( new ControlPositioner( child, offsets ) );
      }

      /// <summary>
      /// Adds a child control to be managed.
      /// </summary>
      /// <param name="controlPositioner">Object used to maintain the control's correct position.</param>
      public void AddChild( ControlPositioner controlPositioner )
      {
         // Watch for null arguments.
         if ( controlPositioner == null )
         {
            throw new ArgumentNullException( "controlPositioner" );
         }

         // Make sure there is a parent control.
         if ( this.parent == null )
         {
            throw new InvalidOperationException( "A parent control must be set before any children are added." );
         }

         // Make sure the parent controls are the same.
         if ( this.parent != controlPositioner.ParentControl )
         {
            throw new InvalidOperationException( "The parent control of the child differs from what is set in the object." );
         }

         this.controls.Add( controlPositioner );
      }

      /// <summary>
      /// Removes a child control.
      /// </summary>
      /// <param name="childControl">Child control to remove.</param>
      public void RemoveChild( Control childControl )
      {
         // Do not allow a null child control.
         if ( childControl == null )
         {
            throw new ArgumentNullException( "childControl" );
         }

         // Find the child control to remove.
         for ( var i = 0; i < this.controls.Count; i++ )
         {
            if ( this.controls[i].ChildControl == childControl )
            {
               this.controls.RemoveAt( i );
               return;
            }
         }

         throw new ArgumentOutOfRangeException( "childControl", "The child control was not found." );
      }

      /// <summary>
      /// Removes all the children.
      /// </summary>
      public void RemoveAll()
      {
         this.controls.Clear();
      }

      /// <summary>
      /// Repositions all the child controls.
      /// </summary>
      public void Reposition()
      {
         // Do not do layout operations until all children have been repositioned.
         this.parent.SuspendLayout();

         // Reposition each child control.
         foreach ( var positioner in this.controls )
         {
            positioner.Reposition();
         }

         // Resume layout operations.
         this.parent.ResumeLayout();
      }
   }

   /// <summary>
   /// Maintains the correct position and size of a control relative to its parent.
   /// </summary>
   public class ControlPositioner
   {
      private Control childControl;
      /// <summary>
      /// Gets the control whose position to maintain.
      /// </summary>
      public Control ChildControl
      {
         get { return this.childControl; }
      }

      /// <summary>
      /// Gets the parent of the control.
      /// </summary>
      public Control ParentControl
      {
         get { return this.childControl.Parent; }
      }

      private FixedOffsets fixedOffsets;
      /// <summary>
      /// Gets the offsets from the child's edges to the parent's edges that will remain fixed.
      /// </summary>
      public FixedOffsets FixedOffsets
      {
         get { return this.fixedOffsets; }
      }

      /// <summary>
      /// The offsets of the child control relative to the parent control.
      /// </summary>
      private Offset childOffset;

      /// <summary>
      /// Creates an object of type ControlPositioner.
      /// </summary>
      /// <param name="childControl">The control whose position to maintain.</param>
      /// <param name="offsets">Offsets from the child's edges to the parent's edges that will remain fixed.</param>
      public ControlPositioner( Control childControl, FixedOffsets offsets )
      {
         // Do not allow a null child control.
         if ( childControl == null )
         {
            throw new ArgumentNullException( "childControl" );
         }

         this.childControl = childControl;
         this.fixedOffsets = offsets;

         // Store the offsets of the child from the parent.
         this.childOffset = new Offset();
         this.childOffset.Top = this.childControl.Top - this.ChildControl.Parent.ClientRectangle.Top;
         this.childOffset.Bottom = this.childControl.Bottom - this.ChildControl.Parent.ClientRectangle.Bottom;
         this.childOffset.Left = this.childControl.Left - this.ChildControl.Parent.ClientRectangle.Left;
         this.childOffset.Right = this.childControl.Right - this.ChildControl.Parent.ClientRectangle.Right;
      }

      /// <summary>
      /// Reposition the child control.
      /// </summary>
      public void Reposition()
      {
         // Default to the current position of the child control.
         var newTop = this.childControl.Top;
         var newBottom = this.childControl.Bottom;
         var newLeft = this.childControl.Left;
         var newRight = this.childControl.Right;

         // Check if the top offset should remain fixed.
         if ( FixedOffsets.Top == ( FixedOffsets.Top & this.fixedOffsets ) )
         {
            newTop = this.childOffset.Top - this.ParentControl.ClientRectangle.Top;
         }

         // Check if the bottom offset should remain fixed.
         if ( FixedOffsets.Bottom == ( FixedOffsets.Bottom & this.fixedOffsets ) )
         {
            newBottom = this.ParentControl.ClientRectangle.Height + this.childOffset.Bottom;

            // If the top offset is also supposed to be fixed, calculate its offset
            // so the height doesn't change.
            if ( 0 == ( FixedOffsets.Top & this.fixedOffsets ) )
            {
               newTop = newBottom - this.childControl.Height;
            }
         }

         // Check if the left offset should remain fixed.
         if ( FixedOffsets.Left == ( FixedOffsets.Left & this.fixedOffsets ) )
         {
            newLeft = this.childOffset.Left - this.ParentControl.ClientRectangle.Left;
         }

         // Check if the right offset should remain fixed.
         if ( FixedOffsets.Right == ( FixedOffsets.Right & this.fixedOffsets ) )
         {
            newRight = this.ParentControl.ClientRectangle.Width + this.childOffset.Right;

            // If the left offset is also supposed to be fixed, calculate its offset
            // so the width doesn't change.
            if ( 0 == ( FixedOffsets.Left & this.fixedOffsets ) )
            {
               newLeft = newRight - this.childControl.Width;
            }
         }

         // Reposition the child control.
         this.childControl.Location = new Point( newLeft, newTop );
         this.childControl.Size = new Size( newRight - newLeft, newBottom - newTop );

         //System.Diagnostics.Trace.WriteLine( String.Format( "Control: {0} - Location: {1}, Size: {2}", this.childControl, this.childControl.Location, this.childControl.Size ) );
      }

      #region Offset class

      /// <summary>
      /// Controls offsets for the four sides of an object.
      /// </summary>
      private class Offset
      {
         /// <summary>
         /// Top offset.
         /// </summary>
         public int Top
         {
            get;
            set;
         }

         /// <summary>
         /// Bottom offset.
         /// </summary>
         public int Bottom
         {
            get;
            set;
         }

         /// <summary>
         /// Left offset.
         /// </summary>
         public int Left
         {
            get;
            set;
         }

         /// <summary>
         /// Right offset.
         /// </summary>
         public int Right
         {
            get;
            set;
         }

         /// <summary>
         /// Creates an object of type Offset.
         /// </summary>
         public Offset()
         {
         }

         /// <summary>
         /// Creates an object of type Offset.
         /// </summary>
         /// <param name="top">Top offset.</param>
         /// <param name="bottom">Bottom offset.</param>
         /// <param name="left">Left offset.</param>
         /// <param name="right">Right offset.</param>
         public Offset( int top, int bottom, int left, int right )
         {
            this.Top = top;
            this.Bottom = bottom;
            this.Left = left;
            this.Right = right;
         }
      }

      #endregion
   }
}
