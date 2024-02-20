using Mona.SDK.Core.Assets;
using Mona.SDK.Core.Assets.Interfaces;

namespace Mona.SDK.Core.UIElements
{
    public class MonaAvatarAssetsVisualElement : MonaAssetsVisualElement
    {
        protected override string GetHeader() => "Mona Avatar Assets";

        protected override IMonaAssetItem CreateValue()
        {
            return (IMonaAssetItem)new MonaAvatarAsset();
        }
    }
}