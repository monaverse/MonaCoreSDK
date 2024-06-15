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
        public MonaNetworkType _NetworkType = MonaNetworkType.None;
        public MonaNetworkType NetworkType { get => _NetworkType; set => _NetworkType = value; }

        public MonaNetworkType GetNetworkType() => NetworkType;
    }
}
