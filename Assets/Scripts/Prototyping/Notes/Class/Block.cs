using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Orion;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeauTambour.Prototyping
{
    public class Block : Tilable, IBootable
    {
        public OrionEvent<Color,Shape> OnInitialization = new OrionEvent<Color,Shape>();
        public OrionEvent<double> OnBootUp = new OrionEvent<double>();
        public OrionEvent<double> OnShutdown = new OrionEvent<double>();
        public OrionEvent<double> OnMove = new OrionEvent<double>();
        
        public override object Link => this;
        
        public Color Color { get; private set; }
        public Shape Shape { get; private set; }
        
        int IBootable.Priority => 0;

        public void BootUp()
        {
            var shapes = Enum.GetValues(typeof(Shape)) as Shape[];
            Shape = shapes[Random.Range(1, shapes.Length)];
            
            var colors = Enum.GetValues(typeof(Color)) as Color[];
            Color = colors[Random.Range(0, colors.Length)];
            
            OnInitialization.Invoke(Color, Shape);
            Repository.Get<RythmHandler>().MakeStandardEnqueue(BootUp, 1);
        }
        private void BootUp(double time, double offset) => OnBootUp.Invoke(time / (1f - offset));

        public void ShutDown()
        {
            Repository.Get<PlayArea>().Unregister(this);
            var rythmHandler = Repository.Get<RythmHandler>();
        
            rythmHandler.MakePlainEnqueue(ShutDown, 2);
            rythmHandler.MakeStandardEnqueue(ShutDown, 1);
        }
        private void ShutDown(int beat, double offset)
        {
            if (beat == 2) gameObject.SetActive(false);
        }
        private void ShutDown(double time, double offset) => OnShutdown.Invoke(time / (1f - offset));

        public void Move(double ratio) => OnMove.Invoke(ratio);
    }
}