using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Fly Animation Assets")]
    public class MonaFlyAnimationAssetsDefinition : MonaAnimationAssetsDefinition
    {
        public MonaFlyAnimationAssetsDefinition()
        {
            _monaAsset = new MonaFlyAnimationAssets();
        }
    }
}