using Mona.SDK.Core.Assets;
using Mona.SDK.Core.Assets.Interfaces;
using UnityEngine.UIElements;

namespace Mona.SDK.Core.UIElements
{
    public class MonaBodyAssetsVisualElement : MonaAssetsVisualElement
    {
        protected override string GetHeader() => "Mona Body Prefab Assets";

        protected override IMonaAssetItem CreateValue()
        {
            return (IMonaAssetItem)new MonaBodyAsset();
        }
    }
}