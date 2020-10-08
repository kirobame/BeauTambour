using UnityEngine;

namespace Orion
{
    /// <summary>
    /// Allows to affect any passed animator : (Animation playing, Parameter modification, etc...).
    /// </summary>
    public interface IAnimatorAffect
    {
        /// <summary>
        /// Plays the affect.
        /// </summary>
        /// <param name="animator">The animator to play the affect on.</param>
        void Play(Animator animator);
    }
}