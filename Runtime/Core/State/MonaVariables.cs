using Mona.SDK.Core.Body;
using Mona.SDK.Core.Events;
using Mona.SDK.Core.Network;
using Mona.SDK.Core.Network.Interfaces;
using Mona.SDK.Core.State.Structs;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Mona.SDK.Core.Utils;
using Mona.SDK.Core.EasyUI;

namespace Mona.SDK.Core.State
{
    [Serializable]
    public class MonaVariables : IMonaVariables, IDisposable
    {
        protected INetworkMonaVariables _networkState;
        public INetworkMonaVariables NetworkVariables => _networkState;

        protected IMonaBody _monaBody;
        protected bool _hasUI;

        public bool HasUI() => _hasUI;

        public MonaVariables(GameObject gameObject = null)
        {
            SetGameObject(gameObject);
        }

        public void SetGameObject(GameObject gameObject)
        { 
            if (gameObject == null) return;
            _monaBody = gameObject.GetComponent<IMonaBody>();
            if (_monaBody == null)
                _monaBody = MonaBodyFactory.Create(gameObject);
        }

        [SerializeReference]
        protected List<IMonaVariablesValue> _values = new List<IMonaVariablesValue>();

        public List<IMonaVariablesValue> VariableList
        {
            get => _values;
            set => _values = value;
        }

        private Dictionary<int, IMonaVariablesValue> _variablesIndex = new Dictionary<int, IMonaVariablesValue>();
        private Dictionary<string, int> _variablesIndexByName = new Dictionary<string, int>();
        private Dictionary<string, IMonaVariablesValue> _variablesCache;

        private void InitVariableNames()
        {
            if(_variablesCache == null)
                _variablesCache = new Dictionary<string, IMonaVariablesValue>(15);
        }

        public void CacheVariableNames()
        {
            InitVariableNames();
            _hasUI = false;
            for (var i = 0; i < _values.Count; i++)
            {
                var v = _values[i];
                _variablesCache[v.Name] = v;
                if(v is IEasyUINumericalDisplay)
                {
                    if (((IEasyUINumericalDisplay)v).AllowUIDisplay)
                        _hasUI = true;
                }
            }
        }

        public IMonaVariablesValue GetVariableByIndex(int index)
        {
            return _variablesIndex[index];
        }

        public int GetVariableIndexByName(string name)
        {
            if(_variablesIndexByName.ContainsKey(name))
                return _variablesIndexByName[name];
            return -1;
        }

        public IMonaVariablesValue GetVariable(string name)
        {
            if (!Application.isPlaying)
            {
                for (var i = 0; i < _values.Count; i++)
                {
                    var v = _values[i];
                    if (v != null && v.Name == name)
                        return v;
                }
            }
            CacheVariableNames();
            if (_variablesCache != null && _variablesCache.ContainsKey(name))
                return _variablesCache[name];            
            return null;
        }

        public void Dispose()
        {
        }


        public IMonaVariablesValue GetVariable(string name, Type type, bool createIfNotFound = true)
        {
            var value = GetVariable(name);
            if (value != null) return value;

            if (!createIfNotFound)
                return null;

            var newValue = (IMonaVariablesValue)Activator.CreateInstance(type);
            newValue.Name = name;

            _values.Add(newValue);
            if (_variablesCache == null) InitVariableNames();
            _variablesCache[name] = newValue;

            return newValue;
        }

        public IMonaVariablesValue CreateVariable(string name, Type type, int i)
        {
            var prop = (IMonaVariablesValue)Activator.CreateInstance(type);
            prop.Name = name;
            _values[i] = prop;
            if (_variablesCache == null) InitVariableNames();
            _variablesCache[name] = prop;
            return prop;
        }

        private bool CanSet(IMonaVariablesValue prop)
        {
            var ret = _networkState == null || _networkState.HasControl();
            if (ret)
            {
                return true;
            }

            //if(!prop.IsLocal) Debug.Log($"{nameof(CanSet)} cannot set {_monaBody.Transform.gameObject.name} {prop.Name}, i do not have control and it's not local");
            return prop.IsLocal;
        }

        public void Set(string name, IMonaBody value, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesBody), createIfNotFound);

            if (prop == null)
                return;

