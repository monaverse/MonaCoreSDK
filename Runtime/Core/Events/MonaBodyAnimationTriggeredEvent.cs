using System.Collections.Generic;

namespace Mona.SDK.Core.Events
{
    public struct MonaBodyAnimationTriggeredEvent
    {
        public string ClipName;
        public MonaBodyAnimationTriggeredEvent(string clipName)
        {
            ClipName = clipName;
        }
    }
}