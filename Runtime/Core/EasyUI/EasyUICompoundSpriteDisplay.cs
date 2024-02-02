using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUICompoundSpriteDisplay
    {
        public EasyUISpriteDisplay _primarySprite;
        public EasyUISpriteDisplay _backgroundSprite;
        public EasyUISpriteDisplay _foregroundSprite;
        public EasyUIStringDisplay _textDisplay;
    }
}
