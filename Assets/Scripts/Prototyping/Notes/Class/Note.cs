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
        public Shape Shape => shape;
        
        public OrionEvent<UnityEngine.Color> OnInitialization = new OrionEvent<UnityEngine.Color>();
        public OrionEvent<double> OnActivation = new OrionEvent<double>();

        [Space, SerializeField] private ColorRegistry colorRegistry;

        private Shape shape;
        
        int IResolvable.Priority => 0;

        public void Initialize(Shape shape, Color color, Vector2Int index)
        {
            this.shape = shape;
            
            OnInitialization.Invoke(colorRegistry[color]);
            Place(index);
        }
        
        public void Activate()
        {
            gameObject.SetActive(true);
            
            Onset = Tile.Position;
            Outset = Repository.Get<PlayArea>()[Tile.Index + Vector2Int.right].Position;
            
            Repository.Get<ResolutionPhase>().TryEnqueue(this);
            Repository.Get<RythmHandler>().MakeStandardEnqueue(ResolveTime, 1);
        }

        public void Resolve()
        {
            var playArea = Repository.Get<PlayArea>();
            for (var x = Tile.Index.x + 1; x < playArea.Size.x; x++)
            {
                if (!playArea[x, Tile.Index.y][TilableType.Block].Any()) continue;
                
                var block = playArea[x, Tile.Index.y][TilableType.Block].First().Link as Bloc;
                if ((block.Shape & shape) == block.Shape)
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
        
        private void ResolveTime(double time, double offset) => OnActivation.Invoke(time / (1f - offset));
    }
}