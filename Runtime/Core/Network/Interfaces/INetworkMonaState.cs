using Mona.SDK.Core.State.Structs;

namespace Mona.SDK.Core.Network.Interfaces
{
    public interface INetworkMonaState
    {
        void UpdateValue(IMonaStateValue value);
    }
}