using Mona.SDK.Core.Network.Enums;
using Mona.SDK.Core.Network.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.Network
{
    [Serializable]
    public class MonaNetworkSettings : IMonaNetworkSettings
    {
        public MonaNetworkType NetworkType = MonaNetworkType.Shared;

        public MonaNetworkType GetNetworkType() => NetworkType;
    }
}
