#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine;
using UnityEngine.UIElements;
using Mona.SDK.Core.Assets;
using Mona.SDK.Core.UIElements;
using Mona.SDK.Core.Assets.Interfaces;

namespace Mona.SDK.Core.UIEditors
{
#if UNITY_EDITOR
    public class MonaAssetEditor : Editor
    {
        private VisualElement _root;
        private MonaAssetsVisualElement _assetsEditor;

        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();
            _assetsEditor = new MonaAssetsVisualElement();
            _assetsEditor.SetMonaAssets((IMonaAssetProvider)target);
            _assetsEditor.TrackSerializedObjectValue(serializedObject, HandleCallback);
            _root.Add(_assetsEditor);
            return _root;
        }

        public void OnDestroy()
        {
            //if (_tagEditor != null)
            //    _tagEditor.Dispose();
        }

        private void HandleCallback(SerializedObject so)
        {
            so.ApplyModifiedProperties();
            if (target != null)
            {
                EditorUtility.SetDirty(target);
                Undo.RecordObject(target, "change brain");
            }
            Debug.Log($"{nameof(HandleCallback)}");
        }
    }
#endif
}