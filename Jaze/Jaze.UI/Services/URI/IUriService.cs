using Jaze.UI.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.UI.Services.URI
{
    public interface IUriService
    {
        Uri Create(UriAction action, string parameter);

        Tuple<UriAction, string> Parse(Uri uri);
    }
}