using Mona.SDK.Core.Body;
using Mona.SDK.Core.Events;
using Mona.SDK.Core.Network;
using Mona.SDK.Core.Network.Interfaces;
using Mona.SDK.Core.State.Structs;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Mona.SDK.Core.State
{
    [Serializable]
    public class MonaVariables : IMonaVariables, IDisposable
    {
        protected INetworkMonaVariables _networkState;
        protected IMonaBody _monaBody;

        public MonaVariables(GameObject gameObject = null)
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
        protected List<IMonaVariablesValue> _values = new List<IMonaVariablesValue>();

        public List<IMonaVariablesValue> VariableList
        {
            get => _values;
            set
            {
                _values = value;
                _variablesCache.Clear();
                for(var i = 0;i < _values.Count; i++)
                {
                    var v = _values[i];
                    _variablesCache[v.Name] = v;
                }
            }
        }

        private Dictionary<int, IMonaVariablesValue> _variablesIndex = new Dictionary<int, IMonaVariablesValue>();
        private Dictionary<string, int> _variablesIndexByName = new Dictionary<string, int>();
        private Dictionary<string, IMonaVariablesValue> _variablesCache = new Dictionary<string, IMonaVariablesValue>();

        public IMonaVariablesValue GetVariableByIndex(int index)
        {
            return _variablesIndex[index];
        }

        public int GetVariableIndexByName(string name)
        {
            return _variablesIndexByName[name];
        }

        public IMonaVariablesValue GetVariable(string name)
        {
            if (!System.Object.ReferenceEquals(_variablesCache, null) && _variablesCache.ContainsKey(name))
                return _variablesCache[name];

            for (var i = 0; i < _values.Count; i++)
            {
                if (_values[i] != null && _values[i].Name == name)
                    return _values[i];
            }

            return null;
        }

        public void Dispose()
        {
        }


        public IMonaVariablesValue GetVariable(string name, Type type)
        {
            var value = GetVariable(name);
            if (value != null) return value;

            var newValue = (IMonaVariablesValue)Activator.CreateInstance(type);
            newValue.Name = name;

            _values.Add(newValue);
            _variablesCache[name] = newValue;

            return newValue;
        }

        public IMonaVariablesValue CreateVariable(string name, Type type, int i)
        {
            var prop = (IMonaVariablesValue)Activator.CreateInstance(type);
            prop.Name = name;
            _values[i] = prop;
            _variablesCache[name] = prop;

            return prop;
        }

        public void Set(string name, IMonaBody value)
        {
            var prop = GetVariable(name, typeof(MonaVariablesBody));
            var propValue = ((IMonaVariablesBodyValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
        }

        public IMonaBody GetBody(string name)
        {
            var prop = GetVariable(name, typeof(MonaVariablesBody));
            return ((IMonaVariablesBodyValue)prop).Value;
        }

        public void Set(string name, bool value, bool isNetworked = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesBool));
            var propValue = ((IMonaVariablesBoolValue)prop);
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
            var prop = GetVariable(name, typeof(MonaVariablesBool));
            return ((IMonaVariablesBoolValue)prop).Value;
        }

        public void Set(string name, float value, bool isNetworked = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesFloat));
            var propValue = ((IMonaVariablesFloatValue)prop);
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
            var prop = GetVariable(name, typeof(MonaVariablesFloat));
            return ((IMonaVariablesFloatValue)prop).Value;
        }

        public void Set(string name, int value, bool isNetworked = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesInt));
            var propValue = ((IMonaVariablesIntValue)prop);
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
            var prop = GetVariable(name, typeof(MonaVariablesInt));
            return ((IMonaVariablesIntValue)prop).Value;
        }

        public void Set(string name, string value, bool isNetworked = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesString));
            var propValue = ((IMonaVariablesStringValue)prop);
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
            var prop = GetVariable(name, typeof(MonaVariablesString));
            return ((IMonaVariablesStringValue)prop).Value;
        }

        public void Set(string name, Vector2 value, bool isNetworked = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesVector2));
            var propValue = ((IMonaVariablesVector2Value)prop);
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
            var prop = GetVariable(name, typeof(MonaVariablesFloat));
            return ((IMonaVariablesVector2Value)prop).Value;
        }

        public void Set(string name, Vector3 value, bool isNetworked = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesVector3));
            var propValue = ((IMonaVariablesVector3Value)prop);
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
            var prop = GetVariable(name, typeof(MonaVariablesVector3));
            return ((IMonaVariablesVector3Value)prop).Value;
        }

        public string GetValueAsString(string variableName)
        {
            var variable = GetVariable(variableName);

            if (variable is IMonaVariablesFloatValue)
                return ((IMonaVariablesFloatValue)variable).Value.ToString();
            else if (variable is IMonaVariablesStringValue)
                return ((IMonaVariablesStringValue)variable).Value;
            else if (variable is IMonaVariablesBoolValue)
                return ((IMonaVariablesBoolValue)variable).Value.ToString();
            else if (variable is IMonaVariablesVector2Value)
                return ((IMonaVariablesVector2Value)variable).Value.ToString();
            else if (variable is IMonaVariablesVector3Value)
                return ((IMonaVariablesVector3Value)variable).Value.ToString();

            return string.Empty;
        }

        public void SetNetworkVariables(INetworkMonaVariables state)
        {
            _networkState = state;
            SyncValuesOnNetwork();
        }

        public void SyncValuesOnNetwork()
        {
            if (_networkState == null) return;

            _variablesIndex.Clear();
            _variablesIndexByName.Clear();
            for (var i = 0; i < _values.Count; i++)
            {
                _variablesIndex[i] = _values[i];
                _variablesIndexByName[_values[i].Name] = i;
                //Debug.Log($"{nameof(SyncValuesOnNetwork)} {_values[i].Name}, index {i}");
                _networkState.UpdateValue(_values[i]);
            }
        }

        protected virtual void FireValueEvent(string variableName, IMonaVariablesValue value)
        {
            value.Change();
            EventBus.Trigger<MonaValueChangedEvent>(new EventHook(MonaCoreConstants.VALUE_CHANGED_EVENT, _monaBody), new MonaValueChangedEvent(variableName, value));
        }
    }
}