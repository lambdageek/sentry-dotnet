using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Sentry.Protocol;

namespace Sentry.Benchmarks
{
    public class BreadcrumbOverheadBenchmarks
    {
        private const string Message = "Message";
        private const string Type = "Type";
        private const string Category = "Category";
        private static readonly Dictionary<string, string> Data = new Dictionary<string, string>
        {
            { Message, Type }
        };
        private const BreadcrumbLevel Level = BreadcrumbLevel.Critical;

        private IDisposable _sdk;

        [Params(1, 10, 100)]
        public int BreadcrumbsCount;

        [GlobalSetup(Target = nameof(EnabledClient_AddBreadcrumb) + "," +
                              nameof(EnabledSdk_PushScope_AddBreadcrumb_PopScope))]
        public void EnabledSdk() => _sdk = SentryCore.Init(Constants.ValidDsn);

        [GlobalCleanup(Target = nameof(EnabledClient_AddBreadcrumb) + "," +
                                nameof(EnabledSdk_PushScope_AddBreadcrumb_PopScope))]
        public void DisableDsk() => _sdk.Dispose();

        [Benchmark(Baseline = true, Description = "Disabled SDK: Add breadcrumbs")]
        public void DisabledClient_AddBreadcrumb()
        {
            for (int i = 0; i < BreadcrumbsCount; i++)
            {
                SentryCore.AddBreadcrumb(
                    Message,
                    Type,
                    Category,
                    Data,
                    Level);
            }
        }

        [Benchmark(Description = "Enabled SDK: Add breadcrumbs")]
        public void EnabledClient_AddBreadcrumb()
        {
            for (int i = 0; i < BreadcrumbsCount; i++)
            {
                SentryCore.AddBreadcrumb(
                    Message,
                    Type,
                    Category,
                    Data,
                    Level);
            }
        }

        [Benchmark(Description = "Disabled SDK: Push scope, add breadcrumbs, pop scope")]
        public void DisabledClient_PushScope_AddBreadcrumb_PopScope()
        {
            using (SentryCore.PushScope())
            {
                for (int i = 0; i < BreadcrumbsCount; i++)
                {
                    SentryCore.AddBreadcrumb(
                        Message,
                        Type,
                        Category,
                        Data,
                        Level);
                }
            }
        }

        [Benchmark(Description = "Enabled SDK: Push scope, add breadcrumbs, pop scope")]
        public void EnabledSdk_PushScope_AddBreadcrumb_PopScope()
        {
            using (SentryCore.PushScope())
            {
                for (int i = 0; i < BreadcrumbsCount; i++)
                {
                    SentryCore.AddBreadcrumb(
                        Message,
                        Type,
                        Category,
                        Data,
                        Level);
                }
            }
        }

    }
}
