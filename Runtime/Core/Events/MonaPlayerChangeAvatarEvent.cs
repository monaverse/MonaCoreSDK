using UnityEngine;

namespace Mona.SDK.Core.Events
{
    public struct MonaPlayerChangeAvatarEvent
    {
        public int PlayerId;
        public Animator Avatar;
        public MonaPlayerChangeAvatarEvent(int playerId, Animator avatar)
        {
            PlayerId = playerId;
            Avatar = avatar;
        }
    }
}