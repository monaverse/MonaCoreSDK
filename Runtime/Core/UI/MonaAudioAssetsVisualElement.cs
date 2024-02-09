using Mona.SDK.Core.Assets;
using Mona.SDK.Core.Assets.Interfaces;
using UnityEngine.UIElements;

namespace Mona.SDK.Core.UIElements
{
    public class MonaAudioAssetsVisualElement : MonaAssetsVisualElement
    {
        protected override string GetHeader() => "Mona Audio Assets";

        protected override IMonaAssetItem CreateValue()
        {
            return (IMonaAssetItem)new MonaAudioAsset();
        }
    }
}