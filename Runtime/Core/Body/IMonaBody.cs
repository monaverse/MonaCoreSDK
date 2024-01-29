using Mona.SDK.Core.Body.Enums;
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

        void SetLayer(string layerName, bool includeChildren, bool isNetworked = true);
        void ResetLayer();
        void ApplyForce(Vector3 force, ForceMode forceMode, bool isNetworked = true);
        void MoveDirection(Vector3 direction, bool isKinematic = true, bool isNetworked = true);
        void SetPosition(Vector3 position, bool isKinematic = true, bool isNetworked = true);
        void RotateAround(Vector3 direction, float angle, bool isKinematic, bool isNetowrked = true);
        void RotateTowards(Vector3 direction, float angle, bool isKinematic = false, bool isNetworked = true);
        void SetRotation(Quaternion rotation, bool isKinematic = true, bool isNetworked = true);
        void SetScale(Vector3 scale, bool isNetworked = true);
        void SetKinematic(bool isKinematic, bool isNetworked = true);
        void SetColor(Color color, bool isNetworked = true);
        void SetVisible(bool visible, bool isNetworked = true);
        void SetActive(bool active, bool isNetworked = true);
        void SetParent(Transform parent);

        void SetDragType(DragType dragType);
        void SetDrag(float drag);
        void SetAngularDrag(float drag);
        void SetVelocity(Vector3 velocity);
        void SetAngularVelocity(Vector3 velocity);
        void SetFriction(float friction);
        void SetBounce(float bounce);
        void SetUseGravity(bool useGravity);
        void SetOnlyApplyDragWhenGrounded(bool apply);

        Color GetColor();
        bool GetVisible();
        Vector3 GetPosition();
        Quaternion GetRotation();
        Transform GetParent();

        IMonaBody FindChildByTag(string tag);
        Transform FindChildTransformByTag(string tag);
        List<IMonaBody> FindChildrenByTag(string tag);
        List<IMonaBody> Children();
        bool HasMonaTag(string tag);

        bool HasControl();
        void TakeControl();
        void ReleaseControl();
        void Pause();
        void Resume();
    }
}