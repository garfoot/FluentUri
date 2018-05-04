using System;
using System.Reflection;
using FluentAssertions;
using Garfoot.FluentUriBuilder;
using NUnit.Framework;

namespace FluentUriBuilder.UnitTests
{
    [TestFixture]
    public class QueryStringTests
    {
        [TestCase("aKey=aValue", "aKey=aValue")]
        [TestCase("aKey", "aKey=")]
        [TestCase("aKey1=aValue1&aKey2", "aKey1=aValue1&aKey2=")]
        [TestCase("aKey1=aValue1&aKey2=aVal+ue2", "aKey1=aValue1&aKey2=aVal+ue2")]
        [TestCase("aKey1=aValue1&aKey2=aVal+ue2&aKey1=&aKey3=aValue3", "aKey1=aValue1&aKey2=aVal+ue2&aKey1=&aKey3=aValue3")]
        public void Parse_StringResultMatches(string input, string expected)
        {
            var queryString = QueryString.Parse(input);

            queryString.AsString().Should().Be(expected, "output query string should match the input");
        }

        [Test]
        public void AddSingle_ValueExists()
        {
            var queryString = new QueryString();
            queryString.Add("aKey");

            queryString.HasKey("aKey").Should().BeTrue("Key is present");
        }


        [Test]
        public void AddSingleWithValue_ValueExistsWithValue()
        {
            var queryString = new QueryString();
            queryString.Add("aKey", "aValue");

            queryString["aKey"].Should().Be("aValue", "value was added against the key");
        }

        [Test]
        public void HasItems_EmptyCollection_ReturnsFalse()
        {
            var queryString = new QueryString();

            queryString.HasItems.Should().BeFalse("no items in the collection");
        }

        [Test]
        public void HasItems_CollectionHasItems_ReturnsTrue()
        {
            var queryString = new QueryString();
            queryString.Add("aKey");

            queryString.HasItems.Should().BeTrue("items in the collection");
        }

        [Test]
        public void Indexer_NoMatch_ReturnsNull()
        {
            var queryString = new QueryString();
            queryString.Add("aKey1", "aValue1");
            queryString.Add("aKey2", "aValue2");
            queryString.Add("aKey3", "aValue3");

            queryString["notFound"].Should().BeNull("key is not present");
        }

        [Test]
        public void Indexer_CaseInsensitiveLookup_ReturnsValue()
        {
            var queryString = new QueryString();
            queryString.Add("AKEY", "aValue1");

            queryString["akey"].Should().Be("aValue1", "key is not case-sensitive");
        }

        [Test]
        public void Indexer_MultipleWithKey_ReturnsFirst()
        {
            var queryString = new QueryString();
            queryString.Add("aKey1", "aValue1");
            queryString.Add("aKey1", "aValue2");
            queryString.Add("aKey1", "aValue3");

            queryString["aKey1"].Should().Be("aValue1");
        }

        [Test]
        public void GetItems_MultipleWithKey_ReturnsAllMatchingOnly()
        {
            var queryString = new QueryString();
            queryString.Add("aKey1", "aValue1");
            queryString.Add("aKey2", "aValue2");
            queryString.Add("aKey1", "aValue3");

            queryString.GetItems("aKey1")
                       .Should().BeEquivalentTo(new[] {"aValue1", "aValue3"}, "matching keys only are returned");
        }


        [Test]
        public void AddMultipleTuple_AddsAll()
        {
            var queryString = new QueryString();
            queryString.Add(("aKey1", "aValue1"), ("aKey1", "aValue2"), ("aKey1", "aValue3"));

            queryString.GetItems("aKey1")
                       .Should().BeEquivalentTo(new[] {"aValue1", "aValue2", "aValue3"}, "all items added are present");
        }

        [Test]
        public void AsString_MultipleItemsWithSameKey_MaintainsOrder()
        {
            var queryString = new QueryString();
            queryString.Add("aKey1", "aValue1");
            queryString.Add("aKey2", "aValue2");
            queryString.Add("aKey1", "aValue3");

            var result = queryString.AsString();

            result.Should().Be("aKey1=aValue1&aKey2=aValue2&aKey1=aValue3", "query string should maintain order of items added");
        }

        [Test]
        public void AsString_MultipleItems_ReturnsAllItems()
        {
            var queryString = new QueryString();
            queryString.Add("aKey1", "aValue1");
            queryString.Add("aKey2", "aValue2");
            queryString.Add("aKey3", "aValue3");

            var result = queryString.AsString();

            result.Should().Be("aKey1=aValue1&aKey2=aValue2&aKey3=aValue3", "query string should contain all items");
        }

        [Test]
        public void AsString_ItemWithNoValue_ReturnsCorrectQueryString()
        {
            var queryString = new QueryString();
            queryString.Add("aKey1", "aValue1");
            queryString.Add("aKey2");
            queryString.Add("aKey3", "aValue3");

            var result = queryString.AsString();

            result.Should().Be("aKey1=aValue1&aKey2=&aKey3=aValue3", "query string should contain all items");
        }

        [Test]
        public void AsString_ValueContainsUrlInvalidChars_ReturnsEncoded()
        {
            var queryString = new QueryString();
            queryString.Add("aKey1", "escape&me please");

            var result = queryString.AsString();

            result.Should().Be("aKey1=escape%26me+please", "query string values should be URL encoded");
        }
    }
}