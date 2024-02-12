using Mona.SDK.Core.Assets.Interfaces;
using System;
using UnityEngine;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaAnimationAsset : IMonaAnimationAssetItem
    {
        [SerializeField] private string _prefabId = "";
        public string PrefabId { get => _prefabId; set => _prefabId = value; }

        [SerializeField] private AnimationClip _animation;
        public AnimationClip Value { get => _animation; set => _animation = value; }
    }
}