using System;
using System.Collections.Generic;
using Garfoot.FluentUriBuilder.Interfaces;

namespace Garfoot.FluentUriBuilder
{
    internal class FluentUriContext :
        IFluentUriInitial,
        IFluentUriScheme,
        IFluentUri
    {
        public FluentUriOptions Options { get; set; }
        public FluentUriInfo UriInfo { get; } = new FluentUriInfo();
    }

    internal class FluentUriInfo
    {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public IList<string> PathSegments { get; } = new List<string>();
        public QueryString Query { get; } = new QueryString();
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Fragment { get; set; }
    }
}