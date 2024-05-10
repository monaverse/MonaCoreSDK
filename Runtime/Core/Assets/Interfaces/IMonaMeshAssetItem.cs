using UnityEngine;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaMeshAssetItem : IMonaAssetItem
    {
        public Mesh Value { get; set; }
    }
}