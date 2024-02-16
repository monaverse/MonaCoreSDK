using Mona.SDK.Core.Assets;
using Mona.SDK.Core.Assets.Interfaces;

namespace Mona.SDK.Core.UIElements
{
    public class MonaAnimationAssetsVisualElement : MonaAssetsVisualElement
    {
        protected override string GetHeader() => "Mona Animation Assets";

        protected override IMonaAssetItem CreateValue()
        {
            return (IMonaAssetItem)new MonaAnimationAsset();
        }
    }
}