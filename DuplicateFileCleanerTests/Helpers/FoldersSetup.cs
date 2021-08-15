using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileCleanerTests
{
    public class FoldersSetup
    {
        private static readonly string BaseFolder = AppDomain.CurrentDomain.BaseDirectory;
        private const string clearFolderName = "TOCLEAN";
        private const string backupFolderName = "BACKUP";

        public FoldersSetup( Guid ID )
        {
            RootPath = GetTestingFolderName( ID );
            ToCleanPath = Path.Combine( RootPath, clearFolderName );
            BackupPath = Path.Combine( RootPath, backupFolderName );
        }

        public string RootPath { get; }
        public string ToCleanPath { get; }
        public string BackupPath { get; }

        private string GetTestingFolderName( Guid folderID )
        {
            var folderName = $"Test_{folderID.ToString()}";
            return Path.Combine( BaseFolder, folderName );
        }
    }
}
