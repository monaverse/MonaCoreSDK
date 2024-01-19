using UnityEngine;

namespace Mona.Core.Events
{
    public struct MonaPlayerNameEvent
    {
        public string PlayerName;
        public int PlayerId;

        public MonaPlayerNameEvent(string playerName, int playerId)
        {
            PlayerName = playerName;
            PlayerId = playerId;
        }
    }
}