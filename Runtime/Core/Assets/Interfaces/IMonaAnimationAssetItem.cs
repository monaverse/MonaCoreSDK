using UnityEngine;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaAnimationAssetItem : IMonaAssetItem
    {
        public AnimationClip Value { get; set; }
        public int Layer { get; set; }
        public float LayerWeight { get; set; }
    }
}