using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUIScreenZone : MonoBehaviour
    {
        [SerializeField] private EasyUIScreenPosition _placement;
        [SerializeField] private int _maxChildren;

        [SerializeField] private GameObject _variablePrefab;
        [SerializeField] private GameObject _placeholderPrefab;
        private List<EasyUIVariableDisplayElement> _variableDisplayElements = new List<EasyUIVariableDisplayElement>();

        public EasyUIScreenPosition Placement => _placement;
        public int MaxChildren => _maxChildren;

        public void AddVariable(IEasyUINumericalDisplay variable)
        {
            foreach (EasyUIVariableDisplayElement element in _variableDisplayElements)
            {
                if (element.VariableReference == variable)
                    return;
            }

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
            _variableDisplayElements = _variableDisplayElements.OrderByDescending(element => element.Priority).ToList();

            for (int i = 0; i < _variableDisplayElements.Count; i++)
            {
                _variableDisplayElements[i].transform.SetParent(null);
                _variableDisplayElements[i].transform.SetParent(gameObject.transform);
                _variableDisplayElements[i].gameObject.SetActive(i < _maxChildren);
            }
        }
    }
}
