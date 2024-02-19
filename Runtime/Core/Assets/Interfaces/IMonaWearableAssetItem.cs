using Mona.SDK.Core.Body;
using UnityEngine;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaWearableAssetItem : IMonaAssetItem
    {
        public GameObject Value { get; set; }
    }
}