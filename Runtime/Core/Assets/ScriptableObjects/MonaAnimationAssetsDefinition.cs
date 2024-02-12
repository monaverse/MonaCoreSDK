using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaAnimationAssetsDefinition : MonaAssetsDefinition
    {
        public MonaAnimationAssetsDefinition()
        {
            _monaAsset = new MonaAnimationAssets();
        }
    }  
}