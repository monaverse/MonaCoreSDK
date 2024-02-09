using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Emote Animation Assets")]
    public class MonaEmoteAnimationAssets : MonaAnimationAssets
    {
        public const string EMOTE_YES = "Yes";
        public const string EMOTE_NO = "No";
        public const string EMOTE_GIVE = "Give";
        public const string EMOTE_TAKE = "Take";
        public const string EMOTE_STEAL = "Steal";
        public const string EMOTE_WAVE = "Wave";
        public const string EMOTE_PUT_DOWN = "Put Down";
        public const string EMOTE_PICK_UP = "Pick Up";
        public const string EMOTE_HUG = "Hug";
        public const string EMOTE_KISS = "Kiss";
        public const string EMOTE_DISMISS = "Dismiss";
        public const string EMOTE_TALK_PLEASANT = "Talk - Pleasant";
        public const string EMOTE_YELL_PLEASANT = "Yell - Pleasant";
        public const string EMOTE_TALK_HAPPY = "Talk - Happy";
        public const string EMOTE_YELL_HAPPY = "Yell - Happy";
        public const string EMOTE_TALK_EXCITED = "Talk - Excited";
        public const string EMOTE_YELL_EXCITED = "Yell - Excited";
        public const string EMOTE_TALK_SAD = "Talk - Sad";
        public const string EMOTE_YELL_SAD = "Yell - Sad";
        public const string EMOTE_TALK_ANGRY = "Talk - Angry";
        public const string EMOTE_YELL_ANGRY = "Yell - Angry";
        public const string EMOTE_TALK_SHY = "Talk - Shy";
        public const string EMOTE_TALK_CONFUSED = "Talk - Confused";
        public const string EMOTE_TALK_THINKING = "Talk - Thinking";
        public const string EMOTE_TALK_EUREKA = "Talk - Eureka";
        public const string EMOTE_IDLE_PLEASANT = "Idle - Pleasant";
        public const string EMOTE_IDLE_HAPPY = "Idle - Happy";
        public const string EMOTE_IDLE_EXCITED = "Idle - Excited";
        public const string EMOTE_IDLE_SAD = "Idle - Sad";
        public const string EMOTE_IDLE_ANGRY = "Idle - Angry";
        public const string EMOTE_IDLE_SHY = "Idle - Shy";
        public const string EMOTE_IDLE_CONFUSED = "Idle - Confused";
        public const string EMOTE_IDLE_THINKING = "Idle - Thinking";
        public const string EMOTE_IDLE_EUREKA = "Idle - Eureka";
        public const string EMOTE_DANCE_A = "Dance A";
        public const string EMOTE_DANCE_B = "Dance B";
        public const string EMOTE_DANCE_C = "Dance C";

        public override List<string> DefaultNames => new List<string>()
        {
            EMOTE_YES,
            EMOTE_NO,
            EMOTE_GIVE,
            EMOTE_TAKE,
            EMOTE_STEAL,
            EMOTE_WAVE,
            EMOTE_PUT_DOWN,
            EMOTE_PICK_UP,
            EMOTE_HUG,
            EMOTE_KISS,
            EMOTE_DISMISS,
            EMOTE_TALK_PLEASANT,
            EMOTE_YELL_PLEASANT,
            EMOTE_TALK_HAPPY,
            EMOTE_YELL_HAPPY,
            EMOTE_TALK_EXCITED,
            EMOTE_YELL_EXCITED,
            EMOTE_TALK_SAD,
            EMOTE_YELL_SAD,
            EMOTE_TALK_ANGRY,
            EMOTE_YELL_ANGRY,
            EMOTE_TALK_SHY,
            EMOTE_TALK_CONFUSED,
            EMOTE_TALK_THINKING,
            EMOTE_TALK_EUREKA,
            EMOTE_IDLE_PLEASANT,
            EMOTE_IDLE_HAPPY,
            EMOTE_IDLE_EXCITED,
            EMOTE_IDLE_SAD,
            EMOTE_IDLE_ANGRY,
            EMOTE_IDLE_SHY,
            EMOTE_IDLE_CONFUSED,
            EMOTE_IDLE_THINKING,
            EMOTE_IDLE_EUREKA,
            EMOTE_DANCE_A,
            EMOTE_DANCE_B,
            EMOTE_DANCE_C
        };
    }
}