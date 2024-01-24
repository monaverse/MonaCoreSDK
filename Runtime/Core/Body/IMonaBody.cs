using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.Body
{
    public interface IMonaBody
    {
        event Action OnStarted;

        string LocalId { get; }
        Transform ActiveTransform { get; }
        Rigidbody ActiveRigidbody { get; }
        Transform Transform { get; }
        List<string> MonaTags { get; }
        Camera Camera { get; }
        bool LocalOnly { get; }
        INetworkMonaBodyClient NetworkBody { get; }

        void SetLayer(string layerName, bool includeChildren, bool isNetworked = true);
        void MoveDirection(Vector3 direction, bool isKinematic = true, bool isNetworked = true);
        void SetPosition(Vector3 position, bool isKinematic = true, bool isNetworked = true);
        void RotateAround(Vector3 direction, float angle, bool isKinematic, bool isNetowrked = true);
        void RotateTowards(Vector3 direction, float angle, bool isKinematic = false, bool isNetworked = true);
        void SetRotation(Quaternion rotation, bool isKinematic = true, bool isNetworked = true);
        void SetScale(Vector3 scale, bool isNetworked = true);
        void SetKinematic(bool isKinematic, bool isNetworked = true);
        void SetColor(Color color, bool isNetworked = true);

        Color GetColor();
        Vector3 GetPosition();
        Quaternion GetRotation();

        IMonaBody FindChildByTag(string tag);
        Transform FindChildTransformByTag(string tag);
        List<IMonaBody> FindChildrenByTag(string tag);
        bool HasMonaTag(string tag);

        bool HasControl();
        void TakeControl();
        void ReleaseControl();
    }
}