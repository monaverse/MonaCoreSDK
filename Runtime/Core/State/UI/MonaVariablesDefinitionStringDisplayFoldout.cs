using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;
using Mona.SDK.Core.EasyUI;
using TMPro;

namespace Mona.SDK.Core.State.UIElements
{
    public class MonaVariablesDefinitionStringDisplayFoldout : Foldout
    {
        [SerializeField] private EasyUIStringDisplay _stringDisplayElement;
        [SerializeField] private MonaVariablesDefinitionCoreDisplayVisualElement _coreElement;
        public TextField _elementText;
        public EnumField _fontType;
        public EnumField _textAlignment;

#if UNITY_EDITOR
        public ObjectField _elementFont;
#endif

        public void SetDisplay(EasyUIStringDisplay newDisplayElement, string labelName)
        {
            _stringDisplayElement = newDisplayElement;
            this.text = labelName;
            Refresh();
        }

        public MonaVariablesDefinitionStringDisplayFoldout()
        {
            style.paddingLeft = style.paddingRight = style.paddingTop = style.paddingBottom = 10;
            style.marginLeft = style.marginRight = style.marginTop = style.marginBottom = 10;
            style.borderLeftWidth = style.borderRightWidth = style.borderTopWidth = style.borderBottomWidth = 1;
            style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);

            _elementText = new TextField("Text");
            _fontType = new EnumField("Font Type", EasyUIElementDisplayDefaultOrCustom.Default);
            _textAlignment = new EnumField("Alignment", EasyUITextAlignment.Default);

            _elementText.RegisterValueChangedCallback((evt) =>
            {
                _stringDisplayElement.ElementText = evt.newValue;
                Refresh();
            });

            _fontType.RegisterValueChangedCallback((evt) =>
            {
                _stringDisplayElement.FontType = (EasyUIElementDisplayDefaultOrCustom)evt.newValue;
                Refresh();
            });

            _textAlignment.RegisterValueChangedCallback((evt) =>
            {
                _stringDisplayElement.TextAlignment = (EasyUITextAlignment)evt.newValue;
                Refresh();
            });

            Add(_elementText);
            Add(_fontType);


#if UNITY_EDITOR
            _elementFont = new ObjectField("Font");
            _elementFont.objectType = typeof(TMP_FontAsset);
            _elementFont.RegisterValueChangedCallback((evt) =>
            {
                _stringDisplayElement.ElementFont = (TMP_FontAsset)evt.newValue;
                Refresh();
            });


            Add(_elementFont);
#endif
            Add(_textAlignment);


            _coreElement = new MonaVariablesDefinitionCoreDisplayVisualElement();
            Add(_coreElement);
            value = false;
        }

        private void Refresh()
        {
            if (_stringDisplayElement == null)
                return;

            _elementText.value = _stringDisplayElement.ElementText;
            _fontType.value = _stringDisplayElement.FontType;
#if UNITY_EDITOR
            _elementFont.value = _stringDisplayElement.ElementFont;
#endif
            _textAlignment.value = _stringDisplayElement.TextAlignment;

            _elementText.style.display = DisplayStyle.Flex;
            _fontType.style.display = DisplayStyle.Flex;

#if UNITY_EDITOR
            _elementFont.style.display = _stringDisplayElement.FontType == EasyUIElementDisplayDefaultOrCustom.Custom ?
                DisplayStyle.Flex : DisplayStyle.None;
#endif

            _textAlignment.style.display = DisplayStyle.Flex;

            _coreElement.SetDisplay(_stringDisplayElement);
        }
    }
}


