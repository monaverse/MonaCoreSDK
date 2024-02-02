using Mona.SDK.Core.Input.Interfaces;
using System.Collections.Generic;

namespace Mona.SDK.Core.Events
{
    public struct MonaBodyFixedTickEvent
    {
        public float DeltaTime;
        public bool HasInput;

        public MonaBodyFixedTickEvent(float deltaTime, bool hasInput)
        {
            DeltaTime = deltaTime;
            HasInput = hasInput;
        }
        
    }
}