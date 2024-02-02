using Mona.SDK.Core.Input.Interfaces;
using System.Collections.Generic;

namespace Mona.SDK.Core.Events
{
    public struct MonaTickEvent
    {
        public float DeltaTime;

        public MonaTickEvent(float deltaTime)
        {
            DeltaTime = deltaTime;
        }

    }
}