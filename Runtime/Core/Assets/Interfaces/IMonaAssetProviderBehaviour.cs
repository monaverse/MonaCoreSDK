using Mona.SDK.Core.Body;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaAnimationAssetProviderBehaviour : IMonaAssetProviderBehaviour { }
    public interface IMonaAudioAssetProviderBehaviour : IMonaAssetProviderBehaviour { }
    public interface IMonaAvatarAssetProviderBehaviour : IMonaAssetProviderBehaviour { }
    public interface IMonaBodyAssetProviderBehaviour : IMonaAssetProviderBehaviour { }
    public interface IMonaMaterialAssetProviderBehaviour : IMonaAssetProviderBehaviour { }
    public interface IMonaTextureAssetProviderBehaviour : IMonaAssetProviderBehaviour { }
    public interface IMonaWearableAssetProviderBehaviour : IMonaAssetProviderBehaviour { }

    public interface IMonaAssetProviderBehaviour
    {
        IMonaAssetProvider MonaAssetProvider { get; }
    }
}