            var propValue = ((IMonaVariablesBodyValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
        }

        public IMonaBody GetBody(string name, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesBody), createIfNotFound);
            return ((IMonaVariablesBodyValue)prop).Value;
        }

        public void Set(string name, bool value, bool isNetworked = true, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesBool), createIfNotFound);

            if (prop == null)
                return;

            if (!CanSet(prop) && isNetworked)
                return;

            var propValue = ((IMonaVariablesBoolValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }

            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public List<IMonaBody> GetBodyArray(string name, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesBodyArray), createIfNotFound);
            return ((IMonaVariablesBodyArrayValue)prop).Value;
        }

        public void Set(string name, List<IMonaBody> value, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesBodyArray), createIfNotFound);

            if (prop == null)
                return;

            var propValue = ((IMonaVariablesBodyArrayValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
        }

        public bool GetBool(string name, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesBool), createIfNotFound);
            return ((IMonaVariablesBoolValue)prop).Value;
        }

        public void Set(string name, float value, bool isNetworked = true, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesFloat), createIfNotFound);

            if (prop == null)
                return;

            if (!CanSet(prop) && isNetworked)
                return;

            var propValue = ((IMonaVariablesFloatValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public float GetFloat(string name, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesFloat), createIfNotFound);
            return ((IMonaVariablesFloatValue)prop).ValueToReturnFromTile;
        }

        public void Set(string name, int value, bool isNetworked = true, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesInt), createIfNotFound);

            if (prop == null)
                return;

            if (!CanSet(prop) && isNetworked)
                return;

            var propValue = ((IMonaVariablesIntValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public int GetInt(string name, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesInt), createIfNotFound);
            return ((IMonaVariablesIntValue)prop).Value;
        }

        public void Set(string name, string value, bool isNetworked = true, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesString), createIfNotFound);

            if (prop == null)
                return;

            if (!CanSet(prop) && isNetworked)
                return;

            var propValue = ((IMonaVariablesStringValue)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public string GetString(string name, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesString), createIfNotFound);
            return ((IMonaVariablesStringValue)prop).Value;
        }

        public void Set(string name, Vector2 value, bool isNetworked = true, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesVector2), createIfNotFound);

            if (prop == null)
                return;

            if (!CanSet(prop) && isNetworked)
                return;

            var propValue = ((IMonaVariablesVector2Value)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public Vector2 GetVector2(string name, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesFloat), createIfNotFound);
            return ((IMonaVariablesVector2Value)prop).Value;
        }

        public void Set(string name, Vector3 value, bool isNetworked = true, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesVector3), createIfNotFound);


            if (prop == null)
                return;

            if (!CanSet(prop) && isNetworked)
                return;

            var propValue = ((IMonaVariablesVector3Value)prop);
            if (propValue.Value != value)
            {
                propValue.Value = value;
                FireValueEvent(name, prop);
            }
            if (isNetworked)
                _networkState?.UpdateValue(prop);
        }

        public Vector3 GetVector3(string name, bool createIfNotFound = true)
        {
            var prop = GetVariable(name, typeof(MonaVariablesVector3), createIfNotFound);
            return ((IMonaVariablesVector3Value)prop).Value;
        }

        public string GetValueAsString(string variableName)
        {
            var variable = GetVariable(variableName);

            if (variable is IMonaVariablesFloatValue)
                return ((IEasyUINumericalDisplay)variable).FormattedNumber;
            else if (variable is IMonaVariablesStringValue)
                return ((IMonaVariablesStringValue)variable).Value;
            else if (variable is IMonaVariablesBoolValue)
                return ((IMonaVariablesBoolValue)variable).Value.ToString();
            else if (variable is IMonaVariablesVector2Value)
                return ((IMonaVariablesVector2Value)variable).Value.ToString();
            else if (variable is IMonaVariablesVector3Value)
                return ((IMonaVariablesVector3Value)variable).Value.ToString();
            else if (variable is IMonaVariablesBodyArrayValue)
                return ((IMonaVariablesBodyArrayValue)variable).Value.ConvertAll<string>(x => x.LocalId).ToString();

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
                
                _networkState.UpdateValue(_values[i]);
            }
        }

        public virtual void FireValueEvent(string variableName, IMonaVariablesValue value, bool isNetworked = false)
        {
            value.Change();
            MonaEventBus.Trigger<MonaValueChangedEvent>(new EventHook(MonaCoreConstants.VALUE_CHANGED_EVENT, _monaBody), new MonaValueChangedEvent(variableName, value));

            if (isNetworked)
                _networkState?.UpdateValue(value);
        }
    }
}