using System.Collections;
using System.Collections.Generic;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Player : Tilable
    {
        #if UNITY_EDITOR
        
        private int xMax => GetMax().x;
        private int yMax => GetMax().y;
        
        private Vector2Int GetMax()
        {
            var playArea = FindObjectOfType<PlayArea>();
            if (playArea != null) return playArea.IntendedSize;
            else return Vector2Int.zero;
        }

        #endif
        
        public override object Link => this;
        
        public RectInt IndexedBounds
        {
            get
            {
                var width = horizontalLimits.y - horizontalLimits.x;
                var height = verticalLimits.y - verticalLimits.x;
                return new RectInt(horizontalLimits.x, verticalLimits.x, width, height);
            }
        }

        public bool IsCurrentBeatClaimed { get; private set; }

        [SerializeField, MinMaxSlider(0, "xMax")] private Vector2Int horizontalLimits;
        [SerializeField, MinMaxSlider(0, "yMax")] private Vector2Int verticalLimits;

        private double inputClock;
        
        void Start()
        {
            var rythmHandler = Repository.Get<RythmHandler>();

            rythmHandler.OnBeat += OnBeat;
            rythmHandler.OnTimeAdvance += OnTimeAdvance;
        }

        public override void Place(Vector2Int index)
        {
            var indexedBounds = IndexedBounds;
            index = MathBt.Clamp(index, indexedBounds.min, indexedBounds.max);
            
            base.Place(index);
        }

        public void ClaimCurrentBeat()
        {
            if (IsCurrentBeatClaimed) return;
            IsCurrentBeatClaimed = true;
        }

        private void OnBeat(double beat) => inputClock = 0d;
        private void OnTimeAdvance(double delta)
        {
            if (IsCurrentBeatClaimed && inputClock >= 1d - RythmHandler.StandardErrorMargin) IsCurrentBeatClaimed = false;
            inputClock += delta;
        }
    }
}