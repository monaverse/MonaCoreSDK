using Mona.SDK.Core.Assets;
using Mona.SDK.Core.Assets.Interfaces;
using UnityEngine.UIElements;

namespace Mona.SDK.Core.UIElements
{
    public class MonaMeshAssetsVisualElement : MonaAssetsVisualElement
    {
        protected override string GetHeader() => "Mona Mesh Assets";

        protected override IMonaAssetItem CreateValue()
        {
            return (IMonaAssetItem)new MonaMeshAsset();
        }
    }
}