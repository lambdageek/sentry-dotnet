using System;
using Microsoft.AspNetCore.Http;

namespace Sentry.AspNetCore
{
    public class FormRequestPayloadExtractor : IRequestPayloadExtractor
    {
        private const string SupportedContentType = "application/x-www-form-urlencoded";

        public object ExtractPayload(HttpRequest request)
        {
            return SupportedContentType
                .Equals(request.ContentType, StringComparison.InvariantCulture)
                ? request.Form
                : null;
        }
    }
}
