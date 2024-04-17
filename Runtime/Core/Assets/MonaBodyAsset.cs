using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Body;
using System;
using UnityEngine;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaBodyAsset : IMonaBodyAssetItem
    {
        [SerializeField] private string _prefabId = "";
        public string PrefabId { get => _prefabId; set => _prefabId = value; }

        [SerializeField] private MonaBody _monaBody;
        public MonaBody Value { get => _monaBody; set => _monaBody = value; }

        [SerializeField] private string _monaBodyUrl;
        public string Url { get => _monaBodyUrl; set => _monaBodyUrl = value; }

        public MonaBodyAsset()
        {
        }
    }
}