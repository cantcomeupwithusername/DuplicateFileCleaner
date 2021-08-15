using System;
using System.Collections.Generic;
using System.IO;

namespace DuplicateFileCleaner
{
    public interface IFileHashInfoProvider
    {
        IAsyncEnumerable<FileHashInfo> Provide(string folderPath);
    }

    public class FileHashInfoProvider : IFileHashInfoProvider
    {
        private readonly ILogger logger;

        public FileHashInfoProvider(ILogger logger)
        {
            this.logger = logger;
        }


        public async IAsyncEnumerable<FileHashInfo> Provide( string folderPath )
        {
            if ( !Directory.Exists( folderPath ) )
                throw new DirectoryNotFoundException( $"Folder {folderPath} doesn't exist" );            

            foreach (var filePath in Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories ))
            {
                var hash = await Sha256Helper.GetHash( filePath );
                logger.Write( $"FILE:{filePath} HASH:{hash}" );

                yield return new FileHashInfo( filePath, hash );
            }
        }
    }
}
