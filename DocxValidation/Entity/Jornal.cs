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
                    using (var tw = new StreamWriter(filePath, true))
                    {
                        for (int i = writenRecords; i < records.Count; i++)
                        {
                            tw.WriteLine(records[i].ToString());
                            writenRecords++;
                        }
                    }
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
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            records.Add(ReadRecord(line));
                            writenRecords++;
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
                    string[] parts = record.Split(':');
                    if (parts.Length != 4)  
                    {
                        throw new Exception($"Ошибка обработки записи: запись не соответствует шаблону");
                    }

                    string firstPart = $"{parts[0]}:{parts[1]}:{parts[2]}";
                    string[] secondParts = firstPart.Split(' ');

                    // Извлечение сообщения
                    string message = parts[3].Substring(1, parts[3].Length - 1);
                    string time = secondParts[0].Substring(1, secondParts[0].Length - 2);
                    string module = secondParts[2];
                    string type = secondParts[1];
                    return new Record(type, message, time, module);
                }
                catch(Exception e)
                {
                    throw e;
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
