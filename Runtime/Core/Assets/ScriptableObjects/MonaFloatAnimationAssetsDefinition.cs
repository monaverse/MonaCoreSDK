using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Float Animation Assets")]
    public class MonaFloatAnimationAssetsDefinition : MonaAnimationAssetsDefinition
    {
        public MonaFloatAnimationAssetsDefinition()
        {
            _monaAsset = new MonaFloatAnimationAssets();
        }
    }
}