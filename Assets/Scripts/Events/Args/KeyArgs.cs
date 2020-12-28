using System;

namespace BeauTambour
{
    public class KeyArgs<TKey> : EventArgs
    {
        public KeyArgs(TKey value) => this.value = value;
        
        public TKey Value => value;
        private TKey value;
    }
}