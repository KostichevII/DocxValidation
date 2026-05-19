using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocChecker;

namespace DocxValidation
{
    public class FieldSaver
    {

        public string Type;

        public double LM; // Left margin
        public double RM; // Right margin
        public string FLT; // First line type
        public double FLS; // First line size

        public int SB; // Spacing before
        public int SA; // Spacing after
        public string LST; // Line spacing type
        public double LSV; // Line spacing value

        public string MF; // Main Font
        public int FS; // Font size

        public string AF; // Additional font
        public int FR; // Font size range

        public bool ItalicTerms; // Italic for terms
        public bool BoldHead; // Bold headings

        // Выравнивание текста
        public string TAL;

        public bool Ital;
        public bool Bold;
        public bool Und;

        // Положение номера списка
        public double LNP;
        // Положение текста после номера
        public double LTP;
        // Настройка символа после номера
        public string LSN;
        // Настройка табуляции 
        public double TS;

        public bool SetType(string TypeName)
        {
            switch (TypeName)
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
                        return false;
                    }
            }
            return true;
        } 
        public string TypeToString()
        {
            switch (Type)
            {
                case "MainText":
                    {
                        return "Основной текст";
                    }
                case "MainHeaders":
                    {
                        return "Заголовки";
                    }
                case "Labels":
                    {
                        return "Подписи к рисункам/таблицам";
                    }
                case "TableHeaders":
                    {
                        return "Заголовки таблиц";
                    }
                case "TableText":
                    {
                        return "Основной текст таблицы";
                    }
                default:
                    {
                        return "";
                    }
            }
        }
        public ExpectionType TakeType()
        {
            switch (Type)
            {
                case "MainText":
                    {
                        return ExpectionType.MainText;
                    }
                case "MainHeaders":
                    {
                        return ExpectionType.MainTextHeader;
                    }
                case "Labels":
                    {
                        return ExpectionType.MainTextLabel;
                    }
                case "TableHeaders":
                    {
                        return ExpectionType.TableHeader;
                    }
                case "TableText":
                    {
                        return ExpectionType.TableText;
                    }
                default:
                    {
                        return ExpectionType.Unknow;
                    }
            }
        }
        public DocChecker.CheckerClasses.Expection ConvertToExpection()
        {
            DocChecker.CheckerClasses.Expection Exp = new DocChecker.CheckerClasses.Expection();


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
                        return null;
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
                        return null;
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
                        Exp.setSpacing("auto", 240 * LSV,
                           DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SB),
                           DocChecker.CheckerFuncs.ConvertValue("twips", "pt", SA));
                        break;
                    }
                default:
                    {
                        return null;
                    }
            }
            switch (LSN)
            {
                case "Табуляция":
                    {
                        Exp.setListIndentation(Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LNP)),
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LTP)),
                            "tab", -1);
                        break;
                    }
                case "Табуляция с настройкой позиции":
                    {
                        Exp.setListIndentation(Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LNP)),
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LTP)), 
                            "tab", TS);
                        break;
                    }
                case "Пробел":
                    {
                        Exp.setListIndentation(Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LNP)),
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LTP)), 
                            "space", -1);
                        break;
                    }
                case "(нет)":
                    {
                        Exp.setListIndentation( Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LNP)), 
                            Math.Round(DocChecker.CheckerFuncs.ConvertValue("twips", "cm", LTP)), 
                            "nothing", -1);
                        break;
                    }
                default:
                    {
                        return null;
                    }
            }

            Exp.setAllowance(AF, FR * 2, ItalicTerms, BoldHead, false);
            Exp.setFont(MF, FS * 2, Ital, Bold, Und);
            Exp.Type = TakeType();
            if (Exp.Type == ExpectionType.Unknow)
            {
                return null;
            }
            return Exp;
        }
        public string FileExport()
        {
            StringBuilder exportString = new StringBuilder();

            exportString.AppendLine("{");
            exportString.AppendLine($"{Type}");
            exportString.AppendLine($"{LM}");
            exportString.AppendLine($"{RM}");
            exportString.AppendLine($"{FLS}");
            exportString.AppendLine($"{FLT}");
            exportString.AppendLine($"{SB}");
            exportString.AppendLine($"{SA}");
            exportString.AppendLine($"{LSV}");
            exportString.AppendLine($"{LST}");
            exportString.AppendLine($"{TAL}");
            exportString.AppendLine($"{FS}");
            exportString.AppendLine($"{MF}");
            exportString.AppendLine($"{FR}");
            exportString.AppendLine($"{AF}");
            exportString.AppendLine($"{ItalicTerms}");
            exportString.AppendLine($"{BoldHead}");
            exportString.AppendLine($"{Ital}");
            exportString.AppendLine($"{Bold}");
            exportString.AppendLine($"{Und}");
            exportString.AppendLine($"{LNP}");
            exportString.AppendLine($"{LTP}");
            exportString.AppendLine($"{LSN}");
            exportString.AppendLine($"{TS}");
            exportString.AppendLine("}");

            return exportString.ToString();
        }
        public bool StringToBool(string val)
        {
            if (val == "True")
            {
                return true;
            }
            if (val == "False")
            {
                return false;
            }

            throw new Exception("Ошибка чтения значения bool");
        }
        public bool ConvertString(List<string> stringParams)
        {
            try
            {
                Type = stringParams[0];

                LM = Double.Parse(stringParams[1]);
                RM = Double.Parse(stringParams[2]);
                FLS = Double.Parse(stringParams[3]);
                FLT = stringParams[4];

                SB = Int32.Parse(stringParams[5]);
                SA = Int32.Parse(stringParams[6]);
                LSV = Double.Parse(stringParams[7]);
                LST = stringParams[8];

                TAL = stringParams[9];

                FS = Int32.Parse(stringParams[10]);
                MF = stringParams[11];
                FR = Int32.Parse(stringParams[12]);
                AF = stringParams[13];

                ItalicTerms = StringToBool(stringParams[14]);
                BoldHead = StringToBool(stringParams[15]);

                Ital = StringToBool(stringParams[16]);
                Bold = StringToBool(stringParams[17]);
                Und = StringToBool(stringParams[18]);

                LNP = Double.Parse(stringParams[19]);
                LTP = Double.Parse(stringParams[20]);
                LSN = stringParams[21];
                TS = Double.Parse(stringParams[22]);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        public FieldSaver()
        {

        }
    }
}
