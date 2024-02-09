using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Float Animation Assets")]
    public class MonaFloatAnimationAssets : MonaAnimationAssets
    {
        public const string FLOAT_FORWARD = "Float Forward";
        public const string FLOAT_FORWARD_FAST = "Float Forward Fast";
        public const string FLOAT_BACKWARD = "Float Backward";
        public const string FLOAT_LEFT = "Float Left";
        public const string FLOAT_RIGHT = "Float Right";
        public const string FLOAT_UP = "Float Up";
        public const string FLOAT_UP_FAST = "Float Up Fast";
        public const string FLOAT_DOWN = "Float Down";
        public const string FLOAT_DOWN_FAST = "Float Down Fast";
        public const string FLOAT_TURN_LEFT = "Float Turn Left";
        public const string FLOAT_TURN_RIGHT = "Float Turn Right";
        public const string FLOAT_PITCH_UP = "Float Pitch Up";
        public const string FLOAT_PITCH_DOWN = "Float Pitch Down";
        public const string FLOAT_COLLIDE = "Float Collide";
        public const string FLOAT_COLLIDE_FAST = "Float Collide Fast";

        public const string FLOAT_CALM_IDLE = "Float Calm Idle";
        public const string FLOAT_CALM_FIDGET_A = "Float Calm Fidget A";
        public const string FLOAT_CALM_FIDGET_B = "Float Calm Fidget B";
        public const string FLOAT_CALM_FIDGET_C = "Float Calm Fidget C";

        public const string FLOAT_AGGRO_IDLE = "Float Aggro Idle";
        public const string FLOAT_AGGRO_FIDGET_A = "Float Aggro Fidget A";
        public const string FLOAT_AGGRO_FIDGET_B = "Float Aggro Fidget B";
        public const string FLOAT_AGGRO_FIDGET_C = "Float Aggro Fidget C";

        public override List<string> DefaultNames => new List<string>()
        {
            FLOAT_FORWARD,
            FLOAT_FORWARD_FAST,
            FLOAT_BACKWARD,
            FLOAT_LEFT,
            FLOAT_RIGHT,
            FLOAT_UP,
            FLOAT_UP_FAST,
            FLOAT_DOWN,
            FLOAT_DOWN_FAST,
            FLOAT_TURN_LEFT,
            FLOAT_TURN_RIGHT,
            FLOAT_PITCH_UP,
            FLOAT_PITCH_DOWN,
            FLOAT_COLLIDE,
            FLOAT_COLLIDE_FAST,

            FLOAT_CALM_IDLE,
            FLOAT_CALM_FIDGET_A,
            FLOAT_CALM_FIDGET_B,
            FLOAT_CALM_FIDGET_C,

            FLOAT_AGGRO_IDLE,
            FLOAT_AGGRO_FIDGET_A,
            FLOAT_AGGRO_FIDGET_B,
            FLOAT_AGGRO_FIDGET_C
    };
    }
}