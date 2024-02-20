using UnityEngine;

namespace Mona.SDK.Core.Events
{
    public struct MonaPlayerChangeAvatarEvent
    {
        public Animator Avatar;
        public MonaPlayerChangeAvatarEvent(Animator avatar)
        {
            Avatar = avatar;
        }
    }
}