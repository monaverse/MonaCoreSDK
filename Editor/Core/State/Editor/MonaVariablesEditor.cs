using Mona.SDK.Core.State.UIElements;
using UnityEditor;
using UnityEngine.UIElements;

namespace Mona.SDK.Core.State.UIEditors
{
#if UNITY_EDITOR
    [CustomEditor(typeof(MonaVariables))]
    public class MonaVariablesEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var self = (IMonaVariables)target;
            var root = new MonaVariablesVisualElement();
            root.SetState(self);
            return root;
        }
    }
#endif
}