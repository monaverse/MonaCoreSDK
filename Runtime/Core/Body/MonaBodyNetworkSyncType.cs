using System;

namespace Mona.SDK.Core.Body
{
    [Serializable]
    public enum MonaBodyNetworkSyncType
    {
        NotNetworked,
        NetworkTransform,
        NetworkRigidbody
    }

#if UNITY_EDITOR

#endif
}
