using System;
using System.Threading.Tasks;
using Sentry.Protocol;

namespace Sentry.Internal
{
    internal sealed class DisabledSentryClient : ISentryClient, IDisposable
    {
        public static DisabledSentryClient Instance = new DisabledSentryClient();

        public bool IsEnabled => false;

        private DisabledSentryClient() { }

        public void ConfigureScope(Action<Scope> configureScope) { }
        public Task ConfigureScopeAsync(Func<Scope, Task> configureScope) => Task.CompletedTask;

        public IDisposable PushScope() => this;
        public IDisposable PushScope<TState>(TState state) => this;

        public void BindClient(ISentryClient client) { }

        public Guid CaptureEvent(SentryEvent evt, Scope scope = null) => Guid.Empty;

        public void Dispose() { }
    }
}
