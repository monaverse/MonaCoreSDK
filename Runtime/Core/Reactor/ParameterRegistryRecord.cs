using UnityEngine;
using System;

namespace Mona
{
    public partial class MonaReactor
    {
        [Serializable]
        public struct ParameterRegistryRecord
        {
            public string name;
            public ValueType valueType;
            public Animator animator;
        }
    }
}