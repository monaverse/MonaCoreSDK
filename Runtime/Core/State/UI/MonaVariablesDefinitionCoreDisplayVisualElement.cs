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
    public class MonaVariablesDefinitionCoreDisplayVisualElement : VisualElement
    {
        private IEasyUICommonElementDisplay _commonDisplayElement;
        public Toggle _displayElement;
        public EnumField _elementType;

#if UNITY_EDITOR
        public ColorField _elementColor;
#endif
        public Toggle _useShadow;
        public Vector2Field _shadowOffset;

        public void SetDisplay(IEasyUICommonElementDisplay newCommonDisplayElement)
        {
            _commonDisplayElement = newCommonDisplayElement;
            Refresh();
        }

        public MonaVariablesDefinitionCoreDisplayVisualElement()
        {
            _displayElement = new Toggle("Display This Element");
            _elementType = new EnumField("Display Type", EasyUIElementDisplayType.Default);
            _useShadow = new Toggle("Use Shadow");
            _shadowOffset = new Vector2Field("Shadow Offset");

            _displayElement.RegisterValueChangedCallback((evt) =>
            {
                _commonDisplayElement.DisplayElement = evt.newValue;
                Refresh();
            });

            _elementType.RegisterValueChangedCallback((evt) =>
            {
                _commonDisplayElement.ElementType = (EasyUIElementDisplayType)evt.newValue;
                Refresh();
            });

            _useShadow.RegisterValueChangedCallback((evt) =>
            {
                _commonDisplayElement.UseShadow = evt.newValue;
                Refresh();
            });

            _shadowOffset.RegisterValueChangedCallback((evt) =>
            {
                _commonDisplayElement.ShadowOffset = evt.newValue;
                Refresh();
            });

            Add(_displayElement);
            Add(_elementType);

#if UNITY_EDITOR
            _elementColor = new ColorField("Color");
            _elementColor.RegisterValueChangedCallback((evt) =>
            {
                _commonDisplayElement.ElementColor = evt.newValue;
                Refresh();
            });
            Add(_elementColor);
#endif

            Add(_useShadow);
            Add(_shadowOffset);
        }

        private void Refresh()
        {
            if (_commonDisplayElement == null)
                return;

            _displayElement.value = _commonDisplayElement.DisplayElement;
            _elementType.value = _commonDisplayElement.ElementType;
            _useShadow.value = _commonDisplayElement.UseShadow;
            _shadowOffset.value = _commonDisplayElement.ShadowOffset;

            _displayElement.style.display = DisplayStyle.Flex;

            if (_commonDisplayElement.DisplayElement)
            {
                _elementType.style.display = DisplayStyle.Flex;
                _useShadow.style.display = DisplayStyle.Flex;
                _shadowOffset.style.display = _commonDisplayElement.UseShadow ?
                    DisplayStyle.Flex : DisplayStyle.None;
            }
            else
            {
                _elementType.style.display = DisplayStyle.None;
                _useShadow.style.display = DisplayStyle.None;
                _shadowOffset.style.display = DisplayStyle.None;
            }

#if UNITY_EDITOR
            _elementColor.value = _commonDisplayElement.ElementColor;
            _elementColor.style.display = _commonDisplayElement.DisplayElement ?
                DisplayStyle.Flex : DisplayStyle.None;
#endif
        }
    }
}
