using System;  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mona.SDK.Core.State.Structs;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Mona.SDK.Core.EasyUI;

namespace Mona.SDK.Core.State.UIElements
{
    public class MonaVariablesExpandedWindow : EditorWindow
    {
        private IMonaVariablesValue _variable;
        protected Action callback;

        protected TextField _name;
        protected FloatField _value;
        protected EnumField _minMaxType;
        protected FloatField _min;
        protected FloatField _max;

        protected Toggle _allowUIDisplay;
        protected EnumField _displaySpace;
        protected EnumField _screenPosition;
        protected EnumField _objectPosition;
        
        protected IntegerField _priority;

        protected MonaVariablesDefinitionStringDisplayFoldout _displayName;
        protected MonaVariablesDefinitionStringDisplayFoldout _tooltip;
        protected MonaVariablesDefinitionCompoundSpriteDisplayFoldout _primaryIcon;
        protected MonaVariablesDefinitionSpriteDisplayFoldout _uiBackground;
        protected EnumField _valueDisplayType;
        protected EnumField _fillType;
        protected MonaVariablesDefinitionCompoundSpriteDisplayFoldout _horizontalGaugeVisual;
        protected MonaVariablesDefinitionStringDisplayFoldout _numberDisplay;

        public IMonaVariablesValue Variable { get => _variable; set => _variable = value; }

        public static void Open(IMonaVariablesValue newVariable, Action newCallback)
        {
            var window = GetWindow<MonaVariablesExpandedWindow>("Variable Editor");
            window.SetupVariable(newVariable);
            window.callback = newCallback;
        }

        public void SetupVariable(IMonaVariablesValue newVariable)
        {
            Variable = newVariable;
            Refresh();
        }

        private void Refresh()
        {
            _name.value = _variable.Name;
            _value.value = ((IMonaVariablesFloatValue)_variable).Value;
            _minMaxType.value = ((IMonaVariablesFloatValue)_variable).MinMaxType;
            _min.value = ((IMonaVariablesFloatValue)_variable).Min;
            _max.value = ((IMonaVariablesFloatValue)_variable).Max;

            if (((IMonaVariablesFloatValue)_variable).UseMinMax)
            {
                _min.style.display = DisplayStyle.Flex;
                _max.style.display = DisplayStyle.Flex;
            }
            else
            {
                _min.style.display = DisplayStyle.None;
                _max.style.display = DisplayStyle.None;
            }

            _allowUIDisplay.value = ((IEasyUINumericalDisplay)_variable).AllowUIDisplay;
            _displaySpace.value = ((IEasyUINumericalDisplay)_variable).DisplaySpace;
            _screenPosition.value = ((IEasyUINumericalDisplay)_variable).ScreenPosition;
            _objectPosition.value = ((IEasyUINumericalDisplay)_variable).ObjectPosition;
            _priority.value = ((IEasyUINumericalDisplay)_variable).Priority;

            _displayName.SetDisplay(((IEasyUINumericalDisplay)_variable).DisplayName, "Display Name");
            _tooltip.SetDisplay(((IEasyUINumericalDisplay)_variable).Tooltip, "Tooltip");
            _primaryIcon.SetDisplay(((IEasyUINumericalDisplay)_variable).PrimaryIcon, "Primary Icon");
            _uiBackground.SetDisplay(((IEasyUINumericalDisplay)_variable).UIBackground, "UI Background");
            _valueDisplayType.value = ((IEasyUINumericalDisplay)_variable).ValueDisplayType;
            _fillType.value = ((IEasyUINumericalDisplay)_variable).FillType;
            _horizontalGaugeVisual.SetDisplay(((IEasyUINumericalDisplay)_variable).HorizontalGaugeVisual, "Horizontal Gauge");
            _numberDisplay.SetDisplay(((IEasyUINumericalDisplay)_variable).NumberDisplay, "Number Display");

            if (((IEasyUINumericalDisplay)_variable).AllowUIDisplay)
            {
                _displaySpace.style.display = DisplayStyle.Flex;
                _priority.style.display = DisplayStyle.Flex;

                if (((IEasyUINumericalDisplay)_variable).DisplaySpace == EasyUIDisplaySpace.HeadsUpDisplay)
                {
                    _screenPosition.style.display = DisplayStyle.Flex;
                    _objectPosition.style.display = DisplayStyle.None;
                }
                else
                {
                    _screenPosition.style.display = DisplayStyle.None;
                    _objectPosition.style.display = DisplayStyle.Flex;
                }

                _displayName.style.display = DisplayStyle.Flex;
                _tooltip.style.display = DisplayStyle.Flex;
                _primaryIcon.style.display = DisplayStyle.Flex;
                _uiBackground.style.display = DisplayStyle.Flex;

                if (((IMonaVariablesFloatValue)_variable).UseMinMax)
                {
                    _valueDisplayType.style.display = DisplayStyle.Flex;

                    _fillType.style.display = ((IEasyUINumericalDisplay)_variable).DisplayAsGauge ?
                        DisplayStyle.Flex : DisplayStyle.None;

                    _horizontalGaugeVisual.style.display = ((IEasyUINumericalDisplay)_variable).UseHorizontalGauge ?
                        DisplayStyle.Flex : DisplayStyle.None;
                }
                else
                {
                    _valueDisplayType.style.display = DisplayStyle.None;
                    _fillType.style.display = DisplayStyle.None;
                    _horizontalGaugeVisual.style.display = DisplayStyle.None;
                }

                _numberDisplay.style.display = DisplayStyle.Flex;
            }
            else
            {
                _displaySpace.style.display = DisplayStyle.None;
                _screenPosition.style.display = DisplayStyle.None;
                _objectPosition.style.display = DisplayStyle.None;
                _priority.style.display = DisplayStyle.None;

                _displayName.style.display = DisplayStyle.None;
                _tooltip.style.display = DisplayStyle.None;
                _primaryIcon.style.display = DisplayStyle.None;
                _uiBackground.style.display = DisplayStyle.None;
                _valueDisplayType.style.display = DisplayStyle.None;
                _fillType.style.display = DisplayStyle.None;
                _horizontalGaugeVisual.style.display = DisplayStyle.None;
                _numberDisplay.style.display = DisplayStyle.None;
            }

            
        }

