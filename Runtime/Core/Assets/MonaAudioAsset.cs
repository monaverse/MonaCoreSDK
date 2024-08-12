using Mona.SDK.Core.Assets.Interfaces;
using System;
using UnityEngine;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaAudioAsset : IMonaAudioAssetItem
    {
        [SerializeField] private string _prefabId = "";
        public string PrefabId { get => _prefabId; set => _prefabId = value; }

        [SerializeField] private AudioClip _audioClip;
        public AudioClip Value { get => _audioClip; set => _audioClip = value; }

        [SerializeField] private string _url;
        public string Url { get => _url; set => _url = value; }
    }
}