using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileCleaner
{
    public static class Sha256Helper
    {
        public static async Task<string> GetHash( string filePath )
        {
            using ( var stream = File.OpenRead( filePath ) )
            {
                return await GetHash( stream );
            }            
        }

        public static async Task<string> GetHash( Stream stream )
        {
            using ( var sha256 = SHA256.Create() )
            {
                var hash = await sha256.ComputeHashAsync( stream );
                return HexFromBytes( hash );
            }
        }

        public static string GetHashByText(string text)
        {
            using ( var sha256 = SHA256.Create() )
            {
                var hash = sha256.ComputeHash( Encoding.UTF8.GetBytes( text ) );
                return HexFromBytes( hash );
            }
        }

        private static string HexFromBytes(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();

            foreach ( var b in bytes )
                sb.Append( b.ToString( "X2" ) );

            return sb.ToString();
        }
    }
}
