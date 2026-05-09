using DocChecker;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static JornalWriter.JornalClass;

namespace DocxValidation
{
    public partial class TemplateForm : Form
    {
        public TemplateForm()
        {
            InitializeComponent();
            ReadAndShowFiles();
        }

        string CurrectType = "";
        int currectPos = -1;
        List<FieldSaver> FieldsSavers = new List<FieldSaver>();
        List<FieldSaver> TempSavers = new List<FieldSaver>();
        Template template = new Template();

        private void SaveFields(string Type)
        {
            int SaverPosition = -1;
            FieldSaver Fields = new FieldSaver();

            for (int i = 0; i < FieldsSavers.Count; i++)
            {
                if (FieldsSavers[i].TypeToString() == Type)
                {
                    SaverPosition = i;
                    Fields = FieldsSavers[i];
                    break;
                }
            }
            if (SaverPosition == -1)
            {
                if (!Fields.SetType(TextType.Text))
                {
                    MessageBox.Show("Ошибка сохранения настроек");
                    return;
                }
            }

            Fields.LM = Convert.ToDouble(LeftM.Value);
            Fields.RM = Convert.ToDouble(RightM.Value);
            Fields.FLS = Convert.ToDouble(FirstLS.Value);
            Fields.FLT = FirstLT.Text;
            Fields.SB = Convert.ToInt32(SpB.Value);
            Fields.SA = Convert.ToInt32(SpA.Value);
            Fields.LSV = Convert.ToDouble(LineSV.Value);
            Fields.LST = LineST.Text;
            Fields.TAL = TextAl.Text;
            Fields.FS = Convert.ToInt32(FontS.Value);
            Fields.MF = MainF.Text;
            Fields.FR = Convert.ToInt32(FontR.Value);
            Fields.AF = AddF.Text;
            Fields.ItalicTerms = ItalTerms.Checked;
            Fields.BoldHead = BoldHeadings.Checked;
            Fields.Ital = Italic.Checked;
            Fields.Bold = Bold.Checked;
            Fields.Und = Under.Checked;
            Fields.TAL = TextAl.Text;
            Fields.LNP = Convert.ToDouble(ListSpacing.Value);
            Fields.LTP = Convert.ToDouble(ListTextInd.Value);
            Fields.LSN = SymAfterNum.Text;
            Fields.TS = Convert.ToDouble(TabVal.Value);

            if (SaverPosition != -1)
            {
                FieldsSavers[SaverPosition] = Fields;
            }
            else
            {
                FieldsSavers.Add(Fields);
            }
        }
        private void SaveSectionsFields()
        {
            DocChecker.CheckerClasses.SectionInfo PortInfo = new CheckerClasses.SectionInfo();
            DocChecker.CheckerClasses.SectionInfo LandInfo = new CheckerClasses.SectionInfo();
            PortInfo.Top = Convert.ToDouble(SectionTopPortrait.Value);
            PortInfo.Bottom = Convert.ToDouble(SectionBottomPortrait.Value);
            PortInfo.Left = Convert.ToDouble(SectionLeftPortrait.Value);
            PortInfo.Right = Convert.ToDouble(SectionRightPortrait.Value);
            PortInfo.Header = Convert.ToDouble(SectionHeaderPortrait.Value);
            PortInfo.Footer = Convert.ToDouble(SectionFooterPortrait.Value);

            PortInfo.Orientation = "portrait";
            PortInfo.PageWidth = 11906;
            PortInfo.PageHeight = 16838;


            LandInfo.Top = Convert.ToDouble(SectionTopLandScape.Value);
            LandInfo.Bottom = Convert.ToDouble(SectionBottomLandScape.Value);
            LandInfo.Left = Convert.ToDouble(SectionLeftLandScape.Value);
            LandInfo.Right = Convert.ToDouble(SectionRightLandScape.Value);
            LandInfo.Header = Convert.ToDouble(SectionHeaderLandScape.Value);
            LandInfo.Footer = Convert.ToDouble(SectionFooterLandScape.Value);

            LandInfo.Orientation = "landscape";
            LandInfo.PageWidth = 16838;
            LandInfo.PageHeight = 11906;

            template.Sections.Add(PortInfo);
            template.Sections.Add(LandInfo);
        }


