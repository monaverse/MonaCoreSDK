using Mona.SDK.Core.Body;
using System;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaStateBody : IMonaStateValue, IMonaStateBodyValue
    {
        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public IMonaBody _value;

        public IMonaBody Value { get => _value; set => _value = value; }

        public MonaStateBody() { }

    }
}