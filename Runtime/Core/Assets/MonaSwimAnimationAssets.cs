using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaSwimAnimationAssets : MonaAnimationAssets
    {
        public const string SWIM_FORWARD = "Swim Forward";
        public const string SWIM_FORWARD_FAST = "Swim Forward Fast";
        public const string SWIM_BACKWARD = "Swim Backward";
        public const string SWIM_LEFT = "Swim Left";
        public const string SWIM_RIGHT = "Swim Right";
        public const string SWIM_UP = "Swim Up";
        public const string SWIM_UP_FAST = "Swim Up Fast";
        public const string SWIM_DOWN = "Swim Down";
        public const string SWIM_DOWN_FAST = "Swim Down Fast";
        public const string SWIM_TURN_LEFT = "Swim Turn Left";
        public const string SWIM_TURN_RIGHT = "Swim Turn Right";
        public const string SWIM_PITCH_UP = "Swim Pitch Up";
        public const string SWIM_PITCH_DOWN = "Swim Pitch Down";
        public const string SWIM_COLLIDE = "Swim Collide";
        public const string SWIM_FAST_COLLIDE = "Swim Fast Collide";

        public const string SWIM_CALM_IDLE = "Swim Calm Idle";
        public const string SWIM_CALM_FIDGET_A = "Swim Calm Fidget A";
        public const string SWIM_CALM_FIDGET_B = "Swim Calm Fidget B";
        public const string SWIM_CALM_FIDGET_C = "Swim Calm Fidget C";

        public const string SWIM_AGGRO_IDLE = "Swim Aggro Idle";
        public const string SWIM_AGGRO_FIDGET_A = "Swim Aggro Fidget A";
        public const string SWIM_AGGRO_FIDGET_B = "Swim Aggro Fidget B";
        public const string SWIM_AGGRO_FIDGET_C = "Swim Aggro Fidget C";

        public override List<string> DefaultNames => new List<string>()
        {
            SWIM_FORWARD,
            SWIM_FORWARD_FAST,
            SWIM_BACKWARD,
            SWIM_LEFT,
            SWIM_RIGHT,
            SWIM_UP,
            SWIM_UP_FAST,
            SWIM_DOWN,
            SWIM_DOWN_FAST,
            SWIM_TURN_LEFT,
            SWIM_TURN_RIGHT,
            SWIM_PITCH_UP,
            SWIM_PITCH_DOWN,
            SWIM_COLLIDE,
            SWIM_FAST_COLLIDE,

            SWIM_CALM_IDLE,
            SWIM_CALM_FIDGET_A,
            SWIM_CALM_FIDGET_B,
            SWIM_CALM_FIDGET_C,

            SWIM_AGGRO_IDLE,
            SWIM_AGGRO_FIDGET_A,
            SWIM_AGGRO_FIDGET_B,
            SWIM_AGGRO_FIDGET_C
        };
    }
}