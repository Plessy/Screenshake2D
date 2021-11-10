using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace TeamEssell.Screenshake2D
{
    [CustomEditor(typeof(Screenshake2D))]
    public class Screenshake2DEditor : Editor
    {
        protected SerializedProperty StrengthEndProperty;

        protected SerializedProperty UseChunkingProperty;
        protected SerializedProperty ChunkSizeProperty;

        protected SerializedProperty UseSplitCurvesProperty;
        protected SerializedProperty CurveProperty;
        protected SerializedProperty PositionCurveProperty;
        protected SerializedProperty RotationCurveProperty;
        protected SerializedProperty ScaleCurveProperty;

        protected SerializedProperty InitialPoolSizeProperty;
        protected SerializedProperty TrimOnUpdateProperty;

        protected SerializedProperty EnableTestingProperty;
        protected SerializedProperty UseScriptableObjectProperty;
        protected SerializedProperty TestingDataProperty;
        protected SerializedProperty TestingSOProperty;

        private bool _isinitialized;
        private SerializedObject _lastobject; //May be unnecessary? Just to be safe.
        
        public override void OnInspectorGUI()
        {
            InitializeProperties();
            //DebugPrintCurve();
            ValidateCurves();
            DrawProperties();
            DrawTesting();
            serializedObject.ApplyModifiedProperties();
        }

        private void InitializeProperties()
        {
            if (_isinitialized)
            {
                if (_lastobject == serializedObject)
                {
                    return;
                }
            }

            _isinitialized = true;
            _lastobject = serializedObject;

            StrengthEndProperty = serializedObject.FindProperty("StrengthEnd");

            UseChunkingProperty = serializedObject.FindProperty("UseChunking");
            ChunkSizeProperty = serializedObject.FindProperty("ChunkSize");

            UseSplitCurvesProperty = serializedObject.FindProperty("UseSplitCurves");
            CurveProperty = serializedObject.FindProperty("Curve");
            PositionCurveProperty = serializedObject.FindProperty("PositionCurve");
            RotationCurveProperty = serializedObject.FindProperty("RotationCurve");
            ScaleCurveProperty = serializedObject.FindProperty("ScaleCurve");

            InitialPoolSizeProperty = serializedObject.FindProperty("InitialPoolSize");
            TrimOnUpdateProperty = serializedObject.FindProperty("TrimOnUpdate");

            EnableTestingProperty = serializedObject.FindProperty("EnableTesting");
            UseScriptableObjectProperty = serializedObject.FindProperty("UseScriptableObject");
            TestingDataProperty = serializedObject.FindProperty("TestingData");
            TestingSOProperty = serializedObject.FindProperty("TestingSO");
    }

        private void ValidateCurves()
        {
            var apply = Screenshake2DHelper.ValidateCurve(CurveProperty);
            
            if (Screenshake2DHelper.ValidateCurve(PositionCurveProperty))
            {
                apply = true;
            }

            if (Screenshake2DHelper.ValidateCurve(RotationCurveProperty))
            {
                apply = true;
            }

            if (Screenshake2DHelper.ValidateCurve(ScaleCurveProperty))
            {
                apply = true;
            }

            if (apply)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DebugPrintCurve()
        {
            var curve = CurveProperty.animationCurveValue;
            
            foreach (var key in curve.keys)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Time: " + key.time);
                sb.AppendLine("Value: " + key.value);
                sb.AppendLine("In Tangent: " + key.inTangent);
                sb.AppendLine("Out Tangent: " + key.outTangent);
                sb.AppendLine("In Weight: " + key.inWeight);
                sb.AppendLine("Out Weight: " + key.outWeight);
                sb.AppendLine("Weighted Mode: " + key.weightedMode);
                
                Debug.Log(sb.ToString());
            }
        }
        
        private void DrawProperties()
        {
            //Strength End
            EditorGUILayout.PropertyField(StrengthEndProperty);

            //Chunking
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(UseChunkingProperty);

            if (UseChunkingProperty.boolValue)
            {
                EditorGUILayout.PropertyField(ChunkSizeProperty);
            }

            //Curves
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(UseSplitCurvesProperty);

            if (!UseSplitCurvesProperty.boolValue)
            {
                EditorGUILayout.PropertyField(CurveProperty);
            }
            else
            {
                EditorGUILayout.PropertyField(PositionCurveProperty);
                EditorGUILayout.PropertyField(RotationCurveProperty);
                EditorGUILayout.PropertyField(ScaleCurveProperty);
            }

            //Pooling
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(InitialPoolSizeProperty);
            EditorGUILayout.PropertyField(TrimOnUpdateProperty);

            if (!TrimOnUpdateProperty.boolValue)
            {
                EditorGUILayout.HelpBox("TrimOnUpdate is set to off. Trimming pools finished objects and should be done regularly as to not inflate the pool size. Call Trim() manually to perform this task yourself.", MessageType.Warning);
            }
        }

        private void DrawTesting()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(EnableTestingProperty);

            if (EnableTestingProperty.boolValue)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(UseScriptableObjectProperty);
                EditorGUILayout.Space();

                if (!UseScriptableObjectProperty.boolValue)
                {
                    EditorGUILayout.PropertyField(TestingDataProperty);
                }
                else
                {
                    EditorGUILayout.PropertyField(TestingSOProperty);
                }

                EditorGUILayout.Space();
                
                if (GUILayout.Button("Shake"))
                {
                    var shaker = (Screenshake2D) target;

                    var type = shaker.GetType();
                    var method = type.GetMethod("TestShake", BindingFlags.NonPublic | BindingFlags.Instance);
                    method.Invoke(shaker, null);
                }
            }
        }
    }
}