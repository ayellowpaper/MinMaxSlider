using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DaleOfWinter.Tools
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class MinMaxSliderAttribute : PropertyAttribute
    {
        public readonly float Min;
        public readonly float Max;
        public readonly SliderFieldPosition MinFieldPosition;
        public readonly SliderFieldPosition MaxFieldPosition;

        public MinMaxSliderAttribute(float min, float max, SliderFieldPosition minFieldPosition = SliderFieldPosition.Left, SliderFieldPosition maxFieldPosition = SliderFieldPosition.Right)
        {
            Min = min;
            Max = max;
            MinFieldPosition = minFieldPosition;
            MaxFieldPosition = maxFieldPosition;
        }
    }

    public enum SliderFieldPosition
    {
        None,
        Left,
        Right
    }
}