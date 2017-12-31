using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeZone
{
    class LogModel
    {
        string[] SplittedLines;
        public LogModel(string line)
        {
            SplittedLines = line.Split(' ');
        }
        public string date
        {
            get
            {
                return SplittedLines[0];
            }
        }
        public string time
        {
            get
            {
                return SplittedLines[1].Replace(":"+SplittedLines[1].Split(':')[2], " ");
            }
        }
        public string level
        {
            get
            {
                return SplittedLines[3];
            }
        }
        public string logger
        {
            get
            {
                return SplittedLines[4];
            }
        }
        public string message
        {
            get
            {
                string mes="";
                for (int i = 6; i < SplittedLines.Length; i++)
                    mes = mes + " " + SplittedLines[i];
                return mes;
            }
        }
    }
}
