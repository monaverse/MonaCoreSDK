using UnityEngine;

namespace Mona.SDK.Core.Events
{
    public struct MonaPlayerLeftEvent
    {
        public int PlayerId;

        public MonaPlayerLeftEvent(int playerId)
        {
            PlayerId = playerId;
        }
    }
}