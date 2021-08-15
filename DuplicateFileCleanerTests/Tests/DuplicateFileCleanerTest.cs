using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DuplicateFileCleaner;
using System.Linq;

namespace DuplicateFileCleanerTests.Tests
{
    [TestClass]
    public class DuplicateFileCleanerTest
    {
        private TestingEnviroment testingEnviroment;

        [TestInitialize()]
        public void Initialize()
        {
            testingEnviroment = new TestingEnviroment( 5, 5, 2 );
        }

        [TestCleanup()]
        public void CleanUp()
        {
            testingEnviroment.Dispose();
        }

        [TestMethod]
        public async Task Folder_Cleaned()
        {
            var foldersSetup = testingEnviroment.FoldersSetup;
            var fileHasheshProvider = testingEnviroment.FileHashProvider;
            var testFiles = testingEnviroment.TestFiles;

            IDuplicateFileCleaner cleaner = new DuplicateFileCleaner.DuplicateFileCleaner( fileHasheshProvider );
            await cleaner.Clean( foldersSetup.ToCleanPath, foldersSetup.BackupPath );

            var expected = testFiles.OrderBy( h => h.Name ).ToList();
            var actual = testingEnviroment.GetResult().OrderBy( h => h.Name ).ToList();

            CollectionAssert.AreEqual( expected, actual );
        }


    }
}
