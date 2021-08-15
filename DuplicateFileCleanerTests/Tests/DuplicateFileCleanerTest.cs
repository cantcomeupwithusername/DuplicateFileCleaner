using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DuplicateFileCleaner;
using System.Linq;
using System.Diagnostics;

namespace DuplicateFileCleanerTests.Tests
{
    [TestClass]
    public class DuplicateFileCleanerTest
    {
        private TestingEnviroment testingEnviroment;

        [TestInitialize()]
        public void Initialize()
        {
            testingEnviroment = new TestingEnviroment( 5, 6, 2 );
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

            var expected = testFiles.OrderBy( h => h.Hash ).ToList();
            var actual = testingEnviroment.GetResult().OrderBy( h => h.Hash ).ToList();

            CollectionAssert.AreEqual( expected, actual );
        }


    }
}
