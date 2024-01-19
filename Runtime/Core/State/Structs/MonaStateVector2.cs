using System;
using UnityEngine;

namespace Mona.Core.State.Structs
{
    [Serializable]
    public class MonaStateVector2 : IMonaStateValue, IMonaStateVector2Value
    {
        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public Vector2 _value;

        public Vector2 Value { get => _value; set => _value = value; }

        public MonaStateVector2() { }
    }
}