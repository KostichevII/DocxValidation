namespace DocxValidation
{
    partial class JornalForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FileList = new System.Windows.Forms.ListBox();
            this.RefreshB = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.SortMod = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextList = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.checkUnknow = new System.Windows.Forms.CheckBox();
            this.checkError = new System.Windows.Forms.CheckBox();
            this.checkFatal = new System.Windows.Forms.CheckBox();
            this.checkWarning = new System.Windows.Forms.CheckBox();
            this.checkNormal = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // FileList
            // 
            this.FileList.FormattingEnabled = true;
            this.FileList.Location = new System.Drawing.Point(12, 28);
            this.FileList.Name = "FileList";
            this.FileList.Size = new System.Drawing.Size(205, 407);
            this.FileList.TabIndex = 0;
            this.FileList.SelectedIndexChanged += new System.EventHandler(this.FileList_SelectedIndexChanged);
            // 
            // RefreshB
            // 
            this.RefreshB.Location = new System.Drawing.Point(12, 442);
            this.RefreshB.Name = "RefreshB";
            this.RefreshB.Size = new System.Drawing.Size(205, 68);
            this.RefreshB.TabIndex = 2;
            this.RefreshB.Text = "Обновить";
            this.RefreshB.UseVisualStyleBackColor = true;
            this.RefreshB.Click += new System.EventHandler(this.RefreshB_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.SortMod);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TextList);
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Location = new System.Drawing.Point(224, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(556, 481);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Сортировать:";
            // 
            // SortMod
            // 
            this.SortMod.FormattingEnabled = true;
            this.SortMod.Items.AddRange(new object[] {
            "Нет",
            "По типу",
            "По времени",
            "По модулю"});
            this.SortMod.Location = new System.Drawing.Point(142, 39);
            this.SortMod.Name = "SortMod";
            this.SortMod.Size = new System.Drawing.Size(151, 21);
            this.SortMod.TabIndex = 4;
            this.SortMod.Text = "Нет";
            this.SortMod.TextChanged += new System.EventHandler(this.SortMod_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Отображать тип ошибок:";
            // 
            // TextList
            // 
            this.TextList.FormattingEnabled = true;
            this.TextList.HorizontalScrollbar = true;
            this.TextList.Location = new System.Drawing.Point(4, 64);
            this.TextList.Name = "TextList";
            this.TextList.Size = new System.Drawing.Size(547, 394);
            this.TextList.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.checkUnknow, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkError, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkFatal, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkWarning, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkNormal, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(142, 10);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(314, 22);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // checkUnknow
            // 
            this.checkUnknow.AutoSize = true;
            this.checkUnknow.Checked = true;
            this.checkUnknow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUnknow.Location = new System.Drawing.Point(249, 3);
            this.checkUnknow.Name = "checkUnknow";
            this.checkUnknow.Size = new System.Drawing.Size(66, 16);
            this.checkUnknow.TabIndex = 3;
            this.checkUnknow.Text = "Unknow";
            this.checkUnknow.UseVisualStyleBackColor = true;
            this.checkUnknow.CheckedChanged += new System.EventHandler(this.checkUnknow_CheckedChanged);
            // 
            // checkError
            // 
            this.checkError.AutoSize = true;
            this.checkError.Checked = true;
            this.checkError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkError.Location = new System.Drawing.Point(140, 3);
            this.checkError.Name = "checkError";
            this.checkError.Size = new System.Drawing.Size(48, 16);
            this.checkError.TabIndex = 2;
            this.checkError.Text = "Error";
            this.checkError.UseVisualStyleBackColor = true;
            this.checkError.CheckedChanged += new System.EventHandler(this.checkError_CheckedChanged);
            // 
            // checkFatal
            // 
            this.checkFatal.AutoSize = true;
            this.checkFatal.Checked = true;
            this.checkFatal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkFatal.Location = new System.Drawing.Point(194, 3);
            this.checkFatal.Name = "checkFatal";
            this.checkFatal.Size = new System.Drawing.Size(49, 16);
            this.checkFatal.TabIndex = 4;
            this.checkFatal.Text = "Fatal";
            this.checkFatal.UseVisualStyleBackColor = true;
            this.checkFatal.CheckedChanged += new System.EventHandler(this.checkFatal_CheckedChanged);
            // 
            // checkWarning
            // 
            this.checkWarning.AutoSize = true;
            this.checkWarning.Checked = true;
            this.checkWarning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkWarning.Location = new System.Drawing.Point(68, 3);
            this.checkWarning.Name = "checkWarning";
            this.checkWarning.Size = new System.Drawing.Size(66, 16);
            this.checkWarning.TabIndex = 1;
            this.checkWarning.Text = "Warning";
            this.checkWarning.UseVisualStyleBackColor = true;
            this.checkWarning.CheckedChanged += new System.EventHandler(this.checkWarning_CheckedChanged);
            // 
            // checkNormal
            // 
            this.checkNormal.AutoSize = true;
            this.checkNormal.Checked = true;
            this.checkNormal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkNormal.Location = new System.Drawing.Point(3, 3);
            this.checkNormal.Name = "checkNormal";
            this.checkNormal.Size = new System.Drawing.Size(59, 16);
            this.checkNormal.TabIndex = 0;
            this.checkNormal.Text = "Normal";
            this.checkNormal.UseVisualStyleBackColor = true;
            this.checkNormal.CheckedChanged += new System.EventHandler(this.checkNormal_CheckedChanged);
            // 
            // JornalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 522);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.RefreshB);
            this.Controls.Add(this.FileList);
            this.Name = "JornalForm";
            this.Text = "Журналы";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.JornalForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox FileList;
        private System.Windows.Forms.Button RefreshB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkNormal;
        private System.Windows.Forms.CheckBox checkWarning;
        private System.Windows.Forms.CheckBox checkError;
        private System.Windows.Forms.CheckBox checkUnknow;
        private System.Windows.Forms.ListBox TextList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox checkFatal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox SortMod;
        private System.Windows.Forms.Label label1;
    }
}