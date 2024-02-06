using System;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaVariablesVector2 : IMonaVariablesValue, IMonaVariablesVector2Value
    {
        public event Action OnChange = delegate { };

        public void Change() => OnChange();

        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public Vector2 _value;

        public Vector2 Value { get => _value; set => _value = value; }

        public MonaVariablesVector2() { }
    }
}