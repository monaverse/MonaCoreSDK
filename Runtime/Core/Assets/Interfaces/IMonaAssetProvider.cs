using Mona.SDK.Core.Body;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaAssetProvider
    {
        List<IMonaAssetItem> AllAssets { get; }
        List<string> DefaultNames { get; }
        List<string> AllNames { get;  }
        IMonaAssetItem GetMonaAsset(string prefabId);
        IMonaAssetItem GetMonaAssetByIndex(int index);
        IMonaAssetItem CreateAsset(string name, Type type, int i);
    }
}