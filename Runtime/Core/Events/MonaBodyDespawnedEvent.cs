using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Events
{
    public struct MonaBodyDespawnedEvent
    {
        public IMonaBody Body;
        public MonaBodyDespawnedEvent(IMonaBody body)
        {
            Body = body;
        }
    }
}