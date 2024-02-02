using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Network.Interfaces
{
    public interface IMonaNetworkSpawner
    {
        int LocalPlayerId { get; }
        void SetSpaceNetworkSettings(IMonaNetworkSettings settings);
        void RegisterMonaReactor(IMonaReactor reactor);
        void RegisterMonaBody(IMonaBody monaBody);
        void RegisterNetworkMonaReactor(INetworkMonaReactorClient reactor);
        void RegisterNetworkMonaBody(INetworkMonaBodyClient monaBody);
        void RegisterMonaPrefabProvider(IMonaPrefabProvider provider);
        void SpawnLocalMonaReactor(INetworkMonaReactorClient reactor);
        void SpawnLocalMonaBody(INetworkMonaBodyClient monaBody);
    }
}