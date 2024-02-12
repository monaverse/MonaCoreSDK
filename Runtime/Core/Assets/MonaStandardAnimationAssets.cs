using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaStandardAnimationAssets : MonaAnimationAssets
    {
        public const string WALK_FORWARD = "Walk Forward";
        public const string RUN_FORWARD = "Run Forward";
        public const string WALK_BACKWARD = "Walk Backward";
        public const string RUN_BACKWARD = "Run Backward";

        public const string STRAFE_LEFT = "Strafe Left";
        public const string STRAFE_RIGHT = "Strafe Right";
        public const string TURN_LEFT = "Turn Left";
        public const string TURN_RIGHT = "Turn Right";
        public const string SKIDDING = "Skidding";

        public const string SITTING_DOWN = "Sitting - Sit Down";
        public const string SITTING_GET_UP = "Sitting - Get Up";
        public const string SITTING_UP = "Sit Up";

        public const string JUMP_START = "Jump Start";
        public const string JUMPING = "Jumping";
        public const string JUMP_FLIP = "Jump Flip";
        public const string JUMP_FALL = "Jump Fall";
        public const string JUMP_LAND = "Jump Land";

        public const string CROUCH_DOWN = "Crouch Down";
        public const string CROUCH_IDLE = "Crouch Idle";
        public const string CROUCH_RETURN = "Crouch Return";

        public const string FALLING = "Falling";
        public const string FALL_LAND = "Fall Land";
        public const string SLIDE_FORWARD = "Slide Forward";
        public const string SLIDE_IDLE = "Slide Idle";
        public const string SLIDE_RETURN = "Slide Return";

        public const string WALK_COLLIDER = "Walk Collide";
        public const string RUN_COLLIDER = "Run Collide";

        public const string GLIDE_FORWARD = "Glide Forward";
        public const string GLIDE_BACK = "Glide Back";
        public const string GLIDE_LEFT = "Glide Left";
        public const string GLIDE_RIGHT = "Glide Right";
        public const string GLIDE_UP = "Glide Up";
        public const string GLIDE_DOWN = "Glide Down";
        public const string GLIDE_IDLE = "Glide Idle";

        public override List<string> DefaultNames => new List<string>()
        {
            WALK_FORWARD,
            RUN_FORWARD,
            WALK_BACKWARD,
            RUN_BACKWARD,

            STRAFE_LEFT,
            STRAFE_RIGHT,
            TURN_LEFT,
            TURN_RIGHT,
            SKIDDING,

            SITTING_DOWN,
            SITTING_GET_UP,
            SITTING_UP,

            JUMP_START,
            JUMPING,
            JUMP_FLIP,
            JUMP_FALL,
            JUMP_LAND,

            CROUCH_DOWN,
            CROUCH_IDLE,
            CROUCH_RETURN,

            FALLING,
            FALL_LAND,
            SLIDE_FORWARD,
            SLIDE_IDLE,
            SLIDE_RETURN,

            WALK_COLLIDER,
            RUN_COLLIDER,

            GLIDE_FORWARD,
            GLIDE_BACK,
            GLIDE_LEFT,
            GLIDE_RIGHT,
            GLIDE_UP,
            GLIDE_DOWN,
            GLIDE_IDLE
        };
    }
}