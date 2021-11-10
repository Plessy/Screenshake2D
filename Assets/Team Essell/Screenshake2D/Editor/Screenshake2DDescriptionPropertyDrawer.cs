using UnityEditor;
using UnityEngine;

namespace TeamEssell.Screenshake2D
{
    [CustomPropertyDrawer(typeof(Screenshake2DDescription))]
    public class Screenshake2DDescriptionPropertyDrawer : PropertyDrawer
    {
        protected SerializedProperty UseChannelProperty;
        protected SerializedProperty ChannelIDProperty;
        protected SerializedProperty ChannelModeProperty;

        protected SerializedProperty ShakeStrengthProperty;
        protected SerializedProperty DecayFlatRateProperty;
        protected SerializedProperty DecayPercentRateProperty;
        protected SerializedProperty CycleRateProperty;

        protected SerializedProperty UsePositionProperty;
        protected SerializedProperty PositionScaleProperty;
        protected SerializedProperty RailRotationRateProperty;

        protected SerializedProperty UseRotationProperty;
        protected SerializedProperty ScreenRotationRateProperty;

        protected SerializedProperty UseScaleProperty;
        protected SerializedProperty MaxScaleDifferenceProperty;
        
        protected SerializedProperty OverrideCurvesProperty;
        protected SerializedProperty UseSplitCurvesProperty;
        protected SerializedProperty CurveProperty;
        protected SerializedProperty PositionCurveProperty;
        protected SerializedProperty RotationCurveProperty;
        protected SerializedProperty ScaleCurveProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var title = EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), title);
            position.height = 16;

            FindAllProperties(property);
            ValidateCurves();
            DrawProperties(position);

            EditorGUI.EndProperty();
        }

        //I wonder if I can cache these? The setup seems to imply that all properties aren't given individual editor objects, making it impossible to cache.
        private void FindAllProperties(SerializedProperty property)
        {
            UseChannelProperty = property.FindPropertyRelative("UseChannel");
            ChannelIDProperty = property.FindPropertyRelative("ChannelID");
            ChannelModeProperty = property.FindPropertyRelative("ChannelMode");

            ShakeStrengthProperty = property.FindPropertyRelative("ShakeStrength");
            DecayFlatRateProperty = property.FindPropertyRelative("DecayFlatRate");
            DecayPercentRateProperty = property.FindPropertyRelative("DecayPercentRate");
            CycleRateProperty = property.FindPropertyRelative("CycleRate");

            UsePositionProperty = property.FindPropertyRelative("UsePosition");
            PositionScaleProperty = property.FindPropertyRelative("PositionScale");
            RailRotationRateProperty = property.FindPropertyRelative("RailRotationRate");

            UseRotationProperty = property.FindPropertyRelative("UseRotation");
            ScreenRotationRateProperty = property.FindPropertyRelative("ScreenRotationRate");

            UseScaleProperty = property.FindPropertyRelative("UseScale");
            MaxScaleDifferenceProperty = property.FindPropertyRelative("MaxScaleDifference");

            OverrideCurvesProperty = property.FindPropertyRelative("OverrideCurves");
            UseSplitCurvesProperty = property.FindPropertyRelative("UseSplitCurves");
            CurveProperty = property.FindPropertyRelative("Curve");
            PositionCurveProperty = property.FindPropertyRelative("PositionCurve");
            RotationCurveProperty = property.FindPropertyRelative("RotationCurve");
            ScaleCurveProperty = property.FindPropertyRelative("ScaleCurve");
        }

        private void FindBoolProperties(SerializedProperty property)
        {
            UseChannelProperty = property.FindPropertyRelative("UseChannel");
            UsePositionProperty = property.FindPropertyRelative("UsePosition");
            UseRotationProperty = property.FindPropertyRelative("UseRotation");
            UseScaleProperty = property.FindPropertyRelative("UseScale");
            OverrideCurvesProperty = property.FindPropertyRelative("OverrideCurves");
            UseSplitCurvesProperty = property.FindPropertyRelative("UseSplitCurves");
        }

        private void DrawProperties(Rect position)
        {
            //Channels
            DrawProperty(UseChannelProperty, ref position);

            if (UseChannelProperty.boolValue)
            {
                DrawProperty(ChannelIDProperty, ref position);
                DrawProperty(ChannelModeProperty, ref position);
            }

            //Shaking
            AddSpace(ref position);

            DrawProperty(ShakeStrengthProperty, ref position);
            DrawProperty(DecayFlatRateProperty, ref position);
            DrawProperty(DecayPercentRateProperty, ref position);
            DrawProperty(CycleRateProperty, ref position);

            //Position
            AddSpace(ref position);

            DrawProperty(UsePositionProperty, ref position);

            if (UsePositionProperty.boolValue)
            {
                DrawProperty(PositionScaleProperty, ref position);
                DrawProperty(RailRotationRateProperty, ref position);
            }

            //Rotation
            AddSpace(ref position);

            DrawProperty(UseRotationProperty, ref position);

            if (UseRotationProperty.boolValue)
            {
                DrawProperty(ScreenRotationRateProperty, ref position);
            }

            //Scale
            AddSpace(ref position);

            DrawProperty(UseScaleProperty, ref position);

            if (UseScaleProperty.boolValue)
            {
                DrawProperty(MaxScaleDifferenceProperty, ref position);
            }

            //Curves
            AddSpace(ref position);

            DrawProperty(OverrideCurvesProperty, ref position);

            if (OverrideCurvesProperty.boolValue)
            {
                DrawProperty(UseSplitCurvesProperty, ref position);

                if (!UseSplitCurvesProperty.boolValue)
                {
                    DrawProperty(CurveProperty, ref position);
                }
                else
                {
                    DrawProperty(PositionCurveProperty, ref position);
                    DrawProperty(RotationCurveProperty, ref position);
                    DrawProperty(ScaleCurveProperty, ref position);
                }
            }
        }

        private void DrawProperty(SerializedProperty property, ref Rect position)
        {
            EditorGUI.PropertyField(position, property, true);
            position.y += 20;
        }

        private void AddSpace(ref Rect position)
        {
            position.y += 20;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            FindBoolProperties(property);

            var numlines = 9;
            var numspaces = 5;

            if (UseChannelProperty.boolValue)
            {
                numlines += 2;
            }

            if (UsePositionProperty.boolValue)
            {
                numlines += 2;
            }

            if (UseRotationProperty.boolValue)
            {
                numlines += 1;
            }
            
            if (UseScaleProperty.boolValue)
            {
                numlines += 1;
            }

            if (OverrideCurvesProperty.boolValue)
            {
                numlines += 1;

                if (!UseSplitCurvesProperty.boolValue)
                {
                    numlines += 1;
                }
                else
                {
                    numlines += 3;
                }
            }

            return (numlines + numspaces) * 20;
        }

        private void ValidateCurves()
        {
            Screenshake2DHelper.ValidateCurve(CurveProperty);
            Screenshake2DHelper.ValidateCurve(PositionCurveProperty);
            Screenshake2DHelper.ValidateCurve(RotationCurveProperty);
            Screenshake2DHelper.ValidateCurve(ScaleCurveProperty);
        }
    }
}