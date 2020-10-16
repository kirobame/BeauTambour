using System.Collections;
using System.Collections.Generic;
using Orion;
using Shapes;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Musician : Tilable
    {
        public override object Link => this;
        
        public bool HasAlreadyPlayed { get; private set; }
        
        public OrionEvent<double> OnMove = new OrionEvent<double>();
        public OrionEvent<double> OnShift = new OrionEvent<double>();

        [SerializeField] private Color color;

        [Space, SerializeField] private Shape leftShape;
        [SerializeField] private Shape rightShape;
        [SerializeField] private Note notePrefab;

        private Note leftNote;
        private Note rightNote;
        
        private bool isShifting;

        void Start()
        {
            Repository.Get<RoundHandler>()[PhaseType.Setup].OnStart += () => HasAlreadyPlayed = false; 
            
            leftNote = Initialize(leftShape);
            rightNote = Initialize(rightShape);
            
            Note Initialize(Shape shape)
            {
                var note = Instantiate(notePrefab);
                note.Initialize(shape, color, Tile.Index);

                return note;
            }
        }

        public void PrepareShift(int direction)
        {
            var playArea = Repository.Get<PlayArea>();
            var index = Tile.Index + Vector2Int.up * direction;

            Onset = Tile.Position;
            isShifting = true;
            
            if (index.y < 0) Outset = playArea[0, playArea.Size.y - 1].Position;
            else if (index.y >= playArea.Size.y) Outset = playArea[0, 0].Position;
            else
            {
                Outset = playArea[index].Position;
                isShifting = false;
            }
        }
        public void Shift(double ratio)
        {
            if (isShifting) OnShift.Invoke(ratio);
            else OnMove.Invoke(ratio);
        }

        public void PlayNote(int selection)
        {
            var note = selection < 0 ? leftNote : rightNote;
            
            note.Position = Tile.Position;
            note.ActualizeTiling();
            note.Activate();

            HasAlreadyPlayed = true;
        }
    }
}