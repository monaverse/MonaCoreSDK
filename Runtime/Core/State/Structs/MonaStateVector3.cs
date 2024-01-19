using System;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaStateVector3 : IMonaStateValue, IMonaStateVector3Value
    {
        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public Vector3 _value;

        public Vector3 Value { get => _value; set => _value = value; }

        public MonaStateVector3() { }
    }
}