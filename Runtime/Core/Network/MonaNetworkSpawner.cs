using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mona.SDK.Core.Network.Interfaces;
using Mona.SDK.Core.Body;
using Mona.SDK.Core.Assets.Interfaces;

public class MonaNetworkSpawner : MonoBehaviour, IMonaNetworkSpawner
{
    public virtual int LocalPlayerId => 0;

    public virtual void RegisterMonaBody(IMonaBody monaBody)
    {
        throw new System.NotImplementedException();
    }

    public virtual void RegisterMonaPrefabProvider(IMonaAssetProvider provider)
    {
        throw new System.NotImplementedException();
    }

    public virtual void RegisterMonaReactor(IMonaReactor reactor)
    {
        throw new System.NotImplementedException();
    }

    public virtual void RegisterNetworkMonaBody(INetworkMonaBodyClient monaBody)
    {
        throw new System.NotImplementedException();
    }

    public virtual void RegisterNetworkMonaReactor(INetworkMonaReactorClient reactor)
    {
        throw new System.NotImplementedException();
    }

    public virtual void RegisterNetworkMonaVariables(INetworkMonaVariables monaVariables)
    {
        throw new System.NotImplementedException();
    }

    public virtual void SetSpaceNetworkSettings(IMonaNetworkSettings settings)
    {
        throw new System.NotImplementedException();
    }

    public virtual void SpawnLocalMonaBody(INetworkMonaBodyClient monaBody)
    {
        throw new System.NotImplementedException();
    }

    public virtual void SpawnLocalMonaReactor(INetworkMonaReactorClient reactor)
    {
        throw new System.NotImplementedException();
    }
}
