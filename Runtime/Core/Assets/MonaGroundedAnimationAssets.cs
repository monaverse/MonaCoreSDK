using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mona.SDK.Core.Assets
{   
    [Serializable]
    [CreateAssetMenu(menuName = "Mona Brains/Animation/Grounded Animation Assets")]
    public class MonaGroundedAnimationAssets : MonaAnimationAssets
    {
        public const string GROUND_CALM_IDLE = "Ground Calm Idle";
        public const string GROUND_CALM_FIDGET_A = "Ground Calm Fidget A";
        public const string GROUND_CALM_FIDGET_B = "Ground Calm Fidget B";
        public const string GROUND_CALM_FIDGET_C = "Ground Calm Fidget C";

        public const string GROUND_AGGRO_IDLE = "Ground Aggro Idle";
        public const string GROUND_AGGRO_FIDGET_A = "Ground Aggro Fidget A";
        public const string GROUND_AGGRO_FIDGET_B = "Ground Aggro Fidget B";
        public const string GROUND_AGGRO_FIDGET_C = "Ground Aggro Fidget C";

        public override List<string> DefaultNames => new List<string>()
        {
            GROUND_CALM_IDLE,
            GROUND_CALM_FIDGET_A,
            GROUND_CALM_FIDGET_A,
            GROUND_CALM_FIDGET_C,
            GROUND_AGGRO_IDLE,
            GROUND_AGGRO_FIDGET_A,
            GROUND_AGGRO_FIDGET_B,
            GROUND_AGGRO_FIDGET_C
        };
    }

}