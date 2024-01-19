﻿using System;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaStateString : IMonaStateValue, IMonaStateStringValue
    {
        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public string _value;

        public string Value { get => _value; set => _value = value; }

        public MonaStateString() { }
    }
}