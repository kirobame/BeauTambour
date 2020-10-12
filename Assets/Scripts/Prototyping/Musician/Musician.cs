using System.Collections;
using System.Collections.Generic;
using Orion;
using Shapes;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Musician : Tilable, ITweenable<Vector2>
    {
        public override object Link => this;
        
        public Vector2 Start { get; private set; }
        public Vector2 End { get; private set; }
        
        public OrionEvent<double> OnMove = new OrionEvent<double>();
        public OrionEvent<double> OnShift = new OrionEvent<double>();
        
        private bool isShifting;
        
        public void PrepareShift(int direction)
        {
            var playArea = Repository.Get<PlayArea>();
            var index = Tile.Index + Vector2Int.up * direction;

            Start = Tile.Position;
            isShifting = true;
            
            if (index.y < 0) End = playArea[0, playArea.Size.y - 1].Position;
            else if (index.y >= playArea.Size.y) End = playArea[0, 0].Position;
            else
            {
                End = playArea[index].Position;
                isShifting = false;
            }
        }

        public void Shift(double ratio)
        {
            if (isShifting) OnShift.Invoke(ratio);
            else OnMove.Invoke(ratio);
        }

        void ITweenable<Vector2>.Apply(Vector2 position)
        {
            Position = position;
            ActualizeTiling();
        }
    }
}