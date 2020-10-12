using Orion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Note : Tilable, IResolvable
    {
        public override object Link => this;
        
        [Space, SerializeField] private Shape shapesMatched;
        
        int IResolvable.Priority => 0;
        
        void Start() => Repository.Get<ResolutionPhase>().TryEnqueue(this);
        
        public void Resolve()
        {
            var playArea = Repository.Get<PlayArea>();
            for (var x = Tile.Index.x + 1; x < playArea.Size.x; x++)
            {
                if (!playArea[x, Tile.Index.y][TilableType.NoteRecipient].Any()) continue;
                
                var block = playArea[x, Tile.Index.y][TilableType.NoteRecipient].First().Link as Bloc;
                if ((block.Shape & shapesMatched) == block.Shape)
                {
                    Debug.Log("MATCH!");
                }
                else
                {
                    Debug.Log("NO MATCH!");
                }
                    
                return;
            }
            
            Debug.Log("NOTHING!");
        }
    }
}