using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static DocChecker.CheckerClasses;
using static JornalWriter.JornalClass;

namespace DocChecker
{

    public class CheckerClasses
    {
        //Класс для хранения полных настроек
        public class CheckParametrs
        {
            public List<Expection> exp;
            //sections[0] - параметры книжной ориентации
            //sections[1] - параметры альбомной ориентации
            public List<SectionInfo> sections;

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
            public void setListIndentation(double hanging, double firstLine, double left, string symAfter, double tabV)
            {
                listExpextions.Hanging = hanging;
                listExpextions.FirstLine = firstLine;
                listExpextions.Left = left;
                listExpextions.TabValue = tabV;
                listExpextions.SymAfterNum = symAfter;

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
        }
        //Перечесление типов ошибок текста
        public enum ErrorType
        {
            //Ошибки Run
            Italic,
            Bold,
            UnderLine,
            FontType,
            FontSize,

            //Ошибка выравнивания
            Justification,

            //Ошибки междустрочного интервала
            LineSpacingValue,
            LineSpacingRule,
            BeforeLineValue,
            AfterLineValue,

            //Ошибки отступов параграфов
            LeftIdent,
            RightIdent,
            FirstLine,
            Hanging,

            //Ошибки списков
            ListTextIdent,
            ListNumIdentHanging,
            ListNumIdentFirstLine,
            ListRightIdent,

            //Ошибки форматироавния страниц
            SectionErrorTop,
            SectionErrorBottom,
            SectionErrorLeft,
            SectionErrorRight,
            SectionErrorHeader,
            SectionErrorFooter,

            SectionErrorOrientation,

            SectionErrorPageWidth,
            SectionErrorPageHeight,

            none
        }
        //Перечисление типов текста
        public enum ExpectionType
        {
            MainText,
            MainTextHeader,
            TableText,
            TableHeader,
            MainTextLabel,
            SectionError,
            Unknow
        }
        //Класс для хранения ошибок
        public class ErrorRecord
        {
            public ExpectionType type;
            // Для таблиц :
            // Position[0] - номер таблицы
            // Position[1] - номер строки
            // Position[2] - номер ячейки
            // Position[3] - номер параграфа

            //Для параграфов Position[0]- номер параграфа
            // Position[1] -текст параграфа

            //Для размеров страниц:
            //Position[0] - номер секции

            public List<string> Position;
            public List<(ErrorType, List<string>)> ErrorList;

