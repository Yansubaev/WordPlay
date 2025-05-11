using Source.UI.Transitions;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Source.UI.Editor
{
    [CustomPropertyDrawer(typeof(ScreenTransitionEntry))]
    public class ScreenTransitionEntryDrawer : PropertyDrawer
    {
        #region private fields
        private static Type[] _cachedTypes;
        #endregion

        #region public methods

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty fromProp = property.FindPropertyRelative("From");
            SerializedProperty toProp = property.FindPropertyRelative("To");
            SerializedProperty typeProp = property.FindPropertyRelative("Type");

            EditorGUI.BeginProperty(position, label, property);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            Rect fromRect = new Rect(position.x, position.y, position.width, lineHeight);
            Rect toRect = new Rect(position.x, position.y + lineHeight + spacing, position.width, lineHeight);
            Rect typeRect = new Rect(position.x, position.y + 2 * (lineHeight + spacing), position.width, lineHeight);

            DrawTypeDropdown(fromRect, fromProp, "From");
            DrawTypeDropdown(toRect, toProp, "To");

            EditorGUI.PropertyField(typeRect, typeProp);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 3 * EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;
        }

        #endregion

        #region private methods

        private static Type[] GetAllowedTypes()
        {
            if (_cachedTypes == null)
            {
                _cachedTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type =>
                        !type.IsAbstract &&
                        typeof(UIScreenType).IsAssignableFrom(type))
                    .ToArray();
            }
            return _cachedTypes;
        }

        private void DrawTypeDropdown(Rect rect, SerializedProperty prop, string label)
        {
            UIScreenType current = prop.managedReferenceValue as UIScreenType;

            var types = GetAllowedTypes();
            string[] displayNames = types.Select(t =>
            {
                var instance = Activator.CreateInstance(t) as UIScreenType;
                return instance?.GetScreenType().Name ?? t.Name;
            }).ToArray();

            int currentIndex = Array.FindIndex(types, t => t == current?.GetType());

            int selected = EditorGUI.Popup(rect, label, currentIndex, displayNames);
            if (selected >= 0 && (current == null || current.GetType() != types[selected]))
            {
                prop.managedReferenceValue = Activator.CreateInstance(types[selected]) as UIScreenType;
            }
        }

        #endregion
    }
}