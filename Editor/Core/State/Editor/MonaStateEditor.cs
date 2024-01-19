using Mona.Core.Body;
using Mona.Core.State.UIElements;
using System;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine.UIElements;

namespace Mona.Core.State.UIEditors
{
    [CustomEditor(typeof(MonaState))]
    public class MonaStateEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var self = (IMonaState)target;
            var root = new MonaStateVisualElement();
            root.SetState(self);
            return root;
        }
    }
}