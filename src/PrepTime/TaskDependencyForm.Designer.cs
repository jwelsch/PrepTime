namespace PrepTime
{
   partial class TaskDependencyForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose( bool disposing )
      {
         if ( disposing && ( components != null ) )
         {
            components.Dispose();
         }
         base.Dispose( disposing );
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.cancelButton = new System.Windows.Forms.Button();
         this.okButton = new System.Windows.Forms.Button();
         this.independentListView = new System.Windows.Forms.ListView();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.dishComboBox = new System.Windows.Forms.ComboBox();
         this.SuspendLayout();
         // 
         // cancelButton
         // 
         this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cancelButton.Location = new System.Drawing.Point(197, 227);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(75, 23);
         this.cancelButton.TabIndex = 0;
         this.cancelButton.Text = "Cancel";
         this.cancelButton.UseVisualStyleBackColor = true;
         // 
         // okButton
         // 
         this.okButton.Location = new System.Drawing.Point(116, 227);
         this.okButton.Name = "okButton";
         this.okButton.Size = new System.Drawing.Size(75, 23);
         this.okButton.TabIndex = 1;
         this.okButton.Text = "OK";
         this.okButton.UseVisualStyleBackColor = true;
         this.okButton.Click += new System.EventHandler(this.okButton_Click);
         // 
         // independentListView
         // 
         this.independentListView.CheckBoxes = true;
         this.independentListView.FullRowSelect = true;
         this.independentListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.independentListView.Location = new System.Drawing.Point(12, 79);
         this.independentListView.MultiSelect = false;
         this.independentListView.Name = "independentListView";
         this.independentListView.Size = new System.Drawing.Size(260, 142);
         this.independentListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
         this.independentListView.TabIndex = 10;
         this.independentListView.UseCompatibleStateImageBehavior = false;
         this.independentListView.View = System.Windows.Forms.View.Details;
         this.independentListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.independentListView_ItemChecked);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(9, 63);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(59, 13);
         this.label2.TabIndex = 9;
         this.label2.Text = "Dish tasks:";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(9, 9);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(42, 13);
         this.label1.TabIndex = 8;
         this.label1.Text = "Dishes:";
         // 
         // dishComboBox
         // 
         this.dishComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.dishComboBox.FormattingEnabled = true;
         this.dishComboBox.Location = new System.Drawing.Point(12, 25);
         this.dishComboBox.Name = "dishComboBox";
         this.dishComboBox.Size = new System.Drawing.Size(260, 21);
         this.dishComboBox.TabIndex = 7;
         this.dishComboBox.SelectedIndexChanged += new System.EventHandler(this.dishComboBox_SelectedIndexChanged);
         // 
         // TaskDependencyForm
         // 
         this.AcceptButton = this.okButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.cancelButton;
         this.ClientSize = new System.Drawing.Size(284, 262);
         this.Controls.Add(this.independentListView);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.dishComboBox);
         this.Controls.Add(this.okButton);
         this.Controls.Add(this.cancelButton);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "TaskDependencyForm";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Task Dependencies";
         this.Load += new System.EventHandler(this.TaskDependencyForm_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.Button okButton;
      private System.Windows.Forms.ListView independentListView;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox dishComboBox;
   }
}