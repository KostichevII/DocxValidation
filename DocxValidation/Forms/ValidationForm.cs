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

namespace DocxValidation
{
    public partial class ValidationForm : Form
    {
        string CurrectType = "";
        List<FieldSaver> FieldsSavers = new List<FieldSaver>();
        List<FieldSaver> Save = new List<FieldSaver>();

        List<DocChecker.CheckerClasses.Expection> SavedExpections = new List<DocChecker.CheckerClasses.Expection>();
        Template template = null;
        bool templateGood = true;

        public ValidationForm()
        {
            InitializeComponent();
        }

        private DocChecker.CheckerClasses.ExpectionType StringConvertToExp(string Stype)
        {
            switch (Stype)
            {
                case "Основной текст":
                    {
                        return CheckerClasses.ExpectionType.MainText;
                    }
                case "Заголовки":
                    {
                        return CheckerClasses.ExpectionType.MainTextHeader;
                    }
                case "Подписи к рисункам/таблицам":
                    {
                        return CheckerClasses.ExpectionType.MainTextLabel;
                    }
                case "Заголовки таблиц":
                    {
                        return CheckerClasses.ExpectionType.TableHeader;
                    }
                case "Основной текст таблицы":
                    {
                        return CheckerClasses.ExpectionType.TableText;
                    }
                default:
                    {
                        return CheckerClasses.ExpectionType.Unknow;
                    }
            }
        }
        private bool ReadFields(DocChecker.CheckerClasses.ExpectionType type)
        {

            DocChecker.CheckerClasses.Expection Exp = new DocChecker.CheckerClasses.Expection();
            int SavedPos = -1;
            
            for(int i = 0; i<SavedExpections.Count; i++)
            {
                if (SavedExpections[i].Type == type)
                {
                    SavedPos = i;
                    break;
                }
            }

            double LM; // Left margin
            double RM; // Right margin
            string FLT; // First line type
            double FLS; // First line size

            int SB; // Spacing before
            int SA; // Spacing after
            string LST; // Line spacing type
            double LSV; // Line spacing value

            string TAL; // Text aligment

            string MF; // Main Font
            int FS; // Font size

            string AF; // Additional font
            int FR; // Font size range

            bool ItalicTerms; // Italic for terms
            bool BoldHead; // Bold headings

            LM = Convert.ToDouble(LeftM.Value);
            RM = Convert.ToDouble(RightM.Value);
            FLS = Convert.ToDouble(FirstLS.Value);
            if (!String.IsNullOrEmpty(FirstLT.Text))
            {
                FLT = FirstLT.Text;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }

            SB = Convert.ToInt32(SpB.Value);
            SA = Convert.ToInt32(SpA.Value);
            LSV = Convert.ToDouble(LineSV.Value);
            if (!String.IsNullOrEmpty(LineST.Text))
            {
                LST = LineST.Text;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }

            if (!String.IsNullOrEmpty(TextAl.Text))
            {
                TAL = TextAl.Text;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }

            FS = Convert.ToInt32(FontS.Value); 
            if (!String.IsNullOrEmpty(MainF.Text))
            {
                MF = MainF.Text;
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }

            FR = Convert.ToInt32(FontR.Value);
            if (!String.IsNullOrEmpty(AddF.Text))
            {
                AF = AddF.Text;
            }
            else
            {
                AF = null;
            }

            ItalicTerms = ItalTerms.Checked;
            BoldHead = BoldHeadings.Checked;

            bool It = Italic.Checked;
            bool Bol = Bold.Checked;
            bool Und = Under.Checked;

            // Положение номера списка
            double LNP = Convert.ToDouble(ListSpacing.Value);
            // Положение текста после номера
            double LTP = Convert.ToDouble(ListTextInd.Value);
            // Настройка символа после номера
            string LSN = SymAfterNum.Text;
            // Настройка табуляции 
            double TS = Convert.ToDouble(TabVal.Value);


            switch (TAL)
            {
                case "По левому краю":
                    {
                        Exp.setJustification("left");
                        break;
                    }
                case "По правому краю":
                    {
                        Exp.setJustification("right");
                        break;
                    }
                case "По ширине":
                    {
                        Exp.setJustification("both");
                        break;
                    }
                case "По центру":
                    {
                        Exp.setJustification("center");
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Неверно заданный способ выравнивания");
                        return false;
                    }
            }
            switch (FLT)
            {
                case "Отсутствует":
                    {

                        Exp.setIdentetion(0, 0,
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", RM)),
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LM)));
                        break;
                    }
                case "Отступ":
                    {
                        Exp.setIdentetion(0,
                           Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", FLS), 0),
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", RM), 0),
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LM), 0));
                        break;
                    }
                case "Выступ":
                    {
                        Exp.setIdentetion(
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", FLS)), 0,
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", RM)),
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LM)));
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Неверно задан отступ первой строки");
                        return false;
                    }
            }
            switch (LST)
            {
                case "Одинарный":
                    {
                        Exp.setSpacing("auto", 240,
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
                case "1,5 строки":
                    {
                        Exp.setSpacing("auto", 360,
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
                case "Двойной":
                    {
                        Exp.setSpacing("auto", 480,
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
                case "Минимум":
                    {
                        Exp.setSpacing("atLeast",
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", LSV),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
                case "Точно":
                    {
                        Exp.setSpacing("exact",
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", LSV),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
                case "Множитель":
                    {
                        Exp.setSpacing("auto", 240* LSV,
                           DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                           DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
            }
            switch (LSN)
            {
                case "Табуляция":
                    {
                        Exp.setListIndentation(LNP, LNP, LTP, "tab", -1);
                        break;
                    }
                case "Табуляция с настройкой позиции":
                    {
                        Exp.setListIndentation(LNP, LNP, LTP, "tab", TS);
                        break;
                    }
                case "Пробел":
                    {
                        Exp.setListIndentation(LNP, LNP, LTP, "space", -1);
                        break;
                    }
                case "(нет)":
                    {
                        Exp.setListIndentation(LNP, LNP, LTP, "nothing", -1);
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Ошибка сохранения: Поле символа после номера имеет недопустимое значение");
                        return false;
                    }
            }

            Exp.setAllowance(AF, FR*2, ItalicTerms, BoldHead, false);
            Exp.setFont(MF, FS*2, It, Bol, Und);
            Exp.Type = type;
            if (SavedPos != -1)
            {
                SavedExpections[SavedPos] = Exp;
            }
            else
            {
                SavedExpections.Add(Exp);
            }

            return true;
        }
        private void SaveFields(string Type)
        {
            int SaverPosition = -1;
            FieldSaver Fields = new FieldSaver();

            for (int i = 0; i<FieldsSavers.Count; i++)
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
            foreach (var Params in SavedExpections)
            {
                if (Params.Type == DocChecker.CheckerClasses.ExpectionType.MainText)
                {
                    return true;
                }
            }

            MessageBox.Show("Ошибка: Обязательно должны быть заданы параметры для основного текста");
            return false;
        }
        private void ChangeChecks()
        {
            bool check;
            for (int i = 0; i < CheckSavedTypes.Items.Count; i++)
            {
                check = false;
                foreach (var exp in SavedExpections)
                {
                    if (StringConvertToExp(CheckSavedTypes.Items[i].ToString()) == exp.Type)
                    {
                        CheckSavedTypes.SetItemChecked(i, true);
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    CheckSavedTypes.SetItemChecked(i, false);
                }
            }
           
        }
        private void TempChangeCheck()
        {
            bool check;
            for (int i = 0; i < CheckSavedTypes.Items.Count; i++)
            {
                check = false;
                foreach (var exp in FieldsSavers)
                {
                    if (CheckSavedTypes.Items[i].ToString() == exp.TypeToString())
                    {
                        CheckSavedTypes.SetItemChecked(i, true);
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    CheckSavedTypes.SetItemChecked(i, false);
                }
            }
        }

        private void FileDialogButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string fileName = openFileDialog1.FileName;
            DocAdress.Text = fileName;
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
        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(DocAdress.Text))
            {
                MessageBox.Show("Не выбран проверяемый документ");
                return;
            }

            if (CheckSave())
            {
                if (!templateGood)
                {
                    MessageBox.Show("Не выбраны действия с выбранным шаблоном");
                    return;
                }
                string result = DocChecker.CheckerFuncs.CheckDocument(DocAdress.Text, SavedExpections, true);
                using (var tw = new StreamWriter("result.txt", false))
                {
                    tw.WriteLine(result);
                }
            }
        }
        private void TextB_Click(object sender, EventArgs e)
        {
            DocChecker.CheckerClasses.ExpectionType type = StringConvertToExp(TextType.Text);
            if (type == CheckerClasses.ExpectionType.Unknow)
            {
                MessageBox.Show("Выберите тип текста, параметры которого хотите сохранить");
                return;
            }
            if (ReadFields(type))
            {
                int couter = 0;
                foreach (var Type in CheckSavedTypes.Items)
                {
                    if (TextType.Text == Type.ToString())
                    {
                        CheckSavedTypes.SetItemChecked(couter, true);
                        return;
                    }
                    couter++;
                }
            }
            MessageBox.Show("Ошибка отметки сохранения типа текста");
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
                Save = FieldsSavers;
                FieldsSavers = template.Fields;
                templateGood = false;
                TextB.Enabled = false;
                ParamsClear.Enabled = false;
                ParametrsReturn.Enabled = true;
                TableParamSave.Enabled = true;

                TemplateDate.Text = template.date.Date.ToString("d");
                TemplateName.Text = template.Name;
                TemplatePath.Text = template.filepath;

                TempChangeCheck();

            }
            catch (Exception ex)
            {
                template = null;
                MessageBox.Show($"Ошибка импорта файла: {ex.Message}");
            }
        }
        private void TableParamSave_Click(object sender, EventArgs e)
        {
            try
            {
                SavedExpections = template.ConvertToExpection();
                FieldsSavers = template.Fields;
                ChangeChecks();
                TextB.Enabled = true;
                ParamsClear.Enabled = true;
                templateGood = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения параметров: {ex.Message}");
                return;
            }
        }
        private void ParametrsReturn_Click(object sender, EventArgs e)
        {
            TextB.Enabled = true;
            ParamsClear.Enabled = true;
            templateGood = true;
            ParametrsReturn.Enabled = false;
            TableParamSave.Enabled = false;
            FieldsSavers = Save;
            TemplateDate.Text = "";
            TemplateName.Text = "";
            TemplatePath.Text = "";
            ChangeChecks();
        }
        private void ParamsClear_Click(object sender, EventArgs e)
        {
            SetStandartFields();
            int Pos = -1;


            DocChecker.CheckerClasses.ExpectionType type;

            switch (TextType.Text)
            {
                case "Основной текст":
                    {
                        type = CheckerClasses.ExpectionType.MainText;
                        break;
                    }
                case "Заголовки":
                    {
                        type = CheckerClasses.ExpectionType.MainTextHeader;
                        break;
                    }
                case "Подписи к рисункам/таблицам":
                    {
                        type = CheckerClasses.ExpectionType.MainTextLabel;
                        break;
                    }
                case "Заголовки таблиц":
                    {
                        type = CheckerClasses.ExpectionType.TableHeader;
                        break;
                    }
                case "Основной текст таблицы":
                    {
                        type = CheckerClasses.ExpectionType.TableText;
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Ошибка удаления сохранённых параметров");
                        return;
                    }

            }

            for (int i = 0; i < SavedExpections.Count; i++)
            {
                if (SavedExpections[i].Type == type)
                {
                    Pos = i;
                    break;
                }
            }

            if (Pos != -1)
            {
                int counter = 0;
                SavedExpections.RemoveAt(Pos);

                foreach (var Type in CheckSavedTypes.Items) 
                { 
                    if (TextType.Text == Type.ToString())
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
