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
            MinMaxSliderAttribute minMaxAttribute = (MinMaxSliderAttribute)attribute;
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                DOWEditorGUI.DoMinMaxSlider(position, property, minMaxAttribute.Min, minMaxAttribute.Max, minMaxAttribute.MinFieldPosition, minMaxAttribute.MaxFieldPosition);
            }
            else if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                DOWEditorGUI.DoIntMinMaxSlider(position, property, Mathf.RoundToInt(minMaxAttribute.Min), Mathf.RoundToInt(minMaxAttribute.Max), minMaxAttribute.MinFieldPosition, minMaxAttribute.MaxFieldPosition);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use MinMaxSlider with Vector2 or Vector2Int.");
            }
        }
    }
}