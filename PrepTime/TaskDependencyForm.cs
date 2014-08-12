using System;
using System.Windows.Forms;
using System.Collections.Generic;
using PrepTime.Properties;

namespace PrepTime
{
   public partial class TaskDependencyForm : Form
   {
      /// <summary>
      /// Gets or sets the task whose dependencies will be edited.
      /// </summary>
      public ITask Task
      {
         get;
         set;
      }

      /// <summary>
      /// Gets the IDs of the dependencies of the task.
      /// </summary>
      public int[] DependencyIDs
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets or sets the entity graph.
      /// </summary>
      private PrepGraph Graph
      {
         get;
         set;
      }

      /// <summary>
      /// True to ignore the next item checked event, false otherwise.
      /// </summary>
      private bool ignoreNextItemChecked = false;

      public TaskDependencyForm()
      {
         InitializeComponent();
      }

      private void TaskDependencyForm_Load( object sender, EventArgs e )
      {
         try
         {
            var width = this.independentListView.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            this.independentListView.Columns.Add( String.Empty, width );

            if ( this.Task == null )
            {
               throw new Exception( "No task assigned." );
            }

            foreach ( var dish in Program.DataModel.Dishes )
            {
               if ( !Program.Controller.TaskCanHaveDependency( this.Task.ID, dish.ID ) )
               {
                  continue;
               }

               this.dishComboBox.Items.Add( dish );
            }

            this.Graph = new PrepGraph( Program.DataModel.DependencyGraph );

            if ( this.dishComboBox.Items.Count > 0 )
            {
               this.dishComboBox.SelectedIndex = 0;
            }
         }
         catch ( Exception ex )
         {
            System.Diagnostics.Trace.WriteLine( ex );
            PrepTimeMessageBox.ShowError( String.Format( "Failed to load dependencies\n{0}", ex.Message ) );
         }
      }

      private void dishComboBox_SelectedIndexChanged( object sender, EventArgs e )
      {
         var dish = (IDish) this.dishComboBox.SelectedItem;

         this.independentListView.ItemChecked -= this.independentListView_ItemChecked;
         this.LoadDishTasks( dish );
         this.independentListView.ItemChecked += this.independentListView_ItemChecked;
      }

      private void independentListView_ItemChecked( object sender, ItemCheckedEventArgs e )
      {
         if ( this.ignoreNextItemChecked )
         {
            this.ignoreNextItemChecked = false;
            return;
         }

         var task = (ITask) e.Item.Tag;

         if ( e.Item.Checked )
         {
            if ( !this.Graph.TryAddDependencies( this.Task.ID, new IEntity[] { task } ) )
            {
               PrepTimeMessageBox.ShowError( Resources.ErrorCircularDependency );
               this.ignoreNextItemChecked = true;
               e.Item.Checked = false;
            }
         }
         else
         {
            this.Graph.RemoveDependencies( this.Task.ID, new IEntity[] { task } );
         }
      }

      private void okButton_Click( object sender, EventArgs e )
      {
         this.DependencyIDs = this.Graph.GetDependencies( this.Task.ID );

         this.DialogResult = DialogResult.OK;
      }

      /// <summary>
      /// Loads the tasks of the specified dish.
      /// </summary>
      /// <param name="dish">Dish containing the tasks to load.</param>
      private void LoadDishTasks( IDish dish )
      {
         this.independentListView.Items.Clear();

         var dependencies = this.Graph.GetDependencies( this.Task.ID );

         for ( var i = 0; i < dish.Tasks.Count; i++ )
         {
            var task = dish.Tasks[i];

            if ( task.ID == this.Task.ID )
            {
               continue;
            }

            var item = new ListViewItem( String.Format( "{0}. {1}", i + 1, task.Description ) );
            item.Tag = task;

            this.independentListView.Items.Add( item );

            var tempArray = new IEntity[] { task };

            var alreadyDependency = Array.Exists<int>( dependencies, ( element ) =>
               {
                  return element == task.ID;
               } );

            if ( alreadyDependency )
            {
               item.Checked = true;
               continue;
            }

            if ( !this.Graph.TryAddDependencies( this.Task.ID, tempArray ) )
            {
               item.ForeColor = System.Drawing.SystemColors.GrayText;
            }

            this.Graph.RemoveDependencies( this.Task.ID, tempArray );

            //foreach ( var dependency in dependencies )
            //{
            //   if ( task.ID == dependency )
            //   {
            //      item.Checked = true;
            //      break;
            //   }
            //}
         }
      }
   }
}
