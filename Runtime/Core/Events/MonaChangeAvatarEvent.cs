using UnityEngine;

namespace Mona.SDK.Core.Events
{
    public struct MonaChangeAvatarEvent
    {
        public Animator Avatar;
        public MonaChangeAvatarEvent(Animator avatar)
        {
            Avatar = avatar;
        }
    }
}