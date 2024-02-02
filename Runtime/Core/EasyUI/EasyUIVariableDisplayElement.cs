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
        public EasyUIImageSettings _borderImageSettings;
        public EasyUITextSettings _textSettings;
    }

    public class EasyUIVariableDisplayElement : MonoBehaviour
    {
        [SerializeField] private GameObject _iconCounterElementPrefab;
        [SerializeField] private EasyUIImageSettings _elementBackground;
        [SerializeField] private EasyUIVisualElement _primaryIcon;
        [SerializeField] private EasyUIVisualElement _horizontalGauge;
        [SerializeField] private EasyUIVisualElement _highVolumeCounter;
        [SerializeField] private EasyUIVisualElement _textDisplay;
        [SerializeField] private GameObject _iconCounter;
    }
}
