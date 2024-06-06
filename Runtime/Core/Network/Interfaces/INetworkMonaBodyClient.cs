using Mona.SDK.Core.Input;
using UnityEngine;

namespace Mona.SDK.Core.Network.Interfaces
{
    public interface INetworkMonaBodyClient
    {
        float DeltaTime { get; }
        Transform NetworkTransform { get; }
        Rigidbody NetworkRigidbody { get; }
        string LocalId { get; }
        string PrefabId { get; }
        void SetLocalInput(MonaInput input);
        void SetActive(bool active);
        bool HasControl();
        void TakeControl();
        void ReleaseControl();
        void SetIdentifier(string localId, string prefabId, bool locallyOwnedMonaBody);
        void SetSyncPositionAndRigidbody(bool syncPositionAndRigidbody);
        void SetKinematic(bool isKinematic);
        void SetPosition(Vector3 position, bool isKinematic = false);
        void SetRotation(Quaternion rotation, bool isKinematic = false);
        void TeleportPosition(Vector3 position, bool isKinematic = false, bool setToLocal = false);
        void TeleportRotation(Quaternion rotation, bool isKinematic = false);
        void TeleportGlobalRotation(Vector3 axis, float value);
        void TeleportScale(Vector3 scale, bool isKinematic = false);
        void SetScale(Vector3 scale);
        void SetColor(Color color);
        void SetLayer(string layerName, bool includeChildren);
        void SetVisible(bool vis);
        void SetPaused(bool paused);
        void SetAnimator(Animator animator);
        void TriggerAnimation(string clipName);
        void SetPlayer(int playerId, int clientId, string name);
    }
}
