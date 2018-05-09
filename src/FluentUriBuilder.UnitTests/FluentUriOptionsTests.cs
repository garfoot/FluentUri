using System;
using FluentAssertions;
using Garfoot.FluentUriBuilder;
using Garfoot.FluentUriBuilder.Interfaces;
using NUnit.Framework;

namespace FluentUriBuilder.UnitTests
{
    [TestFixture]
    public class FluentUriOptionsTests
    {
        [Test]
        public void AlwaysSlashTerminatePath_TurnedOff_ReturnedPathHasNoTrailingSlash()
        {
            IFluentUri uri = FluentUri.Create(new FluentUriOptions {AlwaysSlashTerminatePath = false})
                                                    .Scheme("http")
                                                    .Host("aHost")
                                                    .AddPathSegment("/test/path");

            string result = uri.AsString();

            result.Should().Be("http://aHost/test/path", "no trailing /");
        }

        [Test]
        public void AlwaysSlashTerminatePath_TurnedOn_ReturnedPathHasNoTrailingSlash()
        {
            IFluentUri uri = FluentUri.Create(new FluentUriOptions {AlwaysSlashTerminatePath = true})
                                                    .Scheme("http")
                                                    .Host("aHost")
                                                    .AddPathSegment("/test/path");

            string result = uri.AsString();

            result.Should().Be("http://aHost/test/path/", "trailing /");
        }

        [TestCase("aPassword")]
        [TestCase(null)]
        [TestCase("")]
        public void AllowPassword_TurnedOff_ThrowsInvalidUriException(string password)
        {
            IFluentUri uri = FluentUri.Create(new FluentUriOptions {AllowPasswordInUserInfo = false})
                                                    .Scheme("http")
                                                    .Host("aHost");

            var exception = Assert.Catch<InvalidUriException>(() => uri.Password(password), "An exception is expected if a password is set");
            exception.Message.Should().Contain("Password", "the exception should be about the password");
        }

        [Test]
        public void OptionsNeededForParse_ParseSucceeds()
        {
            var result = FluentUri.Parse("http://user:pass@www.example.com", new FluentUriOptions {AllowPasswordInUserInfo = true})
                                  .AsString();

            result.Should().Be("http://user:pass@www.example.com/", "URI should parse ok with the options set");
        }
    }
}