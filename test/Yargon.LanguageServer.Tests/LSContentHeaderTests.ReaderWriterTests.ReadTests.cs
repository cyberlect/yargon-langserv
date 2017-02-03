using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Yargon.LanguageServer
{
    partial class LSContentHeaderTests
    {
        partial class ReaderWriterTests
        {
            /// <summary>
            /// Tests the <see cref="LSContentHeader.ReaderWriter.Read"/> method.
            /// </summary>
            [TestFixture]
            public sealed class ReadTests
            {
                [Test]
                public void ContentLength_IsParsedToInteger()
                {
                    // Arrange
                    string input = "Content-Length: 123\r\n\r\n";
                    var reader = new LSContentHeader.ReaderWriter();

                    // Act
                    var result = reader.Read(new StringReader(input));

                    // Assert
                    Assert.That(result.ToDictionary(), Is.EqualTo(new Dictionary<String, Object>
                    {
                        ["Content-Length"] = 123
                    }));
            }
            }
        }
    }
}
