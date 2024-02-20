using System;
using UnityEngine;
using UnityEngine.UIElements;
using Mona.SDK.Core.EasyUI;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

namespace Mona.SDK.Core.State.UIElements
{
    [Serializable]
    public class MonaVariablesDefinitionSpriteDisplayFoldout : Foldout
    {

#if UNITY_EDITOR
        public ObjectField _elementSprite;
#endif

        [SerializeField] private EasyUISpriteDisplay _spriteDisplayElement;
        [SerializeField] private MonaVariablesDefinitionCoreDisplayVisualElement _coreElement;
        
        public void SetDisplay(EasyUISpriteDisplay newSpriteDisplay, string labelName)
        {
            _spriteDisplayElement = newSpriteDisplay;
            this.text = labelName;
            Refresh();
        }

        public MonaVariablesDefinitionSpriteDisplayFoldout()
        {
            style.paddingLeft = style.paddingRight = style.paddingTop = style.paddingBottom = 10;
            style.marginLeft = style.marginRight = style.marginTop = style.marginBottom = 10;
            style.borderLeftWidth = style.borderRightWidth = style.borderTopWidth = style.borderBottomWidth = 1;
            style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);

#if UNITY_EDITOR
            _elementSprite = new ObjectField("Sprite");
            _elementSprite.objectType = typeof(Sprite);
            _elementSprite.RegisterValueChangedCallback((evt) =>
            {
                _spriteDisplayElement.ElementSprite = (Sprite)evt.newValue;
                Refresh();
            });

            Add(_elementSprite);
#endif

            _coreElement = new MonaVariablesDefinitionCoreDisplayVisualElement();
            Add(_coreElement);
            value = false;
        }

        private void Refresh()
        {
            if (_spriteDisplayElement == null)
                return;

#if UNITY_EDITOR
            _elementSprite.style.display = DisplayStyle.Flex;
            _elementSprite.value = _spriteDisplayElement.ElementSprite;
#endif

            _coreElement.SetDisplay(_spriteDisplayElement);
        }
    }
}
