﻿using System;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaStateFloat : IMonaStateValue, IMonaStateFloatValue
    {
        public event Action OnChange = delegate { };

        public void Change() => OnChange();

        [SerializeField]
        private string _name;

        public string Name { get => _name; set => _name = value; }

        [SerializeField]
        public float _value = 1f;

        public float Value { get => _value; set => _value = value; }

        public MonaStateFloat() { }
    }
}