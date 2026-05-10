using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocChecker;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocxValidation
{
    public partial class CheckForm : Form
    {
        List<FieldSaver> FieldsSavers = new List<FieldSaver>();
        DocChecker.CheckerClasses.CheckParametrs CheckParams = new DocChecker.CheckerClasses.CheckParametrs();
        List<DocChecker.CheckerClasses.Expection> SavedExpections = new List<DocChecker.CheckerClasses.Expection>();
        Template template = null;
        bool ParamsSaved = true;


        public CheckForm()
        {
            InitializeComponent();
        }

        private void GridShow(List<ErrorRecord> records)
        {
            ErrorGrid.Rows.Clear();
            int rowCounter = 0;
            foreach (var record in records) 
            {
                ErrorGrid.Rows.Add();
                ErrorGrid.Rows[rowCounter].Cells[0].Value = record.PositionToString();

                ErrorGrid.Rows[rowCounter].Cells[1].Value = record.TypeToString();

                ErrorGrid.Rows[rowCounter].Cells[2].Value = record.ConvertError(CheckParams);

                rowCounter++;
            }
        }
        private void resultShow(List<ErrorRecord> records, string path)
        {
            GridShow(records);

            int errosCounter = 0;
            foreach (var record in records) {
                errosCounter += record.ErrorsCount();
            }

            ErrorCount.Text = errosCounter.ToString();

            CheckedFileName.Text = Path.GetFileNameWithoutExtension(path);
        }
        private void FileDialogButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string fileName = openFileDialog1.FileName;
            DocAdress.Text = fileName;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CheckParams.exp = SavedExpections;
            if (String.IsNullOrEmpty(DocAdress.Text))
            {
                MessageBox.Show("Не выбран проверяемый документ");
                return;
            }

            if (ParamsSaved)
            {
                List<ErrorRecord> result = DocChecker.CheckerFuncs.CheckDocument(DocAdress.Text, CheckParams, true);
                resultShow(result, DocAdress.Text);
            }
        }
        private void TableFileDialog_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "templates");
            if (!Directory.Exists(path))
            {
                openFileDialog2.InitialDirectory= Directory.GetCurrentDirectory();
            }
            else
            {
                openFileDialog2.InitialDirectory = path;
            }

            try
            {
                if (openFileDialog2.ShowDialog() == DialogResult.Cancel)
                    return;
                string fileName = openFileDialog2.FileName;
                template = new Template();
                if (!template.ReadFile(fileName))
                {
                    MessageBox.Show("Ошибка чтения файла");
                    template = null;
                    return;
                }
                FieldsSavers = template.Fields;

                TemplateDate.Text = template.date.Date.ToString("d");
                TemplateName.Text = template.Name;
                TemplatePath.Text = template.filepath;

                SavedExpections = template.ConvertToExpection();
                CheckParams.sections = template.ConvertSectionsToTwips();
                ParamsSaved = true;
                CheckStartB.Enabled = true;
                
            }
            catch (Exception ex)
            {
                template = null;
                MessageBox.Show($"Ошибка импорта файла: {ex.Message}");
            }
        }

        private void ManualB_Click(object sender, EventArgs e)
        {

        }
    }
}
