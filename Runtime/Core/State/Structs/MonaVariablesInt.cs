using System;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaVariablesInt : IMonaVariablesValue, IMonaVariablesIntValue
    {
        public event Action OnChange = delegate { };

        public void Change() => OnChange();

        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public int _value;
        private int _resetValue;

        public int Value { get => _value; set => _value = value; }

        public MonaVariablesInt() { }

        public void Reset()
        {
            _value = _resetValue;
        }

        public void SaveReset()
        {
            _resetValue = _value;
        }
    }
}