using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Standard Animation Assets")]
    public class MonaStandardAnimationAssetsDefinition : MonaAnimationAssetsDefinition
    {
        public MonaStandardAnimationAssetsDefinition()
        {
            _monaAsset = new MonaStandardAnimationAssets();
        }
    }
}