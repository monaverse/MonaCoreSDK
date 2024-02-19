using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Prefabs/Mona Wearable Assets")]
    public class MonaWearableAssetsDefinition : MonaAssetsDefinition
    {
        public MonaWearableAssetsDefinition()
        {
            _monaAsset = new MonaWearableAssets();
        }
    }
}