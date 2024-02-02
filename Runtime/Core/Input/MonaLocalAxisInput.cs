using Mona.SDK.Core.Input.Enums;
using Mona.SDK.Core.Input.Interfaces;
using UnityEngine;

namespace Mona.SDK.Core.Input
{
    public struct MonaLocalAxisInput : IMonaLocalAxisInput
    {
        public MonaInputType Type { get; set; }
        public MonaInputState State { get; set; }
        public Vector2 Value { get; set; }
    }
}