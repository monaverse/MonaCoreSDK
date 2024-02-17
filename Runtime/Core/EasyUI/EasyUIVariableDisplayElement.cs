using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUIVisualElement
    {
        public GameObject[] _prerequisitParents;
        public EasyUIImageSettings _primaryImageSettings;
        public EasyUIImageSettings _backgroundImageSettings;
        public EasyUIImageSettings _foregroundImageSettings;
        public EasyUITextSettings _textSettings;
    }

    [System.Serializable]
    public class EasyUIMultiIconCounter
    {
        public GameObject[] _requiredParents;
        [SerializeField] private GameObject _iconCounter;
    }

    public class EasyUIVariableDisplayElement : MonoBehaviour
    {
        [SerializeField] private GameObject _iconCounterElementPrefab;
        [SerializeField] private EasyUIImageSettings _elementBackground;
        [SerializeField] private EasyUIVisualElement _primaryIcon;
        [SerializeField] private EasyUIVisualElement _horizontalGauge;
        [SerializeField] private EasyUIVisualElement _highVolumeCounter;
        [SerializeField] private EasyUITextSettings _textDisplay;
        [SerializeField] private EasyUIMultiIconCounter _iconCounter;
    }
}