        private void CreateGUI()
        {
            _name = new TextField("Variable Name");
            _name.RegisterValueChangedCallback((evt) =>
            {
                _variable.Name = evt.newValue;
                callback?.Invoke();
            });

            _value = new FloatField("Value");
            _value.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaVariablesFloatValue)_variable).Value = evt.newValue;
                callback?.Invoke();
            });

            _minMaxType = new EnumField("Min/Max Type", MinMaxConstraintType.None);
            _minMaxType.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaVariablesFloatValue)_variable).MinMaxType = (MinMaxConstraintType)evt.newValue;
                callback?.Invoke();
                Refresh();
            });

            _min = new FloatField("Minimum Range");
            _min.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaVariablesFloatValue)_variable).Min = evt.newValue;
                callback?.Invoke();

            });

            _max = new FloatField("Maximum Range");
            _max.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaVariablesFloatValue)_variable).Max = evt.newValue;
                callback?.Invoke();
            });

            rootVisualElement.Add(_name);
            rootVisualElement.Add(_value);
            rootVisualElement.Add(_minMaxType);
            rootVisualElement.Add(_min);
            rootVisualElement.Add(_max);

            _allowUIDisplay = new Toggle("Allow UI Display");
            _allowUIDisplay.RegisterValueChangedCallback((evt) =>
            {
                ((IEasyUINumericalDisplay)_variable).AllowUIDisplay = evt.newValue;
                callback?.Invoke();
                Refresh();
            });

            _displaySpace = new EnumField("Display Space", EasyUIDisplaySpace.OnObject);
            _displaySpace.RegisterValueChangedCallback((evt) =>
            {
                ((IEasyUINumericalDisplay)_variable).DisplaySpace = (EasyUIDisplaySpace)evt.newValue;
                callback?.Invoke();
                Refresh();
            });

            _screenPosition = new EnumField("Screen Position", EasyUIScreenPosition.TopLeft);
            _screenPosition.RegisterValueChangedCallback((evt) =>
            {
                ((IEasyUINumericalDisplay)_variable).ScreenPosition = (EasyUIScreenPosition)evt.newValue;
                callback?.Invoke();
            });

            _objectPosition = new EnumField("Object Position", EasyUIObjectPosition.Above);
            _objectPosition.RegisterValueChangedCallback((evt) =>
            {
                ((IEasyUINumericalDisplay)_variable).ObjectPosition = (EasyUIObjectPosition)evt.newValue;
                callback?.Invoke();
            });

            _priority = new IntegerField("Display Priority");
            _priority.RegisterValueChangedCallback((evt) =>
            {
                ((IEasyUINumericalDisplay)_variable).Priority = evt.newValue;
                callback?.Invoke();
            });

            _valueDisplayType = new EnumField("Display Type", EasyUINumericalLayoutType.NumberDisplay);
            _valueDisplayType.RegisterValueChangedCallback((evt) =>
            {
                ((IEasyUINumericalDisplay)_variable).ValueDisplayType = (EasyUINumericalLayoutType)evt.newValue;
                callback?.Invoke();
                Refresh();
            });

            _fillType = new EnumField("Gauge Fill Type", EasyUIFillType.LeftToRight);
            _fillType.RegisterValueChangedCallback((evt) =>
            {
                ((IEasyUINumericalDisplay)_variable).FillType = (EasyUIFillType)evt.newValue;
                callback?.Invoke();
                Refresh();
            });

            _displayName = new MonaVariablesDefinitionStringDisplayFoldout();
            _tooltip = new MonaVariablesDefinitionStringDisplayFoldout();
            _primaryIcon = new MonaVariablesDefinitionCompoundSpriteDisplayFoldout();
            _uiBackground = new MonaVariablesDefinitionSpriteDisplayFoldout();
            _horizontalGaugeVisual = new MonaVariablesDefinitionCompoundSpriteDisplayFoldout();
            _numberDisplay = new MonaVariablesDefinitionStringDisplayFoldout();

            ScrollView sv = new ScrollView();

            rootVisualElement.Add(sv);

            sv.Add(_allowUIDisplay);
            sv.Add(_displaySpace);
            sv.Add(_screenPosition);
            sv.Add(_objectPosition);
            sv.Add(_priority);
            sv.Add(_displayName);
            sv.Add(_tooltip);
            sv.Add(_primaryIcon);
            sv.Add(_uiBackground);
            sv.Add(_valueDisplayType);
            sv.Add(_fillType);
            sv.Add(_horizontalGaugeVisual);
            sv.Add(_numberDisplay);
        }
    }
}

