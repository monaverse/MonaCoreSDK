using UnityEngine;
using System;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Audio/Mona Audio Assets")]
    public class MonaAudioAssetsDefinition : MonaAssetsDefinition
    {
        public MonaAudioAssetsDefinition()
        {
            _monaAsset = new MonaAudioAssets();
        }
    }
}