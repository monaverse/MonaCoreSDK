using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Body;
using System;
using UnityEngine;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaAvatarAsset : IMonaAvatarAssetItem
    {
        [SerializeField] private string _prefabId = "";
        public string PrefabId { get => _prefabId; set => _prefabId = value; }

        [SerializeField] private GameObject _avatar;
        public GameObject Value { get => _avatar; set => _avatar = value; }

        [SerializeField] private string _avatarUrl;
        public string Url { get => _avatarUrl; set => _avatarUrl = value; }

        public MonaAvatarAsset()
        {
        }
    }
}