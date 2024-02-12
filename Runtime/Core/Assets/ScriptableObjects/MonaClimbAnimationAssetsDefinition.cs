using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Climb Animation Assets")]
    public class MonaClimbAnimationAssetsDefinition : MonaAnimationAssetsDefinition
    {
        public MonaClimbAnimationAssetsDefinition()
        {
            _monaAsset = new MonaClimbAnimationAssets();
        }
    }
}