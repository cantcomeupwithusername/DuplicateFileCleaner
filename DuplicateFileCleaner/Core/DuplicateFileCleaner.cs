using DuplicateFileCleaner.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DuplicateFileCleaner
{
    public interface IDuplicateFileCleaner
    {
        Task Clean( string sourceFolderPath, string backupFolderPath );
    }

    public class DuplicateFileCleaner : IDuplicateFileCleaner
    {
        private const string backupFolderName = "BACKUP";
        private readonly IFileHashInfoProvider fileHashInfoProvider;
        private readonly ILogger logger;

        public DuplicateFileCleaner( IFileHashInfoProvider fileHashInfoProvider )
        {
            this.fileHashInfoProvider = fileHashInfoProvider;
        }

        public DuplicateFileCleaner( IFileHashInfoProvider fileHashInfoProvider, ILogger logger )
            :this(fileHashInfoProvider)
        {            
            this.logger = logger;
        }

        public async Task Clean( string sourceFolderPath, string backupFolderPath )
        {
            InputCheck( sourceFolderPath, backupFolderPath );            
            backupFolderPath = Path.Combine( backupFolderPath, backupFolderName );

            var map = new HashSet<string>();

            await foreach(var fileHashInfo in fileHashInfoProvider.Provide( sourceFolderPath ) )
            {
                if(!map.Contains(fileHashInfo.Hash))
                {
                    map.Add( fileHashInfo.Hash );
                    continue;
                }

                var sourceFilePath = fileHashInfo.FilePath;
                var desitnationFilePath = Path.Combine( backupFolderPath, 
                    Path.GetRelativePath( sourceFolderPath, sourceFilePath ) );
                var destinationDirectory = Path.GetDirectoryName( desitnationFilePath );

                if ( !Directory.Exists( destinationDirectory ) )
                    Directory.CreateDirectory( destinationDirectory );

                File.Move( sourceFilePath, desitnationFilePath, true );
                logger?.Write( $"File {sourceFilePath} was moved to {backupFolderName}" );
            }
        }

        private void InputCheck( string sourceFolderPath, string backupFolderPath )
        {
            if ( !Directory.Exists( sourceFolderPath ) )
                throw new DirectoryNotFoundException( $"Folder {sourceFolderPath} doesn't exist" );

            if ( !Directory.Exists( backupFolderPath ) )
                throw new DirectoryNotFoundException( $"Folder {backupFolderPath} doesn't exist" );

            if ( DirectoryHelper.IsParent( sourceFolderPath, backupFolderPath ) )
                throw new InvalidOperationException( "The backup folder must not be a subfolder of the cleaning folder" );
        }
    }
}
