using System;

namespace Mona.Core.Body
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
