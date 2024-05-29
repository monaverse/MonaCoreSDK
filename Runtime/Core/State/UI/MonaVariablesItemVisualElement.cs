using System;
using Mona.SDK.Core.State.Structs;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine.UIElements;
using UnityEngine;
using System.Text;

namespace Mona.SDK.Core.State.UIElements
{
    public class MonaVariablesItemVisualElement : VisualElement, IDisposable
    {
        protected IMonaVariables _state;
        protected int _index;

        protected DropdownField _typeField;

        protected TextField _nameField;

        protected FloatField _floatField;
        
        protected TextField _stringField;
        protected Toggle _toggleField;
        protected Vector2Field _vector2Field;
        protected Vector3Field _vector3Field;
        protected Label _labelField;

        protected Action callback;

        private Regex _regex;

        public MonaVariablesItemVisualElement(Action newCallback)
        {
            callback = newCallback;
            style.flexDirection = FlexDirection.Row;
            style.width = Length.Percent(100);
            style.paddingBottom = 5;
            
            _typeField = new DropdownField();
            _typeField.style.width = 80;
            _typeField.style.marginRight = 5;
            _typeField.choices = new List<string>()
            {
                MonaCoreConstants.FLOAT_TYPE_LABEL,
                MonaCoreConstants.STRING_TYPE_LABEL,
                MonaCoreConstants.BOOL_TYPE_LABEL,
                MonaCoreConstants.VECTOR2_TYPE_LABEL,
                MonaCoreConstants.VECTOR3_TYPE_LABEL,
                MonaCoreConstants.BODY_ARRAY_TYPE_LABEL
            };
            _typeField.RegisterValueChangedCallback(HandleTypeChanged);

            _nameField = new TextField();
            _nameField.RegisterValueChangedCallback(HandleNameChanged);

            _regex = new Regex("\\d+");
            _nameField.RegisterCallback<BlurEvent>(HandleNameBlurred);
            _nameField.style.width = 100;
            _nameField.style.marginRight = 5;

            _floatField = new FloatField();
            _floatField.style.flexGrow = 1;
            _floatField.RegisterValueChangedCallback(HandleFloatChanged);

            _stringField = new TextField();
            _stringField.style.flexGrow = 1;
            _stringField.RegisterValueChangedCallback(HandleStringChanged);

            _vector2Field = new Vector2Field();
            _vector2Field.style.flexGrow = 1;
            _vector2Field.RegisterValueChangedCallback(HandleVector2Changed);

            _vector3Field = new Vector3Field();
            _vector3Field.style.flexGrow = 1;
            _vector3Field.RegisterValueChangedCallback(HandleVector3Changed);

            _toggleField = new Toggle();
            _toggleField.RegisterValueChangedCallback(HandleBoolChanged);

            _labelField = new Label();
        }

        public void Dispose()
        {
            _typeField.UnregisterValueChangedCallback(HandleTypeChanged);
            _nameField.UnregisterValueChangedCallback(HandleNameChanged);
            _nameField.UnregisterValueChangedCallback(HandleNameChanged);
            _nameField.UnregisterCallback<BlurEvent>(HandleNameBlurred);
            _floatField.UnregisterValueChangedCallback(HandleFloatChanged);
            _stringField.UnregisterValueChangedCallback(HandleStringChanged);
            _vector2Field.UnregisterValueChangedCallback(HandleVector2Changed);
            _vector3Field.UnregisterValueChangedCallback(HandleVector3Changed);
            _toggleField.UnregisterValueChangedCallback(HandleBoolChanged);
        }

        private void HandleBoolChanged(ChangeEvent<bool> evt)
        {
            ((IMonaVariablesBoolValue)_state.VariableList[_index]).Value = evt.newValue;
        }

        private void HandleVector3Changed(ChangeEvent<Vector3> evt)
        {
            ((IMonaVariablesVector3Value)_state.VariableList[_index]).Value = evt.newValue;
        }

        private void HandleVector2Changed(ChangeEvent<Vector2> evt)
        {
            ((IMonaVariablesVector2Value)_state.VariableList[_index]).Value = evt.newValue;
        }

        private void HandleStringChanged(ChangeEvent<string> evt)
        {
            if (_state.VariableList[_index] is IMonaVariablesStringValue)
                ((IMonaVariablesStringValue)_state.VariableList[_index]).Value = evt.newValue;
        }

        private void HandleFloatChanged(ChangeEvent<float> evt)
        {
            ((IMonaVariablesFloatValue)_state.VariableList[_index]).Value = evt.newValue;
            ((IMonaVariablesFloatValue)_state.VariableList[_index]).DefaultValue = evt.newValue;
        }

        private void HandleTypeChanged(ChangeEvent<string> evt)
        {
            CreateValue(evt.newValue);
        }

        private void HandleNameChanged(ChangeEvent<string> evt)
        {
            if (evt.newValue == null || evt.newValue.StartsWith("_")) return;
            if (_state.VariableList[_index].Name != evt.newValue)
            {
                _state.VariableList[_index].Name = evt.newValue;
            }
        }

