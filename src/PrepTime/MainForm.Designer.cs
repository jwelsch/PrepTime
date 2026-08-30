namespace PrepTime
{
   partial class MainForm
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
         this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
         this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
         this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
         this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.label2 = new System.Windows.Forms.Label();
         this.taskDescriptionTextBox = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.taskIntervalTextBox = new System.Windows.Forms.TextBox();
         this.taskAddButton = new System.Windows.Forms.Button();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.label1 = new System.Windows.Forms.Label();
         this.groupBox3 = new System.Windows.Forms.GroupBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.beginDateTimeTextBox = new System.Windows.Forms.TextBox();
         this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
         this.label5 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.taskListView = new System.Windows.Forms.ListView();
         this.dishActionComboBox = new PrepTime.MenuComboBox();
         this.dishListBox = new PrepTime.EditListBox();
         this.mainMenuStrip.SuspendLayout();
         this.groupBox2.SuspendLayout();
         this.groupBox3.SuspendLayout();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // mainMenuStrip
         // 
         this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
         this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
         this.mainMenuStrip.Name = "mainMenuStrip";
         this.mainMenuStrip.Size = new System.Drawing.Size(1008, 24);
         this.mainMenuStrip.TabIndex = 0;
         this.mainMenuStrip.Text = "mainMenuStrip";
         // 
         // fileToolStripMenuItem
         // 
         this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
         this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
         this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
         this.fileToolStripMenuItem.Text = "&File";
         // 
         // newToolStripMenuItem
         // 
         this.newToolStripMenuItem.Name = "newToolStripMenuItem";
         this.newToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
         this.newToolStripMenuItem.Text = "&New";
         this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
         // 
         // openToolStripMenuItem
         // 
         this.openToolStripMenuItem.Name = "openToolStripMenuItem";
         this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
         this.openToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
         this.openToolStripMenuItem.Text = "&Open...";
         this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
         // 
         // saveToolStripMenuItem
         // 
         this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
         this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
         this.saveToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
         this.saveToolStripMenuItem.Text = "&Save";
         this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
         // 
         // saveAsToolStripMenuItem
         // 
         this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
         this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
         this.saveAsToolStripMenuItem.Text = "Save &As...";
         this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
         // 
         // toolStripSeparator1
         // 
         this.toolStripSeparator1.Name = "toolStripSeparator1";
         this.toolStripSeparator1.Size = new System.Drawing.Size(152, 6);
         // 
         // exportToolStripMenuItem
         // 
         this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
         this.exportToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
         this.exportToolStripMenuItem.Text = "Export...";
         this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
         // 
         // toolStripSeparator3
         // 
         this.toolStripSeparator3.Name = "toolStripSeparator3";
         this.toolStripSeparator3.Size = new System.Drawing.Size(152, 6);
         // 
         // exitToolStripMenuItem
         // 
         this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
         this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
         this.exitToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
         this.exitToolStripMenuItem.Text = "E&xit";
         this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
         // 
         // helpToolStripMenuItem
         // 
         this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.toolStripSeparator2,
            this.aboutToolStripMenuItem});
         this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
         this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
         this.helpToolStripMenuItem.Text = "&Help";
         // 
         // helpToolStripMenuItem1
         // 
         this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
         this.helpToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F1;
         this.helpToolStripMenuItem1.Size = new System.Drawing.Size(127, 22);
         this.helpToolStripMenuItem1.Text = "&Help...";
         this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
         // 
         // toolStripSeparator2
         // 
         this.toolStripSeparator2.Name = "toolStripSeparator2";
         this.toolStripSeparator2.Size = new System.Drawing.Size(124, 6);
         // 
         // aboutToolStripMenuItem
         // 
         this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
         this.aboutToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
         this.aboutToolStripMenuItem.Text = "&About...";
         this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(6, 22);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(63, 13);
         this.label2.TabIndex = 0;
         this.label2.Text = "Description:";
         // 
         // taskDescriptionTextBox
         // 
         this.taskDescriptionTextBox.Location = new System.Drawing.Point(75, 19);
         this.taskDescriptionTextBox.Name = "taskDescriptionTextBox";
         this.taskDescriptionTextBox.Size = new System.Drawing.Size(276, 20);
         this.taskDescriptionTextBox.TabIndex = 1;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(24, 50);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(45, 13);
         this.label3.TabIndex = 2;
         this.label3.Text = "Interval:";
         // 
         // taskIntervalTextBox
         // 
         this.taskIntervalTextBox.Location = new System.Drawing.Point(75, 46);
         this.taskIntervalTextBox.Name = "taskIntervalTextBox";
         this.taskIntervalTextBox.Size = new System.Drawing.Size(135, 20);
         this.taskIntervalTextBox.TabIndex = 3;
         this.taskIntervalTextBox.Leave += new System.EventHandler(this.taskIntervalTextBox_Leave);
         // 
         // taskAddButton
         // 
         this.taskAddButton.Location = new System.Drawing.Point(276, 45);
         this.taskAddButton.Name = "taskAddButton";
         this.taskAddButton.Size = new System.Drawing.Size(75, 23);
         this.taskAddButton.TabIndex = 5;
         this.taskAddButton.Text = "Add";
         this.taskAddButton.UseVisualStyleBackColor = true;
         this.taskAddButton.Click += new System.EventHandler(this.taskAddButton_Click);
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.label1);
         this.groupBox2.Controls.Add(this.taskAddButton);
         this.groupBox2.Controls.Add(this.taskIntervalTextBox);
         this.groupBox2.Controls.Add(this.label2);
         this.groupBox2.Controls.Add(this.label3);
         this.groupBox2.Controls.Add(this.taskDescriptionTextBox);
         this.groupBox2.Location = new System.Drawing.Point(375, 27);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(357, 84);
         this.groupBox2.TabIndex = 1;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Task";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(216, 50);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(44, 13);
         this.label1.TabIndex = 4;
         this.label1.Text = "HH:MM";
         // 
         // groupBox3
         // 
         this.groupBox3.Controls.Add(this.dishActionComboBox);
         this.groupBox3.Controls.Add(this.dishListBox);
         this.groupBox3.Location = new System.Drawing.Point(12, 27);
         this.groupBox3.Name = "groupBox3";
         this.groupBox3.Size = new System.Drawing.Size(357, 84);
         this.groupBox3.TabIndex = 0;
         this.groupBox3.TabStop = false;
         this.groupBox3.Text = "Dishes";
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.beginDateTimeTextBox);
         this.groupBox1.Controls.Add(this.endDateTimePicker);
         this.groupBox1.Controls.Add(this.label5);
         this.groupBox1.Controls.Add(this.label4);
         this.groupBox1.Location = new System.Drawing.Point(738, 27);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(258, 84);
         this.groupBox1.TabIndex = 2;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Times";
         // 
         // beginDateTimeTextBox
         // 
         this.beginDateTimeTextBox.Location = new System.Drawing.Point(49, 19);
         this.beginDateTimeTextBox.Name = "beginDateTimeTextBox";
         this.beginDateTimeTextBox.ReadOnly = true;
         this.beginDateTimeTextBox.Size = new System.Drawing.Size(200, 20);
         this.beginDateTimeTextBox.TabIndex = 1;
         // 
         // endDateTimePicker
         // 
         this.endDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
         this.endDateTimePicker.Location = new System.Drawing.Point(49, 46);
         this.endDateTimePicker.Name = "endDateTimePicker";
         this.endDateTimePicker.Size = new System.Drawing.Size(200, 20);
         this.endDateTimePicker.TabIndex = 3;
         this.endDateTimePicker.ValueChanged += new System.EventHandler(this.endDateTimePicker_ValueChanged);
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(14, 50);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(29, 13);
         this.label5.TabIndex = 2;
         this.label5.Text = "End:";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(6, 22);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(37, 13);
         this.label4.TabIndex = 0;
         this.label4.Text = "Begin:";
         // 
         // taskListView
         // 
         this.taskListView.FullRowSelect = true;
         this.taskListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
         this.taskListView.HideSelection = false;
         this.taskListView.Location = new System.Drawing.Point(12, 117);
         this.taskListView.MultiSelect = false;
         this.taskListView.Name = "taskListView";
         this.taskListView.Size = new System.Drawing.Size(984, 601);
         this.taskListView.TabIndex = 3;
         this.taskListView.UseCompatibleStateImageBehavior = false;
         this.taskListView.View = System.Windows.Forms.View.Details;
         this.taskListView.SelectedIndexChanged += new System.EventHandler(this.taskListView_SelectedIndexChanged);
         this.taskListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.taskListView_MouseDoubleClick);
         // 
         // dishActionComboBox
         // 
         this.dishActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.dishActionComboBox.FormattingEnabled = true;
         this.dishActionComboBox.Location = new System.Drawing.Point(248, 20);
         this.dishActionComboBox.Name = "dishActionComboBox";
         this.dishActionComboBox.Size = new System.Drawing.Size(103, 21);
         this.dishActionComboBox.TabIndex = 1;
         // 
         // dishListBox
         // 
         this.dishListBox.FormattingEnabled = true;
         this.dishListBox.Location = new System.Drawing.Point(6, 19);
         this.dishListBox.Name = "dishListBox";
         this.dishListBox.Size = new System.Drawing.Size(235, 56);
         this.dishListBox.TabIndex = 0;
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1008, 730);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.groupBox3);
         this.Controls.Add(this.taskListView);
         this.Controls.Add(this.mainMenuStrip);
         this.Controls.Add(this.groupBox2);
         this.MainMenuStrip = this.mainMenuStrip;
         this.Name = "MainForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "PrepTime";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
         this.Load += new System.EventHandler(this.MainForm_Load);
         this.mainMenuStrip.ResumeLayout(false);
         this.mainMenuStrip.PerformLayout();
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         this.groupBox3.ResumeLayout(false);
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ListView taskListView;
      private System.Windows.Forms.MenuStrip mainMenuStrip;
      private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
      private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
      private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox taskDescriptionTextBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox taskIntervalTextBox;
      private System.Windows.Forms.Button taskAddButton;
      private System.Windows.Forms.GroupBox groupBox2;
      private EditListBox dishListBox;
      private System.Windows.Forms.GroupBox groupBox3;
      private MenuComboBox dishActionComboBox;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.DateTimePicker endDateTimePicker;
      private System.Windows.Forms.TextBox beginDateTimeTextBox;
      private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
   }
}

