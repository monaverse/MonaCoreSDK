using Mona.SDK.Core.State.Structs;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace Mona.SDK.Core.State.UIElements
{
    public class MonaVariablesVisualElement : VisualElement
    {
        private IMonaVariables _state;
        private ListView _list;
        private Action callback;

        public MonaVariablesVisualElement(Action newCallback)
        {
            callback = newCallback;
            style.flexDirection = FlexDirection.Column;

            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.backgroundColor = new Color(.1f, .1f, .1f);
            header.style.paddingLeft = header.style.paddingRight = header.style.paddingTop = header.style.paddingBottom = 5;
            header.style.marginBottom = 5;
            header.style.unityFontStyleAndWeight = FontStyle.Bold;
            header.Add(new Label("Type"));
            header.Add(new Label("Name"));
            header.Add(new Label("Value"));
            header.ElementAt(0).style.width = 80;
            header.ElementAt(0).style.marginRight = 5;
            header.ElementAt(1).style.width = 100;
            header.ElementAt(1).style.marginRight = 5;
            Add(header);

            _list = new ListView(null, 28, () => new MonaVariablesItemVisualElement(callback), (elem, i) => BindStateItem((MonaVariablesItemVisualElement)elem, i));
            //_list.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            _list.showFoldoutHeader = false;
            _list.showAddRemoveFooter = true;
            _list.reorderable = false;
            _list.itemsAdded += (items) =>
            {
                foreach (var e in items)
                {
                    var variable = _state.CreateVariable("Default", typeof(MonaVariablesString), e);
                    var regex = new Regex("\\d+");
                    var count = _state.VariableList.FindAll(x => regex.Replace(x.Name, "") == "Default");
                        count.Remove(variable);
                    if (count.Count > 0)
                    {
                        variable.Name = "Default" + count.Count.ToString("D2");
                    }
                }
            };
            Add(_list);
        }

        private void BindStateItem(MonaVariablesItemVisualElement elem, int i)
        {
            elem.SetStateItem(_state, i);
        }

        public void SetState(IMonaVariables state)
        {
            _state = state;
            _list.itemsSource = _state.VariableList;
            _list.Rebuild();
        }

    }
}