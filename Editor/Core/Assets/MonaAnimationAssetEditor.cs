#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine;
using UnityEngine.UIElements;
using Mona.SDK.Core.Assets;
using Mona.SDK.Core.UIElements;

namespace Mona.SDK.Core.UIEditors
{
#if UNITY_EDITOR
    [CustomEditor(typeof(MonaAnimationAssetsDefinition), editorForChildClasses: true)]
    public class MonaAnimationAssetEditor : MonaBodyAssetEditor
    {
        private VisualElement _root;
        private MonaAnimationAssetsVisualElement _assetsEditor;

        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();
            _assetsEditor = new MonaAnimationAssetsVisualElement();
            _assetsEditor.SetMonaAssets(((MonaAssetsDefinition)target).MonaAsset);
            _assetsEditor.TrackSerializedObjectValue(serializedObject, HandleCallback);
            _root.Add(_assetsEditor);
            return _root;
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