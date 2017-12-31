using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SafeZone
{
    class FileWatcher
    {
        List<FileModel> list = new List<FileModel>();
        ZipManager zipManager;
        public FileWatcher(ZipManager zipManager)
        {
            this.zipManager = zipManager;
            list = FileDB.FileList;
            addPath(Path.GetFullPath("safezone.safe"));
            foreach(FileModel file in list)
            {
                addPath(file.path);
            }

        }
        private void addPath(string file)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(file) + "\\";
            watcher.NotifyFilter = NotifyFilters.LastAccess |
                                    NotifyFilters.LastWrite |
                                    NotifyFilters.FileName |
                                    NotifyFilters.DirectoryName;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.EnableRaisingEvents = true;
        }
        void OnChanged(object sender, FileSystemEventArgs e)
        {
            log.Warn(e.FullPath + " file changed. type="+e.ChangeType);
            if(e.FullPath == Path.GetFullPath("safezone.safe"))
            {
                zipManager.integrityConrol();
            }
        }
        void OnRenamed(object sender, RenamedEventArgs e)
        {
            log.Warn(e.OldFullPath + " file renamed. new name="+e.FullPath);
            if (FileDB.FileList.Any(x => x.path == e.OldFullPath))
            {
                int id = FileDB.FileList.Where(x => x.path == e.OldFullPath).First().id;
                SafeFile safeFile = new SafeFile(e.FullPath);
                FileModel file = new FileModel(id, safeFile.FullPath, safeFile.Hash);
                FileDB.Update(file);
            }
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
