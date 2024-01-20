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

        public IMonaStateValue GetValue(string name, Type type)
        {
            for(var i = 0;i < _values.Count; i++)
            {
                if (_values[i] != null && _values[i].Name == name)
                    return _values[i];
            }
            
            var value = (IMonaStateValue)Activator.CreateInstance(type);
                value.Name = name;
            _values.Add(value);
            return value;
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
                FireBodyEvent(name, propValue.Value);
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
                FireBoolEvent(name, propValue.Value);
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
                FireFloatEvent(name, propValue.Value);
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
                FireIntEvent(name, propValue.Value);
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
                FireStringEvent(name, propValue.Value);
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
                FireVector2Event(name, propValue.Value);
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
                FireVector3Event(name, propValue.Value);
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

        protected IMonaBody _monaBody;

        private void FireIntEvent(string variableName, int value)
        {
            EventBus.Trigger<MonaIntChangedEvent>(new EventHook(MonaCoreConstants.INT_CHANGED_EVENT, _monaBody), new MonaIntChangedEvent(variableName, value));
        }

        private void FireFloatEvent(string variableName, float value)
        {
            EventBus.Trigger<MonaFloatChangedEvent>(new EventHook(MonaCoreConstants.FLOAT_CHANGED_EVENT, _monaBody), new MonaFloatChangedEvent(variableName, value));
        }

        private void FireBoolEvent(string variableName, bool value)
        {
            EventBus.Trigger<MonaBoolChangedEvent>(new EventHook(MonaCoreConstants.BOOL_CHANGED_EVENT, _monaBody), new MonaBoolChangedEvent(variableName, value));
        }

        private void FireStringEvent(string variableName, string value)
        {
            EventBus.Trigger<MonaStringChangedEvent>(new EventHook(MonaCoreConstants.STRING_CHANGED_EVENT, _monaBody), new MonaStringChangedEvent(variableName, value));
        }

        private void FireBodyEvent(string variableName, IMonaBody value)
        {
            EventBus.Trigger<MonaBodyChangedEvent>(new EventHook(MonaCoreConstants.BODY_CHANGED_EVENT, _monaBody), new MonaBodyChangedEvent(variableName, value));
        }

        private void FireVector2Event(string variableName, Vector2 value)
        {
            EventBus.Trigger<MonaVector2ChangedEvent>(new EventHook(MonaCoreConstants.VECTOR2_CHANGED_EVENT, _monaBody), new MonaVector2ChangedEvent(variableName, value));
        }

        private void FireVector3Event(string variableName, Vector3 value)
        {
            EventBus.Trigger<MonaVector3ChangedEvent>(new EventHook(MonaCoreConstants.VECTOR3_CHANGED_EVENT, _monaBody), new MonaVector3ChangedEvent(variableName, value));
        }

    }
}