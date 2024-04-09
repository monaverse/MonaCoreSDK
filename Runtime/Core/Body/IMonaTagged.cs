using System.Collections.Generic;

namespace Mona.SDK.Core.Body
{
    public interface IMonaTagged
    {
        List<string> MonaTags { get; }
        bool HasMonaTag(string tag);
    }
}
