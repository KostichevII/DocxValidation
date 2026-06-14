using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static JornalWriter.JornalClass;
using DocumentFormat.OpenXml.Bibliography;

namespace DocxValidation
{
    public partial class JornalForm : Form
    {
        List<Record> records =new List<Record>();
        Jornal jornal = new Jornal();
        public JornalForm()
        {
            InitializeComponent();
            ReadAndShowFiles();
        }
        private void ReadAndShowFiles()
        {
            FileList.Items.Clear();
            SortMod.Text = "Нет";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(path) ) 
            {
                MessageBox.Show("Не обнаружена папка logs", "Ошибка", (MessageBoxButtons)0, (MessageBoxIcon)16);
                return;
            }

            try
            {
                string[] files = Directory.GetFiles(path, "??.??.???? Log*.txt");

                if (files.Length == 0) 
                {
                    FileList.Text = "Журналов не найдено";
                    return;
                }

                foreach (string file in files) 
                {
                    FileList.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
            catch(Exception e )
            {
                MessageBox.Show($"{e}", "Ошибка", (MessageBoxButtons)0, (MessageBoxIcon)16);
            }
        }
        private List<string> GetFilter()
        {
            List<string> L= new List<string>();
            if (checkError.Checked)
            {
                L.Add("Error");
            }
            if (checkNormal.Checked)
            {
                L.Add("Normal");
            }
            if (checkFatal.Checked)
            {
                L.Add("Fatal");
            }
            if (checkWarning.Checked)
            {
                L.Add("Warning");
            }
            if (checkUnknow.Checked)
            {
                L.Add("Unknow");
            }

            return L;
        }
        private void JornalShow()
        {
            List<string> Filter = GetFilter();
            records.Clear();
            records = jornal.GetRecordsWithSorting(Filter);
            TextList.Items.Clear();

            string mode = SortMod.Text;

            switch (mode)
            {
                case "Нет":
                    {
                        break;
                    }
                case "По времени":
                    {
                        records.Sort(new Comparison<Record>((x, y) => TimeSpan.Compare(x.ReturnTime(), y.ReturnTime())));
                        break;
                    }
                case "По типу":
                    {
                        records.Sort(new Comparison<Record>((x, y) =>  CompareInt(x,y)));
                        break;
                    }
                case "По модулю":
                    {
                        records.Sort(new Comparison<Record>((x, y) => String.Compare(x.GetModuleName(), y.GetModuleName())));
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Задан неверный способ сортировки", "Внимание", (MessageBoxButtons)0, (MessageBoxIcon)48);
                        return;
                    }
            }


            foreach (Record rec in records)
            {
                TextList.Items.Add(rec.ToString());
            }
        }
        private int CompareInt(Record x, Record y)
        {
            int xPriority = x.TypePriority();
            int yPriority = y.TypePriority();

            if (xPriority > yPriority) 
            {
                return -1;
            }
            if (xPriority < yPriority)
            {
                return 1;
            }
            return 0;
        }
        private void RefreshB_Click(object sender, EventArgs e)
        {
            ReadAndShowFiles();
        }
        private void FileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FileList.SelectedIndex != -1)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                if(!Directory.Exists(path) ) 
                {
                    MessageBox.Show("Файл logs не обнаружен", "Ошибка", (MessageBoxButtons)0, (MessageBoxIcon)16);
                    return;
                }

                try
                {
                    string file = Path.Combine(path,FileList.SelectedItem + ".txt");
                    jornal.ReadLog(file);
                    JornalShow();
                }
                catch(Exception exp)
                {
                    MessageBox.Show($"{exp}", "Ошибка открытия файла", (MessageBoxButtons)0, (MessageBoxIcon)16);
                }
            }
        }
        private void checkNormal_CheckedChanged(object sender, EventArgs e)
        {
            JornalShow();
        }
        private void checkWarning_CheckedChanged(object sender, EventArgs e)
        {
            JornalShow();
        }
        private void checkError_CheckedChanged(object sender, EventArgs e)
        {
            JornalShow();
        }
        private void checkFatal_CheckedChanged(object sender, EventArgs e)
        {
            JornalShow();
        }
        private void checkUnknow_CheckedChanged(object sender, EventArgs e)
        {
            JornalShow();
        }
        private void SortMod_TextChanged(object sender, EventArgs e)
        {
            JornalShow();
        }
        private void JornalForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // ????
            //Dispose();
        }
    }
}
