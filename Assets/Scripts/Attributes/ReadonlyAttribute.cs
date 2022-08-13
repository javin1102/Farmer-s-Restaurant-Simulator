using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadonlyAttribute : PropertyAttribute
{
}
#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer(typeof(ReadonlyAttribute))]
public class ReadOnlyAttributeDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect rect, UnityEditor.SerializedProperty prop, GUIContent label)
    {
        bool wasEnabled = GUI.enabled;
        GUI.enabled = false;
        UnityEditor.EditorGUI.PropertyField(rect, prop, label);
        GUI.enabled = wasEnabled;
    }
}
#endif