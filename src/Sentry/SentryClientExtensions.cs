using System;
using System.ComponentModel;

namespace Sentry
{
    /// <summary>
    /// Extension methods for <see cref="ISentryClient"/>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SentryClientExtensions
    {
        /// <summary>
        /// Captures the exception.
        /// </summary>
        /// <param name="client">The Sentry client.</param>
        /// <param name="ex">The exception.</param>
        /// <returns></returns>
        public static Guid CaptureException(this ISentryClient client, Exception ex)
        {
            return !client.IsEnabled
                ? Guid.Empty
                : client.CaptureEvent(new SentryEvent(ex));
        }
    }
}
