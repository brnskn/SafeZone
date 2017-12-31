using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeZone
{
    class FileModel
    {
        public int id;
        public string path;
        public string hash;
        public string title
        {
            get
            {
                return Path.GetFileName(path);
            }
        }

        public FileModel(int id, string path, string hash)
        {
            this.id = id;
            this.path = path;
            this.hash = hash;
        }
    }
}
