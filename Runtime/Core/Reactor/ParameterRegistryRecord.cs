using UnityEngine;
using System;

namespace Mona
{
    [Serializable]
    public struct ParameterRegistryRecord
    {
        public string name;
        public ValueType valueType;
        public Animator animator;
    }
 }