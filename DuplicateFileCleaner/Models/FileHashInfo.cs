using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileCleaner
{
    public class FileHashInfo
    {
        public FileHashInfo( string filePath, string hash )
        {
            FilePath = filePath;
            Hash = hash;
        }

        public string FilePath { get; }
        public string Hash { get; }
    }
}
