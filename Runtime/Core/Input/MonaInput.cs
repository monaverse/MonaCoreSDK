using Mona.SDK.Core.Input.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mona.SDK.Core.Input
{
    /* TODO: Candidate for refactor.
     * Input values change every frame.
     * Needed reference by value that didn't allocate memory.
     * */
    public struct MonaInput
    {
        public int PlayerId;

        public MonaInputState Move;
        public MonaInputState Look;
        public MonaInputState Jump;
        public MonaInputState Action;
        public MonaInputState Sprint;
        public MonaInputState SwitchCamera;
        public MonaInputState Respawn;
        public MonaInputState Debug;
        public MonaInputState ToggleUI;
        public MonaInputState EmoteWheel;
        public MonaInputState EmojiTray;
        public MonaInputState ToggleNametags;
        public MonaInputState Escape;
        public MonaInputState OpenChat;
        public MonaInputState ToggleMouseCapture;

        public override bool Equals(object obj)
        {
            var other = (MonaInput)obj;
            return Move.Equals(other.Move)
                && Look.Equals(other.Look)
                && Jump.Equals(other.Jump)
                && Action.Equals(other.Action)
                && Sprint.Equals(other.Sprint)
                && SwitchCamera.Equals(other.SwitchCamera)
                && Respawn.Equals(other.Respawn)
                && Debug.Equals(other.Debug)
                && ToggleUI.Equals(other.ToggleUI)
                && EmoteWheel.Equals(other.EmoteWheel)
                && EmojiTray.Equals(other.EmojiTray)
                && ToggleNametags.Equals(other.ToggleNametags)
                && Escape.Equals(other.Escape)
                && OpenChat.Equals(other.OpenChat)
                && ToggleMouseCapture.Equals(other.ToggleMouseCapture)
                && Key0.Equals(Key0)
                && Key1.Equals(Key1)
                && Key2.Equals(Key2)
                && Key3.Equals(Key3)
                && Key4.Equals(Key4)
                && Key5.Equals(Key5)
                && Key6.Equals(Key6)
                && Key7.Equals(Key7)
                && Key8.Equals(Key8)
                && Key9.Equals(Key9)
                && Key10.Equals(Key10)
                && MoveValue.Equals(other.MoveValue)
                && LookValue.Equals(other.LookValue)
                && Origin.Equals(other.Origin)
                && Direction.Equals(other.Direction);
        }

        public MonaInputState GetButton(MonaInputType type)
        {
            switch (type)
            {
                case MonaInputType.Move: return Move;
                case MonaInputType.Look: return Look;
                case MonaInputType.Jump: return Look;
                case MonaInputType.Action: return Action;
                case MonaInputType.Sprint: return Sprint;
                case MonaInputType.SwitchCamera: return SwitchCamera;
                case MonaInputType.Respawn: return Respawn;
                case MonaInputType.Debug: return Debug;
                case MonaInputType.ToggleUI: return ToggleUI;
                case MonaInputType.EmoteWheel: return EmoteWheel;
                case MonaInputType.EmojiTray: return EmojiTray;
                case MonaInputType.ToggleNametags: return ToggleNametags;
                case MonaInputType.Escape: return Escape;
                case MonaInputType.OpenChat: return OpenChat;
                case MonaInputType.ToggleMouseCapture: return ToggleMouseCapture;
            }
            return MonaInputState.None;
        }

        public MonaInputState Key0;
        public MonaInputState Key1;
        public MonaInputState Key2;
        public MonaInputState Key3;
        public MonaInputState Key4;
        public MonaInputState Key5;
        public MonaInputState Key6;
        public MonaInputState Key7;
        public MonaInputState Key8;
        public MonaInputState Key9;
        public MonaInputState Key10;

        public MonaInputState GetKey(int idx)
        {
            switch(idx)
            {
                case 0: return Key0;
                case 1: return Key1;
                case 2: return Key2;
                case 3: return Key3;
                case 4: return Key4;
                case 5: return Key5;
                case 6: return Key6;
                case 7: return Key7;
                case 8: return Key8;
                case 9: return Key9;
                case 10: return Key10;
            }
            return MonaInputState.None;
        }

        public Vector2 MoveValue;
        public Vector2 LookValue;
        public Vector3 Origin;
        public Vector3 Direction;
        public Ray Ray;
    }
}