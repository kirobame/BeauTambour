using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour
{
    public class IndexArgs : EventArgs
    {
        public IndexArgs(int value)
        {
            if (value < 0) value = 0;
            this.value = value;
        }

        public int Value => value;
        private int value;
    }
}