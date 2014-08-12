using System;
using System.Drawing;
using System.Windows.Forms;
using PrepTime.Properties;

namespace PrepTime
{
   public partial class TaskForm : Form
   {
      /// <summary>
      /// Gets or sets the description of the task.
      /// </summary>
      public string Description
      {
         get;
         set;
      }

      /// <summary>
      /// Gets or sets the interval of the task.
      /// </summary>
      public TimeSpan Interval
      {
         get;
         set;
      }

      public TaskForm()
      {
         InitializeComponent();
      }

      private void TaskForm_Load( object sender, EventArgs e )
      {
         this.Text = String.Format( this.Text, Resources.TaskActionEdit );

         this.taskDescriptionTextBox.Text = this.Description;
         this.taskIntervalTextBox.Text = TimeSpanToString.Format( this.Interval );
      }

      private void okButton_Click( object sender, EventArgs e )
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

         this.Description = this.taskDescriptionTextBox.Text;
         this.Interval = timeSpan;

         this.DialogResult = DialogResult.OK;
      }
   }
}
