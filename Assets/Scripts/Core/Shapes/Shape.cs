using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-3461654150298456965), CreateAssetMenu(fileName = "NewShape", menuName = "Beau Tambour/Shape")]
    public class Shape : ScriptableObject
    {
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
        #endregion

        public Vector3[] Define(int subDivision, float depth = 0f)
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
        
        public void GenerateRuntimeData()
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
        
        public bool CanStartEvaluation(Vector2 position)
        {
            var firstPoint = points.First();
            var distance = Vector2.Distance(firstPoint.position, position);

            return distance <= firstPoint.toleranceRadius;
        }
        public ShapeAnalysis Evaluate(ShapeAnalysis analysis, Vector2 position)
        {
            var result = default(ShapeAnalysis);
            
            var p2 = runtimePoints[analysis.index + 1].position;
            var p2Distance = Vector2.Distance(position, p2);
            
            if (p2Distance <= runtimePoints[analysis.index + 1].toleranceRadius)
            {
                result = new ShapeAnalysis(0f, 0f, (analysis.index + 1f) / (runtimePoints.Length - 1), true)
                {
                    position = position,
                    index = analysis.index + 1
                };
                return result;
            }
            var settings = Repository.GetSingle<BeauTambourSettings>(Reference.Settings);
            
            var direction = (position - analysis.position).normalized;
            var p1 = runtimePoints[analysis.index].position;
            
            var headingError = Mathf.Clamp01(Vector2.Dot(direction, (p2 - p1).normalized) * -1) * settings.HeadingErrorFactor;
            result.position = position;
            result.index = analysis.index;

            var closest = position.ProjectOnto(p1, p2);
            
            var ratio = (closest - p1).magnitude / (p2 - p1).magnitude;
            var errorMargin = Mathf.Lerp(runtimePoints[analysis.index].toleranceRadius, runtimePoints[analysis.index + 1].toleranceRadius, ratio);
            
            var localRatio = Vector2.Distance(p1, closest) / Vector2.Distance(p1, p2);
            var globalRatio = Mathf.Lerp((float)analysis.index / (runtimePoints.Length - 1), (analysis.index + 1f) / (runtimePoints.Length - 1), localRatio);

            var distance = Vector2.Distance(position, closest);
            if (distance <= errorMargin)
            {
                result.SetData(headingError, localRatio, globalRatio, false);
                return result;
            }
            else
            {
                var spacingError = Mathf.Abs(distance - errorMargin) * settings.SpacingErrorFactor;
                result.SetData(headingError + spacingError, localRatio, globalRatio, false);
                    
                return result;
            }
        }
    }
}