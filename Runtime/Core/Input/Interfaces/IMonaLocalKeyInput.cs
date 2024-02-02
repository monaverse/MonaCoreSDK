using Mona.SDK.Core.Input.Enums;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mona.SDK.Core.Input.Interfaces
{
    public interface IMonaLocalKeyInput : IMonaLocalInput
    {
        public Key Value { get; set; }
    }
}