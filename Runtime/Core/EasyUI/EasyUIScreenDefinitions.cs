using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    public class EasyUIScreenDefinitions : MonoBehaviour
    {

        [SerializeField] private EasyUIScreenZone[] _screenZones;

        [SerializeField] private GameObject _variableDisplayElement;
        [SerializeField] private GameObject _variableDisplayPlaceholder;
        [SerializeField] private GameObject _radarDisplay;
        [SerializeField] private GameObject _mapDisplay;
    }
}
