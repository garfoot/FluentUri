using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Garfoot.FluentUriBuilder;
using NUnit.Framework;

namespace FluentUriBuilder.UnitTests
{
    [TestFixture]
    [Category("UnitTests")]
    public class FluentUriTests
    {
        [Test]
        public void SchemeAndHost_ReturnsCorrectUri()
        {
            var uri = FluentUriExtensions.Create()
                                         .Scheme("http")
                                         .Host("www.example.com")
                                         .AsString();

            uri.Should().Be("http://www.example.com");
        }

        [TestCaseSource(nameof(GetValidPathSegments))]
        public void PathSegments_ReturnsCorrectUri(IEnumerable<string> pathSegments, string expected)
        {
            var uri = FluentUriExtensions.Create()
                                         .Scheme("http")
                                         .Host("www.example.com")
                                         .AddPathSegment(pathSegments)
                                         .AsString();

            uri.Should().Be(expected);
        }

        [Test]
        public void WithPortNot80_ReturnsCorrectUri()
        {
            var uri = FluentUriExtensions.Create()
                                         .Scheme("http")
                                         .Host("www.example.com")
                                         .Port(8080)
                                         .AsString();

            uri.Should().Be("http://www.example.com:8080");
        }

        [Test]
        public void WithPort80_ReturnsCorrectUri()
        {
            var uri = FluentUriExtensions.Create()
                                         .Scheme("http")
                                         .Host("www.example.com")
                                         .Port(80)
                                         .AsString();

            uri.Should().Be("http://www.example.com");
        }


        [TestCase("aUser", null, "http://aUser@www.example.com")]
        [TestCase("aUser", "aPassword", "http://aUser:aPassword@www.example.com")]
        [TestCase(null, "aPassword", "http://:aPassword@www.example.com")]
        public void UsernameAndPassword_ReturnsCorrectUri(string username, string password, string expected)
        {
            var uri = FluentUriExtensions.Create()
                                         .Scheme("http")
                                         .Host("www.example.com")
                                         .Username(username)
                                         .Password(password)
                                         .AsString();

            uri.Should().Be(expected);
        }



        private static IEnumerable GetValidPathSegments()
        {
            yield return new object[] {new[] {"any/path/to/resource"}, "http://www.example.com/any/path/to/resource"};
            yield return new object[] {new[] {"/any/path/to/resource/"}, "http://www.example.com/any/path/to/resource"};
            yield return new object[] {new[] {"any/path/", "to/resource"}, "http://www.example.com/any/path/to/resource"};
            yield return new object[] {new[] {"any", "path", "to", "resource"}, "http://www.example.com/any/path/to/resource"};
        }
    }
}