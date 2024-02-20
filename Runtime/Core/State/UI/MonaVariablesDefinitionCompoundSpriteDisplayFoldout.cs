using System;
using UnityEngine;
using UnityEngine.UIElements;
using Mona.SDK.Core.EasyUI;

namespace Mona.SDK.Core.State.UIElements
{
    public class MonaVariablesDefinitionCompoundSpriteDisplayFoldout : Foldout
    {
        [SerializeField] private EasyUICompoundSpriteDisplay _compoundDisplayElement;

        [SerializeField] private MonaVariablesDefinitionSpriteDisplayFoldout _primarySprite;
        [SerializeField] private MonaVariablesDefinitionSpriteDisplayFoldout _backgroundSprite;
        [SerializeField] private MonaVariablesDefinitionSpriteDisplayFoldout _foregroundSprite;
        [SerializeField] private MonaVariablesDefinitionStringDisplayFoldout _textDisplay;

        public void SetDisplay(EasyUICompoundSpriteDisplay newDisplayElement, string labelName)
        {
            _compoundDisplayElement = newDisplayElement;
            this.text = labelName;
            Refresh();
        }

        public MonaVariablesDefinitionCompoundSpriteDisplayFoldout()
        {
            style.paddingLeft = style.paddingRight = style.paddingTop = style.paddingBottom = 10;
            style.marginLeft = style.marginRight = style.marginTop = style.marginBottom = 10;
            style.borderLeftWidth = style.borderRightWidth = style.borderTopWidth = style.borderBottomWidth = 1;
            style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);

            _primarySprite = new MonaVariablesDefinitionSpriteDisplayFoldout();
            _backgroundSprite = new MonaVariablesDefinitionSpriteDisplayFoldout();
            _foregroundSprite = new MonaVariablesDefinitionSpriteDisplayFoldout();
            _textDisplay = new MonaVariablesDefinitionStringDisplayFoldout();

            Add(_primarySprite);
            Add(_backgroundSprite);
            Add(_foregroundSprite);
            Add(_textDisplay);

            value = false;
        }

        private void Refresh()
        {
            if (_compoundDisplayElement == null)
                return;

            _primarySprite.SetDisplay(_compoundDisplayElement.PrimarySprite, "Primary Sprite");
            _backgroundSprite.SetDisplay(_compoundDisplayElement.BackgroundSprite, "Background Sprite");
            _foregroundSprite.SetDisplay(_compoundDisplayElement.ForegroundSprite, "Foreground Sprite");
            _textDisplay.SetDisplay(_compoundDisplayElement.TextDisplay, "Text");
        }
    }
}
