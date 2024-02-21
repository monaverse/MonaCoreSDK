using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Events
{
    public struct MonaAssetProviderAddedEvent
    {
        public IMonaAssetProvider AssetProvider;
        public MonaAssetProviderAddedEvent(IMonaAssetProvider assetProvider)
        {
            AssetProvider = assetProvider;
        }
    }
}