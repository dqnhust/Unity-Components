using System;
using System.Reflection;
using SubjectNerd.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

[CustomPropertyDrawer(typeof(AbstractInterfaceObject), true)]
// ReSharper disable once CheckNamespace
public class InterfaceObjectEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type GetTargetType()
        {
            var parentObj = property.serializedObject.targetObject;
            var parentType = parentObj.GetType();
            var field = parentType.GetField(property.propertyPath,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null)
            {
                return null;
            }

            var fieldGenericTypes = field.FieldType.GenericTypeArguments;
            if (fieldGenericTypes.Length == 0)
            {
                return null;
            }

            return fieldGenericTypes[0];
        }

        var targetType = GetTargetType();

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var typeLabelRect = new Rect(position.x, position.y, position.width, position.height / 2f);
        EditorGUI.LabelField(typeLabelRect, $"Type: {targetType.Name}");

        var objectRect = new Rect(position.x, position.y + position.height / 2f, position.width, position.height / 2f);
        SerializedProperty targetObject = property.FindPropertyRelative("objTarget");
        EditorGUI.BeginChangeCheck();
        var oldValue = targetObject.objectReferenceValue;

        EditorGUI.ObjectField(objectRect, targetObject, typeof(Object), new GUIContent());
        if (EditorGUI.EndChangeCheck())
        {
            var v = targetObject.objectReferenceValue;
            if (v != null && v.GetType() != targetType)
            {
                if (v is GameObject g)
                {
                    var comp = g.GetComponent(targetType);
                    targetObject.objectReferenceValue = comp != null ? comp : oldValue;
                }
                else
                {
                    targetObject.objectReferenceValue = oldValue;
                }
            }
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2;
    }
}