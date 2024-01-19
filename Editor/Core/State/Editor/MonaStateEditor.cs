using Mona.SDK.Core.State.UIElements;
using UnityEditor;
using UnityEngine.UIElements;

namespace Mona.SDK.Core.State.UIEditors
{
#if UNITY_EDITOR
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
#endif
}