using Mona.SDK.Core.State.Structs;
using UnityEngine;

namespace Mona.SDK.Core.Network
{
    public interface INetworkMonaState
    {
        void UpdateValue(IMonaStateValue value);
    }
}