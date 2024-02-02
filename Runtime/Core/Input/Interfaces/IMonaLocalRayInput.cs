using Mona.SDK.Core.Input.Enums;
using UnityEngine;

namespace Mona.SDK.Core.Input.Interfaces
{
    public interface IMonaLocalRayInput : IMonaLocalInput
    {
        public Ray Value { get; set; }
    }
}