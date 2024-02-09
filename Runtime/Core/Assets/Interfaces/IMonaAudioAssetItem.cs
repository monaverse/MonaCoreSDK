using UnityEngine;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaAudioAssetItem : IMonaAssetItem
    {
        public AudioClip Value { get; set; }
    }
}