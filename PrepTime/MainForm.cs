using System;
using System.Windows.Forms;
using System.Collections.Generic;
using PrepTime.Properties;
using System.IO;

namespace PrepTime
{
   public partial class MainForm : Form
   {
      /// <summary>
      /// Manages the control positions.
      /// </summary>
      private ControlPositionManager positionManager;

      /// <summary>
      /// The combo box that provides options for each task in the list view.
      /// </summary>
      private MenuComboBox taskOptionComboBox;

      /// <summary>
      /// Specifies the number of a new dish name.
      /// </summary>
      private static int defaultDishNumber = 0;

      /// <summary>
      /// Date/time format for the form.
      /// </summary>
      private const string DateTimeFormat = "hh:mm tt MMM dd, yyyy";

      /// <summary>
      /// The path of the loaded file.
      /// </summary>
      private string loadedFilePath = string.Empty;

      private bool loadedDataDirty = false;
      /// <summary>
      /// True if the loaded data is dirty, false otherwise.
      /// </summary>
      private bool LoadedDataDirty
      {
         get { return this.loadedDataDirty; }
         set
         {
            this.loadedDataDirty = value;

            this.SetTitleText();
         }
      }

      /// <summary>
      /// Gets the next number for a new default dish name.
      /// </summary>
      /// <returns>Next number for a new default dish name.</returns>
      private int NextDefaultDishNumber()
      {
         MainForm.defaultDishNumber++;
         return MainForm.defaultDishNumber;
      }

      /// <summary>
      /// Collection of actions that can be performed on a dish.
      /// </summary>
      private Dictionary<DishActionType, DishActionItem> dishActionItems = new Dictionary<DishActionType, DishActionItem>();

      /// <summary>
      /// Collection of action that can be performed on a task.
      /// </summary>
      private Dictionary<TaskActionType, TaskActionItem> taskActionItems = new Dictionary<TaskActionType, TaskActionItem>();

      /// <summary>
      /// Gets the selected dish.
      /// </summary>
      private Dish SelectedDish
      {
         get { return (Dish) this.dishListBox.SelectedItem; }
      }

      public MainForm()
      {
         InitializeComponent();

         var positioners = new ControlPositioner[]
         {
            new ControlPositioner( this.taskListView, FixedOffsets.All )
         };
         this.positionManager = new ControlPositionManager( this, positioners, true );
      }

