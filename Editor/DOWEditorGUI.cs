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
        private static readonly Color OverrideColor = default;

        static DOWEditorGUI()
        {
            if (OverrideColor == default)
            {
                var fieldInfo = typeof(EditorGUI).GetField("k_OverrideMarginColor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (fieldInfo != null && fieldInfo.FieldType == typeof(Color))
                {
                    var color = (Color) fieldInfo.GetValue(null);
                    OverrideColor = color;
                }
                else
                    OverrideColor = new Color(0.003921569f, 0.6f, 47f / 51f, 0.75f);
            }
        }

        /// <summary>
        /// MinMaxSliderInt with GUIContent.
        /// </summary>
        public static Vector2Int MinMaxSliderInt(Rect position, GUIContent content, Vector2Int value, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition = MinMaxSliderAttribute.DefaultMinFieldPosition, SliderFieldPosition maxValueFieldPosition = MinMaxSliderAttribute.DefaultMaxFieldPosition)
        {
            content.tooltip = string.Format(Format, value.x, value.y, value.y - value.x) + (string.IsNullOrEmpty(content.tooltip) ? "" : "\n" + content.tooltip);
            Rect newPosition = EditorGUI.PrefixLabel(position, content);
            return HandleMinMaxSliderInt(newPosition, value, minLimit, maxLimit, null, null, minValueFieldPosition, maxValueFieldPosition);
        }

        /// <summary>
        /// MinMaxSliderInt without a label.
        /// </summary>
        public static Vector2Int MinMaxSliderInt(Rect position, Vector2Int value, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition = MinMaxSliderAttribute.DefaultMinFieldPosition, SliderFieldPosition maxValueFieldPosition = MinMaxSliderAttribute.DefaultMaxFieldPosition) =>
            HandleMinMaxSliderInt(position, value, minLimit, maxLimit, null, null, minValueFieldPosition, maxValueFieldPosition);

        /// <summary>
        /// MinMaxSliderInt with label.
        /// </summary>
        public static Vector2Int MinMaxSliderInt(Rect position, string label, Vector2Int value, int minLimit, int maxLimit, SliderFieldPosition minValueFieldPosition = MinMaxSliderAttribute.DefaultMinFieldPosition, SliderFieldPosition maxValueFieldPosition = MinMaxSliderAttribute.DefaultMaxFieldPosition) =>
            MinMaxSliderInt(position, EditorGUIUtility.TrTempContent(label), value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);

        /// <summary>
        /// MinMaxSlider with GUIContent.
        /// </summary>
        public static Vector2 MinMaxSlider(Rect position, GUIContent content, Vector2 value, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition = MinMaxSliderAttribute.DefaultMinFieldPosition, SliderFieldPosition maxValueFieldPosition = MinMaxSliderAttribute.DefaultMaxFieldPosition)
        {
            content.tooltip = string.Format(Format, value.x, value.y, value.y - value.x) + (string.IsNullOrEmpty(content.tooltip) ? "" : "\n" + content.tooltip);
            Rect newPosition = EditorGUI.PrefixLabel(position, content);
            return HandleMinMaxSlider(newPosition, value, minLimit, maxLimit, null, null, minValueFieldPosition, maxValueFieldPosition);
        }

        /// <summary>
        /// MinMaxSlider without label.
        /// </summary>
        public static Vector2 MinMaxSlider(Rect position, Vector2 value, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition = MinMaxSliderAttribute.DefaultMinFieldPosition, SliderFieldPosition maxValueFieldPosition = MinMaxSliderAttribute.DefaultMaxFieldPosition) =>
            HandleMinMaxSlider(position, value, minLimit, maxLimit, null, null, minValueFieldPosition, maxValueFieldPosition);

        /// <summary>
        /// MinMaxSlider with label.
        /// </summary>
        public static Vector2 MinMaxSlider(Rect position, string label, Vector2 value, float minLimit, float maxLimit, SliderFieldPosition minValueFieldPosition = MinMaxSliderAttribute.DefaultMinFieldPosition, SliderFieldPosition maxValueFieldPosition = MinMaxSliderAttribute.DefaultMaxFieldPosition) =>
            MinMaxSlider(position, EditorGUIUtility.TrTempContent(label), value, minLimit, maxLimit, minValueFieldPosition, maxValueFieldPosition);

        /// <summary>
        /// MinMaxSlider with int values.
        /// </summary>
        private static Vector2Int HandleMinMaxSliderInt(Rect position, Vector2Int value, int minLimit, int maxLimit, SerializedProperty minFieldWrapper, SerializedProperty maxFieldWrapper, SliderFieldPosition minValueFieldPosition = MinMaxSliderAttribute.DefaultMinFieldPosition, SliderFieldPosition maxValueFieldPosition = MinMaxSliderAttribute.DefaultMaxFieldPosition)
        {
            var newValue = HandleMinMaxSlider(position, value, minLimit, maxLimit, minFieldWrapper, maxFieldWrapper, minValueFieldPosition, maxValueFieldPosition);
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
        /// MinMaxSlider with float values.
        /// </summary>
        /// <remarks>We have wrappers here so we can properly show overriden prefab values for min and max fields when using two serialized propertes instead of one.</remarks>
        private static Vector2 HandleMinMaxSlider(Rect position, Vector2 value, float minLimit, float maxLimit, SerializedProperty minFieldWrapper, SerializedProperty maxFieldWrapper, SliderFieldPosition minValueFieldPosition = MinMaxSliderAttribute.DefaultMinFieldPosition, SliderFieldPosition maxValueFieldPosition = MinMaxSliderAttribute.DefaultMaxFieldPosition)
        {
            Vector2 newValue = value;
            int prevIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float spacing = 5;
            // we add some additional space to the field to the right of the slider if we have wrappers, because the blue override bar needs more space
            float additionalRightSpace = (minValueFieldPosition == SliderFieldPosition.Right || maxValueFieldPosition == SliderFieldPosition.Right) && minFieldWrapper != null && maxFieldWrapper != null ? 3f : 0f;
            float fieldWidth = EditorGUIUtility.fieldWidth;
            int fieldsToShow = (minValueFieldPosition == SliderFieldPosition.None ? 0 : 1) + (maxValueFieldPosition == SliderFieldPosition.None ? 0 : 1);
            int leftFields = (minValueFieldPosition == SliderFieldPosition.Left ? 1 : 0) + (maxValueFieldPosition == SliderFieldPosition.Left ? 1 : 0);
            float requiredSpaceForFields = fieldsToShow * (fieldWidth + spacing) + additionalRightSpace;
            bool onlyShowFields = requiredSpaceForFields > position.width;
            if (onlyShowFields)
                fieldWidth = (position.width - additionalRightSpace - (spacing * (fieldsToShow - 1))) / fieldsToShow;
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
                        newValue.x = GetSliderAdjustedValue(newValue.x, minLimit, maxLimit, sliderWidth);
                    if (newValue.y != value.y)
                        newValue.y = GetSliderAdjustedValue(newValue.y, minLimit, maxLimit, sliderWidth);
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
                    propPos.x -= spacing;
                    propPos.width = 2;
                    if (minFieldWrapper.prefabOverride)
                        EditorGUI.DrawRect(propPos, OverrideColor);
                    propPos.width = spacing;
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
                    propPos.x -= spacing;
                    propPos.width = 2;
                    if (maxFieldWrapper.prefabOverride)
                        EditorGUI.DrawRect(propPos, OverrideColor);
                    propPos.width = spacing;
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
        /// Get adjusted value depending on slider length. This rounds to a nice number of digits.
        /// </summary>
        public static float GetSliderAdjustedValue(float val, float minLimit, float maxLimit, float sliderWidth)
        {
            float minDiff = Mathf.Abs((maxLimit - minLimit) / (sliderWidth - GUI.skin.horizontalSlider.padding.horizontal - GUI.skin.horizontalSliderThumb.fixedWidth));
            if (minDiff == 0f)
            {
                int digits = Mathf.Clamp((int)(5f - Mathf.Log10(Mathf.Abs(val))), 0, 15);
                val = (float)Math.Round(val, digits, MidpointRounding.AwayFromZero);
            }
            else
                val = (float)Math.Round(val, Mathf.Clamp(-Mathf.FloorToInt(Mathf.Log10(Mathf.Abs(minDiff))), 0, 15), MidpointRounding.AwayFromZero);
            return Mathf.Clamp(val, Mathf.Min(minLimit, maxLimit), Mathf.Max(minLimit, maxLimit));
        }
    }
}