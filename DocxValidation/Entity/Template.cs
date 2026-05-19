using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using DocChecker;

namespace DocxValidation
{
    public class Template
    {
        public List<SectionInfo> Sections;
        public List<FieldSaver> Fields;
        public string Name;
        public DateTime date;
        public string filepath;

        public Template()
        {
            ClearTemplate();
        }
        public bool RenameFilePath()
        {
            try
            {
                string newPath = Path.Combine(Directory.GetCurrentDirectory(), "templates");
                bool created = false;
                int counter = 0;
                string FileName = Name;
                Fields.Clear();
                string file;
                while (!created)
                {
                    if (counter == 0)
                    {
                        file = Path.Combine(newPath, (FileName + ".temp")).ToString();
                    }
                    else
                    {
                        file = Path.Combine(newPath, (FileName + $"({counter}).temp")).ToString();
                    }

                    if (!File.Exists(file))
                    {
                        File.Move(filepath, file);
                        filepath = file;
                        created = true;
                    }
                    counter++;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public void ClearTemplate()
        {
            Fields = new List<FieldSaver>();
            Sections = new List<SectionInfo>();
            Name = "NewTemplate";
            date = DateTime.MinValue;
            filepath = "";
        }
        public bool DeleteFile()
        {
            try
            {
                if (!File.Exists(filepath))
                {
                    throw new Exception("Выбранного файла не существует");
                }

                File.Delete(filepath);
            }
            catch(Exception ex) { throw ex; };
            ClearTemplate();
            return true;


        }
        public bool SaveFile()
        {
            try
            {
                StringBuilder Save = new StringBuilder();
                Save.AppendLine($"Name:{Name}\nDate:{date.Date.ToString("d")}");
                foreach (var FileSaver in Fields)
                {
                    Save.AppendLine(FileSaver.FileExport());
                }

                foreach( var Section in Sections)
                {
                    Save.AppendLine(Section.FileExport());
                }

                using (StreamWriter writer = new StreamWriter(filepath, false))
                {
                    writer.WriteLine(Save);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public bool CreateNewTemplate()
        {
            DateTime date = DateTime.Now.Date;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "templates");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            bool created = false;
            int counter = 0;
            string FileName = $"NewTemplate";
            Fields.Clear();
            string file;
            while (!created)
            {
                if (counter == 0)
                {
                    file = Path.Combine(filePath, (FileName + ".temp")).ToString();
                }
                else
                {
                    file = Path.Combine(filePath, (FileName + $"({counter}).temp")).ToString();
                }

                if (!File.Exists(file))
                {
                    filepath = file;
                    string[] lines = { $"Name:{Name}", $"Date:{date.Date.ToString("d")}"};
                    File.WriteAllLines(filepath, lines);
                    //File.Create(file);
                    created = true;
                }


                counter++;
            }

            //try
            //{
            //    using (StreamWriter writer = new StreamWriter(filepath, false))
            //    {
            //        writer.WriteLine($"Name:{Name}");
            //        writer.WriteLine($"Date:{date.Date}");
            //    }

            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //}
            return true;
        }
        public bool ReadFile(string Path)
        {
            List<string> FileStrings = new List<string>();
            List<SectionInfo> ReadetSections = new List<SectionInfo>() ;
            List<FieldSaver> ReadetFields = new List<FieldSaver>();

            try
            {
                using (StreamReader reader = new StreamReader(Path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        FileStrings.Add(line);
                    }
                }
                string[] TemporyMas = FileStrings[0].Split(':');

                Name = TemporyMas[1];

                TemporyMas = FileStrings[1].Split(':');

                date = DateTime.Parse(TemporyMas[1]);

                filepath = Path;

                List<string> stringParams = new List<string>();
                int ReadMode = 0; // 0 - чтение параметров текста
                                  // 1 - чтение параметров оформления страниц
                for (int i = 2; i< FileStrings.Count; i++) 
                {
                    switch (FileStrings[i])
                    {
                        case "{":
                            {
                                stringParams.Clear();
                                ReadMode = 0;
                                break;
                            }
                        case "[":
                            {
                                stringParams.Clear();
                                ReadMode = 1;
                                break;
                            }
                        default:
                            {
                                switch (FileStrings[i])
                                {
                                    case "}":
                                        {
                                            FieldSaver fields = new FieldSaver();
                                            if (!fields.ConvertString(stringParams))
                                            {
                                                throw new Exception("Ошибка чтения");
                                            }
                                            ReadetFields.Add(fields);
                                            break;
                                        }
                                    case "]":
                                        {
                                            SectionInfo fields = new SectionInfo();
                                            if (!fields.ConvertString(stringParams))
                                            {
                                                throw new Exception("Ошибка чтения");
                                            }
                                            ReadetSections.Add(fields);
                                            break;
                                        }
                                    default:
                                        {
                                            stringParams.Add(FileStrings[i]);
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                    
                }


                Fields = ReadetFields;
                Sections = ReadetSections;
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        public List<DocChecker.CheckerClasses.Expection> ConvertToExpection()
        {
            try
            {
                List<DocChecker.CheckerClasses.Expection> Exp = new List<CheckerClasses.Expection>();
                foreach(var saver in Fields)
                {
                    Exp.Add(saver.ConvertToExpection());
                }
                return Exp;
            }
            catch(Exception e)  { throw e; };
        }
        public List<SectionInfo> ConvertSectionsToTwips()
        {
            try
            {
                List<SectionInfo> Exp = new List<SectionInfo>();
                foreach (var saver in Sections)
                {
                    Exp.Add(saver.ConvertToTp());
                }
                return Exp;
            }
            catch (Exception e) { throw e; };
        }
    }
}
