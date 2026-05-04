using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DocChecker.CheckerClasses;
using static JornalWriter.JornalClass;

namespace DocChecker
{

    public class CheckerClasses
    {
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
        //Класс для хранения заданных пользователем настроек
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
        public  class FontInfo
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
        public enum FontError
        {
            Italic,
            Bold,
            UnderLine,
            FontType,
            FontSize,
            none
        }
        public enum ExpectionType
        {
            MainText,
            MainTextHeader,
            TableText,
            TableHeader,
            MainTextLabel,
            Unknow
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
        private static string ConvertLineRule(LineSpacingRuleValues rule)
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

            for (int i =0; i< expList.Count; i++)
            {
                if (expList[i].Type == type)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1 )
            {
                return expList[index];
            }
            else
            {
                foreach(Expection exp in expList)
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
        public static string ReadWordDocument(string path, Expection exp, bool MakeJornal)
        {
            jornal.CreateRecordSession();
            jornal.AddRecord("Начало проверки", "Normal", "ReadWordDocument");
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, false))
                {
                    jornal.AddRecord("Документ успешно открыт", "Normal", "ReadWordDocument");
                    Body body = wordDoc.MainDocumentPart.Document.Body;


                    string result = CheckAllParagraphs(body, exp, GetStyleList(wordDoc), 200, wordDoc.MainDocumentPart.NumberingDefinitionsPart.Numbering);
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

        // Метод проверки документа
        public static string CheckDocument(string path, List<Expection> exp, bool MakeJornal)
        {
            jornal.CreateRecordSession();
            jornal.AddRecord("Начало проверки", "Normal", "ReadWordDocument");
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, false))
                {
                    jornal.AddRecord("Документ успешно открыт", "Normal", "ReadWordDocument");
                    Body body = wordDoc.MainDocumentPart.Document.Body;

                    string result = CheckAllElements(body, exp, GetStyleList(wordDoc), 200, wordDoc.MainDocumentPart.NumberingDefinitionsPart.Numbering);
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
        public static string CheckAllElements(Body body, List<Expection> exp, List<Style> styles, int parSymbols, Numbering numbering)
        {
            string Result = "";
            StringBuilder OutPut = new StringBuilder("");
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
                            Result = MainParagraphCheck(paragraph, styles, exp, numbering);

                            if (Result != "")
                            {
                                OutPut.AppendLine($"В параграфе {Paragraphcounter} обнаружены ошибки:");
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
                        }
                        catch(Exception e)
                        {
                            OutPut.AppendLine($"Параграф {Paragraphcounter}: ошибка проверки");
                            jornal.AddRecord($"Ошибка проверки таблицы {Paragraphcounter}: {e.ToString()}", "Error", "MainParagraphCheck");
                        }
                        Paragraphcounter++;
                    }
                }

                if (element is Table)
                {
                    Table table = (Table)element;

                    try
                    {
                        Result = CheckTable(table, exp, styles, numbering, TableCounter);

                        if (Result != "")
                        {
                            OutPut.AppendLine(Result);
                        }
                    }
                    catch(Exception e)
                    {
                        OutPut.AppendLine($"Таблица {TableCounter}: ошибка проверки");
                        jornal.AddRecord($"Ошибка проверки таблицы {TableCounter}: {e.ToString()}", "Error", "CheckTable");
                    }
                    TableCounter++;
                }
            }
            Console.WriteLine(OutPut.ToString());
            return OutPut.ToString();
        }


