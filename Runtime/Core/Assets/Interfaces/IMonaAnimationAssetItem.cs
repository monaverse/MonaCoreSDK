using UnityEngine;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaAnimationAssetItem : IMonaAssetItem
    {
        public Animation Value { get; set; }
    }
}