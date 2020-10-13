using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Bloc : Tilable, IBootable
    {
        public override object Link => this;
        public Shape Shape => shape;
        
        [Space, SerializeField] private Shape shape;
        
        int IBootable.Priority => 0;

        public void BootUp()
        {
            
        }

        public void ShutDown()
        {
            Destroy(this.gameObject);
        }
    }
}