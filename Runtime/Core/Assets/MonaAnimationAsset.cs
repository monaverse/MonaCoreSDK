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

        [SerializeField] private int _layer = 0;
        public int Layer { get => _layer; set => _layer = value; }

        [SerializeField] private float _layerWeight = 0f;
        public float LayerWeight { get => _layerWeight; set => _layerWeight = value; }
    }
}