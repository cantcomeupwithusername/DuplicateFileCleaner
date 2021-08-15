using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicateFileCleaner;

namespace DuplicateFileCleanerTests
{
    public class TestFile : IEquatable<TestFile>
    {
        public TestFile(string name, string content)
        {
            Name = name;
            Content = content;
            Hash = Sha256Helper.GetHashByText( content );
        }

        public string Name { get; }
        public string Content { get; }
        public string Hash { get; }

        public override bool Equals( object obj )
        {
            return Equals( obj as TestFile );
        }

        public bool Equals( TestFile other )
        {
            return other != null &&
                     Hash == other.Hash;
        }
    }
}
