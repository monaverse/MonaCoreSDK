using Mona.SDK.Core.Body.Enums;
using Mona.SDK.Core.Input;
using Mona.SDK.Core.Network.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.Body
{
    public interface IMonaBody
    {
        event Action OnStarted;
        event Action<IMonaBody> OnDisabled;
        event Action OnResumed;
        event Action OnPaused;

        string LocalId { get; }
        Transform ActiveTransform { get; }
        Rigidbody ActiveRigidbody { get; }
        Transform Transform { get; }
        List<string> MonaTags { get; }
        Camera Camera { get; }
        bool LocalOnly { get; }
        INetworkMonaBodyClient NetworkBody { get; }
        IMonaBody Parent { get; }
        bool Grounded { get; }
        Animator Animator { get; }
        MonaBodyAttachType AttachType { get; set; }

        Vector3 InitialPosition { get; }
        Vector3 InitialLocalPosition { get; }
        Quaternion InitialRotation { get; }
        Quaternion InitialLocalRotation { get; }
        Vector3 InitialScale { get; }

        MonaBodyTransformBounds PositionBounds { get; set; }
        MonaBodyTransformBounds RotationBounds { get; set; }

        void AddRigidbody();
        void RemoveRigidbody();
        bool HasCollider();
        void AddCollider();

        bool IsAttachedToRemotePlayer();
        bool IsAttachedToLocalPlayer();

        void CacheColliders();
        void InitializeTags();
        void AddTag(string tag);
        void RemoveTag(string tag);

        void SetLayer(string layerName, bool includeChildren, bool isNetworked = true);
        void ResetLayer();
        void ApplyForce(Vector3 force, ForceMode forceMode, bool isNetworked = true);
        void MoveDirection(Vector3 direction, bool isNetworked = true);
        void SetPosition(Vector3 position, bool isNetworked = true);
        void AddPosition(Vector3 dir, bool isNetworked = true);
        void RotateAround(Vector3 direction, float angle, bool isNetowrked = true);
        //TODO void RotateTowards(Vector3 direction, float angle, bool isKinematic = false, bool isNetworked = true);
        void SetRotation(Quaternion rotation, bool isNetworked = true);

        void BindPosition();
        void BindRotation();

        void TeleportPosition(Vector3 pos, bool isNetworked = true);
        void TeleportRotation(Quaternion rot, bool isNetworked = true);
        void TeleportScale(Vector3 scale, bool isNetworked = true);

        void SetScale(Vector3 scale, bool isNetworked = true);
        void SetKinematic(bool isKinematic, bool isNetworked = true);
        void SetColor(Color color, bool isNetworked = true);
        void SetVisible(bool visible, bool isNetworked = true);
        void SetActive(bool active, bool isNetworked = true);
        void SetTransformParent(Transform parent);

        bool Intersects(SphereCollider collider, bool includeTriggers = false);
        bool Intersects(Collider collider, bool includeTriggers = false);

        bool WithinRadius(IMonaBody body, float radius, bool includeTriggers = false);


        void SetLocalInput(MonaInput input);

        void SetDragType(DragType dragType);
        void SetDrag(float drag);
        void SetAngularDrag(float drag);
        void SetVelocity(Vector3 velocity);
        void SetAngularVelocity(Vector3 velocity);
        void SetFriction(float friction);
        void SetBounce(float bounce);
        void SetUseGravity(bool useGravity);
        void SetOnlyApplyDragWhenGrounded(bool apply);

        void SetAnimator(Animator animator);

        Color GetColor();
        bool GetVisible();
        bool GetActive();
        Vector3 GetPosition();
        Vector3 GetCenter();
        Quaternion GetRotation();
        Transform GetTransformParent();
        Vector3 GetScale();
        Vector3 GetVelocity();
        int GetLayer();

        IMonaBody FindChildByTag(string tag);
        Transform FindChildTransformByTag(string tag);
        List<IMonaBody> FindChildrenByTag(string tag);
        List<IMonaBody> Children();

        void RegisterAsChild(IMonaBody body);
        void UnregisterAsChild(IMonaBody body);

        bool HasMonaTag(string tag);

        void FixedUpdateNetwork(float deltaTime, bool hasInput, List<MonaInput> inputs);

        bool HasControl();
        void TakeControl();
        void ReleaseControl();
        void Pause(bool isNetworked = true);
        void Resume(bool isNetworked = true);

        void TriggerRemoteAnimation(string clipName);
    }
}