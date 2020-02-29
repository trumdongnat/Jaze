using Jaze.UI.Definitions;
using System;

namespace Jaze.UI.Services.URI
{
    public interface IUriService
    {
        Uri Create(UriAction action, string parameter);

        (UriAction action, string parameter) Parse(Uri uri);
    }
}