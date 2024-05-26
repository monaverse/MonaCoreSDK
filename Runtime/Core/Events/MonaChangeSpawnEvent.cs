using Mona.SDK.Core.Body;
using UnityEngine;

namespace Mona.SDK.Core.Events
{
    public struct MonaChangeSpawnEvent
    {
        public IMonaBody Body;
        public MonaChangeSpawnEvent(IMonaBody body)
        {
            Body = body;
        }
    }
}