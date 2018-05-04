using System;

namespace Garfoot.FluentUriBuilder
{
    public class FluentUriOptions
    {
        public bool AlwaysSlashTerminatePath { get; set; } = true;
        public bool AllowPasswordInUserInfo { get; set; } = false;
    }
}