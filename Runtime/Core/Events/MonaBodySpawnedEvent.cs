using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Events
{
    public struct MonaBodySpawnedEvent
    {
        public IMonaBody Body;
        public MonaBodySpawnedEvent(IMonaBody body)
        {
            Body = body;
        }
    }
}