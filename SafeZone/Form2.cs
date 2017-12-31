using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SafeZone
{
    public partial class Form2 : Form
    {
        List<LogModel> list = new List<LogModel>();
        public Form2()
        {
            InitializeComponent();
            var rootAppender = ((Hierarchy)LogManager.GetRepository())
                                         .Root.Appenders.OfType<FileAppender>()
                                         .FirstOrDefault();
            string filename = rootAppender != null ? rootAppender.File : string.Empty;


            Stream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var file = new StreamReader(stream, Encoding.UTF8, true, 128);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                list.Add(new LogModel(line));
            }
            logListView.SetObjects(list);
        }
    }
}
