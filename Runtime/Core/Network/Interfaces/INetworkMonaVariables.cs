using Mona.SDK.Core.Body;
using Mona.SDK.Core.State.Structs;

namespace Mona.SDK.Core.Network.Interfaces
{
    public interface INetworkMonaVariables
    {
        string LocalId { get; }
        string PrefabId { get; }
        int Index { get; }
        bool LocallyOwnedMonaBody { get; }

        void UpdateValue(IMonaVariablesValue value);
        void SetIdentifier(string localId, int i, string prefabId, bool locallyOwnedMonaBody);
        void SetMonaBody(MonaBody monaBody, int index);
    }
}