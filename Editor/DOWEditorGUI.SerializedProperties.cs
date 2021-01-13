using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DaleOfWinter.Tools;

namespace DaleOfWinter.Tools.Editor
{
    public static partial class DOWEditorGUI
    {
        public static void DoIntMinMaxSlider(Rect position, SerializedProperty property, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition)
        {
            DoIntMinMaxSlider(position, property, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, property.displayName);
        }

        public static void DoIntMinMaxSlider(Rect position, SerializedProperty property, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, string label)
        {
            DoIntMinMaxSlider(position, property, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, EditorGUIUtility.TrTempContent(label));
        }

        public static void DoIntMinMaxSlider(Rect position, SerializedProperty property, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, GUIContent content)
        {
            var propertyLabel = EditorGUI.BeginProperty(position, content, property);
            Vector2Int value = property.vector2IntValue;
            EditorGUI.BeginChangeCheck();
            value = DoIntMinMaxSlider(position, propertyLabel, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);
            if (EditorGUI.EndChangeCheck())
            {
                property.vector2IntValue = value;
            }
            EditorGUI.EndProperty();
        }

        public static void DoMinMaxSlider(Rect position, SerializedProperty property, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition)
        {
            DoMinMaxSlider(position, property, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, property.displayName);
        }

        public static void DoMinMaxSlider(Rect position, SerializedProperty property, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, string label)
        {
            DoMinMaxSlider(position, property, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, EditorGUIUtility.TrTempContent(label));
        }

        public static void DoMinMaxSlider(Rect position, SerializedProperty property, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, GUIContent content)
        {
            var propertyLabel = EditorGUI.BeginProperty(position, content, property);
            Vector2 value = property.vector2Value;
            EditorGUI.BeginChangeCheck();
            value = DoMinMaxSlider(position, propertyLabel, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);
            if (EditorGUI.EndChangeCheck())
            {
                property.vector2Value = value;
            }
            EditorGUI.EndProperty();
        }

        public static void DoMinMaxSlider(Rect position, SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition)
        {
            DoMinMaxSlider(position, minProperty, maxProperty, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, minProperty.displayName);
        }

        public static void DoMinMaxSlider(Rect position, SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, string label)
        {
            DoMinMaxSlider(position, minProperty, maxProperty, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, EditorGUIUtility.TrTempContent(label));
        }

        public static void DoMinMaxSlider(Rect position, SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, GUIContent content)
        {
            position = EditorGUI.PrefixLabel(position, content);
            Vector2 value = new Vector2(minProperty.floatValue, maxProperty.floatValue);
            EditorGUI.BeginChangeCheck();
            value = DoMinMaxSlider(position, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, minProperty, maxProperty);
            if (EditorGUI.EndChangeCheck())
            {
                minProperty.floatValue = value.x;
                maxProperty.floatValue = value.y;
            }
        }
    }
}