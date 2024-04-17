using UnityEngine;
using System;
using System.Collections.Generic;
using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Assets.Behaviours
{
    [Serializable]
    public class MonaAnimationAssetProviderBehaviour : MonoBehaviour, IMonaAssetProviderBehaviour
    {
        private MonaAnimationAssets _monaAsset;

        public IMonaAssetProvider MonaAssetProvider {
            get
            {
                if (_monaAsset == null)
                {
                    _monaAsset = new MonaAnimationAssets();
                }
                return _monaAsset;
            }
        }
    }
}