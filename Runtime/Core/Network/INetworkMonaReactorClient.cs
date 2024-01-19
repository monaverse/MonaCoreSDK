using UnityEngine;

namespace Mona.Core.Network
{
    public interface INetworkMonaReactorClient
    {
        string localId { get; }
        string prefabId { get; }
        Transform activeTransform { get; }
        void SetIdentifier(string localId, string prefabId, bool locallyOwnedMonaBody);
        bool HasControl();
        void SetAnimationBool(int registryIndex, bool animatorValue);
        void SetAnimationFloat(int registryIndex, float animatorValue);
        void SetAnimationInt(int registryIndex, int animatorValue);
        void SetAnimationTrigger(int registryIndex);
    }
}