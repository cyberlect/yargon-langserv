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
            /// Tests the <see cref="ContentHeader.ReaderWriter.Write"/> method.
            /// </summary>
            [TestFixture]
            public sealed class WriteTests
            {
                [Test]
                public void EmptyHeader_ResultsInEmptyFields()
                {
                    // Arrange
                    var input = new ContentHeader(new Dictionary<String, Object>());
                    var writer = new ContentHeader.ReaderWriter();

                    // Act
                    string result;
                    using (var stringWriter = new StringWriter())
                    {
                        writer.Write(stringWriter, input);
                        result = stringWriter.ToString();
                    }

                    // Assert
                    string expected = "\r\n";
                    Assert.That(result, Is.EqualTo(expected));
                }

                [Test]
                public void SingleHeader_ResultsInSingleField()
                {
                    // Arrange
                    var input = new ContentHeader(new Dictionary<String, Object>
                    {
                        ["My-Header"] = "abc"
                    });
                    var writer = new ContentHeader.ReaderWriter();

                    // Act
                    string result;
                    using (var stringWriter = new StringWriter())
                    {
                        writer.Write(stringWriter, input);
                        result = stringWriter.ToString();
                    }

                    // Assert
                    string expected = "My-Header: abc\r\n\r\n";
                    Assert.That(result, Is.EqualTo(expected));
                }

                [Test]
                public void MultipleHeaders_ResultInMultipleFields()
                {
                    // Arrange
                    var input = new ContentHeader(new Dictionary<String, Object>
                    {
                        ["My-Header"] = "abc",
                        ["Other-Header"] = "def"
                    });
                    var writer = new ContentHeader.ReaderWriter();

                    // Act
                    string result;
                    using (var stringWriter = new StringWriter())
                    {
                        writer.Write(stringWriter, input);
                        result = stringWriter.ToString();
                    }

                    // Assert
                    string expected = "My-Header: abc\r\nOther-Header: def\r\n\r\n";
                    Assert.That(result, Is.EqualTo(expected));
                }

                [Test]
                public void SingleLongHeader_ResultsInLongField()
                {
                    // Arrange
                    var input = new ContentHeader(new Dictionary<String, Object>
                    {
                        ["My-Header"] = "abc def ghi"
                    });
                    var writer = new ContentHeader.ReaderWriter {MaximumLineLength = 14};

                    // Act
                    string result;
                    using (var stringWriter = new StringWriter())
                    {
                        writer.Write(stringWriter, input);
                        result = stringWriter.ToString();
                    }

                    // Assert
                    string expected = "My-Header: abc\r\n def ghi\r\n\r\n";
                    Assert.That(result, Is.EqualTo(expected));
                }
            }
        }
    }
}
