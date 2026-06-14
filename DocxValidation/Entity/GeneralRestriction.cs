using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocChecker
{
    public class GeneralRestriction
    {
        public bool EmptySpaceAfterTablesAndLabels;
        public bool EmptySpaceAfterHeaders;

        public GeneralRestriction()
        {
            EmptySpaceAfterTablesAndLabels = false;
            EmptySpaceAfterHeaders = false;
        }

        public GeneralRestriction(bool emptyTL, bool emptyH)
        {
            EmptySpaceAfterTablesAndLabels = emptyTL;
            EmptySpaceAfterHeaders = emptyH;
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

        public string FileExport()
        {
            return $"(\n{EmptySpaceAfterTablesAndLabels}\n{EmptySpaceAfterHeaders}\n)";
        }

        public bool ConvertString(List<string> stringParams)
        {
            try
            {
                EmptySpaceAfterTablesAndLabels = StringToBool(stringParams[0]);
                EmptySpaceAfterHeaders = StringToBool(stringParams[1]);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

    }
}
