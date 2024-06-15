using System.Collections.Generic;
using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Events
{
    public struct MonaBodyScaleChangedEvent
    {
        public IMonaBody Body;
        public MonaBodyScaleChangedEvent(IMonaBody body)
        {
            Body = body;
        }
    }
}