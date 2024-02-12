using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Grounded Animation Assets")]
    public class MonaGroundedAnimationAssetsDefinition : MonaAnimationAssetsDefinition
    {
        public MonaGroundedAnimationAssetsDefinition()
        {
            _monaAsset = new MonaGroundedAnimationAssets();
        }
    }
}