        private void HandleNameBlurred(BlurEvent evt)
        {
            var count = _state.VariableList.FindAll(x => _regex.Replace(x.Name, "") == _nameField.value);
            count.Remove(_state.VariableList[_index]);
            if (count.Count > 0)
            {
                _state.VariableList[_index].Name = _nameField.value + count.Count.ToString("D2");
                _nameField.value = _state.VariableList[_index].Name;
            }
        }

        protected virtual void CreateValue(string value)
        {
            var variable = _state.VariableList[_index];
            var name = (variable != null) ? variable.Name : value + "Value";
            switch (value)
            {
                case MonaCoreConstants.FLOAT_TYPE_LABEL:
                    if (variable != null && variable is IMonaVariablesFloatValue) return;
                    _state.CreateVariable(name, typeof(MonaVariablesFloat), _index);
                    break;
                case MonaCoreConstants.STRING_TYPE_LABEL:
                    if (variable != null && variable is IMonaVariablesStringValue) return;
                    _state.CreateVariable(name, typeof(MonaVariablesString), _index);
                    break;
                case MonaCoreConstants.BOOL_TYPE_LABEL:
                    if (variable != null && variable is IMonaVariablesBoolValue) return;
                    _state.CreateVariable(name, typeof(MonaVariablesBool), _index);
                    break;
                case MonaCoreConstants.VECTOR2_TYPE_LABEL:
                    if (variable != null && variable is IMonaVariablesVector2Value) return;
                    _state.CreateVariable(name, typeof(MonaVariablesVector2), _index);
                    break;
                case MonaCoreConstants.VECTOR3_TYPE_LABEL:
                    if (variable != null && variable is IMonaVariablesVector3Value) return;
                    _state.CreateVariable(name, typeof(MonaVariablesVector3), _index);
                    break;
                case MonaCoreConstants.BODY_ARRAY_TYPE_LABEL:
                    if (variable != null && variable is IMonaVariablesBodyArrayValue) return;
                    _state.CreateVariable(name, typeof(MonaVariablesBodyArray), _index);
                    break;
            }
            Refresh();
        }

        public virtual void Refresh()
        {
            Clear();

            Add(_typeField);

            var value = _state.VariableList[_index];
            Add(_nameField);
            _nameField.value = value.Name;

            var btn = new Button();
            btn.style.width = 20;
            btn.style.height = 20;
            btn.text = "...";

            if (value is IMonaVariablesFloatValue)
             {
                _typeField.value = MonaCoreConstants.FLOAT_TYPE_LABEL;
                Add(_floatField);
                _floatField.value = ((IMonaVariablesFloatValue)value).Value;
#if UNITY_EDITOR
                btn.clicked += () => { MonaVariablesExpandedWindow.Open(_state.VariableList[_index], callback); };
#endif

                //btn.clicked += () => { MonaVariablesExpandedWindow.Open(_state.VariableList[_index], ()=>
                //{
                //    if (_brain != null)
                //    {
                //        EditorUtility.SetDirty(_brain);
                //        Undo.RecordObject(_brain, "change brain");
                //    }
                //});
                //};
                Add(btn);
            }
            else if (value is IMonaVariablesStringValue)
            {
                _typeField.value = MonaCoreConstants.STRING_TYPE_LABEL;
                Add(_stringField);
                _stringField.value = ((IMonaVariablesStringValue)value).Value;
                _stringField.SetEnabled(true);
            }
            else if(value is IMonaVariablesBoolValue)
            {
                _typeField.value = MonaCoreConstants.BOOL_TYPE_LABEL;
                Add(_toggleField);
                _toggleField.value = ((IMonaVariablesBoolValue)value).Value;
            }
            else if(value is IMonaVariablesVector2Value)
            {
                _typeField.value = MonaCoreConstants.VECTOR2_TYPE_LABEL;
                Add(_vector2Field);
                _vector2Field.value = ((IMonaVariablesVector2Value)value).Value;
            }
            else if (value is IMonaVariablesVector3Value)
            {
                _typeField.value = MonaCoreConstants.VECTOR3_TYPE_LABEL;
                Add(_vector3Field);
                _vector3Field.value = ((IMonaVariablesVector3Value)value).Value;
            }
            else if (value is IMonaVariablesBodyArrayValue)
            {
                _typeField.value = MonaCoreConstants.BODY_ARRAY_TYPE_LABEL;
                Add(_labelField);
                var lbl = new StringBuilder();
                ((IMonaVariablesBodyArrayValue)value).Value.ForEach(x => lbl.Append(x.Transform.name + ","));
                _labelField.text = lbl.ToString();
            }
        }

        public void SetStateItem(IMonaVariables state, int i)
        {
            _state = state;
            _index = i;
            Refresh();
        }

        public IMonaVariablesValue GetStateItem()
        {
            return _state.VariableList[_index];
        }
    }
}