        // Метод проверки таблиц
        private static string CheckTable(Table table, List<Expection> expList, List<Style> styles, Numbering numbering, int tableNum)
        {
            // Получаем размеры заголовка таблицы
            int HeaderSize = TableHeaderSizeCalc(table);
            int rowCounter = 0;
            int cellCounter = 0;
            string ErrorMessage = "";
            StringBuilder OutPutMessage = new StringBuilder("");
            List<(int row, int cell, string Error)> Errors = new List<(int row, int cell, string Error)>();

            //  Перебираем все строки
            foreach (TableRow row in table.Elements<TableRow>())
            {
                rowCounter++;
                cellCounter = 0;
                //  Перебираем все ячейки в строке
                foreach (TableCell cell in row.Elements<TableCell>())
                {
                    cellCounter++;
                    //  Перебираем все параграфы в ячейке
                    foreach (Paragraph para in cell.Elements<Paragraph>())
                    {
                        Expection exp = new Expection();
                        try
                        {
                            if (rowCounter <= HeaderSize)
                            {
                                exp = ExpectionTake(expList, ExpectionType.TableHeader);
                            }
                            else
                            {
                                exp = ExpectionTake(expList, ExpectionType.TableText);
                            }
                        }
                        catch (Exception error)
                        {
                            throw error;
                        }

                        ErrorMessage = CheckParagraph(para, styles, exp, numbering);

                        if (ErrorMessage != "")
                        {
                            OutPutMessage.AppendLine($"В {cellCounter} ячейке {rowCounter} строки обнаружены ошибки: \n{ErrorMessage}");
                        }
                    }
                }
            }

            if (OutPutMessage.ToString() != "")
            {
                return $"В таблице {tableNum} обнаружены ошибки: \n{OutPutMessage}";
            }
            else
            {
                return "";
            }
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
        public static string MainParagraphCheck(Paragraph paragraph, List<Style> styles, List<Expection> expList, Numbering numbering)
        {
            Expection exp = new Expection();

            try
            {
                if (ParagraphIsHeader(paragraph, styles))
                {
                    exp = ExpectionTake(expList, ExpectionType.MainTextHeader);
                }
                else
                {
                    exp = ExpectionTake(expList, ExpectionType.MainText);
                }
            }
            catch (Exception error)
            {
                throw error;
            }

            return CheckParagraph(paragraph, styles, exp, numbering);
        }
        // Проверка параграфа (вспомогательный метод без проверки типа)
        public static string CheckParagraph(Paragraph paragraph, List<Style> styles, Expection exp, Numbering numbering)
        {
            string ErrorMessage = "";
            string message = "";
            (bool result, string message) FuncRes;

            FuncRes = CheckLineSpacing(paragraph.ParagraphProperties, exp.paragraphExpections.SpacingBetweenLines, styles);
            if (!FuncRes.result)
            {
                ErrorMessage += FuncRes.message + "\n";
            }

            if (CheckListElement(paragraph.ParagraphProperties))
            {
                message = CheckListIndentation(paragraph.ParagraphProperties, exp.listExpextions, numbering);
                if (message != "")
                {
                    ErrorMessage += message + "\n";
                }
            }
            else
            {
                FuncRes = CheckIndentation(paragraph.ParagraphProperties, exp.paragraphExpections.Indentation, styles);
                if (!FuncRes.result)
                {
                    ErrorMessage += FuncRes.message + "\n";
                }
            }

            message = CheckJustification(paragraph.ParagraphProperties, exp.paragraphExpections.Justification, styles);

            if (message != "")
            {
                ErrorMessage += message + "\n";
            }

            message = CheckRuns(paragraph, styles, exp.runExpections, exp.allowance);

            if (message != "")
            {
                ErrorMessage += "Ошибки Runs:\n" + message + "\n";
            }

            return ErrorMessage;
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
                    Result = CheckParagraph(paragraph, styles, exp, numbering);
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



        // Методы проверки абзацев

        // Получение выравнивания текста
        private static Justification GetEffectiveJustification(ParagraphProperties paragraph, List<Style> styles)
        {

            // Уровень 1: Прямое форматирование
            if (paragraph != null)
            {
                if (paragraph.Justification!= null)
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
                    if(style.StyleParagraphProperties.Justification != null)
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
        private static string CheckJustification(ParagraphProperties paragrah, Justification expected, List<Style> styles)
        {
            Justification recieved = GetEffectiveJustification(paragrah, styles);

            string recString = recieved.Val.ToString();
            string expString = expected.Val.ToString();

            if (recString != expString)
            {
                return $"Неверно заданые параметры выравнивнивания: \nПолучено: {recString}  Ожидалось: {expString}";
            }

            return "";
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
        public static (bool, string) CheckLineSpacing(ParagraphProperties paragraph, SpacingBetweenLines expected, List<Style> styles)
        {
            string ErrorMessage = "";
            bool result = true;


            // Читаем междустрочный интервал в иерархии <явно заданый-заданый стилем- заданый настройками документа>
            SpacingBetweenLines spacing = GetEffectiveLineSpacing(paragraph, styles);

            if (spacing != null)
            {
                string exp = expected.Line?.Value?.ToString() ?? "0";
                string rec = spacing.Line?.Value?.ToString() ?? "0";
                if (exp != rec)
                {
                    result = false;
                    ErrorMessage += "Неверное значение междустрочного интервала: \nОжидалось: " + ConvertValue("pt", "twips", Double.Parse(exp)) + " Получено: " + ConvertValue("pt", "twips", Double.Parse(rec)) + "\n";
                }

                //exp = expected.LineRule?.Value.ToString() ?? "auto";
                //rec = spacing.LineRule?.Value.ToString() ?? "auto;
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
                    result = false;
                    ErrorMessage += "Неверное правило междустрочного интервала: \nОжидалось: " + exp + " Получено: " + rec + "\n";
                }

                exp = expected.Before?.Value.ToString() ?? "0";
                rec = spacing.Before?.Value.ToString() ?? "0";
                if (exp != rec)
                {
                    result = false;
                    ErrorMessage += "Неверное задан отступ перед абзацем: \nОжидалось: " + ConvertValue("pt", "twips", Double.Parse(exp)) + " Получено: " + ConvertValue("pt", "twips", Double.Parse(rec)) + "\n";
                }

                exp = expected.After?.Value.ToString() ?? "0";
                rec = spacing.After?.Value.ToString() ?? "0";
                if (exp != rec)
                {
                    result = false;
                    ErrorMessage += "Неверное задан отступ после абзаца: \nОжидалось: " + ConvertValue("pt", "twips", Double.Parse(exp)) + " Получено: " + ConvertValue("pt", "twips", Double.Parse(rec)) + "\n";
                }
            }
            // Проверяем неявно заданный междустрочный интервал
            else
            {

                //Console.WriteLine("Проверяются стандатные параметры");
                if (expected.LineRule.ToString() != "auto" || expected.Line.ToString() != "240")
                {

                    string expL = expected.Line?.ToString() ?? "0";
                    string expR = "auto";
                    if (expected.LineRule != null)
                    {
                        expR = ConvertLineRule(expected.LineRule.Value);
                    }
                    

                    result = false;
                    //Написать нормальный конвертор значений
                    ErrorMessage += "Неверный междустрочный интервал: \nОжидалось: " + expR + " | " + ConvertValue("pt", "twips", Double.Parse(expL)) +
                    "\nПолучено: auto | 12 pt (стандартные значения)";

                }

            }

            return (result, ErrorMessage);
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
        public static (bool, string) CheckIndentation(ParagraphProperties paragraph, Indentation expected, List<Style> styles)
        {
            string ErrorMessage = "";
            bool result = true;

            Indentation indentation = GetEffectiveIndentation(paragraph, styles);
            bool IsListElement = CheckListElement(paragraph);
            if (IsListElement)
            {
                //Console.WriteLine("0---0");
                //Console.WriteLine(indentation.Left?.ToString() ?? null);
                //Console.WriteLine(indentation.Right?.ToString() ?? null);
                //Console.WriteLine(indentation.FirstLine?.ToString() ?? null);
                //Console.WriteLine(indentation.Hanging?.ToString() ?? null);
                //Console.WriteLine("0---0");
            }

            if (indentation != null)
            {
                if (indentation.Left != null || expected.Left != null)
                {
                    string exp = expected.Left?.ToString() ?? "0";
                    string ind = indentation.Left?.ToString() ?? "0";

                    if (exp != ind)
                    {

                        //if (indentation.Left != null)
                        //{
                        //    ind = indentation.Left.ToString();
                        //}
                        //if (expected.Left != null)
                        //{
                        //    exp = expected.Left.ToString();
                        //}
                        result = false;
                        ErrorMessage += "Неверно определён левый отступ текста: \nОжидалось: " + ConvertValue("cm", "twips", Double.Parse(exp)) + " Получено: " + ConvertValue("cm", "twips", Double.Parse(ind)) + "\n";
                    }
                }
                if (indentation.Right != null || expected.Right != null)
                {

                    string ind = indentation.Right?.ToString() ?? "0";
                    string exp = expected.Right?.ToString() ?? "0";
                    if (exp != ind)
                    {
                        //if (indentation.Right != null)
                        //{
                        //    ind = indentation.Right.ToString();
                        //}
                        //if (expected.Right != null)
                        //{
                        //    exp = expected.Right.ToString();
                        //}
                        result = false;
                        ErrorMessage += "Неверно определён правый отступ текста: \nОжидалось: " + ConvertValue("cm", "twips", Double.Parse(exp)) + " Получено: " + ConvertValue("cm", "twips", Double.Parse(ind)) + "\n";
                    }
                }
                if (indentation.FirstLine != null || expected.FirstLine != null)
                {

                    string ind = indentation.FirstLine?.ToString() ?? "0";
                    string exp = expected.FirstLine?.ToString() ?? "0";

                    if (exp != ind)
                    {
                        //if (indentation.FirstLine != null)
                        //{
                        //    ind = indentation.FirstLine.ToString();
                        //}
                        //if (expected.FirstLine != null)
                        //{
                        //    exp = expected.FirstLine.ToString();
                        //}
                        result = false;
                        ErrorMessage += "Неверно определён отступ красной строки текста: \nОжидалось: " + ConvertValue("cm", "twips", Double.Parse(exp)) + " Получено: " + ConvertValue("cm", "twips", Double.Parse(ind)) + "\n";
                    }
                }

                if (indentation.Hanging != null || expected.Hanging != null)
                {
                    string ind = indentation.Hanging?.ToString() ?? "0";
                    string exp = expected.Hanging?.ToString() ?? "0";

                    if (exp != ind)
                    {
                        //if (indentation.Hanging != null)
                        //{
                        //    ind = indentation.Hanging.ToString();
                        //}
                        //if (expected.Hanging != null)
                        //{
                        //    exp = expected.Hanging.ToString();
                        //}
                        result = false;
                        ErrorMessage += "Неверно определён выступ первой строки текста: \nОжидалось: " + ConvertValue("cm", "twips", Double.Parse(exp)) + " Получено: " + ConvertValue("cm", "twips", Double.Parse(ind)) + "\n";
                    }
                }

            }
            else
            {
                if (expected.Left != null && expected.Left.Value.ToString() != "0")
                {
                    ErrorMessage += $"Неверно определён левый отступ текста: \nОжидалось: {ConvertValue("cm", "twips", Double.Parse(expected.Left.Value))} Получено: 0\n";
                    result = false;
                }
                if (expected.Right != null && expected.Right.Value.ToString() != "0")
                {
                    ErrorMessage += $"Неверно определён правый отступ текста: \nОжидалось: {ConvertValue("cm", "twips", Double.Parse(expected.Right.Value))} Получено: 0\n";
                    result = false;
                }
                if (expected.FirstLine != null && expected.FirstLine.Value.ToString() != "0")
                {
                    ErrorMessage += $"Неверно определён отступ красной строки текста: \nОжидалось: {ConvertValue("cm", "twips", Double.Parse(expected.FirstLine.Value))}  Получено: 0\n";
                    result = false;
                }
                if (expected.Hanging != null && expected.Hanging.Value.ToString() != "0")
                {
                    ErrorMessage += $"Неверно определён выступ первой строки текста: \nОжидалось: {ConvertValue("cm", "twips", Double.Parse(expected.Hanging.Value))}  Получено: 0\n";
                    result = false;
                }
            }

            return (result, ErrorMessage);
        }
        //Проверка отступов элементов списка
        public static string CheckListIndentation(ParagraphProperties paragraph, ListInd expected, Numbering numbering)
        {
            ListInd recieved = GetEffectiveListIndentation(paragraph, numbering);
            string result = "";
            if (recieved.Hanging != -1)
            {
                if (recieved.Hanging != expected.Hanging)
                {
                    result += $"Обнаружена ошибка отступа номера: \nОжидалось:{expected.Hanging}  Получено:{recieved.Hanging}\n";
                }
            }
            else
            {
                if (recieved.FirstLine != expected.FirstLine)
                {
                    result += $"Обнаружена ошибка отступа номера: \nОжидалось:{expected.FirstLine}  Получено:{recieved.FirstLine}\n";
                }
            }

            if (recieved.Left != expected.Left)
            {
                result += $"Обнаружена ошибка отступа текста: \nОжидалось:{expected.FirstLine}  Получено:{recieved.FirstLine}\n";
            }

            int right = int.Parse(paragraph.Indentation?.Right ?? "0");
            if (right != 0)
            {
                result += $"Обнаружена правый отступ текста: \nПолучено:{right}\n";
            }

            return result;
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
            if (numInstance?.AbstractNumId == null) return null;

            var abstractNum = numbering.Elements<AbstractNum>()
                .FirstOrDefault(a => a.AbstractNumberId == numInstance.AbstractNumId.Val);

            var baseLevel = abstractNum?.Elements<Level>()
                .FirstOrDefault(l => l.LevelIndex == ilvl);
            var lvlOverride = numInstance.Elements<LevelOverride>()
                .FirstOrDefault(o => o.LevelIndex == ilvl);

            Level level = lvlOverride?.Level ?? baseLevel;

            if (level?.PreviousParagraphProperties == null &&
                paragraph.Indentation == null)
                return null;

            // Собираем отступы из списка и параграфа
            var listInd = level?.PreviousParagraphProperties?.Indentation;
            var paraInd = paragraph.Indentation;

            // Читаем параметры, учитывая приоритет параметров параграфа над параметрами списка
            string left = paraInd?.Left?.Value ?? listInd?.Left?.Value;
            string hanging = paraInd?.Hanging?.Value ?? listInd?.Hanging?.Value;
            string firstLine = paraInd?.FirstLine?.Value ?? listInd?.FirstLine?.Value;

            string tab = level.LevelSuffix?.Val?.Value.ToString() ?? "tab";

            // Разрешаем конфликт Hanging и FirstLine // hanging имеет приоритет
            if (hanging != null)
            {
                return new ListInd(Double.Parse(hanging), -1, Double.Parse(left), tab, 1.25);
            }
            else
            {
                return new ListInd(-1, Double.Parse(firstLine), Double.Parse(left), tab, 1.25);
            }

        }


        //Конвертация списка ошибок в строку
        private static string ConvertErrorRuns(List<(DocChecker.CheckerClasses.FontError, List<string>)> ErrorList, RunProperties expected, Allowance allow)
        {
            string FoundetErrors = "";

            foreach (var Error in ErrorList)
            {
                switch (Error.Item1)
                {
                    case FontError.Bold:
                        {
                            FoundetErrors += "Обнаружен полужирный текст\n";
                            break;
                        }
                    case FontError.Italic:
                        {
                            FoundetErrors += "Обнаружено выделение текста курсивом\n";
                            break;
                        }
                    case FontError.UnderLine:
                        {
                            FoundetErrors += "Обнаружено подчёркивание текста\n";
                            break;
                        }
                    case FontError.FontType:
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
                    case FontError.FontSize:
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
        public static string CheckRuns(Paragraph paragraph, List<Style> styles, RunProperties expected, Allowance allow)
        {
            string Errors = "";

            var runs = paragraph.Elements<Run>();
            List<(DocChecker.CheckerClasses.FontError, List<string>)> ErrorList = new List<(DocChecker.CheckerClasses.FontError, List<string>)>();
            bool foundet;
            bool header = ParagraphIsHeader(paragraph, styles);
            foreach (var run in runs)
            {
                RunProperties recVal = GetEffectiveFontInfo(run, paragraph, styles);


                List<(DocChecker.CheckerClasses.FontError, string)> RunErrorList = CheckFonts(recVal, expected, allow, header);

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
            if (ErrorList.Count != 0)
            {
                Errors = ConvertErrorRuns(ErrorList, expected, allow);
            }

            return Errors;
        }
        // Проверка типа и размера шрифта
        public static List<(DocChecker.CheckerClasses.FontError, string)> CheckFonts(RunProperties recieved, RunProperties expected, Allowance allow, bool header)
        {
            List<(DocChecker.CheckerClasses.FontError, string)> ErrorList = new List<(DocChecker.CheckerClasses.FontError, string)>();

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
                ErrorList.Add((FontError.FontSize, Convert.ToString((recFontSize / 2.0))));
            }
            if (recTypeFont != expTypeFont)
            {
                if (allow.ExtraFontType == null || allow.ExtraFontType != recTypeFont)
                {
                    //result = false;
                    //ErrorMessage += "Неверно заданный тип шрифта: \nОжидалось: " + expFontSize + "  Получено: " + recTypeFont + "\n";
                    ErrorList.Add((FontError.FontType, recTypeFont));
                }
            }
            if (recieved.Italic != null && !allow.AllowItalic)
            {
                //result = false;
                //ErrorMessage += "Обнаружено выделение текста курсивом\n";
                ErrorList.Add((FontError.Italic, ""));
            }
            if (recieved.Bold != null && !allow.BoldHeaders && !header)
            {
                //result = false;
                //ErrorMessage += "Обнаружен полужирный текст\n";
                ErrorList.Add((FontError.Bold, ""));
            }
            if (recieved.Underline != null && !allow.AllowUnderLines)
            {
                //result = false;
                //ErrorMessage += "Обнаружено подчёркивание текста\n";
                ErrorList.Add((FontError.UnderLine, ""));
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
                if (run.RunProperties.Underline != null)
                {
                    if (run.RunProperties.Italic.Val != null)
                    {
                        fontInfo.Italic = "0";
                    }
                    fontInfo.Italic = "1";
                }
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

        // Проверка полей документа
        //    private static bool CheckPageMargin(Body body, Expection exp)
        //    {
        //        var sections = body.Descendants<SectionProperties>();

        //        foreach (var section in sections)
        //        {

        //        }
        //    }
    }
}
