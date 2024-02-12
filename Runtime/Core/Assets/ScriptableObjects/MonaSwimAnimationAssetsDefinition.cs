using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Swim Animation Assets")]
    public class MonaSwimAnimationAssetsDefinition : MonaAnimationAssetsDefinition
    {
        public MonaSwimAnimationAssetsDefinition()
        {
            _monaAsset = new MonaSwimAnimationAssets();
        }
    }
}