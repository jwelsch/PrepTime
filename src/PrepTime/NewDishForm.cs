using System;
using System.Drawing;
using System.Windows.Forms;
using PrepTime.Properties;

namespace PrepTime
{
   public partial class NewDishForm : Form
   {
      /// <summary>
      /// Gets or sets the name of the dish.
      /// </summary>
      public string DishName
      {
         get;
         set;
      }

      public NewDishForm()
      {
         InitializeComponent();
      }

      private void NewDishForm_Load( object sender, EventArgs e )
      {
         this.dishNameTextBox.Text = this.DishName;
      }

      private void okButton_Click( object sender, EventArgs e )
      {
         if ( Program.Controller.DishNameExists( this.dishNameTextBox.Text ) )
         {
            PrepTimeMessageBox.ShowError( Resources.ErrorDishNameExists );
            this.dishNameTextBox.SelectAll();
            this.dishNameTextBox.Focus();

            return;
         }

         this.DishName = this.dishNameTextBox.Text;

         this.DialogResult = DialogResult.OK;
      }
   }
}
