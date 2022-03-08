namespace PrepTime
{
   partial class TaskForm
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
         this.label1 = new System.Windows.Forms.Label();
         this.taskIntervalTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.taskDescriptionTextBox = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // cancelButton
         // 
         this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cancelButton.Location = new System.Drawing.Point(266, 74);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(75, 23);
         this.cancelButton.TabIndex = 6;
         this.cancelButton.Text = "Cancel";
         this.cancelButton.UseVisualStyleBackColor = true;
         // 
         // okButton
         // 
         this.okButton.Location = new System.Drawing.Point(185, 74);
         this.okButton.Name = "okButton";
         this.okButton.Size = new System.Drawing.Size(75, 23);
         this.okButton.TabIndex = 5;
         this.okButton.Text = "OK";
         this.okButton.UseVisualStyleBackColor = true;
         this.okButton.Click += new System.EventHandler(this.okButton_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(222, 41);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(44, 13);
         this.label1.TabIndex = 4;
         this.label1.Text = "HH:MM";
         // 
         // taskIntervalTextBox
         // 
         this.taskIntervalTextBox.Location = new System.Drawing.Point(81, 38);
         this.taskIntervalTextBox.Name = "taskIntervalTextBox";
         this.taskIntervalTextBox.Size = new System.Drawing.Size(135, 20);
         this.taskIntervalTextBox.TabIndex = 3;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(12, 15);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(63, 13);
         this.label2.TabIndex = 0;
         this.label2.Text = "Description:";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(30, 41);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(45, 13);
         this.label3.TabIndex = 2;
         this.label3.Text = "Interval:";
         // 
         // taskDescriptionTextBox
         // 
         this.taskDescriptionTextBox.Location = new System.Drawing.Point(81, 12);
         this.taskDescriptionTextBox.Name = "taskDescriptionTextBox";
         this.taskDescriptionTextBox.Size = new System.Drawing.Size(260, 20);
         this.taskDescriptionTextBox.TabIndex = 1;
         // 
         // TaskForm
         // 
         this.AcceptButton = this.okButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.cancelButton;
         this.ClientSize = new System.Drawing.Size(353, 111);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.taskIntervalTextBox);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.taskDescriptionTextBox);
         this.Controls.Add(this.okButton);
         this.Controls.Add(this.cancelButton);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "TaskForm";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "{0} Task";
         this.Load += new System.EventHandler(this.TaskForm_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.Button okButton;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox taskIntervalTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox taskDescriptionTextBox;
   }
}