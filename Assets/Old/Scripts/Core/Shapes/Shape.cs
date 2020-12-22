using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

namespace Deprecated
{
    //[IconIndicator(-3461654150298456965), CreateAssetMenu(fileName = "NewShape", menuName = "Beau Tambour/General/Shape")]
    public class Shape : ScriptableObject
    {
        public Emotion Emotion => emotion;
        [SerializeField] private Emotion emotion;
        
        public IReadOnlyList<Point> Points => points;
        [SerializeField] private List<Point> points = new List<Point>()
        {
            new Point(new Vector2(-1f, 0f), 0.125f),
            new Point(new Vector2(-0.25f, 0f), 0.125f),
            new Point(new Vector2(0f, 0.25f), 0.125f),
            new Point(new Vector2(0f, 1f), 0.125f),
        };

        public IReadOnlyList<Point> RuntimePoints => runtimePoints;
        private Point[] runtimePoints;

        #region Edition

        #if UNITY_EDITOR
        public void SetPointPosition(int index, Vector2 position)
        {
            var point = points[index];
            point.position = position;

            points[index] = point;
        }
        public void SetPointErrorRadius(int index, float radius)
        {
            var settings = BeauTambourSettings.GetOrCreateSettings();
            radius = Mathf.Clamp(radius, settings.ToleranceRadiusRange.x, settings.ToleranceRadiusRange.y);

            var point = points[index];
            point.toleranceRadius = radius;

            points[index] = point;
        }

        public void AddNewSection(Vector2 destination)
        {
            var settings = BeauTambourSettings.GetOrCreateSettings();
            
            var lastPoint = points.Last();
            var direction = destination - lastPoint.position;

            var firstTangent = lastPoint.position + direction * 0.25f;
            points.Add(new Point(firstTangent, settings.StandardToleranceRadius));

            var secondTangent = lastPoint.position + direction * 0.75f;
            points.Add(new Point(secondTangent, settings.StandardToleranceRadius));
        
            points.Add(new Point(destination, settings.StandardToleranceRadius));
        }
        public void InsertNewSection(Vector2 destination, int from)
        {
            var section = new Point[3];
            var settings = BeauTambourSettings.GetOrCreateSettings();
        
            var firstTangent = destination + (points[from + 1].position - destination) * 0.25f;
            section[0] = new Point(firstTangent, settings.StandardToleranceRadius);
            section[1] = new Point(destination, settings.StandardToleranceRadius);
        
            var secondTangent = destination + (points[from + 2].position - destination) * 0.25f;
            section[2] = new Point(secondTangent, settings.StandardToleranceRadius);
        
            points.InsertRange(from + 2, section);
        }
        public void DeleteSection(int index)
        {
            var range = Vector2Int.zero;
        
            if (index == 0) range = new Vector2Int(0,3);
            else if (index == points.Count - 1) range = new Vector2Int(index - 2, 3);
            else range = new Vector2Int(index - 1, 3);
        
            points.RemoveRange(range.x, range.y);
        }
        #endif
        
        #endregion

        #region Generation
        
        public Vector3[] GenerateCopy(int subDivision, float depth = 0f)
        {
            var results = new List<Vector3>();
            for (var i = 0; i < points.Count - 1; i += 3)
            {
                var p1 = points[i].position;
                var p2 = points[i + 1].position;
                var p3 = points[i + 2].position;
                var p4 = points[i + 3].position;

                for (float j = 0; j < subDivision; j++)
                {
                    var ratio = j / subDivision;
                
                    var position = Bezier.GetPoint(p1, p2, p3, p4, ratio);
                    results.Add(new Vector3(position.x, position.y, depth));
                }
            }

            var last = runtimePoints.Last().position;
            results.Add(new Vector3(last.x, last.y, depth));

            return results.ToArray();
        }
        public void GenerateRuntimePoints()
        {
            var settings = Repository.GetSingle<BeauTambourSettings>(Reference.Settings);
            var results = new List<Point>();
            
            for (var i = 0; i < points.Count - 1; i += 3)
            {
                var p1 = points[i].position;
                var p2 = points[i + 1].position;
                var p3 = points[i + 2].position;
                var p4 = points[i + 3].position;

                for (float j = 0; j < settings.CurveSubdivision; j++)
                {
                    var ratio = j / settings.CurveSubdivision;

                    var position = Bezier.GetPoint(p1, p2, p3, p4, ratio);
                    var errorRadius = Mathf.Lerp(points[i].toleranceRadius, points[i + 3].toleranceRadius, ratio);
                
                    results.Add(new Point(position, errorRadius * (settings.RadiusToleranceForgiveness + 1f)));
                }
            }

            var last = points.Last();
            last.toleranceRadius *= settings.RadiusToleranceForgiveness + 1f;
        
            results.Add(last);
            runtimePoints = results.ToArray();
        }
        #endregion
        
        public bool CanStartEvaluation(Vector2 position)
        {
            var firstPoint = points.First();
            var distance = Vector2.Distance(firstPoint.position, position);

            return distance <= firstPoint.toleranceRadius;
        }
        public ShapeAnalysis Evaluate(ShapeAnalysis analysis, Vector2 position)
        {
            var result = default(ShapeAnalysis);
            
            var p1 = runtimePoints[analysis.advancement].position;
            var p2 = runtimePoints[analysis.advancement + 1].position;

            var closest = position.ProjectOnto(p1, p2, out var code);
            var completionDistance = Vector2.Distance(position, p2);
            
            var settings = Repository.GetSingle<BeauTambourSettings>(Reference.Settings);
            if (code == 2 || completionDistance <= settings.ValidationRadius)
            {
                result = new ShapeAnalysis(0f, (analysis.advancement + 1f) / (runtimePoints.Length - 1f), 0f, true)
                {
                    position = position,
                    advancement = analysis.advancement + 1
                };
                result.SetSource(analysis.Source);
                return result;
            }
            
            var direction = (position - analysis.position).normalized;

            var headingError = Vector2.Dot(direction, (p2 - p1).normalized);
            if (headingError < 0) headingError = -headingError * 2f;

            headingError *= settings.HeadingErrorFactor;
            
            var localRatio = Vector2.Distance(p1, closest) / Vector2.Distance(p1, p2);
            var errorMargin = Mathf.Lerp(runtimePoints[analysis.advancement].toleranceRadius, runtimePoints[analysis.advancement + 1].toleranceRadius, localRatio);
            
            var globalRatio = Mathf.Lerp((float)analysis.advancement / (runtimePoints.Length - 1), (analysis.advancement + 1f) / (runtimePoints.Length - 1), localRatio);
            var distance = Vector2.Distance(position, closest);
            
            if (distance <= errorMargin) result = new ShapeAnalysis(localRatio, globalRatio, headingError, false);
            else
            {
                var spacingError = Mathf.Abs(distance - errorMargin) * settings.SpacingErrorFactor;
                result = new ShapeAnalysis(localRatio, globalRatio, headingError + spacingError, false);
            }

            result.position = position;
            result.advancement = analysis.advancement;

            result.SetSource(analysis.Source);
            return result;
        }
    }
}