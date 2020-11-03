namespace BeauTambour
{
    public class ShapeAnalysisResult
    {
        public ShapeAnalysisResult(Shape shape, ShapeAnalysis analysis, bool hasBeenCompleted, bool isIncorrect)
        {
            Shape = shape;
            Analysis = analysis;
            
            HasBeenCompleted = hasBeenCompleted;
            IsIncorrect = isIncorrect;
        }
        
        public readonly Shape Shape;
        public readonly ShapeAnalysis Analysis;
        
        public readonly bool HasBeenCompleted;
        public readonly bool IsIncorrect;
    }
}