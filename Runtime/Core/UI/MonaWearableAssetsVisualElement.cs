using Mona.SDK.Core.Assets;
using Mona.SDK.Core.Assets.Interfaces;

namespace Mona.SDK.Core.UIElements
{
    public class MonaWearableAssetsVisualElement : MonaAssetsVisualElement
    {
        protected override string GetHeader() => "Mona Wearable Assets";

        protected override IMonaAssetItem CreateValue()
        {
            return (IMonaAssetItem)new MonaWearableAsset();
        }
    }
}