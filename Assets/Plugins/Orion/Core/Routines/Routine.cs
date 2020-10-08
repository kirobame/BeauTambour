using System.Collections;
using System.Collections.Generic;

namespace Orion
{
    /// <summary>
    /// Represents a self-inbricating sequence of asynchronous methods.
    /// </summary>
    public abstract class Routine
    {
        /// <summary>
        /// Get the whole sequence.
        /// </summary>
        public IEnumerator Call 
        {
            get
            {
                var call = GetCall();
                foreach (var link in chain) call = Chain(call, link);

                return call;
            }
        }
    
        private List<IEnumerator> chain = new List<IEnumerator>();
    
        protected abstract IEnumerator GetCall();

        /// <summary>
        /// Adds a new routine to the sequence.
        /// </summary>
        public void Append(Routine routine) => chain.Add(routine.GetCall());
        /// <summary>
        /// Adds a new routine to the sequence.
        /// </summary>
        public void Append(IEnumerator routine) => chain.Add(routine);

        private IEnumerator Chain(IEnumerator source, IEnumerator routine)
        {
            yield return source;
            yield return routine;
        }
    }
}