using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Garfoot.FluentUriBuilder
{
    public static class FluentUriExtensions
    {
        /// <summary>
        ///     Creates an empty fluent URI context.
        /// </summary>
        /// <returns></returns>
        public static IFluentUriInitial Create()
        {
            var context = new FluentUriContext();

            return context;
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
            var context = (FluentUriContext)uri;

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
            var context = (FluentUriContext)uri;

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
            var context = (FluentUriContext)uri;

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
            var context = (FluentUriContext)uri;

            context.Password = password;

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
            var context = (FluentUriContext)uri;

            builder.Append($"{context.Scheme}://");

            bool hasCredentials = false;
            if (!string.IsNullOrWhiteSpace(context.Username))
            {
                builder.Append(context.Username);
                hasCredentials = true;
            }

            if (!string.IsNullOrWhiteSpace(context.Password))
            {
                builder.Append($":{context.Password}");
                hasCredentials = true;
            }

            if (hasCredentials)
            {
                builder.Append("@");
            }

            builder.Append(context.Host);

            if (context.Port.HasValue && context.Port != 80)
            {
                builder.Append($":{context.Port}");
            }

            if (context.PathSegments.Count > 0)
            {
                builder.Append("/");
                builder.Append(string.Join("/", context.PathSegments));
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

    internal class FluentUriContext :
        IFluentUriInitial,
        IFluentUriScheme,
        IFluentUriBase
    {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public IList<string> PathSegments { get; set; } = new List<string>();
        public IDictionary<string, string> Query { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fragment { get; set; }
    }
}