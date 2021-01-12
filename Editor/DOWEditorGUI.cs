using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace DaleOfWinter.Tools.Editor
{
    public static class DOWEditorGUI
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

        /// <summary>
        /// MinMaxSlider with int values and label.
        /// </summary>
        public static Vector2Int DoIntMinMaxSlider(Rect position, GUIContent content, Vector2Int value, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition)
        {
            Rect newPosition = EditorGUI.PrefixLabel(position, content);
            return DoIntMinMaxSlider(newPosition, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);
        }

        /// <summary>
        /// MinMaxSlider with int values.
        /// </summary>
        public static Vector2Int DoIntMinMaxSlider(Rect position, Vector2Int value, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition)
        {
            var newValue = DoMinMaxSlider(position, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);
            return new Vector2Int(Mathf.RoundToInt(newValue.x), Mathf.RoundToInt(newValue.y));
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

        /// <summary>
        /// MinMaxSlider with float values and label.
        /// </summary>
        public static Vector2 DoMinMaxSlider(Rect position, GUIContent content, Vector2 value, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition)
        {
            Rect newPosition = EditorGUI.PrefixLabel(position, content);
            return DoMinMaxSlider(newPosition, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);
        }

        /// <summary>
        /// MinMaxSlider with float values.
        /// </summary>
        public static Vector2 DoMinMaxSlider(Rect position, Vector2 value, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition)
        {
            Vector2 newValue = value;

            float spacing = 5;
            float fieldWidth = EditorGUIUtility.fieldWidth;
            int fieldsToShow = (minValueFieldPosition == SliderFieldPosition.None ? 0 : 1) + (maxValueFieldPosition == SliderFieldPosition.None ? 0 : 1);
            int leftFields = (minValueFieldPosition == SliderFieldPosition.Left ? 1 : 0) + (maxValueFieldPosition == SliderFieldPosition.Left ? 1 : 0);
            float requiredSpaceForFields = fieldsToShow * (fieldWidth + spacing);
            bool onlyShowFields = requiredSpaceForFields > position.width;
            if (onlyShowFields)
                fieldWidth = (position.width - (spacing * (fieldsToShow - 1))) / fieldsToShow;
            float sliderWidth = onlyShowFields ? 0f : position.width - requiredSpaceForFields;

            if (!onlyShowFields)
            {
                Rect pos = new Rect(position);
                pos.width = sliderWidth;
                pos.x += leftFields * (fieldWidth + spacing);
                EditorGUI.BeginChangeCheck();
                EditorGUI.MinMaxSlider(pos, ref newValue.x, ref newValue.y, minLimit, maxLimit);
                if (EditorGUI.EndChangeCheck())
                {
                    if (newValue.x != value.x)
                    {
                        newValue.x = GetAdjustedValue(newValue.x, minLimit, maxLimit, sliderWidth);
                    }
                    if (newValue.y != value.y)
                    {
                        newValue.y = GetAdjustedValue(newValue.y, minLimit, maxLimit, sliderWidth);
                    }
                }
            }

            if (minValueFieldPosition != SliderFieldPosition.None)
            {
                Rect pos = new Rect(position);
                pos.width = fieldWidth;
                pos.x += minValueFieldPosition == SliderFieldPosition.Left ? 0 : leftFields * (fieldWidth + spacing) + sliderWidth + spacing;
                EditorGUI.BeginChangeCheck();
                newValue.x = EditorGUI.DelayedFloatField(pos, newValue.x);
                if (EditorGUI.EndChangeCheck())
                {
                    newValue.x = Mathf.Clamp(newValue.x, minLimit, maxLimit);
                    if (newValue.y < newValue.x)
                        newValue.y = newValue.x;
                }
            }

            if (maxValueFieldPosition != SliderFieldPosition.None)
            {
                Rect pos = new Rect(position);
                pos.width = fieldWidth;
                pos.x += maxValueFieldPosition == SliderFieldPosition.Left ? (leftFields - 1) * (fieldWidth + spacing) : position.width - fieldWidth;
                EditorGUI.BeginChangeCheck();
                newValue.y = EditorGUI.DelayedFloatField(pos, newValue.y);
                if (EditorGUI.EndChangeCheck())
                {
                    newValue.y = Mathf.Clamp(newValue.y, minLimit, maxLimit);
                    if (newValue.y < newValue.x)
                        newValue.x = newValue.y;
                }
            }

            return new Vector2(newValue.x, newValue.y);
        }

        /// <summary>
        /// 
        /// </summary>
        public static float GetAdjustedValue(float val, float minLimit, float maxLimit, float sliderWidth)
        {
            float f = (maxLimit - minLimit) / (sliderWidth - GUI.skin.horizontalSlider.padding.horizontal - GUI.skin.horizontalSliderThumb.fixedWidth);
            val = RoundBasedOnMinimumDifference(val, Mathf.Abs(f));
            return Mathf.Clamp(val, Mathf.Min(minLimit, maxLimit), Mathf.Max(minLimit, maxLimit));
        }

        /// <summary>
        /// 
        /// </summary>
        public static float RoundBasedOnMinimumDifference(float valueToRound, float minDifference)
        {
            if (minDifference == 0f)
            {
                int digits = Mathf.Clamp((int)(5f - Mathf.Log10(Mathf.Abs(valueToRound))), 0, 15);
                return (float)Math.Round(valueToRound, digits, MidpointRounding.AwayFromZero);
            }
            return (float)Math.Round(valueToRound, Mathf.Clamp(-Mathf.FloorToInt(Mathf.Log10(Mathf.Abs(minDifference))), 0, 15), MidpointRounding.AwayFromZero);
        }
    }
}