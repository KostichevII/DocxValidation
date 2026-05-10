using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DocChecker.CheckerClasses;

namespace DocChecker
{
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
                else
                {
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
}
