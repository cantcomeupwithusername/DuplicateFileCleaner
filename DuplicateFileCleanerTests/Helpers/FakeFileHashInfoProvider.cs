using DuplicateFileCleaner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileCleanerTests
{
    class FakeFileHashInfoProvider : IFileHashInfoProvider
    {
        private readonly IEnumerable<FileHashInfo> testFiles;

        public FakeFileHashInfoProvider(IEnumerable<FileHashInfo> testFiles )
        {
            this.testFiles = testFiles;
        }

        public async IAsyncEnumerable<FileHashInfo> Provide( string folderPath )
        {
            foreach(var testFile in testFiles)
            {
                await Task.CompletedTask;
                yield return testFile;
            }
        }
    }
}
