using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Garfoot.FluentUriBuilder
{
    public class QueryString : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly IList<KeyValuePair<string, string>> queryString = new List<KeyValuePair<string, string>>();

        /// <summary>
        ///     Gets a single item from the collection with the name. If the item is not present this will return null.
        ///     If there are multiple items with the same key, the first will be returned.
        /// </summary>
        /// <param name="key">The key for the item to get.</param>
        /// <returns></returns>
        public string this[string key]
        {
            get { return this.queryString.FirstOrDefault(i => string.Equals(i.Key, key, StringComparison.OrdinalIgnoreCase)).Value; }
        }

        /// <summary>
        ///     Test if the collection has any items in it.
        /// </summary>
        /// <value></value>
        public bool HasItems => this.queryString.Count > 0;

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var pair in this.queryString)
            {
                yield return pair;
            }
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the
        ///     collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        ///     Parse a query string.
        /// </summary>
        /// <param name="query">The query string to parse.</param>
        /// <returns></returns>
        public static QueryString Parse(string query)
        {
            var queryString = new QueryString();

            var pairs = query.Split('&');

            foreach (string pair in pairs)
            {
                int index = pair.IndexOf('=');
                if (index == -1)
                {
                    // Just a key
                    queryString.Add(pair);
                }
                else if (index == 0)
                {
                    // Missing key
                    throw new ArgumentException("Query string arguments must have a key.", nameof(query));
                }
                else if (index < pair.Length)
                {
                    // Key and value
                    string key = pair.Substring(0, index);
                    string value = pair.Substring(index + 1);
                    queryString.Add(key, WebUtility.UrlDecode(value));
                }
                else
                {
                    // Trailing key with no value
                    string key = pair.Substring(0, index);
                    queryString.Add(key);
                }
            }

            return queryString;
        }

        /// <summary>
        ///     Add a key to the collection without a value.
        /// </summary>
        /// <param name="key"></param>
        public void Add(string key)
        {
            this.Add(key, null);
        }

        /// <summary>
        ///     Add a key and value to the collection.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            this.queryString.Add(new KeyValuePair<string, string>(key, value));
        }

        /// <summary>
        ///     Add one or more key / value pairs to the collection.
        /// </summary>
        /// <param name="items"></param>
        public void Add(IEnumerable<KeyValuePair<string, string>> items)
        {
            foreach (var item in items)
            {
                this.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        ///     Add one or more key / value pairs to the collection.
        /// </summary>
        /// <param name="items"></param>
        public void Add(params KeyValuePair<string, string>[] items)
        {
            this.Add((IEnumerable<KeyValuePair<string, string>>) items);
        }

        /// <summary>
        ///     Add one or more key / value pairs to the collection.
        /// </summary>
        /// <param name="items"></param>
        public void Add(IEnumerable<(string Key, string Value)> items)
        {
            foreach ((string Key, string Value) tuple in items)
            {
                this.Add(tuple.Key, tuple.Value);
            }
        }

        /// <summary>
        ///     Add one or more key / value pairs to the collection.
        /// </summary>
        /// <param name="items"></param>
        public void Add(params (string Key, string Value)[] items)
        {
            this.Add((IEnumerable<(string, string)>) items);
        }

        /// <summary>
        ///     Gets all of the items with the specified key. Will return an empty collection if none are present.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IEnumerable<string> GetItems(string key)
        {
            return this.queryString.Where(i => i.Key == key).Select(i => i.Value);
        }

        /// <summary>
        ///     Check if the key exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool HasKey(string key)
        {
            return this.queryString.Any(i => string.Equals(i.Key, key, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///     Render the query string as a correctly URL encoded string. This will not prefix with ?.
        /// </summary>
        /// <returns></returns>
        public string AsString()
        {
            return string.Join("&", this.queryString.Select(i => $"{i.Key}={WebUtility.UrlEncode(i.Value)}"));
        }
    }
}