      private void MainForm_Load( object sender, EventArgs e )
      {
         //
         // Load the Dish EditListBox.
         //

         this.dishListBox.EditBoxShown += ( editBoxShownSender, editBoxShownArgs ) =>
            {
               var dish = (Dish) editBoxShownArgs.Item;
               editBoxShownArgs.EditBoxText = dish.Name;
            };
         this.dishListBox.EditBoxHidden += ( editBoxHiddenSender, editBoxHiddenArgs ) =>
            {
               //System.Diagnostics.Trace.WriteLine( String.Format( "EditBoxHidden - {0}", editBoxHiddenArgs.Reason ) );
               if ( editBoxHiddenArgs.Reason == EditBoxHideReason.EscapeKey )
               {
                  return;
               }

               if ( editBoxHiddenArgs.EditBoxText == string.Empty )
               {
                  editBoxHiddenArgs.Cancel = true;
                  TextBoxBalloonTip.Show( this.dishListBox.EditBoxHandle, Resources.ErrorEmptyDishName );
                  return;
               }

               var dish = (Dish) editBoxHiddenArgs.Item;
               Program.Controller.DishChangeName( dish.ID, editBoxHiddenArgs.EditBoxText );
               this.LoadedDataDirty = true;

               foreach ( ListViewItem item in this.taskListView.Items )
               {
                  var task = (ITask) item.Tag;
                  if ( task.DishID == dish.ID )
                  {
                     item.SubItems[2].Text = ( (IDish) Program.Controller.FindEntity( task.DishID ) ).Name;
                  }
               }
            };
         this.dishListBox.SelectedIndexChanged += ( selectedIndexChangedSender, selectedIndexChangedArgs ) =>
            {
               if ( this.dishListBox.SelectedIndex >= 0 )
               {
                  if ( !this.dishActionComboBox.Items.Contains( this.dishActionItems[DishActionType.Edit] ) )
                  {
                     this.dishActionComboBox.Items.Add( this.dishActionItems[DishActionType.Edit] );
                  }

                  if ( !this.dishActionComboBox.Items.Contains( this.dishActionItems[DishActionType.Delete] ) )
                  {
                     this.dishActionComboBox.Items.Add( this.dishActionItems[DishActionType.Delete] );
                  }
               }
               else
               {
                  if ( this.dishActionComboBox.Items.Contains( this.dishActionItems[DishActionType.Edit] ) )
                  {
                     this.dishActionComboBox.Items.Remove( this.dishActionItems[DishActionType.Edit] );
                  }

                  if ( this.dishActionComboBox.Items.Contains( this.dishActionItems[DishActionType.Delete] ) )
                  {
                     this.dishActionComboBox.Items.Remove( this.dishActionItems[DishActionType.Delete] );
                  }
               }
            };

         //
         // Load the Dish action MenuComboBox.
         //

         this.dishActionItems.Add( DishActionType.Add, new AddDishActionItem( () =>
            {
               var dish = Program.Controller.DishAdd( String.Format( Resources.DefaultDishName, this.NextDefaultDishNumber() ) );
               var index = this.dishListBox.Items.Add( dish );
               this.dishListBox.SelectedIndex = index;
               this.dishListBox.EditItem( index );
               this.LoadedDataDirty = true;
            } ) );
         this.dishActionItems.Add( DishActionType.Edit, new EditDishActionItem( () =>
         {
            this.dishListBox.EditItem( this.dishListBox.SelectedIndex );
            this.LoadedDataDirty = true;
         } ) );
         this.dishActionItems.Add( DishActionType.Delete, new DeleteDishActionItem( () =>
         {
            var dish = (Dish) this.dishListBox.SelectedItem;

            if ( dish.Tasks.Count > 0 )
            {
               if ( DialogResult.Yes != PrepTimeMessageBox.ShowQuestion( Resources.ConfirmDeleteDish ) )
               {
                  return;
               }
            }

            if ( Program.Controller.DishDelete( dish.ID ) )
            {
               var index = this.dishListBox.SelectedIndex;
               this.dishListBox.Items.RemoveAt( index );

               if ( this.dishListBox.Items.Count > 1 )
               {
                  this.dishListBox.SelectedIndex = index - 1;
               }
               else
               {
                  this.dishListBox.SelectedIndex = 0;
               }

               this.taskListView.BeginUpdate();

               for ( var i = this.taskListView.Items.Count - 1; i >= 0; i-- )
               {
                  var task = (ITask) this.taskListView.Items[i].Tag;

                  if ( task.DishID == dish.ID )
                  {
                     this.taskListView.Items.RemoveAt( i );
                  }
               }

               this.taskListView.EndUpdate();

               this.dishActionComboBox.Focus();
               this.LoadedDataDirty = true;
            }
            else
            {
               TextBoxBalloonTip.Show( this.dishListBox.EditBoxHandle, Resources.ErrorMustBeOneDish );
            }
         } ) );

         this.dishActionComboBox.Items.Add( Resources.SelectDishAction );
         this.dishActionComboBox.SelectedIndex = 0;
         this.dishActionComboBox.Items.Add( this.dishActionItems[DishActionType.Add] );
         this.dishActionComboBox.Items.Add( this.dishActionItems[DishActionType.Edit] );
         this.dishActionComboBox.Items.Add( this.dishActionItems[DishActionType.Delete] );
         this.dishActionComboBox.MenuItemSelected += ( menuItemSelectedSender, menuItemSelectedArgs ) =>
            {
               var actionItem = (DishActionItem) menuItemSelectedArgs.MenuItem;
               actionItem.Action();
            };

         //
         // Set up the Task ListView.
         //

         var availableWidth = this.taskListView.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
         var divisions = 20;
         var divisionWidth = availableWidth / divisions;

         var timeColumn = new ColumnHeader();
         timeColumn.Text = Resources.TimeColumn;
         timeColumn.Width = 3 * divisionWidth;

         var intervalColumn = new ColumnHeader();
         intervalColumn.Text = Resources.IntervalColumn;
         intervalColumn.Width = 1 * divisionWidth;

         var dishColumn = new ColumnHeader();
         dishColumn.Text = Resources.DishColumn;
         dishColumn.Width = 4 * divisionWidth;

         var optionsColumn = new ColumnHeader();
         optionsColumn.Text = Resources.OptionsColumn;
         optionsColumn.Width = 2 * divisionWidth;

         var descriptionColumn = new ColumnHeader();
         descriptionColumn.Text = Resources.DescriptionColumn;
         var usedDivisions = 10;
         var remainingWidth = ( divisions - usedDivisions ) * divisionWidth;
         descriptionColumn.Width = remainingWidth + ( ( divisions * availableWidth ) % divisions );

         this.taskListView.Columns.Add( timeColumn );
         this.taskListView.Columns.Add( intervalColumn );
         this.taskListView.Columns.Add( dishColumn );
         this.taskListView.Columns.Add( descriptionColumn );
         this.taskListView.Columns.Add( optionsColumn );

         this.taskListView.ListViewItemSorter = new TaskComparer();

         //
         // Load the Task action MenuComboBox.
         //

         this.taskActionItems.Add( TaskActionType.Edit, new EditTaskActionItem( () =>
            {
               if ( this.taskListView.SelectedItems.Count <= 0 )
               {
                  throw new Exception( "No task selected." );
               }

               var item = this.taskListView.SelectedItems[0];
               var task = (ITask) item.Tag;

               using ( var form = new TaskForm() )
               {
                  form.Description = task.Description;
                  form.Interval = task.Interval;

                  if ( DialogResult.OK == form.ShowDialog() )
                  {
                     this.EditTask( task.ID, form.Description, form.Interval );
                  }
               }
            } ) );
         this.taskActionItems.Add( TaskActionType.Delete, new DeleteTaskActionItem( () =>
         {
            if ( DialogResult.Yes == PrepTimeMessageBox.ShowQuestion( Resources.ConfirmDeleteTask ) )
            {
               this.DeleteTask();
               this.LoadedDataDirty = true;
            }
         } ) );
         this.taskActionItems.Add( TaskActionType.Dependencies, new DependenciesTaskActionItem( () =>
         {
            if ( this.taskListView.SelectedItems.Count <= 0 )
            {
               throw new Exception( "No task selected." );
            }

            var item = this.taskListView.SelectedItems[0];
            var task = (ITask) item.Tag;

            try
            {
               using ( var form = new TaskDependencyForm() )
               {
                  form.Task = task;

                  if ( DialogResult.OK == form.ShowDialog() )
                  {
                     Program.Controller.TaskUpdateDependencies( task.ID, form.DependencyIDs );
                     this.LoadedDataDirty = true;
                     this.UpdateTaskListViewItems();
                  }
               }
            }
            catch ( Exception ex )
            {
               System.Diagnostics.Trace.WriteLine( ex );
               PrepTimeMessageBox.ShowError( String.Format( "Error updating dependencies.\n{0}", ex.Message ) );
            }
         } ) );
         this.taskActionItems.Add( TaskActionType.MoveUp, new MoveUpTaskActionItem( () =>
         {
            if ( this.taskListView.SelectedItems.Count <= 0 )
            {
               throw new Exception( "No task selected." );
            }

            var item = this.taskListView.SelectedItems[0];
            var selectedTask = (ITask) item.Tag;

            for ( var i = item.Index - 1; i >= 0; i-- )
            {
               var taskAbove = (ITask) this.taskListView.Items[i].Tag;

               if ( taskAbove.DishID == selectedTask.DishID )
               {
                  Program.Controller.SwapTasks( taskAbove, selectedTask );
                  this.UpdateTaskListViewItems();
                  this.LoadedDataDirty = true;
                  break;
               }
            }

            //System.Diagnostics.Trace.WriteLine( "Move Up" );
            //Program.DataModel.DependencyGraph.DebugDump();
         } ) );
         this.taskActionItems.Add( TaskActionType.MoveDown, new MoveDownTaskActionItem( () =>
         {
            if ( this.taskListView.SelectedItems.Count <= 0 )
            {
               throw new Exception( "No task selected." );
            }

            var item = this.taskListView.SelectedItems[0];
            var selectedTask = (ITask) item.Tag;

            for ( var i = item.Index + 1; i < this.taskListView.Items.Count; i++ )
            {
               var taskBelow = (ITask) this.taskListView.Items[i].Tag;

               if ( taskBelow.DishID == selectedTask.DishID )
               {
                  Program.Controller.SwapTasks( selectedTask, taskBelow );
                  this.UpdateTaskListViewItems();
                  this.LoadedDataDirty = true;
                  break;
               }
            }

            //System.Diagnostics.Trace.WriteLine( "Move Down" );
            //Program.DataModel.DependencyGraph.DebugDump();
         } ) );

         this.taskOptionComboBox = new MenuComboBox();
         this.taskOptionComboBox.Parent = this;
         this.taskOptionComboBox.Visible = false;
         this.taskOptionComboBox.Size = new System.Drawing.Size( optionsColumn.Width, 21 );
         this.taskOptionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
         this.taskOptionComboBox.FormattingEnabled = true;
         this.taskOptionComboBox.Name = "taskOptionComboBox";
         this.taskOptionComboBox.Items.Add( Resources.SelectDishAction );
         this.taskOptionComboBox.SelectedIndex = 0;
         this.taskOptionComboBox.Items.Add( this.taskActionItems[TaskActionType.Edit] );
         this.taskOptionComboBox.Items.Add( this.taskActionItems[TaskActionType.Delete] );
         this.taskOptionComboBox.Items.Add( this.taskActionItems[TaskActionType.Dependencies] );
         this.taskOptionComboBox.Items.Add( this.taskActionItems[TaskActionType.MoveUp] );
         this.taskOptionComboBox.Items.Add( this.taskActionItems[TaskActionType.MoveDown] );
         this.taskOptionComboBox.MenuItemSelected += ( menuItemSelectedSender, menuItemSelectedArgs ) =>
         {
            var actionItem = (TaskActionItem) menuItemSelectedArgs.MenuItem;
            actionItem.Action();
         };

         //
         // Set up task input text boxes.
         //

         this.taskDescriptionTextBox.KeyPress += ( keyPressSender, keyPressArgs ) =>
            {
               if ( keyPressArgs.KeyChar == 13 )
               {
                  keyPressArgs.Handled = true;
                  this.AddTask();
               }
            };

         this.taskIntervalTextBox.KeyPress += ( keyPressSender, keyPressArgs ) =>
         {
            if ( keyPressArgs.KeyChar == 13 )
            {
               keyPressArgs.Handled = true;
               this.AddTask();
            }
         };

         //
         // Set up the times date/time pickers.
         //

         this.endDateTimePicker.Format = DateTimePickerFormat.Custom;
         this.endDateTimePicker.CustomFormat = MainForm.DateTimeFormat;

         this.LoadMeal();
      }

