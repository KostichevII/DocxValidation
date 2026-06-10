using DocChecker;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornalWriter
{
    public class JornalClass
    {
        public class Record
        {
            public RecordType type;
            public string Message;
            public string ModuleName;
            public string Time;

            public Record()
            {
                type = RecordType.Unknow;
                Message= null;
                Time = null;
                ModuleName= null;
            }
            public Record(string typeS, string message, TimeSpan time, string Module)
            {
                Message = message;
                Time = time.ToString();
                ModuleName = Module;
                switch (typeS)
                {
                    case "Warning":
                        {
                            type = RecordType.Warning; 
                            break;
                        }
                    case "Error":
                        {
                            type = RecordType.Error;
                            break;
                        }
                    case "Normal":
                        {
                            type = RecordType.Normal;
                            break;
                        }
                    case "Fatal":
                        {
                            type = RecordType.Fatal;
                            break;
                        }
                    default:
                        {
                            type= RecordType.Unknow; 
                            break;
                        }
                }
            }
            public Record(string typeS, string message, string time, string Module)
            {
                Message = message;
                Time = time;
                ModuleName = Module;
                switch (typeS)
                {
                    case "Warning":
                        {
                            type = RecordType.Warning;
                            break;
                        }
                    case "Error":
                        {
                            type = RecordType.Error;
                            break;
                        }
                    case "Normal":
                        {
                            type = RecordType.Normal;
                            break;
                        }
                    case "Fatal":
                        {
                            type = RecordType.Fatal;
                            break;
                        }
                    default:
                        {
                            type = RecordType.Unknow;
                            break;
                        }
                }
            }
            public override string ToString()
            {
                return $"[{Time}] {type} {ModuleName}: {Message}";
            } 
            public bool CheckType (string Stype)
            {
                if (type.ToString() == Stype) return true;
                return false;
            }
            public int TypePriority()
            {
                switch (type)
                {
                    case RecordType.Normal: return 1;
                    case RecordType.Warning: return 2;
                    case RecordType.Error: return 3;
                    case RecordType.Fatal: return 4;
                    default: return 5;
                }
            }
            public TimeSpan ReturnTime()
            {
                return TimeSpan.Parse(Time);
            }
            public string GetModuleName()
            {
                return ModuleName;
            }
        }
        public enum RecordType
        {
            Warning,
            Error,
            Normal,
            Fatal,
            Unknow
        }
        public class Jornal
        {
            public List<Record> records;
            private int writeRegularity;
            public string filePath;
            private int writenRecords;

            public Jornal()
            {
                records = new List<Record>();
                writeRegularity = 10;
                writenRecords = 0;
            }
            public bool CreateRecordSession()
            {
                DateTime date = DateTime.Now.Date;

                string path = Path.Combine(Directory.GetCurrentDirectory(), "logs");

                if (!Directory.Exists(path)) 
                { 
                    Directory.CreateDirectory(path);
                }

                bool created = false;
                int counter = 0;
                string FileName = $"{date.ToString("d")} Log";
                string file;
                while (!created)
                {

                    if (counter == 0)
                    {
                        file = Path.Combine(path, (FileName + ".txt")).ToString();
                    }
                    else
                    {
                        file = Path.Combine(path, (FileName + $"({counter}).txt")).ToString();
                    }

                    if (!File.Exists(file))
                    {
                        filePath= file;
                        File.Create(file);
                        created = true;
                    }
                    counter++;
                }
                return true;
            }
            public void AddRecord(string message, string type, string Module)
            {
                records.Add(new Record(type, message, DateTime.Now.TimeOfDay, Module));
            }
            public void RecordsWrite()
            {
                if (!File.Exists(filePath))
                {
                    AddRecord("Файл журнала не найден, создан новый файл", "Error", "RecordsWrite");
                }
                try
                {
                    StringBuilder builder = new StringBuilder("");

                    for (int i = writenRecords; i < records.Count; i++)
                    {
                        builder.AppendLine(records[i].ToString());
                        writenRecords++;
                    }

                    File.WriteAllText(filePath, builder.ToString());
                }
                catch(Exception e)
                {
                    return;
                }
            }
            public void ReadLog(string filePath)
            {
                records.Clear();
                writenRecords= 0;
                try
                {
                    string text = File.ReadAllText(filePath);

                    if (text == "")
                    {
                        return;
                    }

                    string[] textParsed = (text.Replace("\r", "")).Split('\n');

                    foreach (string line in textParsed)
                    {
                        try
                        {
                            records.Add(ReadRecord(line));
                        }
                        catch (Exception e)
                        {
                            if (records.Count != 0)
                            {
                                records[records.Count - 1].Message += line;
                            }
                            else
                            {
                                throw e;
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    throw e;

                }
            }
            public Record ReadRecord(string record)
            {
                try
                {
                    int Pos = record.IndexOf(']');
                    char[] TrimSymbols = { '[', ']' };
                    string time = record.Substring( 0 ,Pos + 1).Trim(TrimSymbols);
                    string nextPart = record.Substring(Pos + 2);
                    Pos = nextPart.IndexOf(":");
                    string[] TypeAndModule = nextPart.Substring(0,Pos).Split(' ');
                    string message = nextPart.Substring(Pos+2).Trim(' ');

                    return new Record(TypeAndModule[0], message, time, TypeAndModule[1]);
                }
                catch(Exception e)
                {
                    throw new Exception($"Ошибка обработки записи: запись не соответствует шаблону");
                }
            }
            public List<Record> GetRecordsWithSorting(List<string> types)
            {
                List<Record> OutputRecords = new List<Record>();

                foreach (Record rec in records)
                {
                    foreach (string type in types)
                    {
                        if (rec.CheckType(type))
                        {
                            OutputRecords.Add(rec);
                        }
                    }
                }

                return OutputRecords;
            }
        }
    }
}
