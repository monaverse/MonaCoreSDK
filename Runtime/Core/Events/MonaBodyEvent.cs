using Mona.SDK.Core.Body.Enums;

namespace Mona.SDK.Core.Events
{
    public struct MonaBodyEvent
    {
        public MonaBodyEventType Type;

        public MonaBodyEvent(MonaBodyEventType type)
        {
            Type = type;
        }

    }
}