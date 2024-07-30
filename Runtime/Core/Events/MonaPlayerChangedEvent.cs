using Mona.SDK.Core.Body;
using UnityEngine;

namespace Mona.SDK.Core.Events
{
    public struct MonaPlayerChangedEvent
    {
        public IMonaBody Body;
        public MonaPlayerChangedEvent(IMonaBody body)
        {
            Body = body;
        }
    }
}