        private void SetStandartFields()
        {
            LeftM.Value = 0;
            RightM.Value = 0;
            FirstLS.Value = Convert.ToDecimal(1.25);
            FirstLT.Text = "Отступ";

            SpB.Value = 0;
            SpA.Value = 0;
            LineSV.Value = 0;
            LineST.Text = "1,5 строки";


            FontS.Value = 14;
            MainF.Text = "Times New Roman";
            FontR.Value = 4;
            AddF.Text = "Calibri";

            ItalTerms.Checked = false;
            BoldHeadings.Checked = false;

            Italic.Checked = false;
            Bold.Checked = false;
            Under.Checked = false;

            TextAl.Text = "По ширине";

            ListSpacing.Value = Convert.ToDecimal(1.25);
            ListTextInd.Value = 0;
            SymAfterNum.Text = "Табуляция";
            TabVal.Value = 0;
        }
        private void SetField(string Type)
        {
            int SaverPosition = -1;
            FieldSaver Fields = new FieldSaver();
            for (int i = 0; i < FieldsSavers.Count; i++)
            {
                if (FieldsSavers[i].TypeToString() == Type)
                {
                    SaverPosition = i;
                    Fields = FieldsSavers[i];
                    break;
                }
            }

            if (SaverPosition == -1)
            {
                SetStandartFields();
            }
            else
            {
                LeftM.Value = Convert.ToDecimal(Fields.LM);
                RightM.Value = Convert.ToDecimal(Fields.RM);
                FirstLS.Value = Convert.ToDecimal(Fields.FLS);
                FirstLT.Text = Fields.FLT;
                SpB.Value = Convert.ToDecimal(Fields.SB);
                SpA.Value = Convert.ToDecimal(Fields.SA);
                LineSV.Value = Convert.ToDecimal(Fields.LSV);
                LineST.Text = Fields.LST;
                TextAl.Text = Fields.TAL;
                FontS.Value = Convert.ToDecimal(Fields.FS);
                MainF.Text = Fields.MF;
                FontR.Value = Convert.ToDecimal(Fields.FR);
                AddF.Text = Fields.AF;
                ItalTerms.Checked = Fields.ItalicTerms;
                BoldHeadings.Checked = Fields.BoldHead;
                Italic.Checked = Fields.Ital;
                Bold.Checked = Fields.Bold;
                Under.Checked = Fields.Und;
                TextAl.Text = Fields.TAL;
                ListSpacing.Value = Convert.ToDecimal(Fields.LNP);
                ListTextInd.Value = Convert.ToDecimal(Fields.LTP);
                SymAfterNum.Text = Fields.LSN;
                TabVal.Value = Convert.ToDecimal(Fields.TS);
            }
        }
        private bool CheckSave()
        {
            foreach (var Params in TempSavers)
            {
                if (Params.Type == "MainText")
                {
                    return true;
                }
            }

            MessageBox.Show("Ошибка: Обязательно должны быть заданы параметры для основного текста");
            return false;
        }
        private bool ReadFields(string type)
        {
            FieldSaver Saver = new FieldSaver();
            int SavedPos = -1;

            for (int i = 0; i < TempSavers.Count; i++)
            {
                if (TempSavers[i].Type == type)
                {
                    SavedPos = i;
                    break;
                }
            }

            Saver.LM = Convert.ToDouble(LeftM.Value);
            Saver.RM = Convert.ToDouble(RightM.Value);
            Saver.FLS = Convert.ToDouble(FirstLS.Value);
            if (!String.IsNullOrEmpty(FirstLT.Text))
            {
                Saver.FLT = FirstLT.Text;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }

            Saver.SB = Convert.ToInt32(SpB.Value);
            Saver.SA = Convert.ToInt32(SpA.Value);
            Saver.LSV = Convert.ToDouble(LineSV.Value);
            if (!String.IsNullOrEmpty(LineST.Text))
            {
                Saver.LST = LineST.Text;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }

            if (!String.IsNullOrEmpty(TextAl.Text))
            {
                Saver.TAL = TextAl.Text;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }

            Saver.FS = Convert.ToInt32(FontS.Value);
            if (!String.IsNullOrEmpty(MainF.Text))
            {
                Saver.MF = MainF.Text;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }

            Saver.FR = Convert.ToInt32(FontR.Value);
            if (!String.IsNullOrEmpty(AddF.Text))
            {
                Saver.AF = AddF.Text;
            }
            else
            {
                Saver.AF = null;
            }

            Saver.ItalicTerms = ItalTerms.Checked;
            Saver.BoldHead = BoldHeadings.Checked;

            Saver.Ital = Italic.Checked;
            Saver.Bold = Bold.Checked;
            Saver.Und = Under.Checked;

            Saver.LNP = Convert.ToDouble(ListSpacing.Value);
            Saver.LTP = Convert.ToDouble(ListTextInd.Value);
            Saver.LSN = SymAfterNum.Text;
            Saver.TS = Convert.ToDouble(TabVal.Value);

            Saver.SetType(TextType.Text);

            if (SavedPos != -1)
            {
                TempSavers[SavedPos] = Saver;
            }
            else
            {
                TempSavers.Add(Saver);
            }

            return true;
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
                MessageBox.Show($"Ошибка: {e}");
            }
        }
        private void TemplateShow()
        {
            FieldsSavers = template.Fields;
            TempSavers = template.Fields;
            TemplateName.Text = template.Name;
            CreateDate.Text = template.date.Date.ToString("d");
            foreach(var saver in FieldsSavers)
            {
                for (int i = 0; i < CheckSavedTypes.Items.Count; i++)
                {
                    if (CheckSavedTypes.Items[i].ToString() == saver.TypeToString())
                    {
                        CheckSavedTypes.SetItemChecked(i, true);
                    }
                }
            }
            SetField(CurrectType);
        }

