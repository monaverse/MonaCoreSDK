using System;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaVariablesVector3 : IMonaVariablesValue, IMonaVariablesVector3Value
    {
        public event Action OnChange = delegate { };

        public void Change() => OnChange();

        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public Vector3 _value;
        private Vector3 _resetValue;

        public Vector3 Value { get => _value; set => _value = value; }

        public MonaVariablesVector3() { }

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