            public ErrorRecord()
            {
                Position = new List<string> { "", "", "", "" };
                ErrorList = new List<(ErrorType, List<string>)>();
            }
            public string TypeToString()
            {
                switch (type)
                {
                    case ExpectionType.MainText:
                        {
                            return "Основной текст";
                        }
                    case ExpectionType.MainTextHeader:
                        {
                            return "Заголовок в тексте";
                            break;
                        }
                    case ExpectionType.MainTextLabel:
                        {
                            return "Подпись к рисунку/таблице";
                            break;
                        }
                    case ExpectionType.TableHeader:
                        {
                            return "Заголовок таблицы";
                            break;
                        }
                    case ExpectionType.TableText:
                        {
                            return "Основная часть таблицы";
                            break;
                        }
                    case ExpectionType.SectionError:
                        {
                            return "-----";
                        }
                    default:
                        {
                            return "Не опознано";
                        }
                }
            }
            public string PositionToString()
            {
                if (type == ExpectionType.MainText || type == ExpectionType.MainTextLabel || type == ExpectionType.MainTextHeader)
                {
                    return $"Параграф {Position[0]} \n{Position[1]}";
                }
                else
                {
                    if (type == ExpectionType.TableHeader || type == ExpectionType.TableText)
                    {
                        return $"Параграф {Position[3]} в {Position[2]} ячейке {Position[1]} строки  в таблице {Position[0]}";
                    }
                    else
                    {
                        if (type == ExpectionType.SectionError)
                        {
                            return $"Раздел {Position[0]}";
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            private Expection ExpectionTake(List<Expection> expList)
            {
                int index = -1;

                for (int i = 0; i < expList.Count; i++)
                {
                    if (expList[i].Type == type)
                    {
                        index = i;
                        break;
                    }
                }

                if (index != -1)
                {
                    return expList[index];
                }
                else
                {
                    foreach (Expection exp in expList)
                    {
                        if (exp.Type == ExpectionType.MainText)
                        {
                            return exp;
                        }
                    }
                }

                throw new Exception("Не обнаружен стиль оформления для основного текста");
            }
            public string ConvertError(CheckParametrs par)
            {
                string ErrorMessage = "";
                Expection expected = ExpectionTake(par.exp);
                foreach (var Error in ErrorList)
                {
                    switch (Error.Item1)
                    {
                        case ErrorType.Justification:
                            {
                                string expString = expected.paragraphExpections.Justification.Val.ToString();
                                ErrorMessage += $"Неверно заданые параметры выравнивнивания: \nПолучено: {Error.Item2[0]}  Ожидалось: {expString}\n";
                                break;
                            }


                        case ErrorType.LineSpacingValue:
                            {
                                string exp = expected.paragraphExpections.SpacingBetweenLines.Line?.Value?.ToString() ?? "0";
                                ErrorMessage += "Неверное значение междустрочного интервала: \nОжидалось: " + CheckerFuncs.ConvertValue("pt", "twips", Double.Parse(exp)) + " Получено: " + CheckerFuncs.ConvertValue("pt", "twips", Double.Parse(Error.Item2[0])) + "\n";
                                break;
                            }
                        case ErrorType.LineSpacingRule:
                            {
                                string exp = "auto";
                                if (expected.paragraphExpections.SpacingBetweenLines.LineRule != null)
                                {
                                    exp = CheckerFuncs.ConvertLineRule(expected.paragraphExpections.SpacingBetweenLines.LineRule.Value);
                                }

                                ErrorMessage += "Неверное правило междустрочного интервала: \nОжидалось: " + exp + " Получено: " + Error.Item2[0] + "\n";
                                break;
                            }
                        case ErrorType.BeforeLineValue:
                            {
                                string exp = expected.paragraphExpections.SpacingBetweenLines.Before?.Value.ToString() ?? "0";
                                ErrorMessage += "Неверное задан отступ перед абзацем: \nОжидалось: " + CheckerFuncs.ConvertValue("pt", "twips", Double.Parse(exp)) + " Получено: " + CheckerFuncs.ConvertValue("pt", "twips", Double.Parse(Error.Item2[0])) + "\n";
                                break;
                            }
                        case ErrorType.AfterLineValue:
                            {
                                string exp = expected.paragraphExpections.SpacingBetweenLines.After?.Value.ToString() ?? "0";
                                ErrorMessage += "Неверное задан отступ после абзаца: \nОжидалось: " + CheckerFuncs.ConvertValue("pt", "twips", Double.Parse(exp)) + " Получено: " + CheckerFuncs.ConvertValue("pt", "twips", Double.Parse(Error.Item2[0])) + "\n";
                                break;
                            }


                        case ErrorType.LeftIdent:
                            {
                                string exp = expected.paragraphExpections.Indentation.Left?.ToString() ?? "0";
                                ErrorMessage += "Неверно определён левый отступ текста: \nОжидалось: " + CheckerFuncs.ConvertValue("cm", "twips", Double.Parse(exp)) + " Получено: " + CheckerFuncs.ConvertValue("cm", "twips", Double.Parse(Error.Item2[0])) + "\n";
                                break;
                            }
                        case ErrorType.RightIdent:
                            {
                                string exp = expected.paragraphExpections.Indentation.Right?.ToString() ?? "0";
                                ErrorMessage += "Неверно определён правый отступ текста: \nОжидалось: " + CheckerFuncs.ConvertValue("cm", "twips", Double.Parse(exp)) + " Получено: " + CheckerFuncs.ConvertValue("cm", "twips", Double.Parse(Error.Item2[0])) + "\n";
                                break;
                            }
                        case ErrorType.FirstLine:
                            {
                                string exp = expected.paragraphExpections.Indentation.FirstLine?.ToString() ?? "0";
                                ErrorMessage += "Неверно определён отступ красной строки текста: \nОжидалось: " + CheckerFuncs.ConvertValue("cm", "twips", Double.Parse(exp)) + " Получено: " + CheckerFuncs.ConvertValue("cm", "twips", Double.Parse(Error.Item2[0])) + "\n";
                                break;
                            }
                        case ErrorType.Hanging:
                            {
                                string exp = expected.paragraphExpections.Indentation.Hanging?.ToString() ?? "0";
                                ErrorMessage += "Неверно определён выступ первой строки текста: \nОжидалось: " + CheckerFuncs.ConvertValue("cm", "twips", Double.Parse(exp)) + " Получено: " + CheckerFuncs.ConvertValue("cm", "twips", Double.Parse(Error.Item2[0])) + "\n";
                                break;
                            }


                        case ErrorType.ListTextIdent:
                            {
                                string exp = expected.listExpextions.Left.ToString();
                                ErrorMessage += $"Обнаружена ошибка отступа текста элемента списка: \nОжидалось:{exp}  Получено:{Error.Item2[0]}\n";
                                break;
                            }
                        case ErrorType.ListNumIdentHanging:
                            {
                                string exp = expected.listExpextions.Hanging.ToString();
                                ErrorMessage += $"Обнаружена ошибка отступа номера элемента списка: \nОжидалось:{expected.listExpextions.Hanging}  Получено:{Error.Item2[0]}\n";
                                break;
                            }
                        case ErrorType.ListNumIdentFirstLine:
                            {
                                string exp = expected.listExpextions.FirstLine.ToString();
                                ErrorMessage += $"Обнаружена ошибка отступа номера элемента списка: \nОжидалось:{expected.listExpextions.FirstLine}  Получено:{Error.Item2[0]}\n";
                                break;
                            }
                        case ErrorType.ListRightIdent:
                            {
                                ErrorMessage += $"Обнаружена правый отступ текста элемента списка: \nПолучено:{Error.Item2[0]}\n";
                                break;
                            }


                        case ErrorType.Bold:
                            {
                                ErrorMessage += "Обнаружен полужирный текст\n";
                                break;
                            }
                        case ErrorType.Italic:
                            {
                                ErrorMessage += "Обнаружено выделение текста курсивом\n";
                                break;
                            }
                        case ErrorType.UnderLine:
                            {
                                ErrorMessage += "Обнаружено подчёркивание текста\n";
                                break;
                            }
                        case ErrorType.FontType:
                            {
                                string exp = expected.runExpections.RunFonts?.Ascii?.ToString() ?? "Не определён";
                                ErrorMessage += "Неверно заданный тип шрифта: \nОжидалось: " +
                                     exp + "Получено: ";
                                for (int i = 0; i < Error.Item2.Count; i++)
                                {
                                    ErrorMessage += Error.Item2[i];
                                    if (i + 1 < Error.Item2.Count)
                                    {
                                        ErrorMessage += ", ";
                                    }
                                }
                                ErrorMessage += "\n";
                                break;
                            }
                        case ErrorType.FontSize:
                            {
                                ErrorMessage += "Неверно заданный размер шрифта: \nОжидалось: " +
                                    Convert.ToString((int.Parse(expected.runExpections.FontSize.Val) / 2.0)) + "+-"
                                    + Convert.ToString((expected.allowance.AccRange / 2.0)) + " Получено: ";
                                for (int i = 0; i < Error.Item2.Count; i++)
                                {
                                    ErrorMessage += Error.Item2[i];
                                    if (i + 1 < Error.Item2.Count)
                                    {
                                        ErrorMessage += ", ";
                                    }
                                }
                                ErrorMessage += "\n";
                                break;
                            }

                        case ErrorType.SectionErrorTop:
                            {
                                string ExpPar = par.sections[1].Top.ToString();
                                if (Error.Item2[1] == "portrait")
                                {
                                    ExpPar = par.sections[0].Top.ToString();
                                }

                                ErrorMessage += $"Неверно заданы параметры верхнего отступа форматирования страниц: Ожидалось:{ExpPar} Получено:{Error.Item2[0]}\n";
                                break;
                            }
                        case ErrorType.SectionErrorBottom:
                            {
                                string ExpPar = par.sections[1].Bottom.ToString();
                                if (Error.Item2[1] == "portrait")
                                {
                                    ExpPar = par.sections[0].Bottom.ToString();
                                }

                                ErrorMessage += $"Неверно заданы параметры отступа снизу форматирования страниц: Ожидалось:{ExpPar} Получено:{Error.Item2[0]}\n";
                                break;
                            }
                        case ErrorType.SectionErrorLeft:
                            {
                                string ExpPar = par.sections[1].Left.ToString();
                                if (Error.Item2[1] == "portrait")
                                {
                                    ExpPar = par.sections[0].Left.ToString();
                                }

                                ErrorMessage += $"Неверно заданы параметры левого отступа форматирования страниц: Ожидалось:{ExpPar} Получено:{Error.Item2[0]}\n";
                                break;
                            }
                        case ErrorType.SectionErrorRight:
                            {
                                string ExpPar = par.sections[1].Right.ToString();
                                if (Error.Item2[1] == "portrait")
                                {
                                    ExpPar = par.sections[0].Right.ToString();
                                }

                                ErrorMessage += $"Неверно заданы параметры правого отступа форматирования страниц: Ожидалось:{ExpPar} Получено:{Error.Item2[0]}\n";
                                break;
                            }
                        case ErrorType.SectionErrorHeader:
                            {
                                string ExpPar = par.sections[1].Header.ToString();
                                if (Error.Item2[1] == "portrait")
                                {
                                    ExpPar = par.sections[0].Header.ToString();
                                }

                                ErrorMessage += $"Неверно заданы параметры верхнего колонтитула форматирования страниц: Ожидалось:{ExpPar} Получено:{Error.Item2[0]}\n";
                                break;
                            }
                        case ErrorType.SectionErrorFooter:
                            {
                                string ExpPar = par.sections[1].Footer.ToString();
                                if (Error.Item2[1] == "portrait")
                                {
                                    ExpPar = par.sections[0].Footer.ToString();
                                }

                                ErrorMessage += $"Неверно заданы параметры нижнего колонтитула форматирования страниц: Ожидалось:{ExpPar} Получено:{Error.Item2[0]}\n";
                                break;
                            }

                        //case ErrorType.SectionErrorOrientation:
                        //    {
                        //        string ExpPar = par.sections[1].Top.ToString();
                        //        if (Error.Item2[1] == "portrait")
                        //        {
                        //            ExpPar = par.sections[0].Top.ToString();
                        //        }

                        //        ErrorMessage += $"Неверно заданы параметры верхнего отступа форматирования страниц: Ожидалось:{ExpPar} Получено:{Error.Item2[0]}\n";
                        //        break;
                        //    }

                        case ErrorType.SectionErrorPageWidth:
                            {
                                string ExpPar = par.sections[1].PageWidth.ToString();
                                if (Error.Item2[1] == "portrait")
                                {
                                    ExpPar = par.sections[0].PageWidth.ToString();
                                }

                                ErrorMessage += $"Неверно заданы параметры ширины форматирования страниц: Ожидалось:{ExpPar} Получено:{Error.Item2[0]}\n";
                                break;
                            }
                        case ErrorType.SectionErrorPageHeight:
                            {
                                string ExpPar = par.sections[1].PageHeight.ToString();
                                if (Error.Item2[1] == "portrait")
                                {
                                    ExpPar = par.sections[0].PageHeight.ToString();
                                }

                                ErrorMessage += $"Неверно заданы параметры высоты форматирования страниц: Ожидалось:{ExpPar} Получено:{Error.Item2[0]}\n";
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
                return ErrorMessage;
            }
            public string ConvertToString(CheckParametrs par)
            {
                string Res = "";
                if (type == ExpectionType.MainText || type == ExpectionType.MainTextLabel || type == ExpectionType.MainTextHeader)
                {
                    Res = $"В параграфе {Position[0]} ";
                    Res += $"(Распознан как {TypeToString()})\n";
                }
                else
                {
                    if (type == ExpectionType.TableHeader || type == ExpectionType.TableText)
                    {
                        Res = $"В параграфе {Position[3]} в {Position[2]} ячейке {Position[1]} строки  в таблице {Position[0]} ";
                        Res += $"(Распознан как {TypeToString()})\n";
                    }
                    else {
                        if (type == ExpectionType.SectionError)
                        {
                            Res = $"В разделе {Position[0]} обнаружены ошибки макета страниц: ";
                        }
                        else
                        {
                            return "";
                        }
                    }
                }

                if (type == ExpectionType.MainText || type == ExpectionType.MainTextLabel || type == ExpectionType.MainTextHeader)
                {
                    Res += Position[1];
                }

                Res += $"Найденные ошибки: \n{ConvertError(par)}\n";

                return Res;
            }
            public int ErrorsCount()
            {
                return ErrorList.Count;
            }
        }
        //Класс для хранения информации о форматировании разделов
        public class SectionInfo
        {
            public int SectionIndex { get; set; }
            public double Top { get; set; }
            public double Bottom { get; set; }
            public double Left { get; set; }
            public double Right { get; set; }
            public double Header { get; set; }
            public double Footer { get; set; }
            public string Orientation { get; set; }     // "portrait" или "landscape"
            public double PageWidth { get; set; }
            public double PageHeight { get; set; }

            public SectionInfo() { }


            public string FileExport()
            {
                StringBuilder exportString = new StringBuilder();

                exportString.AppendLine("[");
                exportString.AppendLine($"{Orientation}");
                exportString.AppendLine($"{Top}");
                exportString.AppendLine($"{Bottom}");
                exportString.AppendLine($"{Left}");
                exportString.AppendLine($"{Right}");
                exportString.AppendLine($"{Header}");
                exportString.AppendLine($"{Footer}");
                exportString.AppendLine($"{PageWidth}");
                exportString.AppendLine($"{PageHeight}");
                exportString.AppendLine("]");

                return exportString.ToString();
            }

            public bool ConvertString(List<string> stringParams)
            {
                try
                {
                    this.Orientation = stringParams[0];

                    this.Top = Double.Parse(stringParams[1]);
                    this.Bottom = Double.Parse(stringParams[2]);
                    this.Left = Double.Parse(stringParams[3]);
                    this.Right = Double.Parse(stringParams[4]);
                    this.Header = Double.Parse(stringParams[5]);
                    this.Footer = Double.Parse(stringParams[6]);
                    this.PageWidth = Double.Parse(stringParams[7]);
                    this.PageHeight = Double.Parse(stringParams[8]);
                }
                catch (Exception e)
                {
                    return false;
                }
                return true;
            }
            public SectionInfo ConvertToTp()
            {
                SectionInfo output = new SectionInfo();
                output.Top = CheckerFuncs.ConvertValue("twips", "cm", this.Top);
                output.Bottom = CheckerFuncs.ConvertValue("twips", "cm", this.Bottom);
                output.Left = CheckerFuncs.ConvertValue("twips", "cm", this.Left);
                output.Right = CheckerFuncs.ConvertValue("twips", "cm", this.Right);
                output.Header = CheckerFuncs.ConvertValue("twips", "cm", this.Header);
                output.Footer = CheckerFuncs.ConvertValue("twips", "cm", this.Footer);
                output.Orientation = this.Orientation;
                output.PageWidth = this.PageWidth;
                output.PageHeight = this.PageHeight;

                return output;
        }
        }
    }
    public class CheckerFuncs
    {
        static Jornal jornal = new Jornal();

        // Метод для конвертации типов
        public static double ConvertValue(string OutputType, string InputType, double InputValue)
        {
            double InputMod = 1;
            double OutputMod = 1;
            // Конвертация значения в значение строк
            switch (InputType)
            {
                case "twips":
                    {
                        break;
                    }
                case "pt":
                    {
                        InputMod = 20;
                        break;
                    }
                case "in":
                    {
                        InputMod = 1440;
                        break;
                    }
                case "cm":
                    {
                        InputMod = 1440 / 2.54;
                        break;
                    }
                case "mm":
                    {
                        InputMod = 1440 / 25.4;
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
            switch (OutputType)
            {
                case "twips":
                    {
                        break;
                    }
                case "pt":
                    {
                        OutputMod = 20;
                        break;
                    }
                case "in":
                    {
                        OutputMod = 1440;
                        break;
                    }
                case "cm":
                    {
                        OutputMod = 1440 / 2.54;
                        break;
                    }
                case "mm":
                    {
                        OutputMod = 1440 / 25.4;
                        break;
                    }
                default:
                    {
                        break;
                    }

            }

            if (OutputType == "twips")
            {
                return Math.Round(InputValue / OutputMod * InputMod, 0);
            }
            return Math.Round(InputValue / OutputMod * InputMod, 2);
        }
        public static string ConvertLineRule(LineSpacingRuleValues rule)
        {
            if (rule == LineSpacingRuleValues.Auto)
            {
                return "auto";
            }
            if (rule == LineSpacingRuleValues.AtLeast)
            {
                return "atLeast";
            }
            if (rule == LineSpacingRuleValues.Exact)
            {
                return "exactly";
            }
            return "unknow";
        }
        private static Expection ExpectionTake(List<Expection> expList, ExpectionType type)
        {
            int index = -1;

            for (int i = 0; i < expList.Count; i++)
            {
                if (expList[i].Type == type)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                return expList[index];
            }
            else
            {
                foreach (Expection exp in expList)
                {
                    if (exp.Type == ExpectionType.MainText)
                    {
                        return exp;
                    }
                }
            }

            jornal.AddRecord("Ошибка поиска настроек оформления: не обнаружено оформление основного текста", "Error", "ExpectionTake");
            throw new Exception("Не обнаружен стиль оформления для основного текста");
        }

        private static bool CreateAndWriteFile(string Result, string CheckedDoc)
        {
            string Path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "checkResults");
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            bool created = false;
            int counter = 0;
            string file;
            string date = DateTime.Now.Date.ToString("d");
            while (!created)
            {
                if (counter == 0)
                {
                    file = System.IO.Path.Combine(Path, (date + "_Check_" + CheckedDoc + ".txt")).ToString();
                }
                else
                {
                    file = System.IO.Path.Combine(Path, (date + "_Check_" + CheckedDoc + $"({counter}).txt")).ToString();
                }

                if (!File.Exists(file))
                {
                    File.WriteAllText(file, Result);
                    created = true;
                }


                counter++;
            }

            return true;
        }
        // Метод проверки документа
        public static List<ErrorRecord> CheckDocument(string path, CheckParametrs exp, bool MakeJornal)
        {
            jornal.CreateRecordSession();
            jornal.AddRecord("Начало проверки", "Normal", "ReadWordDocument");
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, false))
                {
                    jornal.AddRecord("Документ успешно открыт", "Normal", "ReadWordDocument");
                    Body body = wordDoc.MainDocumentPart.Document.Body;

                    List<ErrorRecord> result = CheckAllElements(body, exp, GetStyleList(wordDoc), 200, wordDoc.MainDocumentPart.NumberingDefinitionsPart.Numbering);

                    StringBuilder OutPut = new StringBuilder("");
                    foreach (var res in result)
                    {
                        OutPut.AppendLine(res.ConvertToString(exp));
                    }
                    CreateAndWriteFile(OutPut.ToString(), Path.GetFileNameWithoutExtension(path));

                    jornal.RecordsWrite();
                    return result;
                }
            }

            catch (Exception e)
            {
                jornal.AddRecord($"Не удалось открыть файл :{e}", "Fatal", "ReadWordDocument");
                jornal.RecordsWrite();
                return null;
            }
        }
        // Метод проверки элементов
        public static List<ErrorRecord> CheckAllElements(Body body, CheckParametrs exp, List<Style> styles, int parSymbols, Numbering numbering)
        {
            ErrorRecord result = new ErrorRecord();
            List<ErrorRecord> FinalResults = new List<ErrorRecord>();
            int Paragraphcounter = 1;
            int TableCounter = 1;
            foreach (var element in body.Elements())
            {
                if (element is Paragraph)
                {
                    Paragraph paragraph = (Paragraph)element;
                    if (!String.IsNullOrWhiteSpace(paragraph.InnerText.ToString()))
                    {
                        try
                        {
                            result = MainParagraphCheck(paragraph, styles, exp.exp, numbering, Paragraphcounter);

                            if (paragraph.InnerText.ToString().Length <= parSymbols)
                            {
                                result.Position[1] = $"Текст параграфа: {paragraph.InnerText.ToString()}";
                            }
                            else
                            {
                                result.Position[1] = $"Первые {parSymbols} символов параграфа: {paragraph.InnerText.ToString().Substring(0, parSymbols)}";
                            }


                            if (result.ErrorList.Count != 0)
                            {
                                FinalResults.Add(result);
                            }
                        }
                        catch (Exception e)
                        {
                            jornal.AddRecord($"Ошибка проверки параграфа {Paragraphcounter}: {e.Message.ToString()}", "Error", "MainParagraphCheck");
                        }
                        Paragraphcounter++;
                    }
                }

                if (element is Table)
                {
                    Table table = (Table)element;

                    try
                    {
                        List<ErrorRecord> TableRes = CheckTable(table, exp.exp, styles, numbering, TableCounter);

                        if (TableRes.Count != 0)
                        {
                            foreach (var res in TableRes)
                            {
                                FinalResults.Add(res);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        jornal.AddRecord($"Ошибка проверки таблицы {TableCounter}: {e.Message.ToString()}", "Error", "CheckTable");
                    }
                    TableCounter++;
                }
            }

            try
            {
                FinalResults = FinalResults.Union(CheckSections(GetSectionSize(body), exp.sections)).ToList();
            }
            catch(Exception e)
            {
                jornal.AddRecord($"Ошибка проверки форматирования разделов: {e.ToString()}", "Error", "CheckSections");
            }
            return FinalResults;
        }


        // Метод проверки таблиц
        private static List<ErrorRecord> CheckTable(Table table, List<Expection> expList, List<Style> styles, Numbering numbering, int tableNum)
        {
            // Получаем размеры заголовка таблицы
            int HeaderSize = TableHeaderSizeCalc(table);
            int rowCounter = 0;
            int cellCounter = 0;
            StringBuilder OutPutMessage = new StringBuilder("");
            List<ErrorRecord> records = new List<ErrorRecord>();
            List<(ErrorType, List<string>)> Cellrecords = new List<(ErrorType, List<string>)>();
            //  Перебираем все строки
            foreach (TableRow row in table.Elements<TableRow>())
            {
                rowCounter++;
                cellCounter = 0;
                //  Перебираем все ячейки в строке
                foreach (TableCell cell in row.Elements<TableCell>())
                {
                    cellCounter++;
                    int parCounter = 1;
                    //  Перебираем все параграфы в ячейке
                    foreach (Paragraph para in cell.Elements<Paragraph>())
                    {
                        if (!String.IsNullOrWhiteSpace(para.InnerText.ToString()))
                        {

                            ErrorRecord record = new ErrorRecord();
                            Expection exp = new Expection();
                            try
                            {
                                if (rowCounter <= HeaderSize)
                                {
                                    exp = ExpectionTake(expList, ExpectionType.TableHeader);
                                    record.type = ExpectionType.TableHeader;
                                }
                                else
                                {
                                    exp = ExpectionTake(expList, ExpectionType.TableText);
                                    record.type = ExpectionType.TableText;
                                }
                            }
                            catch (Exception error)
                            {
                                throw error;
                            }

                            Cellrecords = CheckParagraph(para, styles, exp, numbering);

                            if (Cellrecords.Count != 0)
                            {
                                //for (int i = 0; i<FoundetErrors.Count; i++)
                                //{
                                //    for (int j=0; j<record.ErrorList.Count; j++)
                                //    {
                                //        if (FoundetErrors[i].Item1 == record.ErrorList[j].Item1)
                                //        {
                                //            record.ErrorList[j].Item2 = record.ErrorList[j].Item2.Union(FoundetErrors[i].Item2).ToList();
                                //            foudet = true;
                                //            break;
                                //        }
                                //    }
                                //}
                                record.ErrorList = Cellrecords;
                                record.Position[0] = tableNum.ToString();
                                record.Position[1] = rowCounter.ToString();
                                record.Position[2] = cellCounter.ToString();
                                record.Position[3] = parCounter.ToString();

                                records.Add(record);
                            }

                            parCounter++;
                        }


                    }

                    //if (record.ErrorList.Count != 0)
                    //{
                    //    records.Add(record);
                    //}
                }
            }

            return records;
        }
        // Метод, подсчитывающий размер заголовка таблицы. Возвращает -1 если была ошибка подсчёта.
        private static int TableHeaderSizeCalc(Table table)
        {
            int HeaderSize = 1;
            int rowCellCounter;
            List<int> VerticalMergedCollum = new List<int>();

            foreach (TableRow row in table.Elements<TableRow>())
            {
                rowCellCounter = -1;

                //  Перебираем все ячейки в строке
                foreach (TableCell cell in row.Elements<TableCell>())
                {
                    rowCellCounter++;

                    if (cell.TableCellProperties != null)
                    {
                        // Добавляем в список индексы столбцов, где есть вертикальное слияние
                        if (cell.TableCellProperties.VerticalMerge != null)
                        {
                            if (cell.TableCellProperties.VerticalMerge.Val.Value.ToString() == "restart")
                            {
                                VerticalMergedCollum.Add(rowCellCounter);
                            }
                        }


                        // Если столбце ячейки есть в списке столбцов, где не закончено слияние, проверяем продолжено ли слияние
                        if (VerticalMergedCollum.Contains(rowCellCounter))
                        {
                            int ListIndex = VerticalMergedCollum.IndexOf(rowCellCounter);

                            // Если слияние не продолжено, то удаляем из списка
                            if (cell.TableCellProperties.VerticalMerge == null)
                            {
                                VerticalMergedCollum.RemoveAt(rowCellCounter);
                            }
                        }


                    }
                }

                // Проверяем пуст ли список
                if (VerticalMergedCollum.Count == 0)
                {
                    return HeaderSize;
                }
                else
                {
                    HeaderSize++;
                }
            }

            return -1;
        }

        // Чтение списка стилей
        static private List<Style> GetStyleList(WordprocessingDocument doc)
        {
            StyleDefinitionsPart stylePart = doc.MainDocumentPart?.StyleDefinitionsPart;

            if (stylePart?.Styles != null)
            {

                List<Style> StyleList = new List<Style>();

                foreach (Style style in stylePart.Styles.Elements<Style>())
                {
                    StyleList.Add(style);
                }
                jornal.AddRecord("Таблица стилей успешно получена", "Normal", "ReadWordDocument");
                return StyleList;
            }
            else
            {
                jornal.AddRecord("Отсутствует файл styles", "Error", "GetStyleList");
                return null;
            }
        }
        //Проверка является параграф элементом списка
        public static bool CheckListElement(ParagraphProperties parProps)
        {
            if (parProps != null)
            {
                var numPr = parProps.NumberingProperties;
                if (numPr != null)
                {
                    return true;
                }
            }
            return false;
        }

        // Проверка параграфа (основная)
        public static ErrorRecord MainParagraphCheck(Paragraph paragraph, List<Style> styles, List<Expection> expList, Numbering numbering, int parNum)
        {
            Expection exp = new Expection();
            ErrorRecord record = new ErrorRecord();

            try
            {
                if (ParagraphIsHeader(paragraph, styles))
                {
                    exp = ExpectionTake(expList, ExpectionType.MainTextHeader);
                    record.type = ExpectionType.MainTextHeader;
                }
                else
                {
                    exp = ExpectionTake(expList, ExpectionType.MainText);
                    record.type = ExpectionType.MainText;
                }
            }
            catch (Exception error)
            {
                throw error;
            }

            record.ErrorList = CheckParagraph(paragraph, styles, exp, numbering);
            record.Position[0] = parNum.ToString();
            return record;
        }
        // Проверка параграфа (вспомогательный метод без проверки типа)
        public static List<(DocChecker.CheckerClasses.ErrorType, List<string>)> CheckParagraph(Paragraph paragraph, List<Style> styles, Expection exp, Numbering numbering)
        {

            List<(DocChecker.CheckerClasses.ErrorType, List<string>)> FinalErrorList = new List<(DocChecker.CheckerClasses.ErrorType, List<string>)>();
            List<(DocChecker.CheckerClasses.ErrorType, List<string>)> ErrorList = new List<(DocChecker.CheckerClasses.ErrorType, List<string>)>();

            ErrorList = CheckLineSpacing(paragraph.ParagraphProperties, exp.paragraphExpections.SpacingBetweenLines, styles);
            if (ErrorList.Count != 0)
            {
                FinalErrorList = FinalErrorList.Union(ErrorList).ToList();
            }

            if (CheckListElement(paragraph.ParagraphProperties))
            {
                ErrorList = CheckListIndentation(paragraph.ParagraphProperties, exp.listExpextions, numbering);
                if (ErrorList.Count != 0)
                {
                    FinalErrorList = FinalErrorList.Union(ErrorList).ToList();
                }
            }
            else
            {
                ErrorList = CheckIndentation(paragraph.ParagraphProperties, exp.paragraphExpections.Indentation, styles);
                if (ErrorList.Count != 0)
                {
                    FinalErrorList = FinalErrorList.Union(ErrorList).ToList();
                }
            }

            ErrorList = CheckJustification(paragraph.ParagraphProperties, exp.paragraphExpections.Justification, styles);

            if (ErrorList.Count != 0)
            {
                FinalErrorList = FinalErrorList.Union(ErrorList).ToList();
            }

            ErrorList = CheckRuns(paragraph, styles, exp.runExpections, exp.allowance);

            if (ErrorList.Count != 0)
            {
                FinalErrorList = FinalErrorList.Union(ErrorList).ToList();
            }

            return FinalErrorList;
        }



        // Проверка параграфов
        public static string CheckAllParagraphs(Body body, Expection exp, List<Style> styles, int parSymbols, Numbering numbering)
        {
            string Result = "";
            StringBuilder OutPut = new StringBuilder("");
            int counter = 1;

            foreach (var paragraph in body.Elements<Paragraph>())
            {
                //Console.WriteLine($"Параграф {counter} : \n{Result}");
                //Console.WriteLine(paragraph.InnerText.ToString());
                //Console.WriteLine("");
                //Console.WriteLine($"{String.IsNullOrWhiteSpace(paragraph.InnerText.ToString())}");
                //Console.WriteLine("");

                if (!String.IsNullOrWhiteSpace(paragraph.InnerText.ToString()))
                {
                    //Result = CheckParagraph(paragraph, styles, exp, numbering);

                    //Console.WriteLine($"Параграф {counter} : \n{Result}");
                    //OutPut.AppendLine($"Параграф {counter} :");
                    if (Result == "")
                    {
                        OutPut.AppendLine($"В параграфе {counter} не обнаружено ошибок\n");
                    }
                    else
                    {
                        OutPut.AppendLine($"В параграфе {counter} обнаружены ошибки:");
                        if (paragraph.InnerText.ToString().Length <= parSymbols)
                        {
                            OutPut.AppendLine($"Текст параграфа: {paragraph.InnerText.ToString()}");
                        }
                        else
                        {
                            OutPut.AppendLine($"Первые {parSymbols} символов параграфа: {paragraph.InnerText.ToString().Substring(0, parSymbols)}");
                        }
                        OutPut.AppendLine("Обнаруженные ошибки:");
                        OutPut.AppendLine(Result);
                    }

                    counter++;
                }
            }
            Console.WriteLine(OutPut.ToString());
            return OutPut.ToString();
        }

        // Получение выравнивания текста
        private static Justification GetEffectiveJustification(ParagraphProperties paragraph, List<Style> styles)
        {

            // Уровень 1: Прямое форматирование
            if (paragraph != null)
            {
                if (paragraph.Justification != null)
                {
                    return paragraph.Justification;
                }
            }

            // Уровень 2: Стиль абзаца
            if (paragraph.ParagraphStyleId != null)
            {
                var style = GetStyleWithInheritance(paragraph.ParagraphStyleId.ToString(), StyleValues.Paragraph, styles);
                if (style != null)
                {
                    if (style.StyleParagraphProperties.Justification != null)
                    {
                        return style.StyleParagraphProperties.Justification;
                    }
                }
            }

            var defaultStyle = GetDefaultParagraphStyle(styles);
            // Уровень 3: Стиль "Normal" (по умолчанию для абзацев)
            if (defaultStyle != null && defaultStyle.StyleParagraphProperties != null)
            {
                if (defaultStyle.StyleParagraphProperties.Justification != null)
                {
                    return defaultStyle.StyleParagraphProperties.Justification;
                }
            }

            // Уровень 4: Значения по умолчанию (если ничего не найдено)
            return new Justification()
            {
                Val = new JustificationValues("left")
            };

        }
        // Проверка выравнивания текста
        private static List<(DocChecker.CheckerClasses.ErrorType, List<string>)> CheckJustification(ParagraphProperties paragrah, Justification expected, List<Style> styles)
        {
            Justification recieved = GetEffectiveJustification(paragrah, styles);

            List<(DocChecker.CheckerClasses.ErrorType, List<string>)> ErrorList = new List<(DocChecker.CheckerClasses.ErrorType, List<string>)>();
            string recString = recieved.Val.ToString();
            string expString = expected.Val.ToString();

            if (recString != expString)
            {
                ErrorList.Add((ErrorType.Justification, new List<string> { recString }));
            }

            return ErrorList;
        }


        // Получение междустрочного интервала абзаца
        private static SpacingBetweenLines GetEffectiveLineSpacing(ParagraphProperties paragraph, List<Style> styles)
        {
            // 1. Проверяем явно заданный интервал в самом параграфе
            var directSpacing = paragraph?.SpacingBetweenLines;

            if (directSpacing != null)
            {
                return directSpacing;
            }

            // 2. Получаем стиль абзаца
            var styleId = paragraph?.ParagraphStyleId?.Val?.Value;
            if (!string.IsNullOrEmpty(styleId))
            {
                var styleSpacing = GetSpacingFromStyle(styles, styleId);

                if (styleSpacing != null)
                {
                    return styleSpacing;
                }
            }

            // 3. Проверяем стиль "Normal" (базовый стиль документа)
            var normalSpacing = GetSpacingFromStyle(styles, "Normal");

            if (normalSpacing != null)
            {
                return normalSpacing;
            }

            return null;
        }
        // Получение интервала из стиля по его идентификатору
        private static SpacingBetweenLines GetSpacingFromStyle(List<Style> styles, string styleId)
        {
            if (styles == null) return null;

            // Ищем стиль по идентификатору
            var style = styles.FirstOrDefault(s => s.StyleId == styleId);

            if (style == null) return null;

            // Возвращаем интервал из текущего стиля
            var styleSpacing = style.StyleParagraphProperties?.SpacingBetweenLines;
            if (styleSpacing != null)
            {
                return styleSpacing;
            }

            // Проверяем, основан ли стиль на другом стиле (атрибут BasedOn)
            if (style.BasedOn != null && !string.IsNullOrEmpty(style.BasedOn.Val))
            {
                var baseSpacing = GetSpacingFromStyle(styles, style.BasedOn.Val);
                if (baseSpacing != null)
                {
                    return baseSpacing;
                }
            }

            return null;
        }
        // Проверка междустрочного интервала абзаца
        public static List<(DocChecker.CheckerClasses.ErrorType, List<string>)> CheckLineSpacing(ParagraphProperties paragraph, SpacingBetweenLines expected, List<Style> styles)
        {
            List<(DocChecker.CheckerClasses.ErrorType, List<string>)> ErrorList = new List<(DocChecker.CheckerClasses.ErrorType, List<string>)>();


            // Читаем междустрочный интервал в иерархии <явно заданый-заданый стилем- заданый настройками документа>
            SpacingBetweenLines spacing = GetEffectiveLineSpacing(paragraph, styles);

            if (spacing != null)
            {
                string exp = expected.Line?.Value?.ToString() ?? "0";
                string rec = spacing.Line?.Value?.ToString() ?? "0";
                if (exp != rec)
                {
                    ErrorList.Add((ErrorType.LineSpacingValue, new List<string> { rec }));
                }

                exp = "auto";
                exp = "auto";
                if (expected.LineRule != null)
                {
                    exp = ConvertLineRule(expected.LineRule.Value);
                }
                if (spacing.LineRule != null)
                {
                    rec = ConvertLineRule(spacing.LineRule.Value);
                }
                if (exp != rec)
                {
                    ErrorList.Add((ErrorType.LineSpacingRule, new List<string> { rec }));
                }

                exp = expected.Before?.Value.ToString() ?? "0";
                rec = spacing.Before?.Value.ToString() ?? "0";
                if (exp != rec)
                {
                    ErrorList.Add((ErrorType.BeforeLineValue, new List<string> { rec }));
                }

                exp = expected.After?.Value.ToString() ?? "0";
                rec = spacing.After?.Value.ToString() ?? "0";
                if (exp != rec)
                {
                    ErrorList.Add((ErrorType.AfterLineValue, new List<string> { rec }));
                }
            }
            // Проверяем неявно заданный междустрочный интервал
            else
            {
                if (expected.LineRule.ToString() != "auto")
                {
                    ErrorList.Add((ErrorType.LineSpacingRule, new List<string> { "auto" }));
                }
                if (expected.Line.ToString() != "240")
                {
                    ErrorList.Add((ErrorType.LineSpacingValue, new List<string> { "240" }));
                }
            }

            return ErrorList;
        }


        //Получение отступа первой строки
        private static Indentation GetEffectiveIndentation(ParagraphProperties paragraph, List<Style> styles)
        {
            // Сначала проверяем явно заданные отступы
            if (paragraph?.Indentation != null)
                return paragraph?.Indentation;

            // Если нет, ищем стиль абзаца
            var styleId = paragraph?.ParagraphStyleId?.Val?.Value;
            if (styleId != null)
            {
                var style = styles.FirstOrDefault(s => s.StyleId == styleId);

                if (style?.StyleParagraphProperties?.Indentation != null)
                    return style.StyleParagraphProperties.Indentation;
            }

            // Если ничего не найдено, возвращаем null (отступы по умолчанию = 0)
            return null;
        }
        //Проверка отступа первой строки
        public static List<(DocChecker.CheckerClasses.ErrorType, List<string>)> CheckIndentation(ParagraphProperties paragraph, Indentation expected, List<Style> styles)
        {

            List<(DocChecker.CheckerClasses.ErrorType, List<string>)> ErrorList = new List<(DocChecker.CheckerClasses.ErrorType, List<string>)>();

            Indentation indentation = GetEffectiveIndentation(paragraph, styles);

            if (indentation != null)
            {
                if (indentation.Left != null || expected.Left != null)
                {
                    string exp = expected.Left?.ToString() ?? "0";
                    string ind = indentation.Left?.ToString() ?? "0";

                    if (exp != ind)
                    {
                        ErrorList.Add((ErrorType.LeftIdent, new List<string> { ind }));
                    }
                }
                if (indentation.Right != null || expected.Right != null)
                {

                    string ind = indentation.Right?.ToString() ?? "0";
                    string exp = expected.Right?.ToString() ?? "0";
                    if (exp != ind)
                    {
                        ErrorList.Add((ErrorType.RightIdent, new List<string> { ind }));
                    }
                }
                if (indentation.FirstLine != null || expected.FirstLine != null)
                {

                    string ind = indentation.FirstLine?.ToString() ?? "0";
                    string exp = expected.FirstLine?.ToString() ?? "0";

                    if (exp != ind)
                    {
                        ErrorList.Add((ErrorType.FirstLine, new List<string> { ind }));
                    }
                }

                if (indentation.Hanging != null || expected.Hanging != null)
                {
                    string ind = indentation.Hanging?.ToString() ?? "0";
                    string exp = expected.Hanging?.ToString() ?? "0";

                    if (exp != ind)
                    {
                        ErrorList.Add((ErrorType.Hanging, new List<string> { ind }));
                    }
                }

            }
            else
            {
                if (expected.Left != null && expected.Left.Value.ToString() != "0")
                {
                    ErrorList.Add((ErrorType.LeftIdent, new List<string> { "0" }));
                }
                if (expected.Right != null && expected.Right.Value.ToString() != "0")
                {
                    ErrorList.Add((ErrorType.RightIdent, new List<string> { "0" }));
                }
                if (expected.FirstLine != null && expected.FirstLine.Value.ToString() != "0")
                {
                    ErrorList.Add((ErrorType.FirstLine, new List<string> { "0" }));
                }
                if (expected.Hanging != null && expected.Hanging.Value.ToString() != "0")
                {
                    ErrorList.Add((ErrorType.Hanging, new List<string> { "0" }));
                }
            }

            return ErrorList;
        }
        //Проверка отступов элементов списка
        public static List<(DocChecker.CheckerClasses.ErrorType, List<string>)> CheckListIndentation(ParagraphProperties paragraph, ListInd expected, Numbering numbering)
        {
            ListInd recieved = GetEffectiveListIndentation(paragraph, numbering);
            List<(DocChecker.CheckerClasses.ErrorType, List<string>)> ErrorList = new List<(DocChecker.CheckerClasses.ErrorType, List<string>)>();
            if (recieved == null)
            {
                return ErrorList;
            }
            try
            {
                if (recieved.Hanging != -1)
                {
                    if (recieved.Hanging != expected.Hanging)
                    {
                        ErrorList.Add((ErrorType.ListNumIdentHanging, new List<string> { recieved.Hanging.ToString() }));
                    }
                }
                else
                {
                    if (recieved.FirstLine != expected.FirstLine)
                    {
                        ErrorList.Add((ErrorType.ListNumIdentFirstLine, new List<string> { recieved.FirstLine.ToString() }));
                    }
                }

                if (recieved.Left != expected.Left)
                {
                    ErrorList.Add((ErrorType.ListTextIdent, new List<string> { recieved.Left.ToString() }));
                }

                int right = int.Parse(paragraph.Indentation?.Right ?? "0");
                if (right != 0)
                {
                    ErrorList.Add((ErrorType.ListRightIdent, new List<string> { right.ToString() }));
                }
            }
            catch (Exception e)
            {
                jornal.AddRecord($"При проверке элемента списка получена ошибка: {e.Message}", "Error", "CheckListIndentation");
            }
            return ErrorList;
        }
        // Получение отступов элемента списка
        static ListInd GetEffectiveListIndentation(ParagraphProperties paragraph, Numbering numbering)
        {
            // Извлекаем numI и ilvl из абзаца
            var numPr = paragraph.NumberingProperties;
            var numId = numPr?.NumberingId?.Val?.Value;
            var ilvl = numPr?.NumberingLevelReference?.Val?.Value;

            if (numId == null || ilvl == null)
                return null;

            // Ищем определение уровня списка
            var numInstance = numbering.Elements<NumberingInstance>()
                .FirstOrDefault(n => n.NumberID == numId);

            Level level = null;

            if (numInstance?.AbstractNumId == null)
            {
                jornal.AddRecord($"Ошибка получения AbstractId списка с numId:{numId}", "Error", "GetEffectiveListIndentation");
            }
            else
            {
                var abstractNum = numbering.Elements<AbstractNum>()
                    .FirstOrDefault(a => a.AbstractNumberId == numInstance.AbstractNumId.Val);

                var baseLevel = abstractNum?.Elements<Level>()
                    .FirstOrDefault(l => l.LevelIndex == ilvl);
                var lvlOverride = numInstance.Elements<LevelOverride>()
                    .FirstOrDefault(o => o.LevelIndex == ilvl);

                level = lvlOverride?.Level ?? baseLevel;
            }

            if (level?.PreviousParagraphProperties == null && paragraph.Indentation == null)
            {
                jornal.AddRecord($"Ошибка получения параметров списка с numId {numId}, данный элемент списка пропущен", "Error", "GetEffectiveListIndentation");
                return null;
            }

            // Собираем отступы из списка и параграфа
            var listInd = level?.PreviousParagraphProperties?.Indentation;
            var paraInd = paragraph.Indentation;

            // Читаем параметры, учитывая приоритет параметров параграфа над параметрами списка
            string left = paraInd?.Left?.Value ?? listInd?.Left?.Value ?? "0";
            string hanging = paraInd?.Hanging?.Value ?? listInd?.Hanging?.Value;
            string firstLine = paraInd?.FirstLine?.Value ?? listInd?.FirstLine?.Value;

            string tab = level?.LevelSuffix?.Val?.Value.ToString() ?? "tab";

            // Разрешаем конфликт Hanging и FirstLine // hanging имеет приоритет
            if (hanging != null)
            {
                return new ListInd(Double.Parse(hanging), -1, Double.Parse(left), tab, 1.25);
            }
            else
            {
                if (firstLine != null)
                {
                    return new ListInd(-1, Double.Parse(firstLine), Double.Parse(left), tab, 1.25);
                }
                else
                {
                    jornal.AddRecord($"Ошибка получения параметров списка с numId {numId}, данный элемент списка пропущен", "Error", "GetEffectiveListIndentation");
                    return null;
                }
            }

        }


        //Конвертация списка ошибок в строку
        private static string ConvertErrorRuns(List<(DocChecker.CheckerClasses.ErrorType, List<string>)> ErrorList, RunProperties expected, Allowance allow)
        {
            string FoundetErrors = "";

            foreach (var Error in ErrorList)
            {
                switch (Error.Item1)
                {
                    case ErrorType.Bold:
                        {
                            FoundetErrors += "Обнаружен полужирный текст\n";
                            break;
                        }
                    case ErrorType.Italic:
                        {
                            FoundetErrors += "Обнаружено выделение текста курсивом\n";
                            break;
                        }
                    case ErrorType.UnderLine:
                        {
                            FoundetErrors += "Обнаружено подчёркивание текста\n";
                            break;
                        }
                    case ErrorType.FontType:
                        {
                            string exp = expected.RunFonts?.Ascii?.ToString() ?? "Не определён";
                            FoundetErrors += "Неверно заданный тип шрифта: \nОжидалось: " +
                                 exp + "\nПолучено: ";
                            for (int i = 0; i < Error.Item2.Count; i++)
                            {
                                FoundetErrors += Error.Item2[i];
                                if (i + 1 < Error.Item2.Count)
                                {
                                    FoundetErrors += ", ";
                                }
                            }
                            FoundetErrors += "\n";
                            break;
                        }
                    case ErrorType.FontSize:
                        {
                            FoundetErrors += "Неверно заданный размер шрифта: \nОжидалось: " +
                                Convert.ToString((int.Parse(expected.FontSize.Val) / 2.0)) + "+-"
                                + Convert.ToString((allow.AccRange / 2.0)) + "\nПолучено: ";
                            for (int i = 0; i < Error.Item2.Count; i++)
                            {
                                FoundetErrors += Error.Item2[i];
                                if (i + 1 < Error.Item2.Count)
                                {
                                    FoundetErrors += ", ";
                                }
                            }
                            FoundetErrors += "\n";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            return FoundetErrors;
        }
        // Проверка всех Run в параграфе
        public static List<(DocChecker.CheckerClasses.ErrorType, List<string>)> CheckRuns(Paragraph paragraph, List<Style> styles, RunProperties expected, Allowance allow)
        {
            //string Errors = "";

            var runs = paragraph.Elements<Run>();
            List<(DocChecker.CheckerClasses.ErrorType, List<string>)> ErrorList = new List<(DocChecker.CheckerClasses.ErrorType, List<string>)>();
            bool foundet;
            bool header = ParagraphIsHeader(paragraph, styles);
            foreach (var run in runs)
            {
                RunProperties recVal = GetEffectiveFontInfo(run, paragraph, styles);


                List<(DocChecker.CheckerClasses.ErrorType, string)> RunErrorList = CheckFonts(recVal, expected, allow, header);

                // Добавление ошибок из проверки Run
                if (RunErrorList.Count != 0)
                {
                    foreach (var RunError in RunErrorList)
                    {
                        foundet = false;
                        // Проверяем был ли тип такой ошибки уже обнаружен до этого
                        foreach (var Error in ErrorList)
                        {

                            if (Error.Item1 == RunError.Item1)
                            {
                                // Если такой тип ошибки был, то проверяем было ли такое неправильное значение
                                // найдено до этого
                                foreach (var ErrorValue in Error.Item2)
                                {
                                    if (ErrorValue == RunError.Item2)
                                    {
                                        foundet = true;
                                        break;
                                    }
                                }
                                // Если такого значения ошибки не обнаружено, то добавляем значение
                                if (!foundet)
                                {
                                    Error.Item2.Add(RunError.Item2);
                                    foundet = true;
                                }
                                break;
                            }
                        }
                        // Если такого типа ошибки не было обнаружено, то добавляем такой тип ошибки
                        if (!foundet)
                        {
                            ErrorList.Add((RunError.Item1, new List<string> { RunError.Item2 }));
                        }
                    }
                }
            }

            // Конвертируем список найденных ошибок в string для вывода
            //if (ErrorList.Count != 0)
            //{
            //    Errors = ConvertErrorRuns(ErrorList, expected, allow);
            //}

            return ErrorList;
        }
        // Проверка типа и размера шрифта
        public static List<(DocChecker.CheckerClasses.ErrorType, string)> CheckFonts(RunProperties recieved, RunProperties expected, Allowance allow, bool header)
        {
            List<(DocChecker.CheckerClasses.ErrorType, string)> ErrorList = new List<(DocChecker.CheckerClasses.ErrorType, string)>();

            //bool result = true;
            //string ErrorMessage = "";

            string expTypeFont = expected.RunFonts?.Ascii?.ToString() ?? "Не определен";
            string recTypeFont = recieved.RunFonts?.Ascii?.ToString() ?? "Не определен";
            int recFontSize, expFontSize;
            expFontSize = int.Parse(expected.FontSize.Val);
            recFontSize = int.Parse(recieved.FontSize.Val);


            if (expFontSize - allow.AccRange > recFontSize || recFontSize > expFontSize + allow.AccRange)
            {
                //result = false;
                //ErrorMessage += "Неверно заданный размер шрифта: \nОжидалось: " + Convert.ToString((expFontSize / 2.0)) + "+-" + Convert.ToString((allow.AccRange / 2.0)) +
                //    " Получено: " + Convert.ToString((recFontSize / 2.0)) + "\n";
                ErrorList.Add((ErrorType.FontSize, Convert.ToString((recFontSize / 2.0))));
            }
            if (recTypeFont != expTypeFont)
            {
                if (allow.ExtraFontType == null || allow.ExtraFontType != recTypeFont)
                {
                    //result = false;
                    //ErrorMessage += "Неверно заданный тип шрифта: \nОжидалось: " + expFontSize + "  Получено: " + recTypeFont + "\n";
                    ErrorList.Add((ErrorType.FontType, recTypeFont));
                }
            }
            if (recieved.Italic != null && !allow.AllowItalic)
            {
                //result = false;
                //ErrorMessage += "Обнаружено выделение текста курсивом\n";
                ErrorList.Add((ErrorType.Italic, ""));
            }
            if (recieved.Bold != null && !allow.BoldHeaders && !header)
            {
                //result = false;
                //ErrorMessage += "Обнаружен полужирный текст\n";
                ErrorList.Add((ErrorType.Bold, ""));
            }
            if (recieved.Underline != null && !allow.AllowUnderLines)
            {
                //result = false;
                //ErrorMessage += "Обнаружено подчёркивание текста\n";
                ErrorList.Add((ErrorType.UnderLine, ""));
            }

            return ErrorList;
        }

        // Получение параметров шрифта для Run

        private static RunProperties GetEffectiveFontInfo(Run run, Paragraph paragraph, List<Style> styles)
        {
            FontInfo fontInfo = new FontInfo();

            // Уровень 1: Прямое форматирование Run (наивысший приоритет)
            if (run.RunProperties != null)
            {
                // Получаем название шрифта
                if (run.RunProperties.RunFonts != null)
                {
                    fontInfo.FontType = run.RunProperties.RunFonts.Ascii?.Value ??
                              run.RunProperties.RunFonts.HighAnsi?.Value ??
                              run.RunProperties.RunFonts.EastAsia?.Value ??
                              run.RunProperties.RunFonts.ComplexScript?.Value;
                }

                // Получаем размер шрифта
                if (run.RunProperties.FontSize != null && run.RunProperties.FontSize.Val != null)
                {
                    int.TryParse(run.RunProperties.FontSize.Val.Value, out fontInfo.FontSize);
                }

                if (run.RunProperties.Italic != null)
                {
                    if (run.RunProperties.Italic.Val != null)
                    {
                        fontInfo.Italic = "0";
                    }
                    fontInfo.Italic = "1";
                }
                if (run.RunProperties.Bold != null)
                {
                    if (run.RunProperties.Bold.Val != null)
                    {
                        fontInfo.Bold = "0";
                    }
                    fontInfo.Bold = "1";
                }
                //if (run.RunProperties.Underline != null)
                //{
                //    if (run.RunProperties.Underline.Val != null)
                //    {
                //        fontInfo.UnderLine = "0";
                //    }
                //    fontInfo.UnderLine = "1";
                //}
                if (run.RunProperties.Underline != null)
                {
                    fontInfo.UnderLine = run.RunProperties.Underline.Val.ToString();
                }
            }

            if (fontInfo.CheckValues())
            {
                return GenerateRunProperties(fontInfo);
            }

            // Уровень 2: Стиль символов, примененный к Run
            if (run.RunProperties?.RunStyle != null)
            {
                FontInfo charStyleFont = GetFontFromStyle(run.RunProperties.RunStyle.Val.ToString(), StyleValues.Character, styles);

                if (fontInfo.FontType == null)
                {
                    fontInfo.FontType = charStyleFont.FontType;
                }
                if (fontInfo.FontSize == -1)
                {
                    fontInfo.FontSize = charStyleFont.FontSize;
                }
                if (fontInfo.Italic == null)
                {
                    fontInfo.Italic = charStyleFont.Italic;
                }
                if (fontInfo.Bold == null)
                {
                    fontInfo.Bold = charStyleFont.Bold;
                }
                if (fontInfo.UnderLine == null)
                {
                    fontInfo.UnderLine = charStyleFont.UnderLine;
                }
            }

            if (fontInfo.CheckValues())
            {
                return GenerateRunProperties(fontInfo);
            }

            // Уровень 3: Стиль абзаца
            if (paragraph.ParagraphProperties?.ParagraphStyleId != null)
            {
                FontInfo paraStyleFont = GetFontFromStyle(paragraph.ParagraphProperties?.ParagraphStyleId.Val.ToString(), StyleValues.Paragraph, styles);

                if (fontInfo.FontType == null)
                {
                    fontInfo.FontType = paraStyleFont.FontType;
                }
                if (fontInfo.FontSize == -1)
                {
                    fontInfo.FontSize = paraStyleFont.FontSize;
                }
                if (fontInfo.Italic == null)
                {
                    fontInfo.Italic = paraStyleFont.Italic;
                }
                if (fontInfo.Bold == null)
                {
                    fontInfo.Bold = paraStyleFont.Bold;
                }
                if (fontInfo.UnderLine == null)
                {
                    fontInfo.UnderLine = paraStyleFont.UnderLine;
                }
            }
            if (fontInfo.CheckValues())
            {
                return GenerateRunProperties(fontInfo);
            }

            var defaultStyle = GetDefaultParagraphStyle(styles);
            // Уровень 4: Стиль "Normal" (по умолчанию для абзацев)
            if (defaultStyle != null)
            {
                FontInfo normalStyle = GetFontFromStyleProperties(defaultStyle.StyleRunProperties);

                if (fontInfo.FontType == null)
                {
                    fontInfo.FontType = normalStyle.FontType;
                }
                if (fontInfo.FontSize == -1)
                {
                    fontInfo.FontSize = normalStyle.FontSize;
                }
                if (fontInfo.Italic == null)
                {
                    fontInfo.Italic = normalStyle.Italic;
                }
                if (fontInfo.Bold == null)
                {
                    fontInfo.Bold = normalStyle.Bold;
                }
                if (fontInfo.UnderLine == null)
                {
                    fontInfo.UnderLine = normalStyle.UnderLine;
                }
            }

            if (fontInfo.CheckValues())
            {
                return GenerateRunProperties(fontInfo);
            }

            // Уровень 5: Значения по умолчанию (если ничего не найдено)
            if (fontInfo.FontType == null)
            {
                fontInfo.FontType = "Calibry";
            }
            if (fontInfo.FontSize == -1)
            {
                fontInfo.FontSize = 22;
            }

            // Уровень 5: Значения по умолчанию (если ничего не найдено)
            return GenerateRunProperties(fontInfo);

        }
        // Генерирует объект RunProperties
        static private DocumentFormat.OpenXml.Wordprocessing.RunProperties GenerateRunProperties(FontInfo info)
        {
            Bold bold = null;
            Italic italic = null;
            Underline underLine = null;
            ItalicComplexScript italicCompl = null;
            BoldComplexScript boldCompl = null;

            if (info.Italic == "1")
            {
                italic = new Italic();
                italicCompl = new ItalicComplexScript();
            }

            if (info.Bold == "1")
            {
                bold = new Bold();
                boldCompl = new BoldComplexScript();
            }

            if (info.UnderLine != null && info.UnderLine != "none")
            {
                underLine = new Underline() { Val = new UnderlineValues(info.UnderLine) };
            }

            return
                new DocumentFormat.OpenXml.Wordprocessing.RunProperties(
                new RunFonts()
                {
                    Ascii = info.FontType,
                    HighAnsi = info.FontType,
                    EastAsia = info.FontType,
                    ComplexScript = info.FontType
                },
                new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = info.FontSize.ToString() },
                new FontSizeComplexScript() { Val = info.FontSize.ToString() },
                underLine,
                bold,
                boldCompl,
                italicCompl,
                italic
            );
        }
        // Получение шрифт из стиля с учетом наследования (BasedOn)
        private static FontInfo GetFontFromStyle(string styleId, StyleValues styleType, List<Style> styles)
        {
            var style = GetStyleWithInheritance(styleId, styleType, styles);
            if (style == null)
                return new FontInfo();

            return GetFontFromStyleProperties(style.StyleRunProperties);
        }
        //Получение параметров шрифта из стиля и его предков
        private static Style GetStyleWithInheritance(string styleId, StyleValues styleType, List<Style> styles)
        {
            if (styles == null || string.IsNullOrEmpty(styleId))
                return null;

            var visited = new HashSet<string>();
            Style currentStyle = GetStyleById(styleId, styleType, styles);

            if (currentStyle == null)
                return null;

            // Создаем "объединенный" стиль, собирая свойства из всей цепочки
            var mergedStyle = new Style();

            while (currentStyle != null && !visited.Contains(currentStyle.StyleId.Value))
            {
                visited.Add(currentStyle.StyleId.Value);

                // Копируем свойства (свойства потомка имеют приоритет)
                if (currentStyle.StyleParagraphProperties != null && mergedStyle.StyleParagraphProperties == null)
                {
                    mergedStyle.StyleParagraphProperties = (StyleParagraphProperties)currentStyle.StyleParagraphProperties.Clone();
                }

                if (currentStyle.StyleRunProperties != null && mergedStyle.StyleRunProperties == null)
                {
                    mergedStyle.StyleRunProperties = (StyleRunProperties)currentStyle.StyleRunProperties.Clone();
                }

                // Поднимаемся к родительскому стилю
                if (currentStyle.BasedOn != null)
                {
                    currentStyle = GetStyleById(currentStyle.BasedOn.Val.Value, styleType, styles);
                }
                else
                {
                    break;
                }
            }

            return mergedStyle;
        }
        // Получение стиля по ID
        private static Style GetStyleById(string styleId, StyleValues styleType, List<Style> styles)
        {
            if (styles == null || string.IsNullOrEmpty(styleId))
                return null;

            Style st = styles.FirstOrDefault(s => s.StyleId?.Value == styleId && (s.Type == null || s.Type.Value == styleType));

            if (st == null)
            {
                jornal.AddRecord($"Стиль {styleId} применён, но не был найден в таблице стилей", "Warning", "GetStyleById");

            }
            return st;
        }
        // Получение шрифта из свойств стиля
        private static FontInfo GetFontFromStyleProperties(StyleRunProperties runProps /*StyleParagraphProperties parProps*/)
        {
            FontInfo fontInfo = new FontInfo();


            // Сначала проверяем RunProperties стиля
            if (runProps != null)
            {
                if (runProps.RunFonts != null)
                {
                    fontInfo.FontType = runProps.RunFonts.Ascii?.Value ??
                               runProps.RunFonts.HighAnsi?.Value ??
                               runProps.RunFonts.EastAsia?.Value ??
                               runProps.RunFonts.ComplexScript?.Value;
                }

                if (runProps.FontSize != null && runProps.FontSize.Val != null)
                {
                    int.TryParse(runProps.FontSize.Val.Value, out fontInfo.FontSize);
                }

                if (runProps.Italic != null)
                {
                    if (runProps.Italic.Val != null)
                    {
                        fontInfo.Italic = "0";
                    }

                    fontInfo.Italic = "1";
                }

                if (runProps.Bold != null)
                {
                    if (runProps.Bold.Val != null)
                    {
                        fontInfo.Bold = "0";
                    }
                    fontInfo.Bold = "1";
                }

                if (runProps.Underline != null)
                {
                    fontInfo.UnderLine = runProps.Underline.Val.ToString();
                }
            }

            //// Если в RunProperties нет, проверяем ParagraphProperties (для совместимости)
            //if (fontName == null && parProps?. != null)
            //{
            //    fontName = parProps.ParagraphMarkRunProperties.RunFonts.Ascii?.Value;
            //}

            //if (fontName != null || fontSize != -1)
            //{
            //    return (fontName, fontSize);
            //}

            return fontInfo;
        }
        // Получение стиля "Normal" (по умолчанию)
        private static Style GetDefaultParagraphStyle(List<Style> styles)
        {
            if (styles == null)
                return null;

            // Ищем стиль с ID "Normal"
            var normalStyle = GetStyleById("Normal", StyleValues.Paragraph, styles);

            // Если нет, ищем любой стиль, помеченный как Default
            if (normalStyle == null)
            {
                normalStyle = styles.FirstOrDefault(s => s.Default != null && s.Default.Value && (s.Type == null || s.Type.Value == StyleValues.Paragraph));
            }

            return normalStyle;
        }

        // Проверка параграфа на заголовок по стилю
        static private bool ParagraphIsHeader(Paragraph par, List<Style> styles)
        {
            var parID = par.ParagraphProperties?.ParagraphStyleId?.Val ?? null;
            if (parID == null)
            {
                return false;
            }

            Style style = GetStyleById(parID.ToString(), StyleValues.Paragraph, styles);
            if (style != null)
            {
                string NameStyle = style.StyleName.Val.ToString();

                if (NameStyle.Contains("heading"))
                {
                    return true;
                }
            }
            return false;
        }

        //Получение полей документа
        private static List<SectionInfo> GetSectionSize(Body body)
        {
            List<SectionProperties> bodySections = body.Elements<SectionProperties>().ToList();
            List<SectionInfo> AllSections = new List<SectionInfo>();
           
            if (!bodySections.Any())
            {
                bodySections.Add(new SectionProperties());
            }

            for (int i = 0; i < bodySections.Count; i++)
            {
                SectionProperties sectPr = bodySections[i];
                PageMargin margin = sectPr.GetFirstChild<PageMargin>();
                PageSize pageSize = sectPr.GetFirstChild<PageSize>();

                var section = new SectionInfo()
                {
                    SectionIndex = i + 1,
                    Top = margin?.Top?.Value ?? 1440,
                    Bottom = margin?.Bottom?.Value ?? 1440,
                    Left = margin?.Left?.Value ?? 1440,
                    Right = margin?.Right?.Value ?? 1440,
                    Header = margin?.Header?.Value ?? 720,
                    Footer = margin?.Footer?.Value ?? 720,

                    // Данные страницы
                    PageWidth = pageSize?.Width?.Value ?? 11906,
                    PageHeight = pageSize?.Height?.Value ?? 16838,
                    Orientation = "portrait"
                };

                // Определяем ориентацию
                if (pageSize?.Orient?.Value != null)
                {
                    section.Orientation = pageSize.Orient.Value == PageOrientationValues.Landscape
                        ? "landscape"
                        : "portrait";
                }
                else if (pageSize?.Width?.Value != null && pageSize?.Height?.Value != null)
                {
                    // Если атрибут Orient отсутствует, определяем по соотношению сторон
                    if (int.TryParse(pageSize.Width.Value.ToString(), out int width) && int.TryParse(pageSize.Height.Value.ToString(), out int height))
                    {
                        section.Orientation = width > height ? "landscape" : "portrait";
                    }
                }

                AllSections.Add(section);
            }
            return AllSections;
        }
        //Проверка полей документа
        private static List<ErrorRecord> CheckSections(List<SectionInfo> AllSections, List<SectionInfo> exp)
        {
            List<ErrorRecord> errors = new List<ErrorRecord>();

            foreach (var section in AllSections)
            {
                ErrorRecord record = new ErrorRecord();
                SectionInfo expected;

                if (section.Orientation == "portrait")
                {
                    expected = exp[0];
                }
                else
                {
                    expected = exp[1];
                }

                record.Position[0] = section.SectionIndex.ToString();
                record.type = ExpectionType.SectionError;

                if (expected.Header != section.Header)
                {
                    record.ErrorList.Add((ErrorType.SectionErrorHeader, new List<string> { section.Header.ToString(), expected.Orientation }));
                }
                if (expected.Footer != section.Footer)
                {
                    record.ErrorList.Add((ErrorType.SectionErrorFooter, new List<string> { section.Footer.ToString(), expected.Orientation }));
                }
                if (expected.Right != section.Right)
                {
                    record.ErrorList.Add((ErrorType.SectionErrorRight, new List<string> { section.Right.ToString(), expected.Orientation }));
                }
                if (expected.Left != section.Left)
                {
                    record.ErrorList.Add((ErrorType.SectionErrorLeft, new List<string> { section.Left.ToString(), expected.Orientation }));
                }
                if (expected.Top != section.Top)
                {
                    record.ErrorList.Add((ErrorType.SectionErrorTop, new List<string> { section.Top.ToString(), expected.Orientation }));
                }
                if (expected.Bottom != section.Bottom)
                {
                    record.ErrorList.Add((ErrorType.SectionErrorBottom, new List<string> { section.Bottom.ToString(), expected.Orientation }));
                }
                if (expected.PageWidth != section.PageWidth)
                {
                    record.ErrorList.Add((ErrorType.SectionErrorPageWidth, new List<string> { section.PageWidth.ToString(), expected.Orientation }));
                }
                if (expected.PageHeight != section.PageHeight)
                {
                    record.ErrorList.Add((ErrorType.SectionErrorPageHeight, new List<string> { section.PageHeight.ToString(), expected.Orientation }));
                }
                
                if (record.ErrorList.Count != 0) 
                {
                    errors.Add(record);
                }
            }

            return errors;
        }
    }
}
  
