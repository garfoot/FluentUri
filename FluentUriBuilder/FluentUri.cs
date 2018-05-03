using System;
using System.Collections.Generic;
using System.Text;

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
        public static IFluentUriBase Parse(string uri, FluentUriOptions options = null)
        {
            var tempUri = new Uri(uri, UriKind.Absolute);

            var builder = Create(options)
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
                var index = tempUri.UserInfo.IndexOf(':');
                if (index == -1)
                {
                    builder.Username(tempUri.UserInfo);
                }
                else if (index < tempUri.UserInfo.Length)
                {
                    var username = tempUri.UserInfo.Substring(0, index);
                    var password = tempUri.UserInfo.Substring(index + 1);
                    builder.Username(username)
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


    public static class FluentUriExtensions
    {
        /// <summary>
        ///     Creates a new fluent URI builder from a string containing an absolute URI.
        /// </summary>
        /// <param name="uri">The string containing the absolute URI.</param>
        /// <param name="options">Options for the builder.</param>
        /// <returns></returns>
        public static IFluentUriBase UriBuilder(this string uri, FluentUriOptions options = null)
        {
            return FluentUri.Parse(uri, options);
        }

        /// <summary>
        ///     Sets the options on the URI builder.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="optionSetter">An action that configures the required options.</param>
        /// <returns></returns>
        public static IFluentUriBase WithOptions(this IFluentUriBase uri, Action<FluentUriOptions> optionSetter)
        {
            var context = (FluentUriContext) uri;

            optionSetter(context.Options);

            return context;
        }

        /// <summary>
        ///     Sets the options on the URI builder.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="optionSetter">An action that configures the required options.</param>
        /// <returns></returns>
        public static IFluentUriScheme WithOptions(this IFluentUriScheme uri, Action<FluentUriOptions> optionSetter)
        {
            return (IFluentUriScheme) WithOptions((IFluentUriBase) uri, optionSetter);
        }

        /// <summary>
        ///     Sets the options on the URI builder.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="optionSetter">An action that configures the required options.</param>
        /// <returns></returns>
        public static IFluentUriInitial WithOptions(this IFluentUriInitial uri, Action<FluentUriOptions> optionSetter)
        {
            return (IFluentUriInitial) WithOptions((IFluentUriBase) uri, optionSetter);
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

            context.Scheme = scheme;

            return context;
        }

        /// <summary>
        ///     Add a host to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        public static IFluentUriBase Host(this IFluentUriScheme uri, string host)
        {
            var context = (FluentUriContext) uri;

            context.Host = host;

            return context;
        }

        /// <summary>
        ///     Add one or more path segments to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="segments">The path segments to add.</param>
        /// <returns></returns>
        public static IFluentUriBase AddPathSegment(this IFluentUriBase uri, params string[] segments)
        {
            return AddPathSegment(uri, (IEnumerable<string>) segments);
        }

        /// <summary>
        ///     Add one or more path segments to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="segments">The path segments to add.</param>
        /// <returns></returns>
        public static IFluentUriBase AddPathSegment(this IFluentUriBase uri, IEnumerable<string> segments)
        {
            var context = (FluentUriContext) uri;

            foreach (string segment in segments)
            {
                context.PathSegments.Add(segment.Trim('/'));
            }

            return context;
        }

        /// <summary>
        ///     Sets the port for the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="port">The port to use.</param>
        /// <returns></returns>
        public static IFluentUriBase Port(this IFluentUriBase uri, int port)
        {
            var context = (FluentUriContext) uri;

            context.Port = port;

            return context;
        }

        /// <summary>
        ///     Sets the username for the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static IFluentUriBase Username(this IFluentUriBase uri, string username)
        {
            var context = (FluentUriContext) uri;

            context.Username = username;

            return context;
        }

        /// <summary>
        ///     Sets the password for the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static IFluentUriBase Password(this IFluentUriBase uri, string password)
        {
            var context = (FluentUriContext) uri;

            if (!context.Options.AllowPasswordInUserInfo)
            {
                throw new InvalidUriException(
                    "Passwords cannot be encoded into a URI unless the AllowPasswordInUserInfo option is set as this weakens the security of the password.");
            }

            context.Password = password;

            return context;
        }

        /// <summary>
        ///     Add a query string parameter to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="name">The name of the query string parameter.</param>
        /// <param name="value">(optional) The value of the query string parameter, may be null.</param>
        /// <returns></returns>
        public static IFluentUriBase AddQueryParam(this IFluentUriBase uri, string name, string value)
        {
            var context = (FluentUriContext) uri;

            context.Query.Add(name, value);

            return context;
        }

        /// <summary>
        ///     Add a query string parameter to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="items">The items to add. A collection (or params) of tuples of string, string (key, value).</param>
        /// <returns></returns>
        public static IFluentUriBase AddQueryParam(this IFluentUriBase uri,
                                                   params (string Key, string Value)[] items)
        {
            var context = (FluentUriContext) uri;

            context.Query.Add(items);

            return context;
        }

        /// <summary>
        ///     Add a query string parameter to the URI. This overload does not support multiple items with the same key
        ///     in a single operation. Please use one of the other overloads to bulk add items with the same key.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="items">The items to add.</param>
        /// <returns></returns>
        public static IFluentUriBase AddQueryParam(this IFluentUriBase uri, IDictionary<string, string> items)
        {
            var context = (FluentUriContext) uri;

            context.Query.Add(items);

            return context;
        }

        /// <summary>
        ///     Add query string parameters to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="query">The query string.</param>
        /// <returns></returns>
        public static IFluentUriBase AddQueryParam(this IFluentUriBase uri, QueryString query)
        {
            var context = (FluentUriContext)uri;

            context.Query.Add(query);

            return context;
        }
        /// <summary>
        ///     Add a fragment to the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="fragment">The fragment to add to the URI.</param>
        /// <returns></returns>
        public static IFluentUriBase Fragment(this IFluentUriBase uri, string fragment)
        {
            var context = (FluentUriContext) uri;

            context.Fragment = fragment;

            return context;
        }

        /// <summary>
        ///     Render a URI as a string.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string AsString(this IFluentUriRenderable uri)
        {
            var builder = new StringBuilder();
            var context = (FluentUriContext) uri;

            var hasCredentials = false;
            var hasAuthority = false;

            // Add in the scheme
            builder.Append($"{context.Scheme}://");

            // Add in the authority (userinfo, host and port)
            if (!context.Username.IsNullOrWhiteSpace())
            {
                builder.Append(context.Username);
                hasCredentials = true;
            }

            if (!context.Password.IsNullOrWhiteSpace())
            {
                builder.Append($":{context.Password}");
                hasCredentials = true;
            }

            if (hasCredentials)
            {
                builder.Append("@");
                hasAuthority = true;
            }

            if (!context.Host.IsNullOrWhiteSpace())
            {
                builder.Append(context.Host);
                hasAuthority = true;
            }

            if (context.Port.HasValue && context.Port != 80)
            {
                builder.Append($":{context.Port}");
                hasAuthority = true;
            }

            // Add in the path segments
            if (context.PathSegments.Count > 0)
            {
                if (hasAuthority)
                {
                    builder.Append("/");
                }

                builder.Append(string.Join("/", context.PathSegments));
            }

            if (context.Options.AlwaysSlashTerminatePath)
            {
                builder.Append("/");
            }


            // Add in the query string
            if (context.Query.HasItems)
            {
                builder.Append($"?{context.Query.AsString()}");
            }

            // Add in the fragment
            if (!context.Fragment.IsNullOrWhiteSpace())
            {
                builder.Append($"#{context.Fragment}");
            }

            return builder.ToString();
        }
    }

    /// <summary>
    ///     A fluent URI in an initial state with no data.
    /// </summary>
    public interface IFluentUriInitial
    {
    }

    /// <summary>
    ///     A fluent URI with a scheme defined.
    /// </summary>
    public interface IFluentUriScheme
    {
    }

    /// <summary>
    ///     A fluent URI with a base of scheme and host.
    /// </summary>
    public interface IFluentUriBase : IFluentUriRenderable
    {
    }

    /// <summary>
    ///     A fluent URI which can be rendered.
    /// </summary>
    public interface IFluentUriRenderable
    {
    }

    public class FluentUriOptions
    {
        public bool AlwaysSlashTerminatePath { get; set; } = true;
        public bool AllowPasswordInUserInfo { get; set; } = false;
    }

    internal class FluentUriContext :
        IFluentUriInitial,
        IFluentUriScheme,
        IFluentUriBase
    {
        public FluentUriOptions Options { get; set; }

        public string Scheme { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public IList<string> PathSegments { get; } = new List<string>();
        public QueryString Query { get; } = new QueryString();
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fragment { get; set; }
    }
}