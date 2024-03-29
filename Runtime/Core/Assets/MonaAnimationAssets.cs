using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaAnimationAssets : MonaAssets
    {
        public const string TPOSE = "TPose";
        public const string AIM = "Aim";

        public override List<string> DefaultNames => new List<string>()
        {
            TPOSE,
            AIM
        };
    }
}