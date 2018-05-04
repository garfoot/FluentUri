using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Garfoot.FluentUriBuilder;
using Garfoot.FluentUriBuilder.Interfaces;
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
            string uri = FluentUri.Create()
                                  .Scheme("http")
                                  .Host("www.example.com")
                                  .AsString();

            uri.Should().Be("http://www.example.com/");
        }

        [TestCaseSource(nameof(GetValidPathSegments))]
        public void PathSegments_ReturnsCorrectUri(IEnumerable<string> pathSegments, string expected)
        {
            string uri = FluentUri.Create()
                                  .Scheme("http")
                                  .Host("www.example.com")
                                  .AddPathSegment(pathSegments)
                                  .AsString();

            uri.Should().Be(expected);
        }

        [Test]
        public void WithPortNot80_ReturnsUriWithExplicitPort()
        {
            string uri = FluentUri.Create()
                                  .Scheme("http")
                                  .Host("www.example.com")
                                  .Port(8080)
                                  .AsString();

            uri.Should().Be("http://www.example.com:8080/");
        }

        [Test]
        public void WithPort80_ReturnsUriWithImplicitPort()
        {
            string uri = FluentUri.Create()
                                  .Scheme("http")
                                  .Host("www.example.com")
                                  .Port(80)
                                  .AsString();

            uri.Should().Be("http://www.example.com/");
        }


        [Test]
        public void AddQueryParam_MultipleSingleItems_ReturnsCorrectUri()
        {
            string uri = FluentUri.Create()
                                  .Scheme("http")
                                  .Host("www.example.com")
                                  .AddQueryParam("aKey1", "aValue1")
                                  .AddQueryParam("aKey2", "aVal ue2")
                                  .AddQueryParam("aKey1", null)
                                  .AsString();

            uri.Should().Be("http://www.example.com/?aKey1=aValue1&aKey2=aVal+ue2&aKey1=");
        }

        [Test]
        public void AddQueryParam_MultipleParamsItems_ReturnsCorrectUri()
        {
            string uri = FluentUri.Create()
                                  .Scheme("http")
                                  .Host("www.example.com")
                                  .AddQueryParam(("aKey1", "aValue1"), ("aKey2", "aVal ue2"), ("aKey1", null))
                                  .AsString();

            uri.Should().Be("http://www.example.com/?aKey1=aValue1&aKey2=aVal+ue2&aKey1=");
        }

        [Test]
        public void AddQueryParam_DictionaryOfItems_ReturnsCorrectUri()
        {
            string uri = FluentUri.Create()
                                  .Scheme("http")
                                  .Host("www.example.com")
                                  .AddQueryParam(new Dictionary<string, string>
                                  {
                                      ["aKey1"] = "aValue1",
                                      ["aKey2"] = "aVal ue2",
                                      ["aKey3"] = null
                                  })
                                  .AsString();

            uri.Should().Be("http://www.example.com/?aKey1=aValue1&aKey2=aVal+ue2&aKey3=");
        }

        [TestCase("aUser", null, "http://aUser@www.example.com/")]
        [TestCase("aUser", "aPassword", "http://aUser:aPassword@www.example.com/")]
        [TestCase(null, "aPassword", "http://:aPassword@www.example.com/")]
        public void UsernameAndPassword_ReturnsCorrectUri(string username, string password, string expected)
        {
            IFluentUri uri = FluentUri.Create(new FluentUriOptions {AllowPasswordInUserInfo = true})
                                      .Scheme("http")
                                      .Host("www.example.com")
                                      .Username(username);

            if (!password.IsNullOrEmpty())
            {
                uri.Password(password);
            }


            string result = uri.AsString();

            result.Should().Be(expected);
        }

        [Test]
        public void Fragment_ReturnsUriWithFragment()
        {
            string uri = FluentUri.Create(new FluentUriOptions {AlwaysSlashTerminatePath = true})
                                  .Scheme("http")
                                  .Host("www.example.com")
                                  .Fragment("someFragment")
                                  .AsString();

            uri.Should().Be("http://www.example.com/#someFragment");
        }

        private static IEnumerable GetValidPathSegments()
        {
            yield return new object[] {new[] {"any/path/to/resource"}, "http://www.example.com/any/path/to/resource/"};
            yield return new object[] {new[] {"/any/path/to/resource/"}, "http://www.example.com/any/path/to/resource/"};
            yield return new object[] {new[] {"any/path/", "to/resource"}, "http://www.example.com/any/path/to/resource/"};
            yield return new object[] {new[] {"any", "path", "to", "resource"}, "http://www.example.com/any/path/to/resource/"};
        }
    }
}