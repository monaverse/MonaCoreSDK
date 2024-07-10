using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Events
{
    public struct MonaStateAuthorityChangedEvent
    {
        public IMonaBody Body;
        public bool Owned;
        public MonaStateAuthorityChangedEvent(IMonaBody body, bool owned)
        {
            Body = body;
            Owned = owned;
        }
    }
}