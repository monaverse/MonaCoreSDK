using System;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaVariablesString : IMonaVariablesValue, IMonaVariablesStringValue
    {
        public event Action OnChange = delegate { };

        public void Change() => OnChange();

        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public string _value;
        private string _resetValue;

        public string Value { get => _value == null ? "" : _value; set => _value = value; }

        public MonaVariablesString() { }

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