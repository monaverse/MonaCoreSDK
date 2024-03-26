using UnityEngine;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaTextureAssetItem : IMonaAssetItem
    {
        public Texture Value { get; set; }
    }
}