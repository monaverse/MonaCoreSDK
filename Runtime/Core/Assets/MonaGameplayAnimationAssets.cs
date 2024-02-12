using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaGameplayAnimationAssets : MonaAnimationAssets
    {
        public const string GAMEPLAY_INTERACT = "Interact";

        public const string GAMEPLAY_COLLECTED_ITEM = "Collected Item";
        public const string GAMEPLAY_COLLECTED_KEY_ITEM = "Collected Key Item";
        public const string GAMEPLAY_COLLECTIBLE_IDLE = "Collectible Idle";

        public const string GAMEPLAY_POWER_UP = "Power Up";
        public const string GAMEPLAY_POWER_DOWN = "Power Down";

        public const string GAMEPLAY_WIN = "Win";
        public const string GAMEPLAY_LOSE = "Lose";
        public const string GAMEPLAY_TIE = "Tie";

        public const string GAMEPLAY_OPEN_DOOR = "Open Door";
        public const string GAMEPLAY_CLOSE_DOOR = "Close Door";

        public const string GAMEPLAY_PUSH_OBJECT = "Push Object";
        public const string GAMEPLAY_PULL_OBJECT = "Pull Object";

        public const string GAMEPLAY_PHYSICAL_WEAK = "Physical Attack Weak";
        public const string GAMEPLAY_PHYSICAL_STANDARD = "Physical Standard";
        public const string GAMEPLAY_PHYSICAL_STRONG = "Physical Strong";

        public const string GAMEPLAY_PROJECTILE_WEAK = "Projectile Weak";
        public const string GAMEPLAY_PROJECTILE_STANDARD = "Projectile Standard";
        public const string GAMEPLAY_PROJECTILE_STRONG = "Projectile Strong";

        public const string GAMEPLAY_MAGICAL_WEAK = "Magical Weak";
        public const string GAMEPLAY_MAGICAL_STANDARD = "Magical Standard";
        public const string GAMEPLAY_MAGICAL_STRONG = "Magical Strong";

        public const string GAMEPLAY_HIT_WEAK = "Hit Weak";
        public const string GAMEPLAY_HIT_STANDARD = "Hit Standard";
        public const string GAMEPLAY_HIT_STRONG = "Hit Strong";
        public const string GAMEPLAY_HIT_ENVIRONMENTAL = "Hit Environmental";
        public const string GAMEPLAY_HIT_POISON = "Hit Poison";

        public const string GAMEPLAY_DEATH_WEAK = "Death Weak";
        public const string GAMEPLAY_DEATH_STANDARD = "Death Standard";
        public const string GAMEPLAY_DEATH_STRONG = "Death Strong";

        public override List<string> DefaultNames => new List<string>()
        {
            GAMEPLAY_INTERACT,
            GAMEPLAY_COLLECTED_ITEM,
            GAMEPLAY_COLLECTED_KEY_ITEM,
            GAMEPLAY_COLLECTIBLE_IDLE,

            GAMEPLAY_POWER_UP,
            GAMEPLAY_POWER_DOWN,

            GAMEPLAY_WIN,
            GAMEPLAY_LOSE,
            GAMEPLAY_TIE,

            GAMEPLAY_OPEN_DOOR,
            GAMEPLAY_CLOSE_DOOR,

            GAMEPLAY_PUSH_OBJECT,
            GAMEPLAY_PULL_OBJECT,

            GAMEPLAY_PHYSICAL_WEAK,
            GAMEPLAY_PHYSICAL_STANDARD,
            GAMEPLAY_PHYSICAL_STRONG,

            GAMEPLAY_PROJECTILE_WEAK,
            GAMEPLAY_PROJECTILE_STANDARD,
            GAMEPLAY_PROJECTILE_STRONG,

            GAMEPLAY_MAGICAL_WEAK,
            GAMEPLAY_MAGICAL_STANDARD,
            GAMEPLAY_MAGICAL_STRONG,

            GAMEPLAY_HIT_WEAK,
            GAMEPLAY_HIT_STANDARD,
            GAMEPLAY_HIT_STRONG,
            GAMEPLAY_HIT_ENVIRONMENTAL,
            GAMEPLAY_HIT_POISON,

            GAMEPLAY_DEATH_WEAK,
            GAMEPLAY_DEATH_STANDARD,
            GAMEPLAY_DEATH_STRONG
        };
    }
}