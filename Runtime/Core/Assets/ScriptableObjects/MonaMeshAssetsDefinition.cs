using UnityEngine;
using System;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Visuals/Mona Mesh Assets")]
    public class MonaMeshAssetsDefinition : MonaAssetsDefinition
    {
        public MonaMeshAssetsDefinition()
        {
            _monaAsset = new MonaMeshAssets();
        }
    }
}