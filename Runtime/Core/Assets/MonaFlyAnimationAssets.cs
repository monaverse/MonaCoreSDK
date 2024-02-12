using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaFlyAnimationAssets : MonaAnimationAssets
    {
        public const string FLY_FORWARD = "Fly Forward";
        public const string FLY_FORWARD_FAST = "Fly Forward Fast";
        public const string FLY_BACKWARD = "Fly Backward";
        public const string FLY_LEFT = "Fly Left";
        public const string FLY_RIGHT = "Fly Right";
        public const string FLY_UP = "Fly Up";
        public const string FLY_UP_FAST = "Fly Up Fast";
        public const string FLY_DOWN = "Fly Down";
        public const string FLY_DOWN_FAST = "Fly Down Fast";
        public const string FLY_TURN_LEFT = "Fly Turn Left";
        public const string FLY_TURN_RIGHT = "Fly Turn Right";
        public const string FLY_PITCH_UP = "Fly Pitch Up";
        public const string FLY_PITCH_DOWN = "Fly Pitch Down";
        public const string FLY_COLLIDE = "Fly Collide";
        public const string FLY_COLLIDE_FAST = "Fly Collide Fast";
        public const string FLY_DIVE_BOMB = "Fly Dive Bomb";

        public const string FLY_CALM_IDLE = "Fly Calm Idle";
        public const string FLY_CALM_FIDGET_A = "Fly Calm Fidget A";
        public const string FLY_CALM_FIDGET_B = "Fly Calm Fidget B";
        public const string FLY_CALM_FIDGET_C = "FLy Calm Fidget C";

        public const string FLY_AGGRO_IDLE = "Fly Aggro Idle";
        public const string FLY_AGGRO_FIDGET_A = "Fly Aggro Fidget A";
        public const string FLY_AGGRO_FIDGET_B = "Fly Aggro Fidget B";
        public const string FLY_AGGRO_FIDGET_C = "Fly Aggro Fidget C";

        public override List<string> DefaultNames => new List<string>()
        {
            FLY_FORWARD,
            FLY_FORWARD_FAST,
            FLY_BACKWARD,
            FLY_LEFT,
            FLY_RIGHT,
            FLY_UP,
            FLY_UP_FAST,
            FLY_DOWN,
            FLY_DOWN_FAST,
            FLY_TURN_LEFT,
            FLY_TURN_RIGHT,
            FLY_PITCH_UP,
            FLY_PITCH_DOWN,
            FLY_COLLIDE,
            FLY_COLLIDE_FAST,

            FLY_AGGRO_IDLE,
            FLY_AGGRO_FIDGET_A,
            FLY_AGGRO_FIDGET_B,
            FLY_AGGRO_FIDGET_C,

            FLY_CALM_IDLE,
            FLY_CALM_FIDGET_A,
            FLY_CALM_FIDGET_B,
            FLY_CALM_FIDGET_C
        };
    }
}