using UnityEngine;
using System;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Visuals/Mona Material Assets")]
    public class MonaMaterialAssetsDefinition : MonaAssetsDefinition
    {
        public MonaMaterialAssetsDefinition()
        {
            _monaAsset = new MonaMaterialAssets();
        }
    }
}