        private void LineST_TextChanged(object sender, EventArgs e)
        {
            string type = LineST.Text;
            switch (type)
            {
                case "Одинарный":
                case "1,5 строки":
                case "Двойной":
                    {
                        label16.Hide();
                        LineSV.Enabled = false;
                        break;
                    }
                case "Минимум":
                case "Точно":
                    {
                        label16.Show();
                        LineSV.Enabled = true;
                        break;
                    }
                case "Множитель":
                    {
                        label16.Hide();
                        LineSV.Enabled = true;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        private void TextType_TextChanged(object sender, EventArgs e)
        {
            if (CurrectType != "")
            {
                SaveFields(CurrectType);
                CurrectType = TextType.Text;
            }
            SetField(TextType.Text);
        }
        private void TextB_Click(object sender, EventArgs e)
        {

            string Type;

            switch (TextType.Text)
            {

                case "Основной текст":
                    {
                        Type = "MainText";
                        break;
                    }
                case "Заголовки":
                    {
                        Type = "MainHeaders";
                        break;
                    }
                case "Подписи к рисункам/таблицам":
                    {
                        Type = "Labels";
                        break;
                    }
                case "Заголовки таблиц":
                    {
                        Type = "TableHeaders";
                        break;
                    }
                case "Основной текст таблицы":
                    {
                        Type = "TableText";
                        break;
                    }
                default:
                    {

                        MessageBox.Show("Выберите тип текста, параметры которого хотите сохранить");
                        return;
                    }
            }

            if (ReadFields(Type))
            {
                int couter = 0;
                foreach (var Check in CheckSavedTypes.Items)
                {
                    if (TextType.Text == Check.ToString())
                    {
                        CheckSavedTypes.SetItemChecked(couter, true);
                        return;
                    }
                    couter++;
                }
            }
            MessageBox.Show("Ошибка отметки сохранения типа текста");
        }
        private void TemplateCreate_Click(object sender, EventArgs e)
        {
            template.CreateNewTemplate();
            TemplateList.Items.Add(Path.GetFileNameWithoutExtension(template.filepath));
            SetStandartFields();
        }
        private void TemplateImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string fileName = openFileDialog1.FileName;
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "templates");
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                savePath = Path.Combine(savePath, Path.GetFileName(fileName));
                File.Copy(fileName, savePath);
                TemplateList.Items.Add(Path.GetFileNameWithoutExtension(fileName));
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка импорта файла: {ex.Message}");
            }
        }
        private void TemplateDelete_Click(object sender, EventArgs e)
        {
            try
            {
                template.DeleteFile();
                SetStandartFields();
                int couter = 0;
                foreach (var Check in CheckSavedTypes.Items)
                {
                    CheckSavedTypes.SetItemChecked(couter, false);
                    couter++;
                }

                TextType.Text = "Основной текст";
                FieldsSavers.Clear();
                TempSavers.Clear();
                ReadAndShowFiles();

            }
            catch (Exception exc)
            {
                MessageBox.Show($"Ошибка удаления файла: {exc.Message}");
            }
        }
        private void TemplateSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = DateTime.Parse(CreateDate.Text);
                string name = TemplateName.Text;

                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Имя шаблона не может быть пустым");
                    return;
                }
                if (!CheckSave())
                {
                    return;
                }
                template.Fields = TempSavers;
                SaveSectionsFields();
                template.date = date;
                template.Name = name;
                template.SaveFile();
                template.RenameFilePath();
                ReadAndShowFiles();
                MessageBox.Show("Шаблон сохранён");
            }
            catch (Exception er)
            {
                MessageBox.Show($"Ошибка сохранения файла: {er.Message}");
            }

        }
        private void RefreshB_Click(object sender, EventArgs e)
        {
            ReadAndShowFiles();
        }
        private void TemplateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TemplateList.SelectedIndex != -1)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "templates");
                if (!Directory.Exists(path))
                {
                    MessageBox.Show("Ошибка: папка templates не обнаружен");
                    TemplateList.SelectedIndex = currectPos;
                    return;
                }

                try
                {
                    string file = Path.Combine(path, TemplateList.SelectedItem + ".temp");
                    if (!template.ReadFile(file))
                    {
                        MessageBox.Show("Ошибка чтения файла");
                        TemplateList.SelectedIndex = currectPos;
                        return;
                    }
                    TemplateShow();
                    currectPos = TemplateList.SelectedIndex;
                }
                catch (Exception exp)
                {
                    MessageBox.Show($"Ошибка открытия файла: {exp}");
                }
            }
        }
        private void ParamsClear_Click(object sender, EventArgs e)
        {

            SetStandartFields();
            int Pos = -1;
            string Type;

            switch (TextType.Text)
            {

                case "Основной текст":
                    {
                        Type = "MainText";
                        break;
                    }
                case "Заголовки":
                    {
                        Type = "MainHeaders";
                        break;
                    }
                case "Подписи к рисункам/таблицам":
                    {
                        Type = "Labels";
                        break;
                    }
                case "Заголовки таблиц":
                    {
                        Type = "TableHeaders";
                        break;
                    }
                case "Основной текст таблицы":
                    {
                        Type = "TableText";
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Ошибка удаления сохранённых параметров");
                        return;
                    }
            }

            for (int i = 0; i < TempSavers.Count; i++)
            {
                if (TempSavers[i].Type == Type)
                {
                    Pos = i;
                    break;
                }
            }

            if (Pos != -1)
            {
                int counter = 0;
                TempSavers.RemoveAt(Pos);

                foreach (var type in CheckSavedTypes.Items)
                {
                    if (TextType.Text == type.ToString())
                    {
                        CheckSavedTypes.SetItemChecked(counter, false);
                        return;
                    }
                    counter++;
                }
            }

        }
    }
}
