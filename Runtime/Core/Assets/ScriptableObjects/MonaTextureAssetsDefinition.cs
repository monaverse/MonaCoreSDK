using UnityEngine;
using System;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Visuals/Mona Texture Assets")]
    public class MonaTextureAssetsDefinition : MonaAssetsDefinition
    {
        public MonaTextureAssetsDefinition()
        {
            _monaAsset = new MonaTextureAssets();
        }
    }
}