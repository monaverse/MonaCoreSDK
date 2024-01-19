using Mona.Core.State.Structs;
using UnityEngine;

namespace Mona.Core.Network
{
    public interface INetworkMonaState
    {
        void Update(IMonaStateValue value);
    }
}