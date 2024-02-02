using Mona.SDK.Core.Input.Enums;
using UnityEngine;

namespace Mona.SDK.Core.Input.Interfaces
{
    public interface IMonaLocalAxisInput : IMonaLocalInput
    {
        public Vector2 Value { get; set; }
    }
}