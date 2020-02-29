using System;
using Jaze.UI.Definitions;

namespace Jaze.UI.Services.URI
{
    public class UriService : IUriService
    {
        private const string BaseUri = "http://local/";
        private static readonly Uri URI_PREFIX = new Uri(BaseUri);
        private const string ActionKey = "action";
        private const string ParameterKey = "parameter";
        private static readonly UriTemplate UriTemplate = new UriTemplate($"{{{ActionKey}}}/{{{ParameterKey}}}");

        public Uri Create(UriAction action, string parameter)
        {
            return new Uri($"{BaseUri}{action}/{parameter}");
        }

        public (UriAction action, string parameter) Parse(Uri uri)
        {
            var matched = UriTemplate.Match(URI_PREFIX, uri);
            if (matched != null)
            {
                var actionStr = matched.BoundVariables[ActionKey];
                Enum.TryParse<UriAction>(actionStr, true, out var action);
                var parameter = matched.BoundVariables[ParameterKey];
                return (action, parameter);
            }
            else
            {
                return (UriAction.Unknown, string.Empty);
            }
        }
    }
}