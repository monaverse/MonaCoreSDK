using Mona.SDK.Core.Assets;
using Mona.SDK.Core.Assets.Interfaces;
using UnityEngine.UIElements;

namespace Mona.SDK.Core.UIElements
{
    public class MonaTextureAssetsVisualElement : MonaAssetsVisualElement
    {
        protected override string GetHeader() => "Mona Texture Assets";

        protected override IMonaAssetItem CreateValue()
        {
            return (IMonaAssetItem)new MonaTextureAsset();
        }
    }
}