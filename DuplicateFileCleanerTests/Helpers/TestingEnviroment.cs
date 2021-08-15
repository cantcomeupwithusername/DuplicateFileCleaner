using DuplicateFileCleaner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileCleanerTests
{
    public class TestingEnviroment : IDisposable
    {
        private static readonly Random random = new Random();

        public TestingEnviroment(int fileCount, int depth, int width )
        {
            var folderID = Guid.NewGuid();
            FoldersSetup = new FoldersSetup( folderID );
            TestFiles = GenerateFiles( fileCount );
            FileHashProvider = Prepare( depth, width );
        }

        public FoldersSetup FoldersSetup { get; }
        public IFileHashInfoProvider FileHashProvider { get; }
        public IEnumerable<TestFile> TestFiles { get; }

        public void Dispose()
        {
            if ( FoldersSetup == null )
                return;

            Directory.Delete( FoldersSetup.RootPath, true );
        }

        private IFileHashInfoProvider Prepare( int depth, int width )
        {
            Directory.CreateDirectory( FoldersSetup.RootPath );
            Directory.CreateDirectory( FoldersSetup.ToCleanPath );
            Directory.CreateDirectory( FoldersSetup.BackupPath );

            var hashes = new List<FileHashInfo>();
            CreateTestFilesRecursively( depth, width, FoldersSetup.ToCleanPath, hashes );
            IFileHashInfoProvider fake = new FakeFileHashInfoProvider( hashes );

            return fake;
        }

        public IEnumerable<TestFile> GetResult()
        {
            var list = new List<TestFile>();

            foreach ( var filePath in Directory.EnumerateFiles( FoldersSetup.ToCleanPath, "*", SearchOption.AllDirectories ) )
                list.Add( new TestFile( filePath, File.ReadAllText( filePath ) ) );

            return list;
        }

        private void CreateTestFilesRecursively( int depth, int width, string currentFolder, List<FileHashInfo> fileHashes )
        {
            foreach ( var file in TestFiles )
            {
                var filePath = Path.Combine( currentFolder, file.Name );
                File.WriteAllText( filePath, file.Content );
                fileHashes.Add( new FileHashInfo( filePath, file.Hash ) );
            }

            if ( depth <= 0 || width <= 0 )
                return;

            depth--;
            for ( int i = 0; i < width; i++ )
            {
                var path = Path.Combine( currentFolder, $"Folder{i}" );
                Directory.CreateDirectory( path );
                CreateTestFilesRecursively( depth, width, path, fileHashes );
            }
        }

        private TestFile[] GenerateFiles(int count)
        {
            var files = new TestFile[ count ];

            for ( int i = 0; i < count; i++ )
                files[i] = new TestFile( Guid.NewGuid().ToString(), RandomString( 100 ) );

            return files;
        }

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private string RandomString( int length )
        {
            var randomString = new char[ length ];
            for ( int i = 0; i < length; i++ )
                randomString[ i ] = chars[ random.Next( chars.Length ) ];

            return new string( randomString );
        }

    }
}
