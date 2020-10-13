using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public bool IsActive => Repository.Get<RoundHandler>().CurrentType == PhaseType.Acting;
        public RectInt IndexedBounds
        {
            get
            {
                var width = horizontalLimits.y - horizontalLimits.x;
                var height = verticalLimits.y - verticalLimits.x;
                return new RectInt(horizontalLimits.x, verticalLimits.x, width, height);
            }
        }

        [HideInInspector] public int phase = 1;
        
        [SerializeField, MinMaxSlider(0, "xMax")] private Vector2Int horizontalLimits;
        [SerializeField, MinMaxSlider(0, "yMax")] private Vector2Int verticalLimits;

        [ShowInInspector, ReadOnly] private HashSet<ActionType> claimedActionTypes = new HashSet<ActionType>();
        
        public override void Place(Vector2Int index)
        {
            var indexedBounds = IndexedBounds;
            index = MathBt.Clamp(index, indexedBounds.min, indexedBounds.max);
            
            base.Place(index);
        }

        public bool IsActionTypeClaimed(ActionType type) => claimedActionTypes.Contains(type);
        public void ClaimActionType(ActionType type)
        {
            if (claimedActionTypes.Contains(type)) return;
            claimedActionTypes.Add(type);
        }
        public void FreeActionType(ActionType type) => claimedActionTypes.Remove(type);
    }
}