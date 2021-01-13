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
            if (property.propertyType == SerializedPropertyType.Float)
            {
                string maxVariableName = minMaxAttribute.MaxVariableName;
                if (string.IsNullOrEmpty(maxVariableName))
                {
                    EditorGUI.LabelField(position, label.text, "You need to use MaxVariableName when using int to supply the max value. This needs to be a serialized field of type float.");
                    return;
                }

                var maxProp = property.serializedObject.FindProperty(property.propertyPath.Replace(property.name, maxVariableName));
                if (maxProp == null)
                {
                    EditorGUI.LabelField(position, label.text, $"Variable {maxVariableName} needs to exist and needs to be serializable.");
                    return;
                }

                if (maxProp.propertyType != SerializedPropertyType.Float)
                {
                    EditorGUI.LabelField(position, label.text, $"Variable {maxVariableName} needs to be of type float.");
                    return;
                }

                DOWEditorGUI.DoMinMaxSlider(position, property, maxProp, minMaxAttribute.Min, minMaxAttribute.Max, minMaxAttribute.MinFieldPosition, minMaxAttribute.MaxFieldPosition, minMaxAttribute.DisplayName ?? property.displayName);
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {

            }
            else if (property.propertyType == SerializedPropertyType.Vector2)
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