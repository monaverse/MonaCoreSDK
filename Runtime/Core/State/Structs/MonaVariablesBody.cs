using Mona.SDK.Core.Body;
using System;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaVariablesBody : IMonaVariablesValue, IMonaVariablesBodyValue
    {
        public event Action OnChange = delegate { };

        public void Change() => OnChange();

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;

                hash = hash * 23 + (_name?.GetHashCode() ?? 0);
                hash = hash * 23 + _value?.GetHashCode() ?? 0;

                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IMonaVariablesValue other))
            {
                return false;
            }

            return Equals(other);
        }

        public bool Equals(IMonaVariablesValue other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public IMonaBody _value;

        private IMonaBody _resetValue;

        public IMonaBody Value { get => _value; set => _value = value; }

        public MonaVariablesBody() { }

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