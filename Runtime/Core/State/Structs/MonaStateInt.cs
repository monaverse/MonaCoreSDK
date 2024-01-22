using System;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaStateInt : IMonaStateValue, IMonaStateIntValue
    {
        public event Action OnChange = delegate { };

        public void Change() => OnChange();

        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public int _value;

        public int Value { get => _value; set => _value = value; }

        public MonaStateInt() { }
    }
}