using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Yargon.LanguageServer
{
    partial class ContentHeaderTests
    {
        partial class ReaderWriterTests
        {
            /// <summary>
            /// Tests the <see cref="ContentHeader.ReaderWriter.Read"/> method.
            /// </summary>
            [TestFixture]
            public sealed class ReadTests
            {
                [Test]
                public void EmptyFields_ResultsInEmptyHeader()
                {
                    // Arrange
                    string input = "\r\n";
                    var reader = new ContentHeader.ReaderWriter();

                    // Act
                    var result = reader.Read(new StringReader(input));

                    // Assert
                    Assert.That(result.ToDictionary(), Is.EqualTo(new Dictionary<String, Object>()));
                }

                [Test]
                public void SingleField_ResultsInSingleHeader()
                {
                    // Arrange
                    string input = "My-Header: abc\r\n\r\n";
                    var reader = new ContentHeader.ReaderWriter();

                    // Act
                    var result = reader.Read(new StringReader(input));

                    // Assert
                    Assert.That(result.ToDictionary(), Is.EqualTo(new Dictionary<String, Object>
                    {
                        ["My-Header"] = "abc"
                    }));
                }

                [Test]
                public void MultipleFields_ResultInMultipleHeaders()
                {
                    // Arrange
                    string input = "My-Header: abc\r\nOther-Header: def\r\n\r\n";
                    var reader = new ContentHeader.ReaderWriter();

                    // Act
                    var result = reader.Read(new StringReader(input));

                    // Assert
                    Assert.That(result.ToDictionary(), Is.EqualTo(new Dictionary<String, Object>
                    {
                        ["My-Header"] = "abc",
                        ["Other-Header"] = "def"
                    }));
                }

                [Test]
                public void SingleLongField_ResultsInSingleHeader()
                {
                    // Arrange
                    string input = "My-Header: abc\r\n def ghi\r\n\r\n";
                    var reader = new ContentHeader.ReaderWriter();

                    // Act
                    var result = reader.Read(new StringReader(input));

                    // Assert
                    Assert.That(result.ToDictionary(), Is.EqualTo(new Dictionary<String, Object>
                    {
                        ["My-Header"] = "abc def ghi"
                    }));
                }
            }
        }
    }
}
