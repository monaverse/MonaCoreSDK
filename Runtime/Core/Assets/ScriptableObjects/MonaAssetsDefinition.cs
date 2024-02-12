using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mona.SDK.Core.Body;
using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Events;
using Unity.VisualScripting;
using Mona.SDK.Core.Network.Interfaces;

namespace Mona.SDK.Core.Assets
{   
    [Serializable]
    public class MonaAssetsDefinition : ScriptableObject
    {
        [SerializeReference] protected IMonaAssetProvider _monaAsset;

        public MonaAssetsDefinition()
        {
            _monaAsset = new MonaAssets();
        }

        public IMonaAssetProvider MonaAsset {
            get
            {
                _monaAsset.Name = name;
                return _monaAsset;
            }
        }
    }
}