namespace DocxValidation
{
    partial class CheckForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DocAdress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FileDialogButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TemplateList = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.TemplateName = new System.Windows.Forms.TextBox();
            this.TemplateDate = new System.Windows.Forms.TextBox();
            this.ListRefresh = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.CheckStartB = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ErrorGrid = new System.Windows.Forms.DataGridView();
            this.Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Errors = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ErrorCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.CheckedFileName = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Controls.Add(this.DocAdress);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.FileDialogButton);
            this.panel1.Location = new System.Drawing.Point(12, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(227, 96);
            this.panel1.TabIndex = 1;
            // 
            // DocAdress
            // 
            this.DocAdress.Location = new System.Drawing.Point(12, 26);
            this.DocAdress.Name = "DocAdress";
            this.DocAdress.ReadOnly = true;
            this.DocAdress.Size = new System.Drawing.Size(202, 20);
            this.DocAdress.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Проверяемый документ";
            // 
            // FileDialogButton
            // 
            this.FileDialogButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.FileDialogButton.Location = new System.Drawing.Point(12, 52);
            this.FileDialogButton.Name = "FileDialogButton";
            this.FileDialogButton.Size = new System.Drawing.Size(202, 26);
            this.FileDialogButton.TabIndex = 1;
            this.FileDialogButton.Text = "Открыть диалоговое окно";
            this.FileDialogButton.UseVisualStyleBackColor = false;
            this.FileDialogButton.Click += new System.EventHandler(this.FileDialogButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Документ Microsoft Word (.docx)| *.docx";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel2.Controls.Add(this.TemplateList);
            this.panel2.Controls.Add(this.tableLayoutPanel15);
            this.panel2.Controls.Add(this.ListRefresh);
            this.panel2.Controls.Add(this.label18);
            this.panel2.Location = new System.Drawing.Point(12, 130);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 321);
            this.panel2.TabIndex = 2;
            // 
            // TemplateList
            // 
            this.TemplateList.FormattingEnabled = true;
            this.TemplateList.Location = new System.Drawing.Point(3, 92);
            this.TemplateList.Name = "TemplateList";
            this.TemplateList.Size = new System.Drawing.Size(221, 186);
            this.TemplateList.TabIndex = 4;
            this.TemplateList.SelectedIndexChanged += new System.EventHandler(this.TemplateList_SelectedIndexChanged);
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel15.ColumnCount = 2;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Controls.Add(this.label31, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.label32, 0, 1);
            this.tableLayoutPanel15.Controls.Add(this.TemplateName, 1, 0);
            this.tableLayoutPanel15.Controls.Add(this.TemplateDate, 1, 1);
            this.tableLayoutPanel15.Location = new System.Drawing.Point(12, 26);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 2;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(202, 60);
            this.tableLayoutPanel15.TabIndex = 3;
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(4, 1);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(87, 26);
            this.label31.TabIndex = 0;
            this.label31.Text = "Имя:";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label32
            // 
            this.label32.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(4, 28);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(87, 31);
            this.label32.TabIndex = 1;
            this.label32.Text = "Дата создания:";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TemplateName
            // 
            this.TemplateName.Location = new System.Drawing.Point(98, 4);
            this.TemplateName.Name = "TemplateName";
            this.TemplateName.ReadOnly = true;
            this.TemplateName.Size = new System.Drawing.Size(100, 20);
            this.TemplateName.TabIndex = 2;
            // 
            // TemplateDate
            // 
            this.TemplateDate.Location = new System.Drawing.Point(98, 31);
            this.TemplateDate.Name = "TemplateDate";
            this.TemplateDate.ReadOnly = true;
            this.TemplateDate.Size = new System.Drawing.Size(100, 20);
            this.TemplateDate.TabIndex = 3;
            // 
            // ListRefresh
            // 
            this.ListRefresh.Location = new System.Drawing.Point(12, 284);
            this.ListRefresh.Name = "ListRefresh";
            this.ListRefresh.Size = new System.Drawing.Size(202, 23);
            this.ListRefresh.TabIndex = 1;
            this.ListRefresh.Text = "Обновить список";
            this.ListRefresh.UseVisualStyleBackColor = true;
            this.ListRefresh.Click += new System.EventHandler(this.ListRefresh_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(61, 10);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(101, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Загрузка шаблона";
            // 
            // CheckStartB
            // 
            this.CheckStartB.Enabled = false;
            this.CheckStartB.Location = new System.Drawing.Point(13, 457);
            this.CheckStartB.Name = "CheckStartB";
            this.CheckStartB.Size = new System.Drawing.Size(226, 42);
            this.CheckStartB.TabIndex = 4;
            this.CheckStartB.Text = "Проверить документ";
            this.CheckStartB.UseVisualStyleBackColor = true;
            this.CheckStartB.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.Filter = "Файл \"TEMP\" (.temp)| *.temp";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel3.Controls.Add(this.ErrorGrid);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Location = new System.Drawing.Point(250, 28);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(622, 471);
            this.panel3.TabIndex = 5;
            // 
            // ErrorGrid
            // 
            this.ErrorGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ErrorGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ErrorGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Position,
            this.Type,
            this.Errors});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ErrorGrid.DefaultCellStyle = dataGridViewCellStyle4;
            this.ErrorGrid.Location = new System.Drawing.Point(8, 73);
            this.ErrorGrid.Name = "ErrorGrid";
            this.ErrorGrid.RowHeadersVisible = false;
            this.ErrorGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ErrorGrid.Size = new System.Drawing.Size(605, 390);
            this.ErrorGrid.TabIndex = 2;
            // 
            // Position
            // 
            this.Position.HeaderText = "Положение ошибки";
            this.Position.Name = "Position";
            this.Position.ReadOnly = true;
            this.Position.Width = 160;
            // 
            // Type
            // 
            this.Type.HeaderText = "Распознанный тип текста";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Width = 145;
            // 
            // Errors
            // 
            this.Errors.HeaderText = "Ошибки";
            this.Errors.Name = "Errors";
            this.Errors.ReadOnly = true;
            this.Errors.Width = 300;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(8, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(605, 57);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Информация о проверке";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.ErrorCount, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(341, 23);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(258, 28);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // ErrorCount
            // 
            this.ErrorCount.Location = new System.Drawing.Point(176, 4);
            this.ErrorCount.Name = "ErrorCount";
            this.ErrorCount.ReadOnly = true;
            this.ErrorCount.Size = new System.Drawing.Size(77, 20);
            this.ErrorCount.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 26);
            this.label2.TabIndex = 0;
            this.label2.Text = "Количество найденных ошибок";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CheckedFileName, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 22);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(323, 28);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 26);
            this.label4.TabIndex = 0;
            this.label4.Text = "Имя проверяемого файла";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CheckedFileName
            // 
            this.CheckedFileName.Location = new System.Drawing.Point(151, 4);
            this.CheckedFileName.Name = "CheckedFileName";
            this.CheckedFileName.ReadOnly = true;
            this.CheckedFileName.Size = new System.Drawing.Size(168, 20);
            this.CheckedFileName.TabIndex = 1;
            // 
            // CheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(884, 511);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.CheckStartB);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "CheckForm";
            this.Text = "Проверка документов";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel15.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button FileDialogButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button ListRefresh;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button CheckStartB;
        private System.Windows.Forms.TextBox DocAdress;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView ErrorGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox ErrorCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CheckedFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Errors;
        private System.Windows.Forms.ListBox TemplateList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox TemplateName;
        private System.Windows.Forms.TextBox TemplateDate;
    }
}