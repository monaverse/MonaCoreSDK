﻿using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Network
{
    public interface IMonaNetworkSpawner
    {
        int LocalPlayerId { get; }
        void RegisterMonaReactor(IMonaReactor reactor);
        void RegisterMonaBody(IMonaBody monaBody);
        void RegisterNetworkMonaReactor(INetworkMonaReactorClient reactor);
        void RegisterNetworkMonaBody(INetworkMonaBodyClient monaBody);
        void RegisterMonaPrefabProvider(IMonaPrefabProvider provider);
        void SpawnLocalMonaReactor(INetworkMonaReactorClient reactor);
        void SpawnLocalMonaBody(INetworkMonaBodyClient monaBody);
    }
}