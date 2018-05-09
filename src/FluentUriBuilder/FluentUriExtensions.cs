using System;
using System.Collections.Generic;
using Garfoot.FluentUriBuilder.Interfaces;

namespace Garfoot.FluentUriBuilder
{
    public static class FluentUriExtensions
    {
        /// <summary>
        ///     Creates a new fluent URI builder from a string containing an absolute URI.
        /// </summary>
        /// <param name="uri">The string containing the absolute URI.</param>
        /// <param name="options">Options for the builder.</param>
        /// <returns></returns>
        public static IFluentUri UriBuilder(this string uri, FluentUriOptions options = null)
        {
            return FluentUri.Parse(uri, options);
        }


        /// <summary>
        ///     Add a scheme to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="scheme">The scheme to add.</param>
        /// <returns></returns>
        public static IFluentUriScheme Scheme(this IFluentUriInitial uri, string scheme)
        {
            var context = (FluentUriContext) uri;

            context.UriInfo.Scheme = scheme;

            return context;
        }

        /// <summary>
        ///     Add a host to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        public static IFluentUri Host(this IFluentUriScheme uri, string host)
        {
            var context = (FluentUriContext) uri;

            context.UriInfo.Host = host;

            return context;
        }

        /// <summary>
        ///     Add one or more path segments to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="segments">The path segments to add.</param>
        /// <returns></returns>
        public static IFluentUri AddPathSegment(this IFluentUri uri, params string[] segments)
        {
            return AddPathSegment(uri, (IEnumerable<string>) segments);
        }

        /// <summary>
        ///     Add one or more path segments to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="segments">The path segments to add.</param>
        /// <returns></returns>
        public static IFluentUri AddPathSegment(this IFluentUri uri, IEnumerable<string> segments)
        {
            var context = (FluentUriContext) uri;

            foreach (string segment in segments)
            {
                string trimmedSegment = segment.Trim('/');
                if (!trimmedSegment.IsNullOrEmpty())
                {
                    context.UriInfo.PathSegments.Add(trimmedSegment);
                }
            }

            return context;
        }

        /// <summary>
        ///     Sets the port for the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="port">The port to use.</param>
        /// <returns></returns>
        public static IFluentUri Port(this IFluentUri uri, int port)
        {
            var context = (FluentUriContext) uri;

            context.UriInfo.Port = port;

            return context;
        }

        /// <summary>
        ///     Sets the user name for the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="userName">The user name.</param>
        /// <returns></returns>
        public static IFluentUri UserName(this IFluentUri uri, string userName)
        {
            var context = (FluentUriContext) uri;

            context.UriInfo.UserName = userName;

            return context;
        }

        /// <summary>
        ///     Sets the password for the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static IFluentUri Password(this IFluentUri uri, string password)
        {
            var context = (FluentUriContext) uri;

            if (!context.Options.AllowPasswordInUserInfo)
            {
                throw new InvalidUriException(
                    "Passwords cannot be encoded into a URI unless the AllowPasswordInUserInfo option is set.");
            }

            context.UriInfo.Password = password;

            return context;
        }

        /// <summary>
        ///     Add a query string parameter to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="name">The name of the query string parameter.</param>
        /// <param name="value">(optional) The value of the query string parameter, may be null.</param>
        /// <returns></returns>
        public static IFluentUri AddQueryParam(this IFluentUri uri, string name, string value)
        {
            var context = (FluentUriContext) uri;

            context.UriInfo.Query.Add(name, value);

            return context;
        }

        /// <summary>
        ///     Add a query string parameter to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="items">The items to add. A collection (or params) of tuples of string, string (key, value).</param>
        /// <returns></returns>
        public static IFluentUri AddQueryParam(this IFluentUri uri,
                                                   params (string Key, string Value)[] items)
        {
            var context = (FluentUriContext) uri;

            context.UriInfo.Query.Add(items);

            return context;
        }

        /// <summary>
        ///     Add a query string parameter to the URI. This overload does not support multiple items with the same key
        ///     in a single operation. Please use one of the other overloads to bulk add items with the same key.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="items">The items to add.</param>
        /// <returns></returns>
        public static IFluentUri AddQueryParam(this IFluentUri uri, IDictionary<string, string> items)
        {
            var context = (FluentUriContext) uri;

            context.UriInfo.Query.Add(items);

            return context;
        }

        /// <summary>
        ///     Add query string parameters to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="query">The query string.</param>
        /// <returns></returns>
        public static IFluentUri AddQueryParam(this IFluentUri uri, QueryString query)
        {
            var context = (FluentUriContext)uri;

            context.UriInfo.Query.Add(query);

            return context;
        }
        /// <summary>
        ///     Add a fragment to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="fragment">The fragment to add to the URI.</param>
        /// <returns></returns>
        public static IFluentUri Fragment(this IFluentUri uri, string fragment)
        {
            var context = (FluentUriContext) uri;

            context.UriInfo.Fragment = fragment;

            return context;
        }

        /// <summary>
        ///     Render a URI as a string.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string AsString(this IFluentUriRenderable uri)
        {
            var formatter = new UriFormatter();
            return formatter.Format((FluentUriContext) uri);
        }
    }
}