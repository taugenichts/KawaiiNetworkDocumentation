using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Kawaii.NetworkDocumentation.AppDataService
{
    public class JsonContentNegotiator : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter formatter;

        public JsonContentNegotiator(JsonMediaTypeFormatter jsonFormatter)
        {
            this.formatter = jsonFormatter;
        }

        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            return new ContentNegotiationResult(this.formatter, new MediaTypeHeaderValue("application/json"));
        }
    }    
}