using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Events
{
    public struct MonaAssetProviderRemovedEvent
    {
        public IMonaAssetProvider AssetProvider;
        public MonaAssetProviderRemovedEvent(IMonaAssetProvider assetProvider)
        {
            AssetProvider = assetProvider;
        }
    }
}