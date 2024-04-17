using UnityEngine;
using System;
using System.Collections.Generic;
using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Assets.Behaviours
{
    [Serializable]
    public class MonaBodyAssetProviderBehaviour : MonoBehaviour, IMonaAssetProviderBehaviour
    {
        private MonaBodyAssets _monaAsset;

        public IMonaAssetProvider MonaAssetProvider {
            get
            {
                if (_monaAsset == null)
                {
                    _monaAsset = new MonaBodyAssets();
                    IndexChildren();
                }
                return _monaAsset;
            }
        }

        private void IndexChildren()
        {
            var list = new List<IMonaAssetItem>();
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var body = child.GetComponent<MonaBody>();
                var asset = new MonaBodyAsset();
                asset.PrefabId = child.name;
                asset.Value = body;
                list.Add(asset);
            }
            _monaAsset.AllAssets.Clear();
            _monaAsset.AllAssets.AddRange(list);
        }
    }
}