using System.Collections.Generic;
using Flux;

namespace BeauTambour
{
    public class ShapeAnalysisResultsEventArgs : SingleEventArgs<IEnumerable<ShapeAnalysisResult>>
    {
        public ShapeAnalysisResultsEventArgs(IEnumerable<ShapeAnalysisResult> value) : base(value) { }
    }
}