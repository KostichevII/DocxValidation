using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocChecker
{
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
