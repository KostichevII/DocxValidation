namespace DocxValidation
{
    partial class MenuForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Bvalidation = new System.Windows.Forms.Button();
            this.Btemplates = new System.Windows.Forms.Button();
            this.BJornal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Bvalidation
            // 
            this.Bvalidation.Location = new System.Drawing.Point(12, 30);
            this.Bvalidation.Name = "Bvalidation";
            this.Bvalidation.Size = new System.Drawing.Size(319, 62);
            this.Bvalidation.TabIndex = 0;
            this.Bvalidation.Text = "Проверка документов";
            this.Bvalidation.UseVisualStyleBackColor = true;
            this.Bvalidation.Click += new System.EventHandler(this.Bvalidation_Click);
            // 
            // Btemplates
            // 
            this.Btemplates.Location = new System.Drawing.Point(12, 98);
            this.Btemplates.Name = "Btemplates";
            this.Btemplates.Size = new System.Drawing.Size(319, 62);
            this.Btemplates.TabIndex = 1;
            this.Btemplates.Text = "Настройка шаблонов проверки";
            this.Btemplates.UseVisualStyleBackColor = true;
            this.Btemplates.Click += new System.EventHandler(this.Btemplates_Click);
            // 
            // BJornal
            // 
            this.BJornal.Location = new System.Drawing.Point(12, 166);
            this.BJornal.Name = "BJornal";
            this.BJornal.Size = new System.Drawing.Size(319, 62);
            this.BJornal.TabIndex = 5;
            this.BJornal.Text = "Журнал работы проверок";
            this.BJornal.UseVisualStyleBackColor = true;
            this.BJornal.Click += new System.EventHandler(this.BJornal_Click);
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 243);
            this.Controls.Add(this.BJornal);
            this.Controls.Add(this.Btemplates);
            this.Controls.Add(this.Bvalidation);
            this.Name = "MenuForm";
            this.Text = "Меню";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Bvalidation;
        private System.Windows.Forms.Button Btemplates;
        private System.Windows.Forms.Button BJornal;
    }
}

