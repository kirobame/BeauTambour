using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BeauTambour
{
    public class BeauTambourSettings : ScriptableObject
    {
        public const string path = "Assets/Settings/BeauTambourSettings.asset";

        public double RythmMarginTolerance => rythmMarginTolerance;
        [SerializeField] private double rythmMarginTolerance = 0.01f;

        public Vector2 ToleranceRadiusRange => toleranceRadiusRange;
        [SerializeField] private Vector2 toleranceRadiusRange = new Vector2(0.05f, 0.3f);
        public float StandardToleranceRadius => standardToleranceRadius;
        [SerializeField] private float standardToleranceRadius = 0.05f;
        public float RadiusToleranceForgiveness => radiusToleranceForgiveness;
        [SerializeField] private float radiusToleranceForgiveness = 0f;

        public int CurveSubdivision => curveSubdivision;
        [SerializeField] private int curveSubdivision = 1;
        
        public float RecognitionErrorThreshold => recognitionErrorThreshold;
        [SerializeField] private float recognitionErrorThreshold = 0.5f;
        public float HeadingErrorFactor => headingErrorFactor;
        [SerializeField] private float headingErrorFactor = 0.01f;
        public float SpacingErrorFactor => spacingErrorFactor;
        [SerializeField] private float spacingErrorFactor = 0.01f;

        public float ValidationRadius => validationRadius;
        [SerializeField] private float validationRadius;
        public float CompletionQuota => completionQuota;
        [SerializeField] private float completionQuota;
        
        #if UNITY_EDITOR
        
        public static BeauTambourSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<BeauTambourSettings>(path);
            if (settings == null) settings = CreateSettings();
        
            return settings;
        }
        public static BeauTambourSettings CreateSettings()
        {
            var settings = CreateInstance<BeauTambourSettings>();
            AssetDatabase.CreateAsset(settings, path);
            AssetDatabase.SaveAssets();

            return settings;
        }
        public static SerializedObject GetSerializedSettings() => new SerializedObject(GetOrCreateSettings());
        
        #endif
    }
}