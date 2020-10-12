using System.Collections;
using System.Collections.Generic;
using Orion;
using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Musician : Tilable, ITweenable<Vector2>
    {
        public override object Link => this;
        
        public Vector2 Start { get; private set; }
        public Vector2 End { get; private set; }
        
        [FoldoutGroup("Events")] public OrionEvent<double> OnMove = new OrionEvent<double>();
        [FoldoutGroup("Events")] public OrionEvent<double> OnShift = new OrionEvent<double>();
        [FoldoutGroup("Events")] public OrionEvent OnNotePrepared = new OrionEvent();
        [FoldoutGroup("Events")] public OrionEvent<Note> OnNotePlayed = new OrionEvent<Note>();

        [SerializeField] private Note leftNotePrefab;
        [SerializeField] private Note rightNotePrefab;
        
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

        public void PrepareNote()

        void ITweenable<Vector2>.Apply(Vector2 position)
        {
            Position = position;
            ActualizeTiling();
        }
    }
}