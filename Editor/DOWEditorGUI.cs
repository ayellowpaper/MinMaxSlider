using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace DaleOfWinter.Tools.Editor
{
    public static partial class DOWEditorGUI
    {
        public const string SliderControlName = "MinMaxSlider";
        private const string Format = "Min: {0}\nMax: {1}\nRange: {2}";

        /// <summary>
        /// MinMaxSlider with int values and label.
        /// </summary>
        public static Vector2Int DoIntMinMaxSlider(Rect position, GUIContent content, Vector2Int value, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition)
        {
            content.tooltip = string.Format(Format, value.x, value.y, value.y - value.x) + (string.IsNullOrEmpty(content.tooltip) ? "" : "\n" + content.tooltip);
            Rect newPosition = EditorGUI.PrefixLabel(position, content);
            return DoIntMinMaxSlider(newPosition, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);
        }

        /// <summary>
        /// MinMaxSlider with int values.
        /// </summary>
        public static Vector2Int DoIntMinMaxSlider(Rect position, Vector2Int value, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition)
        {
            var newValue = DoMinMaxSlider(position, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);
            Vector2Int actualNewValue = new Vector2Int(Mathf.RoundToInt(newValue.x), Mathf.RoundToInt(newValue.y));
            // after dragging both values with the slider at the same time it's possible that the distance get's messed up because of rounding issues, so we check for that here
            int wantedDistance = Mathf.RoundToInt(newValue.y - newValue.x);
            int currentDistance = actualNewValue.y - actualNewValue.x;
            if (wantedDistance != currentDistance)
            {
                int requiredAdjustment = wantedDistance - currentDistance;
                if (actualNewValue.y + requiredAdjustment <= maxLimit)
                    actualNewValue.y += requiredAdjustment;
                else
                    actualNewValue.x += requiredAdjustment;
            }
            return actualNewValue;
        }

        /// <summary>
        /// MinMaxSlider with float values and label.
        /// </summary>
        public static Vector2 DoMinMaxSlider(Rect position, GUIContent content, Vector2 value, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition)
        {
            content.tooltip = string.Format(Format, value.x, value.y, value.y - value.x) + (string.IsNullOrEmpty(content.tooltip) ? "" : "\n" + content.tooltip);
            Rect newPosition = EditorGUI.PrefixLabel(position, content);
            return DoMinMaxSlider(newPosition, value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);
        }

        /// <summary>
        /// MinMaxSlider with float values.
        /// </summary>
        private static Vector2 DoMinMaxSlider(Rect position, Vector2 value, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition, SliderFieldPosition maxValueFieldPosition, SerializedProperty minFieldWrapper = null, SerializedProperty maxFieldWrapper = null)
        {
            Vector2 newValue = value;
            int prevIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

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
                GUI.SetNextControlName(SliderControlName);
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
                    GUI.FocusControl(SliderControlName);
                }
            }

            if (minValueFieldPosition != SliderFieldPosition.None)
            {
                Rect pos = new Rect(position);
                pos.width = fieldWidth;
                pos.x += minValueFieldPosition == SliderFieldPosition.Left ? 0 : leftFields * (fieldWidth + spacing) + sliderWidth + spacing;
                EditorGUI.BeginChangeCheck();
                if (minFieldWrapper != null)
                {
                    Rect propPos = pos;
                    propPos.x -= 16;
                    EditorGUI.BeginProperty(propPos, GUIContent.none, minFieldWrapper);
                }
                newValue.x = EditorGUI.FloatField(pos, newValue.x);
                if (minFieldWrapper != null)
                    EditorGUI.EndProperty();
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
                if (maxFieldWrapper != null)
                {
                    Rect propPos = pos;
                    propPos.x -= 16;
                    EditorGUI.BeginProperty(propPos, GUIContent.none, maxFieldWrapper);
                }
                newValue.y = EditorGUI.FloatField(pos, newValue.y);
                if (maxFieldWrapper != null)
                    EditorGUI.EndProperty();
                if (EditorGUI.EndChangeCheck())
                {
                    newValue.y = Mathf.Clamp(newValue.y, minLimit, maxLimit);
                    if (newValue.y < newValue.x)
                        newValue.x = newValue.y;
                }
            }

            EditorGUI.indentLevel = prevIndentLevel;
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