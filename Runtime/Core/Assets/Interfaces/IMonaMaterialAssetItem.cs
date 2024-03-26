using UnityEngine;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaMaterialAssetItem : IMonaAssetItem
    {
        public Material Value { get; set; }
    }
}