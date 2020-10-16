using Orion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeauTambour.Prototyping
{
    public class Note : Tilable, IResolvable
    {
        public override object Link => this;
        public Shape Shape => shape;
        
        public OrionEvent<UnityEngine.Color> OnInitialization = new OrionEvent<UnityEngine.Color>();
        public OrionEvent<double> OnActivation = new OrionEvent<double>();
        public OrionEvent<double> OnMatch = new OrionEvent<double>();
        public OrionEvent<double> OnNull = new OrionEvent<double>();

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
            name = Random.Range(0, 100).ToString();
            gameObject.SetActive(true);
            
            Onset = Tile.Position;
            Outset = Repository.Get<PlayArea>()[Tile.Index + Vector2Int.right].Position;
            
            Repository.Get<ResolutionPhase>().TryEnqueue(this);
            Repository.Get<RythmHandler>().MakeStandardEnqueue(Activate, 1);
        }
        private void Activate(double time, double offset) => OnActivation.Invoke(time / (1f - offset));

        public void Resolve()
        {
            var playArea = Repository.Get<PlayArea>();
            var rythmHandler = Repository.Get<RythmHandler>();
            
            for (var x = Tile.Index.x + 1; x < playArea.Size.x; x++)
            {
                if (!playArea[x, Tile.Index.y][TilableType.Block].Any()) continue;
                
                var block = playArea[x, Tile.Index.y][TilableType.Block].First().Link as Block;
                if ((block.Shape & shape) == block.Shape)
                {
                    SetTweenMarks(Tile.Position, block.Tile.Position);

                    rythmHandler.MakePlainEnqueue(Disappear, 2);
                    rythmHandler.MakeStandardEnqueue(Match, 2);
                    
                    block.ShutDown();
                }
                else
                {
                    rythmHandler.MakePlainEnqueue(Disappear, 2);
                    rythmHandler.MakeStandardEnqueue(Null, 2);
                }
                    
                return;
            }
            
            rythmHandler.MakePlainEnqueue(Disappear, 2);
            rythmHandler.MakeStandardEnqueue(Null, 2);
        }

        private void Disappear(int beat, double offset)
        {
            if (beat == 2) gameObject.SetActive(false);
        }
        private void Match(double time, double offset) => OnMatch.Invoke(time / (2f - offset));

        private void Null(double time, double offset) => OnNull.Invoke(time / (2f - offset));
    }
}