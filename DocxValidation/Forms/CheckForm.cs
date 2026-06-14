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
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocxValidation
{
    public partial class CheckForm : Form
    {
        List<FieldSaver> FieldsSavers = new List<FieldSaver>();
        DocChecker.CheckParametrs CheckParams = new DocChecker.CheckParametrs();
        List<DocChecker.Expection> SavedExpections = new List<DocChecker.Expection>();
        Template template = new Template();
        bool ParamsSaved = false;
        bool DocSaved = false;
        int position=-1;


        public CheckForm()
        {
            InitializeComponent();
            ReadAndShowFiles();
        }

        private void CheckReady()
        {
            if (DocSaved && ParamsSaved)
            {
                CheckStartB.Enabled = true;
            }
            else
            {
                CheckStartB.Enabled = false;
            }
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
            DocSaved = true;
            CheckReady();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CheckParams.exp = SavedExpections;
            if (String.IsNullOrEmpty(DocAdress.Text))
            {
                MessageBox.Show("Не выбран проверяемый документ", "Ошибка", (MessageBoxButtons)0, (MessageBoxIcon)16);
                return;
            }

            if (ParamsSaved)
            {
                List<ErrorRecord> result = DocChecker.CheckerFuncs.CheckDocument(DocAdress.Text, CheckParams, true);
                resultShow(result, DocAdress.Text);
            }
        }
        private void ReadAndShowFiles()
        {
            TemplateList.Items.Clear();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "templates");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            try
            {
                string[] files = Directory.GetFiles(path, "*.temp");

                if (files.Length == 0)
                {
                    TemplateList.Text = "Шаблонов не найдено";
                    return;
                }

                foreach (string file in files)
                {
                    TemplateList.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}", "Ошибка", (MessageBoxButtons)0, (MessageBoxIcon)16);
            }
        }
        private void ManualB_Click(object sender, EventArgs e)
        {

        }
        private void TemplateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TemplateList.SelectedIndex != -1)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "templates");
                if (!Directory.Exists(path))
                {
                    MessageBox.Show("Ошибка: папка templates не обнаружен");
                    TemplateList.SelectedIndex = position;
                    return;
                }

                try
                {
                    string file = Path.Combine(path, TemplateList.SelectedItem + ".temp");
                    if (!template.ReadFile(file))
                    {
                        MessageBox.Show("Ошибка чтения файла");
                        TemplateList.SelectedIndex = position;
                        return;
                    }

                    position = TemplateList.SelectedIndex;
                    TemplateDate.Text = template.date.Date.ToString("d");
                    TemplateName.Text = template.Name;

                    SavedExpections = template.ConvertToExpection();
                    CheckParams.sections = template.ConvertSectionsToTwips();
                    CheckParams.restriction = template.Restrictions;

                    ParamsSaved = true;
                    CheckReady();
                }
                catch (Exception exp)
                {
                    MessageBox.Show($"Ошибка открытия файла: {exp}");
                }
            }
        }

        private void ListRefresh_Click(object sender, EventArgs e)
        {
            ReadAndShowFiles();
            position = -1; 
            ParamsSaved= false;
            CheckReady();
        }
    }
}
