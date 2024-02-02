using Mona.SDK.Core.Input.Enums;
using Mona.SDK.Core.Input.Interfaces;
using UnityEngine;

namespace Mona.SDK.Core.Input
{
    public struct MonaLocalRayInput : IMonaLocalRayInput
    {
        public MonaInputType Type { get; set; }
        public MonaInputState State { get; set; }
        public Ray Value { get; set; }
    }
}