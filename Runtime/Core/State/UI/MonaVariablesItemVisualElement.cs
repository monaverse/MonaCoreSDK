using Mona.SDK.Core.State.Structs;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine.UIElements;

namespace Mona.SDK.Core.State.UIElements
{
    public class MonaVariablesItemVisualElement : VisualElement
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
        protected FloatField _floatMin;
        protected FloatField _floatMax;
        protected Toggle _useMinMax;
        protected MinMaxConstraintType _contraintType;

        public MonaVariablesItemVisualElement()
        {
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
                MonaCoreConstants.VECTOR3_TYPE_LABEL
            };
            _typeField.RegisterValueChangedCallback((evt) =>
            {
                CreateValue(evt.newValue);
            });

            _nameField = new TextField();
            _nameField.RegisterValueChangedCallback((evt) =>
            {
                if (evt.newValue == null || evt.newValue.StartsWith("_")) return;
                if (_state.VariableList[_index].Name != evt.newValue)
                {
                    _state.VariableList[_index].Name = evt.newValue;
                }
            });

            var regex = new Regex("\\d+");
            _nameField.RegisterCallback<BlurEvent>((evt) =>
            {
                var count = _state.VariableList.FindAll(x => regex.Replace(x.Name, "") == _nameField.value);
                    count.Remove(_state.VariableList[_index]);
                if (count.Count > 0)
                {
                    _state.VariableList[_index].Name = _nameField.value + count.Count.ToString("D2");
                    _nameField.value = _state.VariableList[_index].Name;
                }                
            });
            _nameField.style.width = 100;
            _nameField.style.marginRight = 5;

            _floatField = new FloatField();
            _floatField.style.flexGrow = 1;
            _floatField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaVariablesFloatValue)_state.VariableList[_index]).Value = evt.newValue;
                ((IMonaVariablesFloatValue)_state.VariableList[_index]).DefaultValue = evt.newValue;
            });

            _stringField = new TextField();
            _stringField.style.flexGrow = 1;
            _stringField.RegisterValueChangedCallback((evt) =>
            {
                if(_state.VariableList[_index] is IMonaVariablesStringValue)
                    ((IMonaVariablesStringValue)_state.VariableList[_index]).Value = evt.newValue;
            });

            _vector2Field = new Vector2Field();
            _vector2Field.style.flexGrow = 1;
            _vector2Field.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaVariablesVector2Value)_state.VariableList[_index]).Value = evt.newValue;
            });

            _vector3Field = new Vector3Field();
            _vector3Field.style.flexGrow = 1;
            _vector3Field.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaVariablesVector3Value)_state.VariableList[_index]).Value = evt.newValue;
            });

            _toggleField = new Toggle();
            _toggleField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaVariablesBoolValue)_state.VariableList[_index]).Value = evt.newValue;
            });
        }

        protected virtual void CreateValue(string value)
        {
            var name = (_state.VariableList[_index] != null) ? _state.VariableList[_index].Name : value + "Value";
            switch (value)
            {
                case MonaCoreConstants.FLOAT_TYPE_LABEL:
                    _state.CreateVariable(name, typeof(MonaVariablesFloat), _index);
                    break;
                case MonaCoreConstants.STRING_TYPE_LABEL:
                    _state.CreateVariable(name, typeof(MonaVariablesString), _index);
                    break;
                case MonaCoreConstants.BOOL_TYPE_LABEL:
                    _state.CreateVariable(name, typeof(MonaVariablesBool), _index);
                    break;
                case MonaCoreConstants.VECTOR2_TYPE_LABEL:
                    _state.CreateVariable(name, typeof(MonaVariablesVector2), _index);
                    break;
                case MonaCoreConstants.VECTOR3_TYPE_LABEL:
                    _state.CreateVariable(name, typeof(MonaVariablesVector3), _index);
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

             if (value is IMonaVariablesFloatValue)
             {
                _typeField.value = MonaCoreConstants.FLOAT_TYPE_LABEL;
                Add(_floatField);
                _floatField.value = ((IMonaVariablesFloatValue)value).Value;
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