using System;
using Garfoot.FluentUriBuilder.Interfaces;

namespace Garfoot.FluentUriBuilder
{
    public static class FluentUri
    {
        /// <summary>
        ///     Creates an empty fluent URI builder.
        /// </summary>
        /// <returns></returns>
        public static IFluentUriInitial Create(FluentUriOptions options = null)
        {
            var context = new FluentUriContext
            {
                Options = options ?? new FluentUriOptions()
            };

            return context;
        }

        /// <summary>
        ///     Creates a new fluent URI builder from a string containing an absolute URI.
        /// </summary>
        /// <param name="uri">The string containing the absolute URI.</param>
        /// <param name="options">Options for the builder.</param>
        /// <returns></returns>
        public static IFluentUri Parse(string uri, FluentUriOptions options = null)
        {
            var tempUri = new Uri(uri, UriKind.Absolute);

            IFluentUri builder = Create(options)
                                     .Scheme(tempUri.Scheme)
                                     .Host(tempUri.Host);

            if (tempUri.AbsolutePath.Length > 0)
            {
                builder.AddPathSegment(tempUri.AbsolutePath);
            }

            if (!tempUri.Fragment.IsNullOrEmpty())
            {
                // Need to substring to strip the leading # from the fragment
                builder.Fragment(tempUri.Fragment.Substring(1));
            }

            if (tempUri.Query.Length > 0)
            {
                // Need to substring to strip the leading ? from the query string
                QueryString queryString = QueryString.Parse(tempUri.Query.Substring(1));
                builder.AddQueryParam(queryString);
            }

            if (tempUri.UserInfo.Length > 0)
            {
                int index = tempUri.UserInfo.IndexOf(':');
                if (index == -1)
                {
                    builder.UserName(tempUri.UserInfo);
                }
                else if (index < tempUri.UserInfo.Length)
                {
                    string userName = tempUri.UserInfo.Substring(0, index);
                    string password = tempUri.UserInfo.Substring(index + 1);
                    builder.UserName(userName)
                           .Password(password);
                }
            }

            if (!tempUri.IsDefaultPort)
            {
                builder.Port(tempUri.Port);
            }

            return builder;
        }
    }
}