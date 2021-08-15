using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DuplicateFileCleanerTests.Tests
{
    [TestClass]
    public class InputParametersTest
    {
        private TestingEnviroment enviroment;

        [TestInitialize()]
        public void Initialize()
        {
            enviroment = new TestingEnviroment( 0, 0, 0 );
        }

        [TestCleanup()]
        public void CleanUp()
        {
            enviroment.Dispose();
        }

        [TestMethod]
        public async Task WhenSoruceDirectoryInvalid_ThrowDirectoryNotFound()
        {
            var cleaner = new DuplicateFileCleaner.DuplicateFileCleaner( enviroment.FileHashProvider);
            var foldersSetup = enviroment.FoldersSetup;

            await Assert.ThrowsExceptionAsync<DirectoryNotFoundException>( () => cleaner.Clean( "invalidDirectory", foldersSetup.BackupPath ) );
            await Assert.ThrowsExceptionAsync<DirectoryNotFoundException>( () => cleaner.Clean( foldersSetup.ToCleanPath, "invalidDirectory" ) );
        }

        [TestMethod]
        public async Task WhenBackupDirectorySubfolderOfSoruceDirectory_ThrowInvalidOperationException()
        {
            var cleaner = new DuplicateFileCleaner.DuplicateFileCleaner( enviroment.FileHashProvider );
            var foldersSetup = enviroment.FoldersSetup;
            var wrongPath = Path.Combine( foldersSetup.ToCleanPath, "Backup" );
            Directory.CreateDirectory( wrongPath );

            await Assert.ThrowsExceptionAsync<InvalidOperationException>( () => cleaner.Clean( foldersSetup.ToCleanPath, wrongPath ) );
        }

    }
}
