using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DaleOfWinter.Tools;

namespace DaleOfWinter.Tools.Editor
{
    public static partial class DOWEditorGUI
    {
        #region MinMaxSlider with SerializedProperty
        public static void MinMaxSliderInt(Rect position, SerializedProperty property, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition) =>
            MinMaxSliderInt(position, property, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, property.displayName);

        public static void MinMaxSliderInt(Rect position, SerializedProperty property, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, string label) =>
            MinMaxSliderInt(position, property, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, EditorGUIUtility.TrTempContent(label));

        public static void MinMaxSliderInt(Rect position, SerializedProperty property, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, GUIContent content)
        {
            var propertyLabel = EditorGUI.BeginProperty(position, content, property);
            Vector2Int value = property.vector2IntValue;
            EditorGUI.BeginChangeCheck();
            value = MinMaxSliderInt(position, propertyLabel, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);
            if (EditorGUI.EndChangeCheck())
            {
                property.vector2IntValue = value;
            }
            EditorGUI.EndProperty();
        }

        public static void MinMaxSlider(Rect position, SerializedProperty property, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition) =>
            MinMaxSlider(position, property, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, property.displayName);

        public static void MinMaxSlider(Rect position, SerializedProperty property, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, string label) => 
            MinMaxSlider(position, property, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, EditorGUIUtility.TrTempContent(label));

        public static void MinMaxSlider(Rect position, SerializedProperty property, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, GUIContent content)
        {
            var propertyLabel = EditorGUI.BeginProperty(position, content, property);
            Vector2 value = property.vector2Value;
            EditorGUI.BeginChangeCheck();
            value = MinMaxSlider(position, propertyLabel, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);
            if (EditorGUI.EndChangeCheck())
            {
                property.vector2Value = value;
            }
            EditorGUI.EndProperty();
        }
        #endregion

        #region MinMaxSlider with 2 SerializedProperties
        public static void MinMaxSliderInt(Rect position, SerializedProperty minProperty, SerializedProperty maxProperty, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition) =>
            MinMaxSliderInt(position, minProperty, maxProperty, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, minProperty.displayName);

        public static void MinMaxSliderInt(Rect position, SerializedProperty minProperty, SerializedProperty maxProperty, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, string label) =>
            MinMaxSliderInt(position, minProperty, maxProperty, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, EditorGUIUtility.TrTempContent(label));

        public static void MinMaxSliderInt(Rect position, SerializedProperty minProperty, SerializedProperty maxProperty, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, GUIContent content)
        {
            position = EditorGUI.PrefixLabel(position, content);
            Vector2Int value = new Vector2Int(minProperty.intValue, maxProperty.intValue);
            EditorGUI.BeginChangeCheck();
            value = HandleMinMaxSliderInt(position, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, minProperty, maxProperty);
            if (EditorGUI.EndChangeCheck())
            {
                minProperty.floatValue = value.x;
                maxProperty.floatValue = value.y;
            }
        }

        public static void MinMaxSlider(Rect position, SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition) =>
            MinMaxSlider(position, minProperty, maxProperty, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, minProperty.displayName);

        public static void MinMaxSlider(Rect position, SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, string label) =>
            MinMaxSlider(position, minProperty, maxProperty, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, EditorGUIUtility.TrTempContent(label));

        public static void MinMaxSlider(Rect position, SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, GUIContent content)
        {
            position = EditorGUI.PrefixLabel(position, content);
            Vector2 value = new Vector2(minProperty.floatValue, maxProperty.floatValue);
            EditorGUI.BeginChangeCheck();
            value = HandleMinMaxSlider(position, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition, minProperty, maxProperty);
            if (EditorGUI.EndChangeCheck())
            {
                minProperty.floatValue = value.x;
                maxProperty.floatValue = value.y;
            }
        }
        #endregion
    }
}