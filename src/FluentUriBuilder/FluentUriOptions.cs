using System;

namespace Garfoot.FluentUriBuilder
{
    public class FluentUriOptions
    {
        /// <summary>
        ///     Always ensure that the URI path ends with '/'.
        /// </summary>
        public bool AlwaysSlashTerminatePath { get; set; } = true;

        /// <summary>
        ///     Allow passwords in the user info section of the URI.
        ///     Exercise caution enabling this option as this will result in the
        ///     password being sent as part of the URI and potentially being
        ///     recorded in logs and / or browser history.
        /// </summary>
        public bool AllowPasswordInUserInfo { get; set; } = false;
    }
}
