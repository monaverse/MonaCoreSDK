using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Climb Animation Assets")]
    public class MonaClimbAnimationAssets : MonaAnimationAssets
    {
        public const string CLIMB_SURFACE_UP = "Climb Surface Up";
        public const string CLIMB_SURFACE_DOWN = "Climb Surface Down";
        public const string CLIMB_SURFACE_RIGHT = "Climb Surface Right";
        public const string CLIMB_SURFACE_LEFT = "Climb Surface Left";
        public const string CLIMB_SURFACE_IDLE = "Climb Surface Idle";
        public const string CLIMB_SURFACE_SLIP = "Climb Surface Slip";
        public const string CLIMB_SURFACE_SLIDE_DOWN = "Climb Surface Slide Down";
        public const string CLIMB_SURFACE_SURFACE_GRIP = "Climb Surface Grip";
        public const string CLIMB_SURFACE_TRANSITION_TO_TOP = "Climb Surface Transition to Top";

        public const string LADDER_UP = "Ladder Up";
        public const string LADDER_DOWN = "Ladder Down";
        public const string LADDER_IDLE = "Ladder Idle";
        public const string LADDER_SLIDE_DOWN = "Ladder Slide Down";
        public const string LADDER_SLIDE_UP = "Ladder Slide Stop";
        public const string LADDER_TRANSITION_TO_TOP = "Ladder Transition to Top";

        public const string ROPE_UP = "Rope Up";
        public const string ROPE_DOWN = "Rope Down";
        public const string ROPE_IDLE = "Rope Idle";
        public const string ROPE_SWING_FORWARD = "Rope Swing Forward";
        public const string ROPE_SWING_BACKWARD = "Rope Swing Backward";
        public const string ROPE_SWING_LEFT = "Rope Swing Left";
        public const string ROPE_SWING_RIGHT = "Rope Swing Right";
        public const string ROPE_TURN_LEFT = "Rope Turn Left";
        public const string ROPE_TURN_RIGHT = "Rope Turn Right";
        public const string ROPE_SLIDE_DOWN = "Rope Slide Down";
        public const string ROPE_SLIDE_STOP = "Rope Slide Stop";
        public const string ROPE_TRANSITION_TO_TOP = " Transition to Top";

        public override List<string> DefaultNames => new List<string>()
        {
            CLIMB_SURFACE_UP,
            CLIMB_SURFACE_DOWN,
            CLIMB_SURFACE_RIGHT,
            CLIMB_SURFACE_LEFT,
            CLIMB_SURFACE_IDLE,
            CLIMB_SURFACE_SLIP,
            CLIMB_SURFACE_SLIDE_DOWN,
            CLIMB_SURFACE_SURFACE_GRIP, 
            CLIMB_SURFACE_TRANSITION_TO_TOP,
            LADDER_UP,
            LADDER_DOWN,
            LADDER_IDLE, 
            LADDER_SLIDE_DOWN,
            LADDER_SLIDE_UP,
            LADDER_TRANSITION_TO_TOP,
            ROPE_UP,
            ROPE_DOWN, 
            ROPE_IDLE,
            ROPE_SWING_FORWARD,
            ROPE_SWING_BACKWARD,
            ROPE_SWING_LEFT,
            ROPE_SWING_RIGHT,
            ROPE_TURN_LEFT,
            ROPE_TURN_RIGHT,
            ROPE_SLIDE_DOWN,
            ROPE_SLIDE_STOP,
            ROPE_TRANSITION_TO_TOP
        };
    }
}