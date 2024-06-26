﻿using Mona.SDK.Core.Body;
using UnityEngine;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaAvatarAssetItem : IMonaAssetItem
    {
        public GameObject Value { get; set; }
        public string Url { get; set; }
    }
}