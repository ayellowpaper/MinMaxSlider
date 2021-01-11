using UnityEngine;
using UnityEditor;
using System;

namespace DaleOfWinter.Tools.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Vector2)
                return;

            MinMaxSliderAttribute minMaxAttribute = (MinMaxSliderAttribute) attribute;

            Vector2 value = property.vector2Value;
            float newx = value.x;
            float newy = value.y;

            var propertyLabel = EditorGUI.BeginProperty(position, label, property);
            var valuePosition = EditorGUI.PrefixLabel(position, propertyLabel);
            float minValue = minMaxAttribute.Min;
            float maxValue = minMaxAttribute.Max;

            float spacing = 5;
            float fieldWidth = EditorGUIUtility.fieldWidth;
            int fieldsToShow = (minMaxAttribute.MinFieldPosition == SliderFieldPosition.None ? 0 : 1) + (minMaxAttribute.MaxFieldPosition == SliderFieldPosition.None ? 0 : 1);
            int leftFields = (minMaxAttribute.MinFieldPosition == SliderFieldPosition.Left ? 1 : 0) + (minMaxAttribute.MaxFieldPosition == SliderFieldPosition.Left ? 1 : 0);
            float requiredSpaceForFields = fieldsToShow * (fieldWidth + spacing);
            bool onlyShowFields = requiredSpaceForFields > valuePosition.width;
            if (onlyShowFields)
                fieldWidth = (valuePosition.width - (spacing * (fieldsToShow - 1))) / fieldsToShow;
            float sliderWidth = onlyShowFields ? 0f : valuePosition.width - requiredSpaceForFields;

            EditorGUI.BeginChangeCheck();
            if (!onlyShowFields)
            {
                Rect pos = new Rect(valuePosition);
                pos.width = sliderWidth;
                pos.x += leftFields * (fieldWidth + spacing);
                EditorGUI.BeginChangeCheck();
                EditorGUI.MinMaxSlider(pos, ref newx, ref newy, minMaxAttribute.Min, minMaxAttribute.Max);
                if (EditorGUI.EndChangeCheck())
                {
                    if (newx != value.x)
                    {
                        newx = GetAdjustedValue(newx, minValue, maxValue, sliderWidth);
                    }
                    if (newy != value.y)
                    {
                        newy = GetAdjustedValue(newy, minValue, maxValue, sliderWidth);
                    }
                }
            }

            if (minMaxAttribute.MinFieldPosition != SliderFieldPosition.None)
            {
                Rect pos = new Rect(valuePosition);
                pos.width = fieldWidth;
                pos.x += minMaxAttribute.MinFieldPosition == SliderFieldPosition.Left ? 0 : leftFields * (fieldWidth + spacing) + sliderWidth + spacing;
                EditorGUI.BeginChangeCheck();
                newx = EditorGUI.DelayedFloatField(pos, newx);
                if (EditorGUI.EndChangeCheck())
                {
                    newx = Mathf.Clamp(newx, minValue, maxValue);
                    if (newy < newx)
                        newy = newx;
                }
            }

            if (minMaxAttribute.MaxFieldPosition != SliderFieldPosition.None)
            {
                Rect pos = new Rect(valuePosition);
                pos.width = fieldWidth;
                pos.x += minMaxAttribute.MaxFieldPosition == SliderFieldPosition.Left ? (leftFields - 1) * (fieldWidth + spacing) : valuePosition.width - fieldWidth;
                EditorGUI.BeginChangeCheck();
                newy = EditorGUI.DelayedFloatField(pos, newy);
                if (EditorGUI.EndChangeCheck())
                {
                    newy = Mathf.Clamp(newy, minValue, maxValue);
                    if (newy < newx)
                        newx = newy;
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                property.vector2Value = new Vector2(newx, newy);
            }

            EditorGUI.EndProperty();
        }

        float GetAdjustedValue(float val, float minValue, float maxValue, float sliderWidth)
        {
            float f = (maxValue - minValue) / (sliderWidth - GUI.skin.horizontalSlider.padding.horizontal - GUI.skin.horizontalSliderThumb.fixedWidth);
            val = RoundBasedOnMinimumDifference(val, Mathf.Abs(f));
            return Mathf.Clamp(val, Mathf.Min(minValue, maxValue), Mathf.Max(minValue, maxValue));
        }

        float RoundBasedOnMinimumDifference(float valueToRound, float minDifference)
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