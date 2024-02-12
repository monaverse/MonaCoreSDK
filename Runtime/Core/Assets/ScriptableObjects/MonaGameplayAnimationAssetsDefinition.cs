using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Gameplay Animation Assets")]
    public class MonaGameplayAnimationAssetsDefinition : MonaAnimationAssetsDefinition
    {
        public MonaGameplayAnimationAssetsDefinition()
        {
            _monaAsset = new MonaGameplayAnimationAssets();
        }
    }
}