      private void newToolStripMenuItem_Click( object sender, EventArgs e )
      {
         if ( this.LoadedDataDirty )
         {
            var result = PrepTimeMessageBox.ShowQuestionCancel( Resources.ConfirmNewDataDirty );

            if ( result == DialogResult.Yes )
            {
               if ( !this.SaveMeal( false ) )
               {
                  return;
               }
            }
            else if ( result == DialogResult.Cancel )
            {
               return;
            }
         }

         this.loadedFilePath = String.Empty;
         Program.CreateNewDataModel();

         this.LoadMeal();
      }

      private void openToolStripMenuItem_Click( object sender, EventArgs e )
      {
         if ( this.LoadedDataDirty )
         {
            var result = PrepTimeMessageBox.ShowQuestionCancel( Resources.ConfirmNewDataDirty );

            if ( result == DialogResult.Yes )
            {
               if ( !this.SaveMeal( false ) )
               {
                  return;
               }
            }
            else if ( result == DialogResult.Cancel )
            {
               return;
            }
         }

         using ( var form = new OpenFileDialog() )
         {
            form.Multiselect = false;
            form.Filter = Resources.FilterMealFile;
            form.FilterIndex = 0;

            if ( DialogResult.OK == form.ShowDialog() )
            {
               this.loadedFilePath = form.FileName;

               this.LoadMeal();
            }
         }
      }

