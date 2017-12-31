using BrightIdeasSoftware;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SafeZone
{
    class ZipManager
    {
        public const string ARCHIVE_NAME = "safezone";
        public const string ZIP_EXT = ".safe";

        ZipFile zip;
        public List<SafeFile> list = new List<SafeFile>();
        private bool safe = true;
        public ZipFile ZipFileRef
        {
            get
            {
                return zip;
            }
        }
        public bool isSafe
        {
            get
            {
                return safe;
            }
            set
            {
                safe = value;
                if(!value)
                    zip.Save();
                else
                    startSafe();
            }
        }

        public ZipManager()
        {
            startSafe();
            update();
        }
        private void startSafe()
        {
            if (!File.Exists(ARCHIVE_NAME + ZIP_EXT))
            {
                zip = new ZipFile();
                zip.Save(ARCHIVE_NAME + ZIP_EXT);
                log.Info("safe file is not exists. created a new.");
            }
            if (isSafe)
                zip = ZipFile.Read(ARCHIVE_NAME + ZIP_EXT);
            log.Info("safezone is active");
        }
        private void update()
        {
            list.Clear();
            using (ZipFile zip = ZipFile.Read(ARCHIVE_NAME + ZIP_EXT))
            {
                foreach (ZipEntry e in zip)
                {
                    list.Add(new SafeFile(e.FileName, e.CreationTime, e.LastModified));
                }
            }
        }

        public void add(string[] files)
        {
            if(zip == null)
                zip = ZipFile.Read(ARCHIVE_NAME + ZIP_EXT);
            foreach (string file in files)
            {
                SafeFile safeFile = new SafeFile(file);
                if (zip.Any(entry => entry.FileName == safeFile.Title))
                {
                    DialogResult dialogResult = MessageBox.Show(safeFile.Title + " isimli dosya zaten mevcut. Değiştirmek ister misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        zip.RemoveEntry(safeFile.Title);
                        zip.AddFile(file).FileName = safeFile.Title;
                        FileDB.Update(safeFile);
                        log.Info(safeFile.FullPath + " file changed by user");
                    }
                }
                else
                {
                    zip.AddFile(safeFile.FullPath).FileName = safeFile.Title;
                    FileDB.Insert(safeFile);
                    log.Info(safeFile.FullPath + " file added to safezone");
                }
            }
            zip.Save();
            if (isSafe)
                zip = ZipFile.Read(ARCHIVE_NAME + ZIP_EXT);
            update();
        }
        public void delete(ObjectListView sender)
        {
            if (zip == null)
                zip = ZipFile.Read(ARCHIVE_NAME + ZIP_EXT);
            foreach (SafeFile safeItem in sender.SelectedObjects)
            {
                zip.RemoveEntry(safeItem.Title);
                FileDB.Delete(safeItem.Title);
                log.Info(safeItem.FullPath + " file deleted by user");
            }
            zip.Save();
            if (isSafe)
                zip = ZipFile.Read(ARCHIVE_NAME + ZIP_EXT);
            update();
        }
        public void createISO()
        {
            run_cmd("iso_manager.exe", "--iso=True");
        }
        public void integrityConrol()
        {
            List<FileModel> list = FileDB.FileList;
            using (ZipFile zip = ZipFile.Read(ARCHIVE_NAME + ZIP_EXT))
            {
                if (Directory.Exists("integrity"))
                {
                    Directory.Delete("integrity", true);
                }
                foreach (ZipEntry e in zip)
                {
                    e.Extract("integrity");
                    FileModel file = list.Where(x => x.title == e.FileName).First();
                    SafeFile safeFile = new SafeFile("integrity/" + e.FileName);
                    if(safeFile.Hash != file.hash)
                    {
                        log.Error(e.FileName + " is changed by other resources. integrity is corrupted.");
                    }
                }
                Directory.Delete("integrity", true);
            }

        }
        private void run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = cmd;
            start.Arguments = string.Format("{0}", args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    try
                    {
                        log.Info("iso file created.");
                        Process.Start(ARCHIVE_NAME + ".iso");
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                    }
                }
            }
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    }
}
