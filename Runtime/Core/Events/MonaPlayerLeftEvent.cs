using UnityEngine;

namespace Mona.Core.Events
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