      private void saveToolStripMenuItem_Click( object sender, EventArgs e )
      {
         this.SaveMeal( false );
      }

      private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
      {
         this.SaveMeal( true );
      }

      private void exitToolStripMenuItem_Click( object sender, EventArgs e )
      {
         this.Close();
      }

      private void helpToolStripMenuItem1_Click( object sender, EventArgs e )
      {
         var helpHtml = (string) Resources.help;

         var assembly = System.Reflection.Assembly.GetExecutingAssembly();
         var directory = Path.GetDirectoryName( assembly.FullName );
         var helpFile = Path.Combine( directory, "help.html" );

         if ( !File.Exists( helpFile ) )
         {
            File.WriteAllText( helpFile, helpHtml, System.Text.Encoding.UTF8 );
         }

         System.Diagnostics.Process.Start( helpFile );
      }

      private void aboutToolStripMenuItem_Click( object sender, EventArgs e )
      {
         using ( var aboutBox = new AboutBox() )
         {
            aboutBox.ShowDialog();
         }
      }

      private void taskIntervalTextBox_Leave( object sender, EventArgs e )
      {
         if ( this.taskIntervalTextBox.Text == string.Empty )
         {
            return;
         }
      }

      private void taskAddButton_Click( object sender, EventArgs e )
      {
         this.AddTask();
      }

