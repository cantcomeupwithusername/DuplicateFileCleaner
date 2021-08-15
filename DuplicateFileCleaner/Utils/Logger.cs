using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileCleaner
{
    public interface ILogger
    {
        void Write( string message );
    }

    public class Logger : ILogger
    {
        public void Write( string message )
        {
            Console.WriteLine( message );
        }
    }
}
