using DocChecker;
using DocumentFormat.OpenXml.Office.Word;
using DocxValidation;
using JornalWriter;
using NuGet.Frameworks;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using static DocChecker.CheckerClasses;

namespace ValidationTesting
{
    [TestClass]
    public class SectionInfoTesting
    {
        [TestMethod]
        public void ConvertToTpTesting()
        {
            SectionInfo test = new SectionInfo();
            test.Bottom = 1;
            test.Top = 1;
            test.Left = 1;
            test.Right = 1;
            test.Header = 1;
            test.Footer = 1;
            test.PageHeight = 200;
            test.PageWidth = 200;
            test.Orientation = "portrait";

            SectionInfo expected = new SectionInfo();
            expected.Bottom = 567;
            expected.Top = 567;
            expected.Left = 567;
            expected.Right = 567;
            expected.Header = 567;
            expected.Footer = 567;
            expected.PageHeight = 200;
            expected.PageWidth = 200;
            expected.Orientation = "portrait";

            SectionInfo recieved = test.ConvertToTp();

            if (expected.Bottom != recieved.Bottom)
            {
                Assert.Fail();
            }
            if (expected.Top != recieved.Top)
            {
                Assert.Fail();
            }
            if (expected.Left != recieved.Left)
            {
                Assert.Fail();
            }
            if (expected.Bottom != recieved.Bottom)
            {
                Assert.Fail();
            }

            if (expected.Header != recieved.Header)
            {
                Assert.Fail();
            }
            if (expected.Footer != recieved.Footer)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ConvertStringTesting()
        {
            List<string> TestStrings = new List<string> { "portrait", "1,25", "1,25", "1,25", "1,25", "1,25", "1,25", "400", "400" };

            SectionInfo test = new SectionInfo();
            test.ConvertString(TestStrings);

            if (test.Bottom != 1.25)
            {
                Assert.Fail();
            }
            if (test.Top != 1.25)
            {
                Assert.Fail();
            }
            if (test.Left != 1.25)
            {
                Assert.Fail();
            }
            if (test.Bottom != 1.25)
            {
                Assert.Fail();
            }
            if (test.Header != 1.25)
            {
                Assert.Fail();
            }
            if (test.Footer != 1.25)
            {
                Assert.Fail();
            }
            if (test.PageWidth != 400)
            {
                Assert.Fail();
            }
            if (test.PageHeight != 400)
            {
                Assert.Fail();
            }
            if (test.Orientation != "portrait")
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void FileExportTesting()
        {
            SectionInfo test = new SectionInfo();
            test.Bottom = 1.5;
            test.Top = 0.5;
            test.Left = 0.125;
            test.Right = 100;
            test.Header = 1;
            test.Footer = 1;
            test.PageHeight = 200;
            test.PageWidth = 500;
            test.Orientation = "portrait";

            string recieved = test.FileExport();
            string expected = "[\r\nportrait\r\n0,5\r\n1,5\r\n0,125\r\n100\r\n1\r\n1\r\n500\r\n200\r\n]\r\n";
            Assert.AreEqual(expected, recieved);
        }
    }
    [TestClass]
    public class FieldSaverTesting
    {

        [TestMethod]
        public void SetTypeTesting_MH()
        {
            FieldSaver saver = new FieldSaver();
            bool confirm = saver.SetType("Заголовки");
            Assert.IsTrue(confirm);
            Assert.AreEqual("MainHeaders", saver.Type);
        }
        [TestMethod]
        public void SetTypeTesting_MT()
        {
            FieldSaver saver = new FieldSaver();
            bool confirm = saver.SetType("Основной текст");
            Assert.IsTrue(confirm);
            Assert.AreEqual("MainText", saver.Type);
        }
        [TestMethod]
        public void SetTypeTesting_L()
        {
            FieldSaver saver = new FieldSaver();
            bool confirm = saver.SetType("Подписи к рисункам/таблицам");
            Assert.IsTrue(confirm);
            Assert.AreEqual("Labels", saver.Type);
        }
        [TestMethod]
        public void SetTypeTesting_TH()
        {
            FieldSaver saver = new FieldSaver();
            bool confirm = saver.SetType("Заголовки таблиц");
            Assert.IsTrue(confirm);
            Assert.AreEqual("TableHeaders", saver.Type);
        }
        [TestMethod]
        public void SetTypeTesting_TT()
        {
            FieldSaver saver = new FieldSaver();
            bool confirm = saver.SetType("Основной текст таблицы");
            Assert.IsTrue(confirm);
            Assert.AreEqual("TableText", saver.Type);
        }
        [TestMethod]
        public void SetTypeTesting_Default()
        {
            FieldSaver saver = new FieldSaver();
            bool confirm = saver.SetType("AA");
            Assert.IsFalse(confirm);
        }

        [TestMethod]
        public void TypeToStringTesting_MH()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainHeaders";
            string result = saver.TypeToString();
            Assert.AreEqual("Заголовки", result);
        }
        [TestMethod]
        public void TypeToStringTesting_MT()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText";
            string result = saver.TypeToString();
            Assert.AreEqual("Основной текст", result);
        }
        [TestMethod]
        public void TypeToStringTesting_L()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "Labels";
            string result = saver.TypeToString();
            Assert.AreEqual("Подписи к рисункам/таблицам", result);
        }
        [TestMethod]
        public void TypeToStringTesting_TH()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "TableHeaders";
            string result = saver.TypeToString();
            Assert.AreEqual("Заголовки таблиц", result);
        }
        [TestMethod]
        public void TypeToStringTesting_TT()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "TableText";
            string result = saver.TypeToString();
            Assert.AreEqual("Основной текст таблицы", result);
        }
        [TestMethod]
        public void TypeToStringTesting_Default()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "";
            string result = saver.TypeToString();
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void TakeTypeTesting_MH()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainHeaders";
            ExpectionType result = saver.TakeType();
            Assert.AreEqual(ExpectionType.MainTextHeader, result);
        }
        [TestMethod]
        public void TakeTypeTesting_MT()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText";
            ExpectionType result = saver.TakeType();
            Assert.AreEqual(ExpectionType.MainText, result);
        }
        [TestMethod]
        public void TakeTypeTesting_L()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "Labels";
            ExpectionType result = saver.TakeType();
            Assert.AreEqual(ExpectionType.MainTextLabel, result);
        }
        [TestMethod]
        public void TakeTypeTesting_TH()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "TableHeaders";
            ExpectionType result = saver.TakeType();
            Assert.AreEqual(ExpectionType.TableHeader, result);
        }
        [TestMethod]
        public void TakeTypeTesting_TT()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "TableText";
            ExpectionType result = saver.TakeType();
            Assert.AreEqual(ExpectionType.TableText, result);
        }
        [TestMethod]
        public void TakeTypeTesting_Default()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "";
            ExpectionType result = saver.TakeType();
            Assert.AreEqual(ExpectionType.Unknow, result);
        }

        [TestMethod]
        public void ConvertStringTesting()
        {
            List<string> strings = new List<string>
            {
                "MainText",
                "1",
                "1",
                "1",
                "FLT",
                "1",
                "1",
                "1",
                "LST",
                "TAL",
                "1",
                "MF",
                "1",
                "AF",
                "True",
                "True",
                "True",
                "True",
                "True",
                "1",
                "1",
                "LSN",
                "1"
            };
            FieldSaver saver = new FieldSaver();
            bool confirm = saver.ConvertString(strings);

            Assert.IsTrue(confirm);
            Assert.AreEqual(saver.Type, "MainText");
            Assert.AreEqual(saver.LM , 1);
            Assert.AreEqual(saver.RM , 1);
            Assert.AreEqual(saver.FLS , 1);
            Assert.AreEqual(saver.FLT , "FLT");
            Assert.AreEqual(saver.SB , 1);
            Assert.AreEqual(saver.SA , 1);
            Assert.AreEqual(saver.LSV , 1);
            Assert.AreEqual(saver.LST , "LST");
            Assert.AreEqual(saver.TAL , "TAL");
            Assert.AreEqual(saver.FS , 1);
            Assert.AreEqual(saver.MF , "MF");
            Assert.AreEqual(saver.FR , 1);
            Assert.AreEqual(saver.AF , "AF");
            Assert.AreEqual(saver.ItalicTerms, true);
            Assert.AreEqual(saver.BoldHead , true);
            Assert.AreEqual(saver.Ital , true);
            Assert.AreEqual(saver.Bold , true);
            Assert.AreEqual(saver.Und , true);
            Assert.AreEqual(saver.LNP , 1);
            Assert.AreEqual(saver.LTP , 1);
            Assert.AreEqual(saver.LSN , "LSN");
            Assert.AreEqual(saver.TS , 1);
        }
        [TestMethod]
        public void ConvertStringTestingError()
        {
            List<string> strings = new List<string>();
            FieldSaver saver = new FieldSaver();
            bool confirm = saver.ConvertString(strings);
            Assert.IsFalse(confirm);
        }

        [TestMethod]
        public void StringToBoolTesting_True()
        {
            FieldSaver saver = new FieldSaver();
            bool confirm = saver.StringToBool("True");
            Assert.IsTrue(confirm);
        }
        [TestMethod]
        public void StringToBoolTesting_False()
        {
            FieldSaver saver = new FieldSaver();
            bool confirm = saver.StringToBool("False");
            Assert.IsFalse(confirm);
        }
        [TestMethod]
        public void StringToBoolTesting_Exception()
        {
            FieldSaver saver = new FieldSaver();
            try
            {
                bool confirm = saver.StringToBool("iwanterror");
            }
            catch(Exception goodException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void FileExportTesting()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText";
            saver.LM = 1;
            saver.RM = 1;
            saver.FLS = 1;
            saver.FLT = "FLT";
            saver.SB = 1;
            saver.SA = 1;
            saver.LSV = 1;
            saver.LST = "LST";
            saver.TAL = "TAL";
            saver.FS = 1;
            saver.MF = "MF";
            saver.FR = 1;
            saver.AF = "AF";
            saver.ItalicTerms = true;
            saver.BoldHead = true;
            saver.Ital = true;
            saver.Bold = true;
            saver.Und = true;
            saver.LNP = 1;
            saver.LTP = 1;
            saver.LSN = "LSN";
            saver.TS = 1;

            string result = saver.FileExport();
            Assert.AreEqual(result, "{\r\nMainText\r\n1\r\n1\r\n1\r\nFLT\r\n1\r\n1\r\n1\r\nLST\r\nTAL\r\n1\r\nMF\r\n1\r\nAF\r\nTrue\r\nTrue\r\nTrue\r\nTrue\r\nTrue\r\n1\r\n1\r\nLSN\r\n1\r\n}\r\n");
        }

        [TestMethod]
        public void ConvertToExpectionTestingMain()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1;  saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный";  saver.TAL = "По левому краю";  saver.FS = 1;  saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF";  saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true;  saver.Bold = true; saver.Und = true;  saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;

            Expection exp = saver.ConvertToExpection();

            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.LineRule, new LineSpacingRuleValues("auto"));
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.Line, "240");
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.Before, "20");
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.After, "20");

            Assert.AreEqual(exp.paragraphExpections.Indentation.Hanging, "0");
            Assert.AreEqual(exp.paragraphExpections.Indentation.FirstLine, "0");
            Assert.AreEqual(exp.paragraphExpections.Indentation.Right, "567");
            Assert.AreEqual(exp.paragraphExpections.Indentation.Left, "567");

            Assert.IsNotNull(exp.runExpections.Italic);
            Assert.IsNotNull(exp.runExpections.Bold);
            Assert.IsNotNull(exp.runExpections.Underline);
            Assert.AreEqual(exp.runExpections.FontSize.Val, "2");
            Assert.AreEqual(exp.runExpections.RunFonts.Ascii, "Calibry");

            Assert.AreEqual(exp.allowance.ExtraFontType, "AF");
            Assert.AreEqual(exp.allowance.AccRange, 2);
            Assert.IsTrue(exp.allowance.AllowItalic);
            Assert.IsTrue(exp.allowance.BoldHeaders);
            Assert.IsFalse(exp.allowance.AllowUnderLines);

            Assert.AreEqual(exp.listExpextions.Hanging, -1);
            Assert.AreEqual(exp.listExpextions.FirstLine, 567);
            Assert.AreEqual(exp.listExpextions.TabValue, -1);
            Assert.AreEqual(exp.listExpextions.Left, 567);
            Assert.AreEqual(exp.listExpextions.SymAfterNum, "tab");

            Assert.AreEqual(exp.Type, ExpectionType.MainText);
            Assert.AreEqual(exp.paragraphExpections.Justification.Val, new JustificationValues("left"));

        }

        [TestMethod]
        public void ConvertToExpectionTesting_Tal1()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.Justification.Val, new JustificationValues("left"));
        }
        [TestMethod]
        public void ConvertToExpectionTesting_Tal2()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По правому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.Justification.Val, new JustificationValues("right"));
        }
        [TestMethod]
        public void ConvertToExpectionTesting_Tal3()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По ширине"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.Justification.Val, new JustificationValues("both"));
        }
        [TestMethod]
        public void ConvertToExpectionTesting_Tal4()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По центру"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.Justification.Val, new JustificationValues("center"));
        }
        [TestMethod]
        public void ConvertToExpectionTesting_Tal5()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = ""; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.IsNull(exp);
        }

        [TestMethod]
        public void ConvertToExpectionTesting_FLT1()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.Indentation.FirstLine, "0");
            Assert.AreEqual(exp.paragraphExpections.Indentation.Hanging, "0");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_FLT2()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отступ"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.Indentation.FirstLine, "567");
            Assert.AreEqual(exp.paragraphExpections.Indentation.Hanging, "0");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_FLT3()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Выступ"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.Indentation.FirstLine, "0");
            Assert.AreEqual(exp.paragraphExpections.Indentation.Hanging, "567");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_FLT4()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = ""; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.IsNull(exp);
        }

        [TestMethod]
        public void ConvertToExpectionTesting_LST1()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.LineRule, new LineSpacingRuleValues("auto"));

            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.Line, "240");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_LST2()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "1,5 строки"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.LineRule, new LineSpacingRuleValues("auto"));
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.Line, "360");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_LST3()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Двойной"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.LineRule, new LineSpacingRuleValues("auto"));
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.Line, "480");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_LST4()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 12;
            saver.LST = "Минимум"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.LineRule, new LineSpacingRuleValues("atLeast"));
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.Line, "240");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_LST5()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 12;
            saver.LST = "Точно"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.LineRule, new LineSpacingRuleValues("exact"));
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.Line, "240");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_LST6()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Множитель"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.LineRule, new LineSpacingRuleValues("auto"));
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.Line, "240");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_LST7()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = ""; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.IsNull(exp);
        }

        [TestMethod]
        public void ConvertToExpectionTesting_LSN1()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.listExpextions.TabValue, -1);
            Assert.AreEqual(exp.listExpextions.SymAfterNum, "tab");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_LSN2()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция с настройкой позиции"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.listExpextions.TabValue, 1);
            Assert.AreEqual(exp.listExpextions.SymAfterNum, "tab");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_LSN3()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Пробел"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.listExpextions.TabValue, -1);
            Assert.AreEqual(exp.listExpextions.SymAfterNum, "space");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_LSN4()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "(нет)"; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.AreEqual(exp.listExpextions.TabValue, -1);
            Assert.AreEqual(exp.listExpextions.SymAfterNum, "nothing");
        }
        [TestMethod]
        public void ConvertToExpectionTesting_LSN5()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = ""; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.IsNull(exp);
        }

        [TestMethod]
        public void ConvertToExpectionTesting_TypeError()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = ""; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = ""; saver.TS = 1;
            Expection exp = saver.ConvertToExpection();
            Assert.IsNull(exp);
        }

    }
    [TestClass]
    public class ErrorRecorTesting
    {
        [TestMethod]
        public void TypeToString_MT()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.MainText;
            Assert.AreEqual("Основной текст", record.TypeToString());
        }
        [TestMethod]
        public void TypeToString_MH()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.MainTextHeader;
            Assert.AreEqual("Заголовок в тексте", record.TypeToString());
        }
        [TestMethod]
        public void TypeToString_ML()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.MainTextLabel;
            Assert.AreEqual("Подпись к рисунку/таблице", record.TypeToString());
        }
        [TestMethod]
        public void TypeToString_TT()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.TableText;
            Assert.AreEqual("Основная часть таблицы", record.TypeToString());
        }
        [TestMethod]
        public void TypeToString_TH()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.TableHeader;
            Assert.AreEqual("Заголовок таблицы", record.TypeToString());
        }
        [TestMethod]
        public void TypeToString_Section()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.SectionError;
            Assert.AreEqual("-----", record.TypeToString());
        }
        [TestMethod]
        public void TypeToString_default()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.Unknow;
            Assert.AreEqual("Не опознано", record.TypeToString());
        }

        [TestMethod]
        public void PositionToStringTesting1()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.MainText;
            record.Position = new List<string> { "1", "1" };
            string result = record.PositionToString();

            Assert.AreEqual(result, "Параграф 1 \n1");
        }
        [TestMethod]
        public void PositionToStringTesting2()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.TableHeader;
            record.Position = new List<string> { "1", "1", "1", "1"};
            string result = record.PositionToString();

            Assert.AreEqual(result, "Параграф 1 в 1 ячейке 1 строки  в таблице 1");
        }
        [TestMethod]
        public void PositionToStringTesting3()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.SectionError;
            record.Position = new List<string> { "1"};
            string result = record.PositionToString();

            Assert.AreEqual(result, "Раздел 1");
        }
        [TestMethod]
        public void PositionToStringTesting4()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.Unknow;
            record.Position = new List<string> { "1" };
            string result = record.PositionToString();

            Assert.AreEqual(result, "");
        }

        [TestMethod]
        public void ExpectionTakeTesting_phase1()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.MainText;

            List<Expection> expList = new List<Expection>
            {
                new Expection()
                {
                    Type = ExpectionType.MainTextLabel
                },
                new Expection()
                {
                    Type = ExpectionType.MainText
                }
            };

            Expection result = record.ExpectionTake(expList);

            Assert.AreEqual(result.Type, ExpectionType.MainText);
        }
        [TestMethod]
        public void ExpectionTakeTesting_phase2()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.TableText;

            List<Expection> expList = new List<Expection>
            {
                new Expection()
                {
                    Type = ExpectionType.MainTextLabel
                },
                new Expection()
                {
                    Type = ExpectionType.MainText
                }
            };

            Expection result = record.ExpectionTake(expList);

            Assert.AreEqual(result.Type, ExpectionType.MainText);
        }
        [TestMethod]
        public void ExpectionTakeTesting_phase3()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.TableText;

            List<Expection> expList = new List<Expection>
            {
                new Expection()
                {
                    Type = ExpectionType.MainTextLabel
                }
            };

            try
            {
                Expection result = record.ExpectionTake(expList);
            }
            catch(Exception GoodException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void ConvertToStringTesting_MT()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.MainText;
            List<Expection> expList = new List<Expection>
            {
                new Expection()
                {
                    Type = ExpectionType.MainTextLabel
                },
                new Expection()
                {
                    Type = ExpectionType.MainText
                }
            };

            record.Position = new List<string> { "1", "1\n" };
            Assert.AreEqual("В параграфе 1 (Распознан как Основной текст)\n1\nНайденные ошибки: \n\n", record.ConvertToString(new CheckParametrs() {exp = expList }));
        }
        [TestMethod]
        public void ConvertToStringTesting_Table()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.TableText;
            List<Expection> expList = new List<Expection>
            {
                new Expection()
                {
                    Type = ExpectionType.MainTextLabel
                },
                new Expection()
                {
                    Type = ExpectionType.MainText
                }
            };
            record.Position = new List<string> { "1", "1", "1", "1" };
            Assert.AreEqual("В параграфе 1 в 1 ячейке 1 строки  в таблице 1 (Распознан как Основная часть таблицы)\nНайденные ошибки: \n\n", record.ConvertToString(new CheckParametrs() { exp = expList }));
        }
        [TestMethod]
        public void ConvertToStringTesting_Section()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.SectionError;
            List<Expection> expList = new List<Expection>
            {
                new Expection()
                {
                    Type = ExpectionType.MainTextLabel
                },
                new Expection()
                {
                    Type = ExpectionType.MainText
                }
            };
            record.Position = new List<string> { "1", "1", "1", "1" };
            Assert.AreEqual("В разделе 1 обнаружены ошибки макета страниц: Найденные ошибки: \n\n", record.ConvertToString(new CheckParametrs() { exp = expList }));
        }
        [TestMethod]
        public void ConvertToStringTesting_Error()
        {
            ErrorRecord record = new ErrorRecord();
            record.type = ExpectionType.Unknow;
            record.Position = new List<string> { "1", "1", "1", "1" };
            Assert.AreEqual("", record.ConvertToString(new CheckParametrs()));
        }
        [TestMethod]
        public void ErrorCountTesting()
        {
            ErrorRecord record = new ErrorRecord();
            Assert.AreEqual(0, record.ErrorsCount());
        }
        [TestMethod]
        public void ConvertErrorTesting()
        {
            CheckParametrs Params = new CheckParametrs();
            Expection exp = new Expection();
            exp.Type = ExpectionType.MainText;
            exp.paragraphExpections.Justification = new Justification()
            {
                Val = new JustificationValues("left")
            };
            exp.paragraphExpections.SpacingBetweenLines = new SpacingBetweenLines()
            {
                Line = "240",
                Before = "0",
                After = "0",
                LineRule = new LineSpacingRuleValues("auto")
            };
            exp.paragraphExpections.Indentation = new Indentation()
            {
                Hanging = "0",
                FirstLine = "0",
                Right = "0",
                Left = "0"
            };
            
            exp.allowance.BoldHeaders = true;
            exp.allowance.AccRange = 4;
            exp.allowance.AllowItalic = true;
            exp.allowance.AllowUnderLines = true;
            exp.allowance.ExtraFontType = "Calibry";

            exp.runExpections = new DocumentFormat.OpenXml.Wordprocessing.RunProperties(
                new RunFonts()
                {
                    Ascii = "Calibry",
                    HighAnsi = "Calibry",
                    EastAsia = "Calibry",
                    ComplexScript = "Calibry"
                },
                new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "12" },
                new FontSizeComplexScript() { Val = "12" },
                new Underline(),
                new Bold(),
                new BoldComplexScript(),
                new ItalicComplexScript(),
                new Italic()
            );

            exp.listExpextions.Left = 240;
            exp.listExpextions.FirstLine = 240;
            exp.listExpextions.Hanging = -1;

            List<Expection> list = new List<Expection>();
            list.Add(exp);

            SectionInfo section = new SectionInfo()
            {
                SectionIndex = 1,
                Top = 1,
                Bottom = 1,
                Left = 1,
                Right = 1,
                Header = 1,
                Footer = 1,
                Orientation = "portrait",    // "portrait" или "landscape"
                PageWidth = 1,
                PageHeight = 1,
            };
            List<SectionInfo> sections = new List<SectionInfo>();
            sections.Add(section);
            section.Orientation = "landscape";
            sections.Add(section);
            List<string> erros = new List<string> { "1", "1" };
            List<(ErrorType, List<string>)> ErrorList = new List<(ErrorType, List<string>)>();
            ErrorList.Add((ErrorType.LineSpacingRule, erros));
            ErrorList.Add((ErrorType.Italic, erros));
            ErrorList.Add((ErrorType.Bold, erros));
            ErrorList.Add((ErrorType.UnderLine, erros));
            ErrorList.Add((ErrorType.FontType, erros));
            ErrorList.Add((ErrorType.FontSize, erros));

            //Ошибка выравнивания
            ErrorList.Add((ErrorType.Justification, erros));

            //Ошибки междустрочного интервала
            ErrorList.Add((ErrorType.LineSpacingValue, erros));
            ErrorList.Add((ErrorType.LineSpacingRule, erros));
            ErrorList.Add((ErrorType.BeforeLineValue, erros));
            ErrorList.Add((ErrorType.AfterLineValue, erros));

            //Ошибки отступов параграфов
            ErrorList.Add((ErrorType.LeftIdent, erros));
            ErrorList.Add((ErrorType.RightIdent, erros));
            ErrorList.Add((ErrorType.FirstLine, erros));
            ErrorList.Add((ErrorType.Hanging, erros));

            //Ошибки списков
            ErrorList.Add((ErrorType.ListTextIdent, erros));
            ErrorList.Add((ErrorType.ListNumIdentHanging, erros));
            ErrorList.Add((ErrorType.ListNumIdentFirstLine, erros));
            ErrorList.Add((ErrorType.ListRightIdent, erros));

            //Ошибки форматироавния страниц
            ErrorList.Add((ErrorType.SectionErrorTop, erros));
            ErrorList.Add((ErrorType.SectionErrorBottom, erros));
            ErrorList.Add((ErrorType.SectionErrorLeft, erros));
            ErrorList.Add((ErrorType.SectionErrorRight, erros));
            ErrorList.Add((ErrorType.SectionErrorHeader, erros));
            ErrorList.Add((ErrorType.SectionErrorFooter, erros));

            ErrorList.Add((ErrorType.SectionErrorOrientation, erros));

            ErrorList.Add((ErrorType.SectionErrorPageWidth, erros));
            ErrorList.Add((ErrorType.SectionErrorPageHeight, erros));

            Params.exp = list;
            Params.sections = sections;

            ErrorRecord records = new ErrorRecord();
            records.type = ExpectionType.MainText;
            records.ErrorList = ErrorList;

            try
            {
                records.ConvertError(Params);
                Assert.IsTrue(true);
            }
            catch (Exception ex) 
            {
                Assert.Fail();
            }

        }
    }
    [TestClass]
    public class RecordTestring
    {
        [TestMethod]
        public void ToStringTesting()
        {
            JornalWriter.JornalClass.Record rec = new JornalClass.Record("Fatal","1","1","1");
            Assert.AreEqual("[1] Fatal 1: 1", rec.ToString());
        }
        [TestMethod]
        public void CheckTypeTesting()
        {
            JornalWriter.JornalClass.Record rec = new JornalClass.Record("Fatal", "1", "1", "1");
            Assert.IsTrue(rec.CheckType("Fatal"));
        }
        [TestMethod]
        public void TypePriorityTesting1()
        {
            JornalWriter.JornalClass.Record rec = new JornalClass.Record();
            rec.type = JornalClass.RecordType.Normal;
            Assert.AreEqual(rec.TypePriority(), 1);
        }
        [TestMethod]
        public void TypePriorityTesting2()
        {
            JornalWriter.JornalClass.Record rec = new JornalClass.Record();
            rec.type = JornalClass.RecordType.Warning;
            Assert.AreEqual(rec.TypePriority(), 2);
        }
        [TestMethod]
        public void TypePriorityTesting3()
        {
            JornalWriter.JornalClass.Record rec = new JornalClass.Record();
            rec.type = JornalClass.RecordType.Error;
            Assert.AreEqual(rec.TypePriority(), 3);
        }
        [TestMethod]
        public void TypePriorityTesting4()
        {
            JornalWriter.JornalClass.Record rec = new JornalClass.Record();
            rec.type = JornalClass.RecordType.Fatal;
            Assert.AreEqual(rec.TypePriority(), 4);
        }
        [TestMethod]
        public void TypePriorityTesting5()
        {
            JornalWriter.JornalClass.Record rec = new JornalClass.Record();
            rec.type = JornalClass.RecordType.Unknow;
            Assert.AreEqual(rec.TypePriority(), 5);
        }
        [TestMethod]
        public void ReturnTimeTesting()
        {
            JornalClass.Record rec = new JornalClass.Record("Fatal", "1", "19:47:26.0942431", "1");
            Assert.AreEqual(rec.ReturnTime().ToString(), "19:47:26.0942431");
        }
        [TestMethod]
        public void GetModuleNameTesting()
        {
            JornalClass.Record rec = new JornalClass.Record("Fatal", "1", "19:47:26.0942431", "1");
            Assert.AreEqual(rec.GetModuleName(), "1");
        }
        [TestMethod]
        public void RecordWithTimeTestring()
        {
            JornalClass.Record rec = new JornalClass.Record("Fatal", "1", DateTime.MinValue.TimeOfDay, "1");
            Assert.IsTrue(true);
        }

    }
    [TestClass]
    public class JornalTesting
    {
        [TestMethod]
        public void ReadRecordTesting()
        {
            string recstr = "[19:47:26.5748260] Warning 1: 1";
            JornalClass.Jornal jornal = new JornalClass.Jornal();
            Assert.AreEqual(JornalClass.RecordType.Warning, jornal.ReadRecord(recstr).type);
        }
        [TestMethod]
        public void ReadRecordTesting2()
        {
            string recstr = "";
            JornalClass.Jornal jornal = new JornalClass.Jornal();
            try
            {
                jornal.ReadRecord(recstr);
            }
            catch (Exception e)
            {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void GetRecordsWithSortingTesting()
        {
            JornalClass.Jornal jornal = new JornalClass.Jornal();
            jornal.records.Add(new JornalClass.Record("Fatal", "1", "1", "1"));
            List<string> types = new List<string>() { "Fatal" };
            Assert.AreEqual(1, jornal.GetRecordsWithSorting(types).Count);
        }
        [TestMethod]
        public void AddRecordTesting() {
            JornalClass.Jornal jornal = new JornalClass.Jornal();
            jornal.AddRecord("Fatal", "1", "1");
            Assert.AreEqual(1, jornal.records.Count);
        }
        [TestMethod]
        public void ReadLogTesting()
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "FileTesting");
            JornalClass.Jornal jornal = new JornalClass.Jornal();
            jornal.ReadLog(Path.Combine(dir, "ReadLogTest.txt"));
            Assert.AreEqual(1, jornal.records.Count);
        }
        [TestMethod]
        public void ReadLogTestingError()
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "FileTesting");
            JornalClass.Jornal jornal = new JornalClass.Jornal();
            try
            {
                jornal.ReadLog(Path.Combine(dir, "ReadLogTestError"));
            }
            catch (Exception good)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void RecordsWriteTesting() 
        {
            JornalClass.Jornal jornal = new JornalClass.Jornal();
            jornal.AddRecord("Fatal", "1", "1");
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "FileTesting");
            jornal.filePath = Path.Combine(dir, "WriteLogTest");
            jornal.RecordsWrite();
            if (File.Exists(Path.Combine(dir, "WriteLogTest")))
            {
                File.Delete(Path.Combine(dir, "WriteLogTest"));
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }

        }
        [TestMethod]
        public void RecordsWriteTestingError()
        {
            JornalClass.Jornal jornal = new JornalClass.Jornal();
            jornal.filePath = "";
            try
            {
                jornal.RecordsWrite();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsTrue(true);
            }

        }

        [TestMethod]
        public void CreateRecordSessionTesting()
        {
            JornalClass.Jornal jornal = new JornalClass.Jornal();
            jornal.CreateRecordSession();
            if (File.Exists(jornal.filePath))
            {
            //    File.Delete(jornal.filePath);
            //    Directory.Delete(Path.Combine(Directory.GetCurrentDirectory(), "logs"));
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }

        }
        [TestMethod]
        public void CreateRecordSessionTesting2()
        {
            JornalClass.Jornal jornal = new JornalClass.Jornal();
            jornal.CreateRecordSession();
            string firstFile = jornal.filePath;
            jornal.CreateRecordSession();
            if (File.Exists(jornal.filePath))
            {
                //File.Delete(jornal.filePath);
                //File.Delete(firstFile);
                //Directory.Delete(Path.Combine(Directory.GetCurrentDirectory(), "logs"));
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }

        }
    }
    [TestClass]
    public class TemplateTesting
    {
        [TestMethod]
        public void ClearTemplateTesting()
        {
            Template temp = new Template();
            temp.ClearTemplate();
            Assert.AreEqual("NewTemplate", temp.Name);
        }
        [TestMethod]
        public void ConvertSectionsToTwipsTesting()
        {
            SectionInfo test = new SectionInfo();
            test.Bottom = 1;
            test.Top = 1;
            test.Left = 1;
            test.Right = 1;
            test.Header = 1;
            test.Footer = 1;
            test.PageHeight = 200;
            test.PageWidth = 200;
            test.Orientation = "portrait";

            List<SectionInfo> list = new List<SectionInfo>() { test };

            SectionInfo expected = new SectionInfo();
            expected.Bottom = 567;
            expected.Top = 567;
            expected.Left = 567;
            expected.Right = 567;
            expected.Header = 567;
            expected.Footer = 567;
            expected.PageHeight = 200;
            expected.PageWidth = 200;
            expected.Orientation = "portrait";

            Template template = new Template();
            template.Sections = list;

            List<SectionInfo> Sections = template.ConvertSectionsToTwips();
            SectionInfo recieved = Sections[0];
            if (expected.Bottom != recieved.Bottom)
            {
                Assert.Fail();
            }
            if (expected.Top != recieved.Top)
            {
                Assert.Fail();
            }
            if (expected.Left != recieved.Left)
            {
                Assert.Fail();
            }
            if (expected.Bottom != recieved.Bottom)
            {
                Assert.Fail();
            }

            if (expected.Header != recieved.Header)
            {
                Assert.Fail();
            }
            if (expected.Footer != recieved.Footer)
            {
                Assert.Fail();
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void ConvertSectionsToTwipsTestingError()
        {
            SectionInfo test = new SectionInfo();
            List<SectionInfo> list = new List<SectionInfo>() { test };
            Template template = new Template();
            template.Sections = list;
            try
            {
                template.ConvertSectionsToTwips();
                Assert.Fail();
            }
            catch(Exception e)
            {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void ConvertToExpectionTesting()
        {
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1; saver.FLS = 1;
            saver.FLT = "Отсутствует"; saver.SB = 1; saver.SA = 1; saver.LSV = 1;
            saver.LST = "Одинарный"; saver.TAL = "По левому краю"; saver.FS = 1; saver.MF = "Calibry";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true; saver.BoldHead = true;
            saver.Ital = true; saver.Bold = true; saver.Und = true; saver.LNP = 2; saver.LTP = 1;
            saver.LSN = "Табуляция"; saver.TS = 1;

            Template temp = new Template();
            temp.Fields = new List<FieldSaver>() { saver };
            
            Expection exp = temp.ConvertToExpection()[0];

            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.LineRule, new LineSpacingRuleValues("auto"));
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.Line, "240");
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.Before, "20");
            Assert.AreEqual(exp.paragraphExpections.SpacingBetweenLines.After, "20");

            Assert.AreEqual(exp.paragraphExpections.Indentation.Hanging, "0");
            Assert.AreEqual(exp.paragraphExpections.Indentation.FirstLine, "0");
            Assert.AreEqual(exp.paragraphExpections.Indentation.Right, "567");
            Assert.AreEqual(exp.paragraphExpections.Indentation.Left, "567");

            Assert.IsNotNull(exp.runExpections.Italic);
            Assert.IsNotNull(exp.runExpections.Bold);
            Assert.IsNotNull(exp.runExpections.Underline);
            Assert.AreEqual(exp.runExpections.FontSize.Val, "2");
            Assert.AreEqual(exp.runExpections.RunFonts.Ascii, "Calibry");

            Assert.AreEqual(exp.allowance.ExtraFontType, "AF");
            Assert.AreEqual(exp.allowance.AccRange, 2);
            Assert.IsTrue(exp.allowance.AllowItalic);
            Assert.IsTrue(exp.allowance.BoldHeaders);
            Assert.IsFalse(exp.allowance.AllowUnderLines);

            Assert.AreEqual(exp.listExpextions.Hanging, -1);
            Assert.AreEqual(exp.listExpextions.FirstLine, 567);
            Assert.AreEqual(exp.listExpextions.TabValue, -1);
            Assert.AreEqual(exp.listExpextions.Left, 567);
            Assert.AreEqual(exp.listExpextions.SymAfterNum, "tab");

            Assert.AreEqual(exp.Type, ExpectionType.MainText);
            Assert.AreEqual(exp.paragraphExpections.Justification.Val, new JustificationValues("left"));
        }
        [TestMethod]
        public void CreateNewTemplateTesting()
        {
            Template temp = new Template();
            temp.CreateNewTemplate();
            if (File.Exists(temp.filepath))
            {
                File.Delete(temp.filepath);
                Directory.Delete(Path.Combine(Directory.GetCurrentDirectory(), "templates"));
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void CreateNewTemplateTesting2()
        {
            Template temp = new Template();
            temp.CreateNewTemplate();
            string first = temp.filepath;
            temp.CreateNewTemplate();
            if (File.Exists(temp.filepath))
            {
                File.Delete(temp.filepath);
                File.Delete(first);
                Directory.Delete(Path.Combine(Directory.GetCurrentDirectory(), "templates"));
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void ReadFileTesting()
        {
            Template temp = new Template();
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "FileTesting", "test.temp");
            Assert.IsTrue(temp.ReadFile(dir));
        }
        [TestMethod]
        public void ReadFileTestingError()
        {
            Template temp = new Template();
            string dir = "";
            Assert.IsFalse(temp.ReadFile(dir));
        }
        [TestMethod]
        public void RenameFilePathTesting()
        {
            Template temp = new Template();
            temp.Name = "rename";
            temp.filepath = Path.Combine(Directory.GetCurrentDirectory(), "templates", "rename.temp");
            temp.RenameFilePath();
            if (File.Exists(temp.filepath))
            {
                File.Move(temp.filepath, Path.Combine(Directory.GetCurrentDirectory(), "templates", "rename.temp"));
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }
            
        }
        [TestMethod]
        public void RenameFilePathTestingError()
        {
            Template temp = new Template();
            temp.Name = null;
            temp.filepath = Path.Combine(Directory.GetCurrentDirectory(), "FileTesting", "rename.temp");
            try
            {
                temp.RenameFilePath();
                Assert.Fail();
            }
            catch(Exception e)
            {
                Assert.IsTrue(true);
            }
            

        }
        [TestMethod]
        public void DeleteFileTesting()
        {
            Template temp = new Template();

            temp.filepath = Path.Combine(Directory.GetCurrentDirectory(), "FileTesting", "1");
            //File.Create(temp.filepath);

            temp.DeleteFile();
            bool res = File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "FileTesting", "1"));
            File.Create(Path.Combine(Directory.GetCurrentDirectory(), "FileTesting", "1"));
            Assert.IsFalse(res);
        }
        [TestMethod]
        public void DeleteFileTestingError()
        {
            Template temp = new Template();

            temp.filepath = Path.Combine(Directory.GetCurrentDirectory(), "FileTesting", "1");

            try
            {
                temp.DeleteFile();
                Assert.Fail();
            }
            catch(Exception e)
            {
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void SaveFileTesting()
        {
            Template temp = new Template();
            temp.filepath = Path.Combine(Directory.GetCurrentDirectory(), "FileTesting", "savetest.temp");
            FieldSaver saver = new FieldSaver();
            saver.Type = "MainText"; saver.LM = 1; saver.RM = 1;
            saver.FLS = 1; saver.FLT = "FLT"; saver.SB = 1;
            saver.SA = 1; saver.LSV = 1; saver.LST = "LST";
            saver.TAL = "TAL"; saver.FS = 1; saver.MF = "MF";
            saver.FR = 1; saver.AF = "AF"; saver.ItalicTerms = true;
            saver.BoldHead = true; saver.Ital = true; saver.Bold = true;
            saver.Und = true; saver.LNP = 1; saver.LTP = 1;
            saver.LSN = "LSN"; saver.TS = 1;
            temp.Fields.Add(saver);

            SectionInfo test = new SectionInfo();
            test.Bottom = 1.5; test.Top = 0.5; test.Left = 0.125;
            test.Right = 100; test.Header = 1; test.Footer = 1;
            test.PageHeight = 200; test.PageWidth = 500; test.Orientation = "portrait";
            temp.Sections.Add(test);

            Assert.IsTrue(temp.SaveFile());
        }
        [TestMethod]
        public void SaveFileTestingError()
        {
            Template temp = new Template();
            try 
            {
                temp.SaveFile();
                Assert.Fail();
            }
            catch(Exception e)
            {
                Assert.IsTrue(true);
            }
        }
    }
    [TestClass]
    public class CheckerClassesTesting
    {
        [TestMethod]
        public void AllowanceTesting1()
        {
            Allowance al = new Allowance();
            Assert.IsNull(al.ExtraFontType);
        }
        [TestMethod]
        public void AllowanceTesting2()
        {
            Allowance al = new Allowance("1", 1 , false, false, false);
            Assert.AreEqual(al.ExtraFontType, "1");
        }
        [TestMethod]
        public void FontInfoTesting()
        {
            FontInfo info = new FontInfo();
            Assert.IsNull(info.FontType);
        }
        [TestMethod]
        public void FontInfoCheckValuesTesting1()
        {
            FontInfo info = new FontInfo();
            Assert.IsFalse(info.CheckValues());
        }
        [TestMethod]
        public void FontInfoCheckValuesTesting2()
        {
            FontInfo info = new FontInfo()
            {
                FontType = "1",
                FontSize = 1,
                Italic = "1",
                Bold = "1",
                UnderLine = "1"
            };
            Assert.IsTrue(info.CheckValues());
        }
        [TestMethod]
        public void ListIndTesting1()
        {
            ListInd listInd = new ListInd();
            Assert.AreEqual(listInd.Hanging, -1);
        }
        [TestMethod]
        public void ListIndTesting2()
        {
            ListInd listInd = new ListInd(1, 1, 1, "tab", 1);
            Assert.AreEqual(listInd.Hanging, 1);
        }
        [TestMethod]
        public void SetParams_1()
        {
            ListInd listInd = new ListInd();
            listInd.SetParams(1, 1, "tab", -1);
            Assert.AreEqual(listInd.Hanging, 0);
        }
        [TestMethod]
        public void SetParams_2()
        {
            ListInd listInd = new ListInd();
            listInd.SetParams(2, 1, "tab", -1);
            Assert.AreEqual(listInd.Hanging, -1);
        }
        [TestMethod]
        public void SetParams_3()
        {
            ListInd listInd = new ListInd();
            listInd.SetParams(1, 2, "tab", -1);
            Assert.AreEqual(listInd.Hanging, 1);
        }

        [TestMethod]
        public void setType1()
        {
            Expection exp = new Expection();
            exp.setType("MainText");
            Assert.AreEqual(ExpectionType.MainText, exp.Type);
        }
        [TestMethod]
        public void setType2()
        {
            Expection exp = new Expection();
            exp.setType("MainTextHeader");
            Assert.AreEqual(ExpectionType.MainTextHeader, exp.Type);
        }
        [TestMethod]
        public void setType3()
        {
            Expection exp = new Expection();
            exp.setType("TableText");
            Assert.AreEqual(ExpectionType.TableText, exp.Type);
        }
        [TestMethod]
        public void setType4()
        {
            Expection exp = new Expection();
            exp.setType("TableHeader");
            Assert.AreEqual(ExpectionType.TableHeader, exp.Type);
        }
        [TestMethod]
        public void setType5()
        {
            Expection exp = new Expection();
            exp.setType("MainTextLabel");
            Assert.AreEqual(ExpectionType.MainTextLabel, exp.Type);
        }
        [TestMethod]
        public void setType6()
        {
            Expection exp = new Expection();
            exp.setType("");
            Assert.AreEqual(ExpectionType.Unknow, exp.Type);
        }
        [TestMethod]
        public void setJustificationTesting()
        {
            Expection exp = new Expection();
            exp.setJustification("left");
            Assert.IsNotNull(exp.paragraphExpections.Justification);
        }
        [TestMethod]
        public void setSpacingTesting()
        {
            Expection exp = new Expection();
            exp.setSpacing("auto", 240, 0, 0);
            Assert.IsNotNull(exp.paragraphExpections.SpacingBetweenLines);
        }
        [TestMethod]
        public void setIdentetionTesting()
        {
            Expection exp = new Expection();
            exp.setIdentetion(0, 0, 0, 0);
            Assert.IsNotNull(exp.paragraphExpections.Indentation);
        }
        [TestMethod]
        public void setListIndentationTesting1()
        {
            Expection exp = new Expection();
            exp.setListIndentation(1, 1, "tab", -1);
            Assert.AreEqual(exp.listExpextions.Hanging, 0);
        }
        [TestMethod]
        public void setListIndentationTesting2()
        {
            Expection exp = new Expection();
            exp.setListIndentation(2, 1, "tab", -1);
            Assert.AreEqual(exp.listExpextions.Hanging, -1);
        }
        [TestMethod]
        public void setListIndentationTesting3()
        {
            Expection exp = new Expection();
            exp.setListIndentation(1, 2, "tab", -1);
            Assert.AreEqual(exp.listExpextions.Hanging, 1);
        }
       
    }
}