      /// <summary>
      /// Saves a meal.
      /// </summary>
      /// <param name="forceShowDialog">True to always show the save as dialog, false otherwise.</param>
      /// <returns>True if the save was successful, false otherwise.</returns>
      private bool SaveMeal( bool forceShowDialog )
      {
         try
         {
            if ( forceShowDialog || String.IsNullOrEmpty( this.loadedFilePath ) )
            {
               using ( var form = new SaveFileDialog() )
               {
                  form.Filter = Resources.FilterMealFile;
                  form.FilterIndex = 0;

                  if ( !String.IsNullOrEmpty( this.loadedFilePath ) )
                  {
                     form.InitialDirectory = Path.GetDirectoryName( this.loadedFilePath );
                     form.FileName = Path.GetFileName( this.loadedFilePath );
                  }

                  if ( DialogResult.OK == form.ShowDialog() )
                  {
                     this.loadedFilePath = form.FileName;
                  }
                  else
                  {
                     return false;
                  }
               }
            }

            var saver = new MealFileSerializer();
            saver.Save( this.loadedFilePath );

            this.LoadedDataDirty = false;
         }
         catch ( Exception ex )
         {
            System.Diagnostics.Trace.WriteLine( ex );
            PrepTimeMessageBox.ShowError( Resources.ErrorSavingMealToFile );
            return false;
         }

         return true;
      }

      /// <summary>
      /// Handles saving dirty data, if there is any.
      /// </summary>
      /// <returns>False if saving the dirty data was cancelled, true otherwise.</returns>
      private bool HandleDirtyData()
      {
         if ( this.LoadedDataDirty )
         {
            var result = PrepTimeMessageBox.ShowQuestionCancel( Resources.ConfirmExitDataDirty );

            if ( result == DialogResult.Yes )
            {
               if ( !this.SaveMeal( false ) )
               {
                  return false;
               }
            }
            else if ( result == DialogResult.Cancel )
            {
               return false;
            }
         }

         return true;
      }

      /// <summary>
      /// Sets the text of the main title bar.
      /// </summary>
      private void SetTitleText()
      {
         var filePart = String.IsNullOrEmpty( this.loadedFilePath ) ? Resources.NewMealTitleText : Path.GetFileName( this.loadedFilePath );
         this.Text = String.Format( "{0} - {1}{2}", Resources.ApplicationName, filePart, this.loadedDataDirty ? " *" : String.Empty );
      }

      /// <summary>
      /// Loads a meal.
      /// </summary>
      /// <returns>True if a meal was loaded, false otherwise.</returns>
      private bool LoadMeal()
      {
         try
         {
            if ( !String.IsNullOrEmpty( this.loadedFilePath ) )
            {
               var loader = new MealFileSerializer();
               loader.Load( this.loadedFilePath );
            }

            if ( this.dishListBox.Items.Count > 0 )
            {
               this.dishListBox.Items.Clear();
            }

            if ( this.taskListView.Items.Count > 0 )
            {
               this.taskListView.Items.Clear();
            }

            foreach ( var dish in Program.DataModel.Dishes )
            {
               this.dishListBox.Items.Add( dish );

               foreach ( var task in dish.Tasks )
               {
                  var item = new ListViewItem( new string[] { task.BeginTime.ToString( MainForm.DateTimeFormat ), TimeSpanToString.Format( task.Interval ), dish.Name, task.Description, String.Empty } );
                  item.Tag = task;
                  this.taskListView.Items.Add( item );
               }
            }

            if ( this.dishListBox.Items.Count <= 0 )
            {
               throw new Exception( Resources.ErrorLoadMealWithNoDishes );
            }

            this.dishListBox.SelectedIndex = 0;

            this.taskDescriptionTextBox.Text = String.Empty;
            this.taskIntervalTextBox.Text = String.Empty;

            //this.UpdateTaskListViewItems();

            this.endDateTimePicker.Value = Program.DataModel.EndTime;
            this.beginDateTimeTextBox.Text = Program.Controller.GetBeginTime().ToString( MainForm.DateTimeFormat );

            this.taskDescriptionTextBox.Focus();

            this.LoadedDataDirty = false;

            this.SetTitleText();
         }
         catch ( Exception ex )
         {
            System.Diagnostics.Trace.WriteLine( ex.ToString() );
            PrepTimeMessageBox.ShowError( ex.Message );

            return false;
         }

         return true;
      }

