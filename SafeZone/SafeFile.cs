using System;
using System.IO;
using System.Security.Cryptography;

namespace SafeZone
{
    class SafeFile
    {
        public SafeFile(string FilePath):this(FilePath, File.GetCreationTime(FilePath), File.GetLastWriteTime(FilePath))
        {
            
        }

        public SafeFile(string FilePath, DateTime CreatedAt, DateTime ModifiedAt)
        {
            this.FilePath = FilePath;
            this.CreatedAt = CreatedAt;
            this.ModifiedAt = ModifiedAt;
        }
        private string FilePath
        {
            get; set;
        }
        public string FullPath
        {
            get
            {
                return FilePath;
            }
        }
        public string Title
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }
        public string Type
        {
            get
            {
                return Path.GetExtension(FilePath);
            }
        }
        public DateTime CreatedAt
        {
            get; set;
        }
        public DateTime ModifiedAt
        {
            get; set;
        }
        public string Hash
        {
            get
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(FilePath))
                    {
                        var hash = md5.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
            }
        }
    }
}
