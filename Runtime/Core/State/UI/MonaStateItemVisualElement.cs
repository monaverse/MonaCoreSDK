using Mona.SDK.Core.State.Structs;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Mona.SDK.Core.State.UIElements
{
    public class MonaStateItemVisualElement : VisualElement
    {
        protected IMonaState _state;
        protected int _index;

        protected DropdownField _typeField;

        protected TextField _nameField;

        protected FloatField _floatField;
        protected TextField _stringField;
        protected Toggle _toggleField;
        protected Vector2Field _vector2Field;
        protected Vector3Field _vector3Field;

        public MonaStateItemVisualElement()
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
                _state.Values[_index].Name = evt.newValue;
            });
            _nameField.style.width = 100;
            _nameField.style.marginRight = 5;

            _floatField = new FloatField();
            _floatField.style.flexGrow = 1;
            _floatField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaStateFloatValue)_state.Values[_index]).Value = evt.newValue;
            });

            _stringField = new TextField();
            _stringField.style.flexGrow = 1;
            _stringField.RegisterValueChangedCallback((evt) =>
            {
                if(_state.Values[_index] is IMonaStateStringValue)
                    ((IMonaStateStringValue)_state.Values[_index]).Value = evt.newValue;
            });

            _vector2Field = new Vector2Field();
            _vector2Field.style.flexGrow = 1;
            _vector2Field.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaStateVector2Value)_state.Values[_index]).Value = evt.newValue;
            });

            _vector3Field = new Vector3Field();
            _vector3Field.style.flexGrow = 1;
            _vector3Field.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaStateVector3Value)_state.Values[_index]).Value = evt.newValue;
            });

            _toggleField = new Toggle();
            _toggleField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaStateBoolValue)_state.Values[_index]).Value = evt.newValue;
            });
        }

        protected virtual void CreateValue(string value)
        {
            var name = (_state.Values[_index] != null) ? _state.Values[_index].Name : value + "Value";
            switch (value)
            {
                case MonaCoreConstants.FLOAT_TYPE_LABEL:
                    _state.CreateValue(name, typeof(MonaStateFloat), _index);
                    break;
                case MonaCoreConstants.STRING_TYPE_LABEL:
                    _state.CreateValue(name, typeof(MonaStateString), _index);
                    break;
                case MonaCoreConstants.BOOL_TYPE_LABEL:
                    _state.CreateValue(name, typeof(MonaStateBool), _index);
                    break;
                case MonaCoreConstants.VECTOR2_TYPE_LABEL:
                    _state.CreateValue(name, typeof(MonaStateVector2), _index);
                    break;
                case MonaCoreConstants.VECTOR3_TYPE_LABEL:
                    _state.CreateValue(name, typeof(MonaStateVector3), _index);
                    break;
            }
            Refresh();
        }

        protected virtual void Refresh()
        {
            Clear();

            Add(_typeField);

            var value = _state.Values[_index];
            Add(_nameField);
            _nameField.value = value.Name;

             if (value is IMonaStateFloatValue)
             {
                _typeField.value = MonaCoreConstants.FLOAT_TYPE_LABEL;
                Add(_floatField);
                _floatField.value = ((IMonaStateFloatValue)value).Value;
            }
            else if (value is IMonaStateStringValue)
            {
                _typeField.value = MonaCoreConstants.STRING_TYPE_LABEL;
                Add(_stringField);
                _stringField.value = ((IMonaStateStringValue)value).Value;
                _stringField.SetEnabled(true);
            }
            else if(value is IMonaStateBoolValue)
            {
                _typeField.value = MonaCoreConstants.BOOL_TYPE_LABEL;
                Add(_toggleField);
                _toggleField.value = ((IMonaStateBoolValue)value).Value;
            }
            else if(value is IMonaStateVector2Value)
            {
                _typeField.value = MonaCoreConstants.VECTOR2_TYPE_LABEL;
                Add(_vector2Field);
                _vector2Field.value = ((IMonaStateVector2Value)value).Value;
            }
            else if (value is IMonaStateVector3Value)
            {
                _typeField.value = MonaCoreConstants.VECTOR3_TYPE_LABEL;
                Add(_vector3Field);
                _vector3Field.value = ((IMonaStateVector3Value)value).Value;
            }
        }

        public void SetStateItem(IMonaState state, int i)
        {
            _state = state;
            _index = i;
            Refresh();
        }
    }
}