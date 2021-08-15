using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileCleaner.Utils
{
    public static class DirectoryHelper
    {
        public static string NormalizePath(string path)
        {
            return Path.GetFullPath( new Uri( path ).LocalPath )
                .TrimEnd( Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar )
                .ToUpperInvariant();
        }

        public static bool IsParent(string path, string subPath)
        {
            return NormalizePath( subPath )
                .StartsWith( NormalizePath( path ), 
                StringComparison.OrdinalIgnoreCase );
        }

    }
}
