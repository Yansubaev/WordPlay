using UnityEngine;
using UnityEditor;

namespace Yans.UI.Editor
{
    [CustomPropertyDrawer(typeof(PrefabScreenInstantiator.OrientedScreenReference))]
    public class OrientedScreenReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Calculate rects
            float padding = 2f;
            float keyWidth = position.width * 0.4f;
            float valueWidth = position.width * 0.6f - padding;

            Rect keyRect = new Rect(position.x, position.y, keyWidth, position.height);
            Rect valueRect = new Rect(position.x + keyWidth + padding, position.y, valueWidth, position.height);

            // Draw fields
            var orientationProp = property.FindPropertyRelative("orientation");
            var screenProp = property.FindPropertyRelative("screen");

            EditorGUI.PropertyField(keyRect, orientationProp, GUIContent.none);
            EditorGUI.PropertyField(valueRect, screenProp, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}