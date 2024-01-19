using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// Serializable wrapper for System.Guid.
/// Can be implicitly converted to/from System.Guid.
///
/// Author: Searous
/// https://forum.unity.com/threads/cannot-serialize-a-guid-field-in-class.156862/
/// </summary>
[Serializable]
public struct SerializableGuid : ISerializationCallbackReceiver
{
    private Guid guid;
    [SerializeField] private string serializedGuid;

    public SerializableGuid(Guid guid)
    {
        this.guid = guid;
        serializedGuid = null;
    }

    public override bool Equals(object obj)
    {
        return obj is SerializableGuid guid &&
                this.guid.Equals(guid.guid);
    }

    public override int GetHashCode()
    {
        return -1324198676 + guid.GetHashCode();
    }

    public void OnAfterDeserialize()
    {
        try
        {
            guid = Guid.Parse(serializedGuid);
        }
        catch
        {
            guid = Guid.Empty;
            Debug.LogWarning($"Attempted to parse invalid GUID string '{serializedGuid}'. GUID will set to System.Guid.Empty");
        }
    }

    public void OnBeforeSerialize()
    {
        serializedGuid = guid.ToString();
    }

    public bool IsEmpty() => guid == Guid.Empty;

    public override string ToString() => guid.ToString();

    public static bool operator ==(SerializableGuid a, SerializableGuid b) => a.guid == b.guid;
    public static bool operator !=(SerializableGuid a, SerializableGuid b) => a.guid != b.guid;
    public static implicit operator SerializableGuid(Guid guid) => new SerializableGuid(guid);
    public static implicit operator Guid(SerializableGuid serializable) => serializable.guid;
    public static implicit operator SerializableGuid(string serializedGuid) => new SerializableGuid(Guid.Parse(serializedGuid));
    public static implicit operator string(SerializableGuid serializedGuid) => serializedGuid.ToString();
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SerializableGuid))]
public class SerializableGuidPropertyDrawer : PropertyDrawer
{

    private float ySep = 20;
    private float buttonSize;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Start property draw
        EditorGUI.BeginProperty(position, label, property);

        // Get property
        SerializedProperty serializedGuid = property.FindPropertyRelative("serializedGuid");

        // Draw label
        position = EditorGUI.PrefixLabel(new Rect(position.x, position.y + ySep / 2, position.width, position.height), GUIUtility.GetControlID(FocusType.Passive), label);
        position.y -= ySep / 2; // Offsets position so we can draw the label for the field centered

        buttonSize = position.width / 3; // Update size of buttons to always fit perfeftly above the string representation field

        // Buttons
        if (GUI.Button(new Rect(position.xMin, position.yMin, buttonSize, ySep - 2), "New"))
        {
            serializedGuid.stringValue = Guid.NewGuid().ToString();
        }
        if (GUI.Button(new Rect(position.xMin + buttonSize, position.yMin, buttonSize, ySep - 2), "Copy"))
        {
            EditorGUIUtility.systemCopyBuffer = serializedGuid.stringValue;
        }
        if (GUI.Button(new Rect(position.xMin + buttonSize * 2, position.yMin, buttonSize, ySep - 2), "Empty"))
        {
            serializedGuid.stringValue = "";
        }

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        Rect pos = new Rect(position.xMin, position.yMin + ySep, position.width, ySep - 2);
        EditorGUI.PropertyField(pos, serializedGuid, GUIContent.none);

        // End property
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Field height never changes, so ySep * 2 will always return the proper hight of the field
        return ySep * 2;
    }
}
#endif
