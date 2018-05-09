using System.Text;

namespace Garfoot.FluentUriBuilder
{
    internal class UriFormatter
    {
        public string Format(FluentUriContext uri)
        {
            var builder = new StringBuilder();

            var hasCredentials = false;
            var hasAuthority = false;

            // Add in the scheme
            builder.Append($"{uri.UriInfo.Scheme}://");

            // Add in the authority (userinfo, host and port)
            if (!uri.UriInfo.UserName.IsNullOrWhiteSpace())
            {
                builder.Append(uri.UriInfo.UserName);
                hasCredentials = true;
            }

            if (!uri.UriInfo.Password.IsNullOrWhiteSpace())
            {
                builder.Append($":{uri.UriInfo.Password}");
                hasCredentials = true;
            }

            if (hasCredentials)
            {
                builder.Append("@");
                hasAuthority = true;
            }

            if (!uri.UriInfo.Host.IsNullOrWhiteSpace())
            {
                builder.Append(uri.UriInfo.Host);
                hasAuthority = true;
            }

            if (uri.UriInfo.Port.HasValue && uri.UriInfo.Port != 80)
            {
                builder.Append($":{uri.UriInfo.Port}");
                hasAuthority = true;
            }

            // Add in the path segments
            if (uri.UriInfo.PathSegments.Count > 0)
            {
                if (hasAuthority)
                {
                    builder.Append("/");
                }

                builder.Append(string.Join("/", uri.UriInfo.PathSegments));
            }

            if (uri.Options.AlwaysSlashTerminatePath)
            {
                builder.Append("/");
            }


            // Add in the query string
            if (uri.UriInfo.Query.HasItems)
            {
                builder.Append($"?{uri.UriInfo.Query.AsString()}");
            }

            // Add in the fragment
            if (!uri.UriInfo.Fragment.IsNullOrWhiteSpace())
            {
                builder.Append($"#{uri.UriInfo.Fragment}");
            }

            return builder.ToString();

        }
    }
}