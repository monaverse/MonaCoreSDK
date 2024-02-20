using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Events
{
    public struct MonaBodyInstantiatedEvent
    {
        public IMonaBody Body;
        public MonaBodyInstantiatedEvent(IMonaBody body)
        {
            Body = body;
        }
    }
}