using System;
using NSubstitute;
using Sentry.Protocol;
using Xunit;

namespace Sentry.Extensions.Logging.Tests
{
    public class SentryLoggerProviderTests
    {
        private class Fixture
        {
            public ISentryScopeManager SentryScopeManager { get; set; } = Substitute.For<ISentryScopeManager>();
            public SentryLoggingOptions SentryLoggingOptions { get; set; } = new SentryLoggingOptions();
            public SentryLoggerProvider GetSut() => new SentryLoggerProvider(SentryScopeManager, SentryLoggingOptions);
        }

        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void CreateLogger_LoggerType_SentryLogger()
        {
            var sut = _fixture.GetSut();

            Assert.IsType<SentryLogger>(sut.CreateLogger("category"));
        }

        [Fact]
        public void CreateLogger_Category_AsProvided()
        {
            var expectedCategory = nameof(SentryLoggerProviderTests);

            var sut = _fixture.GetSut();

            var actual = (SentryLogger)sut.CreateLogger(expectedCategory);

            Assert.Equal(expectedCategory, actual.CategoryName);
        }

        [Fact]
        public void Ctor_CreatesScope()
        {
            _fixture.GetSut();
            _fixture.SentryScopeManager.Received(1).PushScope();
        }

        [Fact(Skip = "Sentry is not accepting integrations ATM")]
        public void Ctor_AddsSdkIntegration()
        {
            var scope = new Scope(null);
            _fixture.SentryScopeManager.When(w => w.ConfigureScope(Arg.Any<Action<Scope>>()))
                .Do(info => info.Arg<Action<Scope>>()(scope));

            _fixture.GetSut();

            Assert.Contains(Constants.IntegrationName, scope.Sdk.Integrations);
        }

        [Fact]
        public void Dispose_DisposesNewScope()
        {
            var disposable = Substitute.For<IDisposable>();
            _fixture.SentryScopeManager.PushScope().Returns(disposable);

            var sut = _fixture.GetSut();

            sut.Dispose();

            disposable.Received(1).Dispose();
        }
    }
}
