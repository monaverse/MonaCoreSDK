using UnityEngine;

namespace Mona.SDK.Core.Network.Interfaces
{
    public interface INetworkMonaReactorClient
    {
        string localId { get; }
        string prefabId { get; }
        Transform ActiveTransform { get; }
        void SetIdentifier(string localId, string prefabId, bool locallyOwnedMonaBody);
        bool HasControl();
        void SetAnimationBool(int registryIndex, bool animatorValue);
        void SetAnimationFloat(int registryIndex, float animatorValue);
        void SetAnimationInt(int registryIndex, int animatorValue);
        void SetAnimationTrigger(int registryIndex);
    }
}