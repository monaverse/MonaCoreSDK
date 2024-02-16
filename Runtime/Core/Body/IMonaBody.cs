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

        void InitializeTags();
        void AddTag(string tag);
        void RemoveTag(string tag);

        void SetLayer(string layerName, bool includeChildren, bool isNetworked = true);
        void ResetLayer();
        void ApplyForce(Vector3 force, ForceMode forceMode, bool isNetworked = true);
        void MoveDirection(Vector3 direction, bool isKinematic = true, bool isNetworked = true);
        void SetPosition(Vector3 position, bool isKinematic = true, bool isNetworked = true);
        void AddPosition(Vector3 dir, bool isKinematic, bool isNetworked = true);
        void RotateAround(Vector3 direction, float angle, bool isKinematic, bool isNetowrked = true);
        //TODO void RotateTowards(Vector3 direction, float angle, bool isKinematic = false, bool isNetworked = true);
        void SetRotation(Quaternion rotation, bool isKinematic = true, bool isNetworked = true);
        void SetScale(Vector3 scale, bool isNetworked = true);
        void SetKinematic(bool isKinematic, bool isNetworked = true);
        void SetColor(Color color, bool isNetworked = true);
        void SetVisible(bool visible, bool isNetworked = true);
        void SetActive(bool active, bool isNetworked = true);
        void SetTransformParent(Transform parent);

        bool Intersects(SphereCollider collider);
        bool Intersects(Collider collider);


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
        void SetApplyPinOnGrounded(bool apply);

        void SetAnimator(Animator animator);

        Color GetColor();
        bool GetVisible();
        bool GetActive();
        Vector3 GetPosition();
        Quaternion GetRotation();
        Transform GetTransformParent();
        Vector3 GetScale();
        int GetLayer();

        IMonaBody FindChildByTag(string tag);
        Transform FindChildTransformByTag(string tag);
        List<IMonaBody> FindChildrenByTag(string tag);
        List<IMonaBody> Children();

        void RegisterAsChild(IMonaBody body);
        void UnregisterAsChild(IMonaBody body);

        bool HasMonaTag(string tag);

        void FixedUpdateNetwork(float deltaTime, bool hasInput, MonaInput input);

        bool HasControl();
        void TakeControl();
        void ReleaseControl();
        void Pause(bool isNetworked = true);
        void Resume(bool isNetworked = true);
    }
}