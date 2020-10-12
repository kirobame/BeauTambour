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
        
        public OrionEvent<double> OnMove = new OrionEvent<double>();
        public OrionEvent<double> OnShift = new OrionEvent<double>();

        [SerializeField] private Note leftNote;
        [SerializeField] private Note rightNote;
        
        private bool isShifting;

        void Start()
        {
            leftNote.Place(Tile.Index);
            rightNote.Place(Tile.Index);
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
            
            note.gameObject.SetActive(true);
            note.transform.SetParent(null);
            note.Position = Tile.Position;
            note.ActualizeTiling();
            
            note.Activate();
        }
    }
}