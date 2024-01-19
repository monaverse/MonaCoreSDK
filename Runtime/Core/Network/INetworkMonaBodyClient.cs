using UnityEngine;

namespace Mona
{
    public interface INetworkMonaBodyClient
    {
        float DeltaTime { get; }
        Transform NetworkTransform { get; }
        Rigidbody NetworkRigidbody { get; }
        string LocalId { get; }
        string PrefabId { get; }
        void SetActive(bool active);
        bool HasControl();
        void TakeControl();
        void ReleaseControl();
        void SetIdentifier(string localId, string prefabId, bool locallyOwnedMonaBody);
        void SetKinematic(bool isKinematic);
        void SetPosition(Vector3 position, bool isKinematic = false);
        void SetRotation(Quaternion rotation, bool isKinematic = false);
        void SetScale(Vector3 scale);
        void SetColor(Color color);
        void SetLayer(string layerName, bool includeChildren);
    }
}
