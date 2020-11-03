
using UnityEngine;

namespace BeauTambour
{
    public struct ShapeAnalysis
    {
        public ShapeAnalysis(float error, float localRatio, float globalRatio, bool goToNext)
        {
            Error = error;
            
            LocalRatio = localRatio;
            GlobalRatio = globalRatio;

            GoToNext = goToNext;

            position = Vector2.zero;
            index = 0;
        }

        public float Error { get; private set; }
        
        public float LocalRatio { get; private set; }
        public float GlobalRatio { get; private set; }

        public bool GoToNext { get; private set; }

        public Vector2 position;
        public int index;

        public void SetData(float error, float localRatio, float globalRatio, bool goToNext)
        {
            Error = error;
            
            LocalRatio = localRatio;
            GlobalRatio = globalRatio;

            GoToNext = goToNext;
        }
        public ShapeAnalysis Copy()
        {
            var copy = new ShapeAnalysis(Error, LocalRatio, GlobalRatio, GoToNext);
            return copy;
        }
    }
}