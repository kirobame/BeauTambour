using System.Collections.Generic;
using Flux;

namespace BeauTambour
{
    public class ShapeAnalyzerResultEventArgs : SingleEventArgs<IEnumerable<ShapeAnalysis>>
    {
        public ShapeAnalyzerResultEventArgs(IEnumerable<ShapeAnalysis> value) : base(value) { }
    }
}