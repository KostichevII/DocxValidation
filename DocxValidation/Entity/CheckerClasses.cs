using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocChecker
{
        //Класс для хранения полных настроек
        public class CheckParametrs
        {
            public List<Expection> exp;
            //sections[0] - параметры книжной ориентации
            //sections[1] - параметры альбомной ориентации
            public List<SectionInfo> sections;

            public GeneralRestriction restriction;

            public CheckParametrs()
            {
                exp = new List<Expection>();
                sections = new List<SectionInfo>();
            }
        }
        // Класс для хранения допущений
        public class Allowance
        {
            public string ExtraFontType;
            public int AccRange;
            public bool AllowItalic;
            public bool BoldHeaders;
            public bool AllowUnderLines;

            public Allowance()
            {
                ExtraFontType = null;
                AccRange = 0;
                AllowItalic = false;
                BoldHeaders = false;
                AllowUnderLines = false;
            }
            public Allowance(string extra, int acc, bool ital, bool bold, bool under)
            {
                ExtraFontType = extra;
                AccRange = acc;
                AllowItalic = ital;
                BoldHeaders = bold;
                AllowUnderLines = under;
            }
        }
        //Класс для хранения заданных пользователем настроек текста
        public class Expection
        {
            public ExpectionType Type;
            public ParagraphProperties paragraphExpections;
            public RunProperties runExpections;
            public Allowance allowance;
            public ListInd listExpextions;

            public Expection()
            {
                paragraphExpections = new ParagraphProperties();
                runExpections = new RunProperties();
                allowance = new Allowance();
                listExpextions = new ListInd();
            }
            public void setType(string type)
            {
                switch (type)
                {
                    case "MainText":
                        {
                            Type = ExpectionType.MainText;
                            break;
                        }
                    case "MainTextHeader":
                        {
                            Type = ExpectionType.MainTextHeader;
                            break;
                        }
                    case "TableText":
                        {
                            Type = ExpectionType.TableText;
                            break;
                        }
                    case "TableHeader":
                        {
                            Type = ExpectionType.TableHeader;
                            break;
                        }
                    case "MainTextLabel":
                        {
                            Type = ExpectionType.MainTextLabel;
                            break;
                        }
                    default:
                        {
                            Type = ExpectionType.Unknow;
                            break;
                        }
                }
            }
            public void setJustification(string justif)
            {
                paragraphExpections.Justification = new Justification()
                {
                    Val = new JustificationValues(justif)
                };
            }
            public void setSpacing(string LineRule, double Line, double Before, double After)
            {
                paragraphExpections.SpacingBetweenLines = new SpacingBetweenLines()
                {
                    Line = Line.ToString(),
                    Before = Before.ToString(),
                    After = After.ToString(),
                    LineRule = new LineSpacingRuleValues(LineRule)
                };
            }
            public void setIdentetion(double Hanging, double FirstLine, double Right, double Left)
            {
                paragraphExpections.Indentation = new Indentation()
                {
                    Hanging = Hanging.ToString(),
                    FirstLine = FirstLine.ToString(),
                    Right = Right.ToString(),
                    Left = Left.ToString()
                };
            }
            public void setAllowance(string extra, int acc, bool ital, bool bold, bool under)
            {
                allowance = new Allowance(extra, acc, ital, bold, under);
            }
            public void setFont(string SFontType, int SFontSize, bool Bitalic, bool Bbold, bool BunderLine)
            {
                Bold bold = null;
                Italic italic = null;
                Underline underLine = null;
                ItalicComplexScript italicCompl = null;
                BoldComplexScript boldCompl = null;

                if (Bitalic)
                {
                    italic = new Italic();
                    italicCompl = new ItalicComplexScript();
                }

                if (Bbold)
                {
                    bold = new Bold();
                    boldCompl = new BoldComplexScript();
                }

                if (BunderLine)
                {
                    underLine = new Underline();
                }

                runExpections = new DocumentFormat.OpenXml.Wordprocessing.RunProperties(
                    new RunFonts()
                    {
                        Ascii = SFontType,
                        HighAnsi = SFontType,
                        EastAsia = SFontType,
                        ComplexScript = SFontType
                    },
                    new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = SFontSize.ToString() },
                    new FontSizeComplexScript() { Val = SFontSize.ToString() },
                    underLine,
                    bold,
                    boldCompl,
                    italicCompl,
                    italic
                );
            }
            public void setListIndentation(double firstLine, double left, string symAfter, double tabV)
            {
                listExpextions.SymAfterNum = symAfter;
                listExpextions.TabValue = tabV;

                if (firstLine > left)
                {
                    // не факт
                    listExpextions.Left = left;
                    listExpextions.FirstLine = firstLine - left;
                    listExpextions.Hanging = -1;
                }
                else if (firstLine < left)
                {
                    listExpextions.Left = left;
                    listExpextions.Hanging = left - firstLine;
                    listExpextions.FirstLine = -1;
                }
                else
                {
                    listExpextions.Left = left;
                    listExpextions.FirstLine = 0;
                    listExpextions.Hanging = 0;
                }

            }
        }
        // Класс для сбора информации о шрифте
        public class FontInfo
        {
            public string FontType;
            public int FontSize;
            public string Italic;
            public string Bold;
            public string UnderLine;

            public FontInfo()
            {
                FontType = null;
                FontSize = -1;
                Italic = null;
                Bold = null;
                UnderLine = null;
            }

            public bool CheckValues()
            {
                if (FontType != null && FontSize != -1 && Italic != null && Bold != null && UnderLine != null)
                {
                    return true;
                }
                return false;
            }
        }
        // Класс для хранения отступов в списках
        public class ListInd
        {
            public double Hanging;
            public double FirstLine;
            public double Left;
            public string SymAfterNum;
            public double TabValue;

            public ListInd()
            {
                Hanging = -1;
                FirstLine = -1;
                Left = -1;
                SymAfterNum = "tab";
                TabValue = -1;
            }

            public ListInd(double hanging, double firstLine, double left, string symbol, double tab)
            {
                Hanging = hanging;
                FirstLine = firstLine;
                Left = left;
                SymAfterNum = symbol;
                TabValue = tab;
            }

            public void SetParams(double firstLine, double left, string symbol, double tab)
            {
                SymAfterNum = symbol;
                TabValue = tab;

                if (firstLine > left)
                {
                    // не факт
                    Left = left;
                    FirstLine = firstLine - left;
                    Hanging = -1;
                }
                else if (firstLine< left)
                {
                    Left = left;
                    Hanging = left - firstLine;
                    FirstLine = -1;
                }
                else
                {
                    Left= left;
                    FirstLine = 0;
                    Hanging = 0;
                }
            }

        }
}
