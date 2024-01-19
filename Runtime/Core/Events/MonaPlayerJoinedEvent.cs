using Mona.Core.Body;
using UnityEngine;

namespace Mona.Core.Events
{
    public struct MonaPlayerJoinedEvent
    {
        public IMonaBody PlayerBody;
        public int PlayerId;
        public bool IsLocal;

        public MonaPlayerJoinedEvent(IMonaBody playerBody, int playerId, bool isLocal)
        {
            PlayerBody = playerBody;
            PlayerId = playerId;
            IsLocal = isLocal;
        }
    }
}