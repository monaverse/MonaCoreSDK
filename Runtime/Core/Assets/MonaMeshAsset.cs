using Mona.SDK.Core.Assets.Interfaces;
using System;
using UnityEngine;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaMeshAsset : IMonaMeshAssetItem
    {
        [SerializeField] private string _prefabId = "";
        public string PrefabId { get => _prefabId; set => _prefabId = value; }

        [SerializeField] private Mesh _mesh;
        public Mesh Value { get => _mesh; set => _mesh = value; }
    }
}