      /// <summary>
      /// Adds the task.
      /// </summary>
      private void AddTask()
      {
         if ( this.taskDescriptionTextBox.Text == string.Empty )
         {
            TextBoxBalloonTip.Show( this.taskDescriptionTextBox, Resources.ErrorTaskDescriptionEmpty );
            return;
         }

         if ( this.taskIntervalTextBox.Text == string.Empty )
         {
            TextBoxBalloonTip.Show( this.taskIntervalTextBox, Resources.ErrorTaskIntervalEmpty );
            return;
         }

         var timeSpan = TimeSpan.MinValue;

         try
         {
            timeSpan = IntervalParser.Parse( this.taskIntervalTextBox.Text );
            //System.Diagnostics.Trace.WriteLine( String.Format( "TimeSpan: {0}", timeSpan ) );
         }
         catch ( ArgumentException ex )
         {
            TextBoxBalloonTip.Show( this.taskIntervalTextBox, ex.Message );
            return;
         }
         catch ( Exception ex )
         {
            System.Diagnostics.Trace.WriteLine( ex );
            PrepTimeMessageBox.ShowError( ex.ToString() );
            return;
         }

         var task = Program.Controller.TaskAdd( this.SelectedDish.ID, this.taskDescriptionTextBox.Text, timeSpan );

         var item = new ListViewItem( new string[] { task.BeginTime.ToString( MainForm.DateTimeFormat ), TimeSpanToString.Format( task.Interval ), this.SelectedDish.Name, task.Description, String.Empty } );
         item.Tag = task;
         this.taskListView.Items.Add( item );
         this.UpdateTaskListViewItems();

         this.taskDescriptionTextBox.Text = String.Empty;
         this.taskIntervalTextBox.Text = String.Empty;
         this.taskDescriptionTextBox.Focus();

         this.beginDateTimeTextBox.Text = Program.Controller.GetBeginTime().ToString( MainForm.DateTimeFormat );
         this.LoadedDataDirty = true;
      }

      /// <summary>
      /// Edits a task.
      /// </summary>
      /// <param name="id">ID of the task to edit.</param>
      /// <param name="newDescription">New description of the task.</param>
      /// <param name="newInterval">New interval of the task.</param>
      private void EditTask( int id, string newDescription, TimeSpan newInterval )
      {
         Program.Controller.TaskChangeDescription( id, newDescription );
         Program.Controller.TaskChangeInterval( id, newInterval );

         this.UpdateTaskListViewItems();
         this.beginDateTimeTextBox.Text = Program.Controller.GetBeginTime().ToString( MainForm.DateTimeFormat );
         this.LoadedDataDirty = true;
      }

      private void OnEditTask()
      {
         if ( this.taskListView.SelectedItems.Count <= 0 )
         {
            throw new Exception( "No task selected." );
         }

         var item = this.taskListView.SelectedItems[0];
         var task = (ITask) item.Tag;

         using ( var form = new TaskForm() )
         {
            form.Description = task.Description;
            form.Interval = task.Interval;

            if ( DialogResult.OK == form.ShowDialog() )
            {
               this.EditTask( task.ID, form.Description, form.Interval );
            }
         }
      }

      /// <summary>
      /// Deletes a task.
      /// </summary>
      private void DeleteTask()
      {
         if ( this.taskListView.SelectedItems.Count <= 0 )
         {
            return;
         }

         var item = this.taskListView.SelectedItems[0];
         var task = (ITask) item.Tag;
         this.taskListView.Items.Remove( item );
         Program.Controller.TaskDelete( task.ID );

         this.UpdateTaskListViewItems();
         this.LoadedDataDirty = true;
      }

