using Mona.SDK.Core.Input.Interfaces;
using System.Collections.Generic;

namespace Mona.SDK.Core.Events
{
    public struct MonaFixedTickEvent
    {
        public float DeltaTime;

        public MonaFixedTickEvent(float deltaTime)
        {
            DeltaTime = deltaTime;
        }

    }
}