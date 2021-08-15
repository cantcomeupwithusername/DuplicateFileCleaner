using System;
using System.Threading.Tasks;
using System.IO;

namespace DuplicateFileCleaner
{
    class Program
    {
        static async Task Main( string[] args )
        {
            ILogger logger = new Logger();
            IFileHashInfoProvider hashInfoProvider = new FileHashInfoProvider( logger );
            IDuplicateFileCleaner duplicateFileCleaner = new DuplicateFileCleaner( hashInfoProvider, logger );

            while(true)
            {
                Console.WriteLine( @"Enter the path to the folder you're going to clean" );
                var sourceFolderPath = GetInput();
                Console.WriteLine( @"Enter the path to the backup folder where the cleaned duplicates will be moved" + Environment.NewLine
                                    + "The backup folder must not be in any subfolder of the cleaning folder" );
                var backupFolderPath = GetInput();

                try
                {
                    await duplicateFileCleaner.Clean( sourceFolderPath, backupFolderPath );
                    Console.WriteLine( $"The folder {sourceFolderPath} is cleaned" );
                    return;
                }
                catch(Exception ex)
                {
                    Console.WriteLine( ex.Message );
                }
            }           
        }


        public static string GetInput()
        {
            var enteredText = Console.ReadLine();

            if ( enteredText.Equals( "exit", StringComparison.OrdinalIgnoreCase ) )
                Environment.Exit( 0 );

            return enteredText;
        }
    }
}
