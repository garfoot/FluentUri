using System;
using FluentAssertions;
using Garfoot.FluentUriBuilder;
using NUnit.Framework;

namespace FluentUriBuilder.UnitTests
{
    [TestFixture]
    public class FluentUriParseTests
    {
        [TestCase("http://www.example.com", "http://www.example.com/")]
        [TestCase("http://www.example.com/this/is/a/path", "http://www.example.com/this/is/a/path/")]
        [TestCase("http://www.example.com:8080/this/is/a/path", "http://www.example.com:8080/this/is/a/path/")]
        [TestCase("http://www.example.com/this/is/a/path#fragment", "http://www.example.com/this/is/a/path/#fragment")]
        [TestCase("http://www.example.com/this/is/a/path?aKey1=aValue1&aKey2=aVal%20ue2", "http://www.example.com/this/is/a/path/?aKey1=aValue1&aKey2=aVal+ue2")]
        [TestCase("http://user:pass@www.example.com/this/is/a/path", "http://user:pass@www.example.com/this/is/a/path/")]
        [TestCase("http://:pass@www.example.com/this/is/a/path", "http://:pass@www.example.com/this/is/a/path/")]
        [TestCase("http://user:@www.example.com/this/is/a/path", "http://user@www.example.com/this/is/a/path/")]
        [TestCase("http://user@www.example.com/this/is/a/path", "http://user@www.example.com/this/is/a/path/")]
        public void Parse_ReturnsUrl(string input, string output)
        {
            string result = FluentUri.Parse(input, new FluentUriOptions {AllowPasswordInUserInfo = true})
                                     .AsString();

            result.Should().Be(output, "url should parse correctly");
        }
    }
}
