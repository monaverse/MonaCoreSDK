using Mona.SDK.Core.Assets;
using Mona.SDK.Core.Assets.Interfaces;
using UnityEngine.UIElements;

namespace Mona.SDK.Core.UIElements
{
    public class MonaMaterialAssetsVisualElement : MonaAssetsVisualElement
    {
        protected override string GetHeader() => "Mona Material Assets";

        protected override IMonaAssetItem CreateValue()
        {
            return (IMonaAssetItem)new MonaMaterialAsset();
        }
    }
}