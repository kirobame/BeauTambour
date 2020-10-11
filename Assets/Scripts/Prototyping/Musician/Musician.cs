using System.Collections;
using System.Collections.Generic;
using Orion;
using Shapes;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Musician : Tilable
    {
        public override object Link => this;

        [SerializeField] private RegularPolygon polygon;
        [SerializeField] private IProxy<AnimationCurve> fadeCurveProxy;
        private AnimationCurve fadeCurve
        {
            get => moveCurveProxy.Read();
            set => moveCurveProxy.Write(value);
        }
        [SerializeField] private IProxy<AnimationCurve> moveCurveProxy;
        private AnimationCurve moveCurve
        {
            get => moveCurveProxy.Read();
            set => moveCurveProxy.Write(value);
        }
        
        private bool isActive;
        
        public void Shift(int direction)
        {
            if (isActive) return;
            
            var shiftDirection = new Vector2Int(0,direction);
            
            var playArea = Repository.Get<PlayArea>();
            
            var index = Tile.Index;
            index += shiftDirection;

            var rythmHandler = Repository.Get<RythmHandler>();
            if (index.y < 0)
            {
                var start = Tile.Position;
                var end = playArea[0, playArea.Size.y - 1].Position;
                rythmHandler.MakeStandardEnqueue(time => Fade(time, shiftDirection, start, end), 1);
            } 
            else if (index.y >= playArea.Size.y)
            {
                var start = Tile.Position;
                var end = playArea[0, 0].Position;
                rythmHandler.MakeStandardEnqueue(time => Fade(time, shiftDirection, start, end), 1);
            }
            else
            {
                var start = Tile.Position;
                var end = playArea[index].Position;
                rythmHandler.MakeStandardEnqueue(time => Move(time, start, end), 1);
            }

            isActive = true;
            rythmHandler.MakePlainEnqueue(Reset, 1);
        }

        private void Move(double time, Vector2 start, Vector2 end)
        {
            Position = Vector2.Lerp(start, end, moveCurve.Evaluate((float)time));
            SendMoveNotification();
        }
        private void Fade(double time, Vector2Int direction, Vector2 start, Vector2 end)
        {
            if (time < 0.5f)
            {
                var moveMax = moveCurve.Evaluate(0.5f);
                Position = Vector2.Lerp(start, start + direction, moveCurve.Evaluate((float)time) / moveMax);

                var fadeMax = fadeCurve.Evaluate(0.5f);
                var color = polygon.Color;

                color.a = Mathf.Lerp(1f, 0f, fadeCurve.Evaluate((float) time) / fadeMax);
                polygon.Color = color;
            }
            else
            {
                var moveMin = moveCurve.Evaluate(0.5f);
                Position = Vector2.Lerp(end - direction, end, (moveCurve.Evaluate((float)time) - moveMin) / (1f - moveMin));

                var fadeMin = fadeCurve.Evaluate(0.5f);
                var color = polygon.Color;

                color.a = Mathf.Lerp(0f, 1f, (fadeCurve.Evaluate((float)time) - fadeMin) / (1 - fadeMin));
                polygon.Color = color;
                
            }
            SendMoveNotification();
        }
        
        private void Reset(int beat)
        {
            if (beat == 1) isActive = false;
        }
    }
}