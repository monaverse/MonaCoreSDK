using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Emote Animation Assets")]
    public class MonaEmoteAnimationAssetsDefinition : MonaAnimationAssetsDefinition
    {
        public MonaEmoteAnimationAssetsDefinition()
        {
            _monaAsset = new MonaEmoteAnimationAssets();
        }
    }
}