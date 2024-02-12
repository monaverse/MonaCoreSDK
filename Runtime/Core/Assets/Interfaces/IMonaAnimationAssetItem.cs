using UnityEngine;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaAnimationAssetItem : IMonaAssetItem
    {
        public AnimationClip Value { get; set; }
    }
}