      private void endDateTimePicker_ValueChanged( object sender, EventArgs e )
      {
         Program.Controller.SetEndTime( this.endDateTimePicker.Value );
         this.beginDateTimeTextBox.Text = Program.Controller.GetBeginTime().ToString( MainForm.DateTimeFormat );
         this.UpdateTaskListViewItems();
         this.LoadedDataDirty = true;
      }

      /// <summary>
      /// Updates items in the task list view.
      /// </summary>
      private void UpdateTaskListViewItems()
      {
         this.taskListView.BeginUpdate();

         foreach ( ListViewItem item in this.taskListView.Items )
         {
            var task = (ITask) item.Tag;
            item.SubItems[0].Text = task.BeginTime.ToString( MainForm.DateTimeFormat );
            item.SubItems[1].Text = TimeSpanToString.Format( task.Interval );
            item.SubItems[2].Text = ((IDish) Program.Controller.FindEntity( task.DishID ) ).Name;
            item.SubItems[3].Text = task.Description;

            //System.Diagnostics.Trace.WriteLine( task.ToString() );
         }

         this.taskListView.Sort();
         this.ManageTaskOptionComboBox();

         this.taskListView.EndUpdate();

         //Program.DataModel.DependencyGraph.DebugDump();
      }

      /// <summary>
      /// Manages how the task option combo box is displayed.
      /// </summary>
      private void ManageTaskOptionComboBox()
      {
         if ( this.taskListView.SelectedIndices.Count > 0 )
         {
            var item = this.taskListView.SelectedItems[0];
            var subitem = item.SubItems[4];
            var screenPoint = this.taskListView.PointToScreen( subitem.Bounds.Location );
            this.taskOptionComboBox.Location = this.PointToClient( screenPoint );
            this.taskOptionComboBox.BringToFront();
            this.taskOptionComboBox.Visible = true;
         }
         else
         {
            this.taskOptionComboBox.Visible = false;
         }
      }

      private void taskListView_SelectedIndexChanged( object sender, EventArgs e )
      {
         this.ManageTaskOptionComboBox();
      }

      private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
      {
         if ( !this.HandleDirtyData() )
         {
            e.Cancel = true;
         }
      }

      private void exportToolStripMenuItem_Click( object sender, EventArgs e )
      {
         string htmlFilePath;

         using ( var form = new SaveFileDialog() )
         {
            form.Filter = Resources.FilterHtmlFile;
            form.FilterIndex = 0;

            if ( !String.IsNullOrEmpty( this.loadedFilePath ) )
            {
               form.InitialDirectory = Path.GetDirectoryName( this.loadedFilePath );
               form.FileName = Path.GetFileNameWithoutExtension( this.loadedFilePath ) + ".html";
            }
            else
            {
               form.FileName = "Untitled.html";
            }

            if ( DialogResult.OK != form.ShowDialog() )
            {
               return;
            }

            htmlFilePath = form.FileName;
         }

         ITask[] orderedTasks;

         try
         {
            orderedTasks = new ITask[this.taskListView.Items.Count];

            for ( var i = 0; i < this.taskListView.Items.Count; i++ )
            {
               orderedTasks[i] = (ITask) this.taskListView.Items[i].Tag;
            }
         }
         catch ( Exception ex )
         {
            System.Diagnostics.Trace.WriteLine( ex );
            PrepTimeMessageBox.ShowError( Resources.ErrorFailedToOrderTasks );
            return;
         }

         string html;

         try
         {
            var generator = new HtmlGenerator();
            html = generator.Generate( orderedTasks );
         }
         catch ( Exception ex )
         {
            System.Diagnostics.Trace.WriteLine( ex );
            PrepTimeMessageBox.ShowError( Resources.ErrorHtmlGeneration );
            return;
         }

         try
         {
            File.WriteAllText( htmlFilePath, html, System.Text.Encoding.UTF8 );
         }
         catch ( Exception ex )
         {
            System.Diagnostics.Trace.WriteLine( ex );
            PrepTimeMessageBox.ShowError( Resources.ErrorWritingHtmlFile );
            return;
         }
      }

      private void taskListView_MouseDoubleClick( object sender, MouseEventArgs e )
      {
         this.OnEditTask();
      }
   }
}
