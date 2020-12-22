using System.Collections.Generic;
using Flux;

namespace Deprecated
{
    public class ShapeAnalyzerResultEventArgs : SingleEventArgs<IEnumerable<ShapeAnalysis>>
    {
        public ShapeAnalyzerResultEventArgs(IEnumerable<ShapeAnalysis> value) : base(value) { }
    }
}