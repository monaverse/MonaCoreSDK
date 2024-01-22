using Mona.SDK.Core.Body;
using Mona.SDK.Core.Events;
using Mona.SDK.Core.Network;
using Mona.SDK.Core.State.Structs;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Mona.SDK.Core.State
{
    [Serializable]
    public class MonaState : IMonaState
    {
        protected INetworkMonaState _networkState;
        protected IMonaBody _monaBody;

        public MonaState(GameObject gameObject = null)
        {
            SetGameObject(gameObject);
        }

        public void SetGameObject(GameObject gameObject)
        { 
            if (gameObject == null) return;
            _monaBody = gameObject.GetComponent<IMonaBody>();
            if (_monaBody == null)
                _monaBody = gameObject.AddComponent<MonaBody>();
        }

        [SerializeReference]
        protected List<IMonaStateValue> _values = new List<IMonaStateValue>();

        public List<IMonaStateValue> Values { get => _values; set => _values = value; }

        public IMonaStateValue GetValue(string name)
        {
            for (var i = 0; i < _values.Count; i++)
            {
                if (_values[i] != null && _values[i].Name == name)
                    return _values[i];
            }
            return null;
        }

        public IMonaStateValue GetValue(string name, Type type)
        {
            var value = GetValue(name);
            if (value != null) return value;
            
            var newValue = (IMonaStateValue)Activator.CreateInstance(type);
            newValue.Name = name;
            _values.Add(newValue);
            return newValue;
        }

        public IMonaStateValue CreateValue(string name, Type type, int i)
        {
            var prop = (IMonaStateValue)Activator.CreateInstance(type);
            prop.Name = name;
            _values[i] = prop;
            return prop;
        }

        public void Set(string name, IMonaBody value)
        {
            var prop = GetValue(name, typeof(MonaStateBody));
            var propValue = ((IMonaStateBodyValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
        }

        public IMonaBody GetBody(string name)
        {
            var prop = GetValue(name, typeof(MonaStateBody));
            return ((IMonaStateBodyValue)prop).Value;
        }

        public void Set(string name, bool value, bool isNetworked = true)
        {
            var prop = GetValue(name, typeof(MonaStateBool));
            var propValue = ((IMonaStateBoolValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public bool GetBool(string name)
        {
            var prop = GetValue(name, typeof(MonaStateBool));
            return ((IMonaStateBoolValue)prop).Value;
        }

        public void Set(string name, float value, bool isNetworked = true)
        {
            var prop = GetValue(name, typeof(MonaStateFloat));
            var propValue = ((IMonaStateFloatValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public float GetFloat(string name)
        {
            var prop = GetValue(name, typeof(MonaStateFloat));
            return ((IMonaStateFloatValue)prop).Value;
        }

        public void Set(string name, int value, bool isNetworked = true)
        {
            var prop = GetValue(name, typeof(MonaStateInt));
            var propValue = ((IMonaStateIntValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public int GetInt(string name)
        {
            var prop = GetValue(name, typeof(MonaStateInt));
            return ((IMonaStateIntValue)prop).Value;
        }

        public void Set(string name, string value, bool isNetworked = true)
        {
            var prop = GetValue(name, typeof(MonaStateString));
            var propValue = ((IMonaStateStringValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public string GetString(string name)
        {
            var prop = GetValue(name, typeof(MonaStateString));
            return ((IMonaStateStringValue)prop).Value;
        }

        public void Set(string name, Vector2 value, bool isNetworked = true)
        {
            var prop = GetValue(name, typeof(MonaStateVector2));
            var propValue = ((IMonaStateVector2Value)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public Vector2 GetVector2(string name)
        {
            var prop = GetValue(name, typeof(MonaStateFloat));
            return ((IMonaStateVector2Value)prop).Value;
        }

        public void Set(string name, Vector3 value, bool isNetworked = true)
        {
            var prop = GetValue(name, typeof(MonaStateVector3));
            var propValue = ((IMonaStateVector3Value)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public Vector3 GetVector3(string name)
        {
            var prop = GetValue(name, typeof(MonaStateVector3));
            return ((IMonaStateVector3Value)prop).Value;
        }

        public void SetNetworkState(INetworkMonaState state)
        {
            _networkState = state;
            SyncValuesOnNetwork();
        }

        public void SyncValuesOnNetwork()
        {
            if (_networkState == null) return;

            for (var i = 0; i < _values.Count; i++)
                _networkState.UpdateValue(_values[i]);
        }

        protected virtual void FireValueEvent(string variableName, IMonaStateValue value)
        {
            EventBus.Trigger<MonaValueChangedEvent>(new EventHook(MonaCoreConstants.VALUE_CHANGED_EVENT, _monaBody), new MonaValueChangedEvent(variableName, value));
        }
    }
}