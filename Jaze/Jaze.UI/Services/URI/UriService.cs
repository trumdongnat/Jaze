using System;
using Jaze.UI.Definitions;

namespace Jaze.UI.Services.URI
{
    public class UriService : IUriService
    {
        private const string BASE_URI = "http://local/";
        private static readonly Uri URI_PREFIX = new Uri(BASE_URI);
        private const string ACTION_KEY = "action";
        private const string PARAMETER_KEY = "parameter";
        private static readonly UriTemplate URI_TEMPLATE = new UriTemplate($"{{{ACTION_KEY}}}/{{{PARAMETER_KEY}}}");

        public Uri Create(UriAction action, string parameter)
        {
            return new Uri($"{BASE_URI}{action}/{parameter}");
        }

        public Tuple<UriAction, string> Parse(Uri uri)
        {
            var matched = URI_TEMPLATE.Match(URI_PREFIX, uri);
            if (matched != null)
            {
                var actionStr = matched.BoundVariables[ACTION_KEY];
                var action = UriAction.Unknown;
                Enum.TryParse<UriAction>(actionStr, true, out action);
                var parameter = matched.BoundVariables[PARAMETER_KEY];
                return new Tuple<UriAction, string>(action, parameter);
            }
            else
            {
                return new Tuple<UriAction, string>(UriAction.Unknown, string.Empty);
            }
        }
    }
}