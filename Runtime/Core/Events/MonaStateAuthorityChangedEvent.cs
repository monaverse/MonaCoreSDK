using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Events
{
    public struct MonaStateAuthorityChangedEvent
    {
        public IMonaBody Body;
        public MonaStateAuthorityChangedEvent(IMonaBody body)
        {
            Body = body;
        }
    }
}