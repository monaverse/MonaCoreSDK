using System;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaStateBool : IMonaStateValue, IMonaStateBoolValue
    {
        public event Action OnChange = delegate { };

        public void Change() => OnChange();

        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public bool _value;

        public bool Value { get => _value; set => _value = value; }

        public MonaStateBool() { }
    }
}