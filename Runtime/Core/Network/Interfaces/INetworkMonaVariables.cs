using Mona.SDK.Core.State.Structs;

namespace Mona.SDK.Core.Network.Interfaces
{
    public interface INetworkMonaVariables
    {
        void UpdateValue(IMonaVariablesValue value);
    }
}