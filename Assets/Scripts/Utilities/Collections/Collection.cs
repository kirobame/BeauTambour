using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour
{
    public abstract class Collection<T> : ScriptableObject where T: UnityEngine.Object
    {
        public T this[int index] => values[index];
        
        public IReadOnlyList<T> Values => values;
        [SerializeField] protected T[] values;
    }

    [CreateAssetMenu(fileName = "NewAudioCollection", menuName = "Beau Tambour/General/Collections/Audio")]
    public class AudioCollection : Collection<AudioClip> { }
}