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

namespace DocxValidation
{
    public partial class ValidationForm : Form
    {
        bool Saved = false;
        DocChecker.CheckerClasses.Expection MainTextExp = new DocChecker.CheckerClasses.Expection();

        public ValidationForm()
        {
            InitializeComponent();
        }

        private bool ReadFields()
        {
            double LM; // Left margin
            double RM; // Right margin
            string FLT; // First line type
            double FLS; // First line size

            int SB; // Spacing before
            int SA; // Spacing after
            string LST; // Line spacing type
            double LSV; // Line spacing value

            string TAl; // Text aligment

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
                TAl = FirstLT.Text;
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

            // Выравнивание текста
            string J = TextAl.Text;

            // Положение номера списка
            double LNP = Convert.ToDouble(ListSpacing.Value);
            // Положение текста после номера
            double LTP = Convert.ToDouble(ListSpacing.Value);
            // Настройка символа после номера
            string LSN = SymAfterNum.Text;
            // Настройка табуляции 
            double TS = Convert.ToDouble(TabVal.Value);


            switch (J)
            {
                case "По левому краю":
                    {
                        MainTextExp.setJustification("left");
                        break;
                    }
                case "По правому краю":
                    {
                        MainTextExp.setJustification("right");
                        break;
                    }
                case "По ширине":
                    {
                        MainTextExp.setJustification("both");
                        break;
                    }
                case "По центру":
                    {
                        MainTextExp.setJustification("center");
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

                        MainTextExp.setIdentetion(0, 0, 
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", RM)),
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LM)));
                        break;
                    }
                case "Отступ":
                    {
                        MainTextExp.setIdentetion(0,
                           Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", FLS), 0),
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", RM), 0),
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LM), 0));
                        break;
                    }
                case "Выступ":
                    {
                        MainTextExp.setIdentetion(
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
                        MainTextExp.setSpacing("auto", 240,
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
                case "1,5 строки":
                    {
                        MainTextExp.setSpacing("auto", 360,
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
                case "Двойной":
                    {
                        MainTextExp.setSpacing("auto", 480,
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
                case "Минимум":
                    {
                        MainTextExp.setSpacing("atLeast",
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", LSV),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
                case "Точно":
                    {
                        MainTextExp.setSpacing("exact", 
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", LSV),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                            DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
                case "Множитель":
                    {
                        MainTextExp.setSpacing("auto", 240*LSV,
                           DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                           DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
            }
            MainTextExp.setAllowance(AF, FR*2, ItalicTerms, BoldHead, false);
            MainTextExp.setFont(MF, FS*2, It, Bol, Und);

            return true;
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

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(DocAdress.Text))
            {
                MessageBox.Show("Не выбран проверяемый документ");
                return;
            }
            string result = DocChecker.CheckerFuncs.ReadWordDocument(DocAdress.Text, MainTextExp, true);
            using (var tw = new StreamWriter("result.txt", true))
            {
                tw.WriteLine(result);
            }
        }

        private void TextB_Click(object sender, EventArgs e)
        {
            if (ReadFields())
            {
                MessageBox.Show("Настройка сохранена");
            }
            
        }

    }
}
