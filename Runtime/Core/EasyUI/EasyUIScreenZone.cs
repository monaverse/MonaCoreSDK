using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUIScreenZone : MonoBehaviour
    {
        [SerializeField] private EasyUIScreenPosition _screenPlacement;
        [SerializeField] private EasyUIObjectPosition _objectPlacement;
        [SerializeField] private int _maxChildren;
        [SerializeField] private bool _reverseDisplayOrder;

        [SerializeField] private GameObject _variablePrefab;
        [SerializeField] private GameObject _placeholderPrefab;
        private List<EasyUIVariableDisplayElement> _variableDisplayElements = new List<EasyUIVariableDisplayElement>();

        public EasyUIScreenPosition ScreenPlacement => _screenPlacement;
        public EasyUIObjectPosition ObjectPlacement => _objectPlacement;
        public int MaxChildren => _maxChildren;

        public void AddVariable(IEasyUINumericalDisplay variable)
        {
            if (variable.DisplayElementReference != null)
                return;

            GameObject variableDisplayObject = GameObject.Instantiate(_variablePrefab, gameObject.transform);
            EasyUIVariableDisplayElement variableDisplayElement = variableDisplayObject.GetComponent<EasyUIVariableDisplayElement>();
            _variableDisplayElements.Add(variableDisplayElement);
            variable.DisplayElementReference = variableDisplayElement;
            variable.UpdateUIDisplay();

            UpdateDisplayElements();
        }

        private void UpdateDisplayElements()
        {
            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);

            for (int i = _variableDisplayElements.Count - 1; i >= 0; i--)
            {
                if (_variableDisplayElements[i] == null)
                    _variableDisplayElements.RemoveAt(i);
            }

            int visibleElementCount = 0;

            foreach (EasyUIVariableDisplayElement displayElement in _variableDisplayElements)
            {
                if (!displayElement.IsPlaceholder)
                    visibleElementCount++;
            }

            if (visibleElementCount < _maxChildren && _placeholderPrefab)
            {
                int placeholdersNeeded = _maxChildren - visibleElementCount;

                for (int i = 0; i < placeholdersNeeded; i++)
                {
                    GameObject placeholder = Instantiate(_placeholderPrefab, gameObject.transform);
                    EasyUIVariableDisplayElement placeholderVariableDisplayElement = placeholder.GetComponent<EasyUIVariableDisplayElement>();

                    if (placeholderVariableDisplayElement != null)
                        _variableDisplayElements.Add(placeholderVariableDisplayElement);
                }
            }

            ReorderElementsByPriority();
        }

        private void ReorderElementsByPriority()
        {
            _variableDisplayElements = _reverseDisplayOrder ?
                _variableDisplayElements.OrderBy(element => element.Priority).ToList() :
                _variableDisplayElements.OrderByDescending(element => element.Priority).ToList();

            for (int i = 0; i < _variableDisplayElements.Count; i++)
            {
                _variableDisplayElements[i].transform.SetParent(null);
                _variableDisplayElements[i].transform.SetParent(gameObject.transform);
                _variableDisplayElements[i].gameObject.SetActive(i < _maxChildren);
            }
        }
    }
}
