using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Body;
using System;
using UnityEngine;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaWearableAsset : IMonaWearableAssetItem
    {
        [SerializeField] private string _prefabId = "";
        public string PrefabId { get => _prefabId; set => _prefabId = value; }

        [SerializeField] private GameObject _wearable;
        public GameObject Value { get => _wearable; set => _wearable = value; }

        public MonaWearableAsset()
        {
        }
    }
}