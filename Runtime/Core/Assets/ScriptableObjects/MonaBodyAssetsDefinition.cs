using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Prefabs/Mona Body Assets")]
    public class MonaBodyAssetsDefinition : MonaAssetsDefinition
    {
        public MonaBodyAssetsDefinition()
        {
            _monaAsset = new MonaBodyAssets();
        }
    }
}