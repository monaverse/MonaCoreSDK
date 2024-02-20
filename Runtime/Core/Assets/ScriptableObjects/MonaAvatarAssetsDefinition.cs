using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Prefabs/Mona Avatar Assets")]
    public class MonaAvatarAssetsDefinition : MonaAssetsDefinition
    {
        public MonaAvatarAssetsDefinition()
        {
            _monaAsset = new MonaAvatarAssets();
        }
    }
}