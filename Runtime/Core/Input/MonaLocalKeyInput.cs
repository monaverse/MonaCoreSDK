using Mona.SDK.Core.Input.Enums;
using Mona.SDK.Core.Input.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mona.SDK.Core.Input
{
    public struct MonaLocalKeyInput : IMonaLocalKeyInput
    {
        public MonaInputType Type { get; set; }
        public MonaInputState State { get; set; }
        public Key Value { get; set; }
    }
}