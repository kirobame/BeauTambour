using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flux;
using Event = Flux.Event;
using Deprecated;

namespace BeauTambour
{
    public class StickIndicatorBehavior : MonoBehaviour
    {  
        [SerializeField] private List<Section> sections;
        [SerializeField] private float smoothing = 1.0f;

        private Vector2 velocity;
        private Transform stickIndicator;

        private void Awake()
        {
            
            Event.Register<Vector2>(StickIndicatorOperation.EventType.OnUpdate, OnUpdate);
            Event.Register<Vector2>(StickIndicatorOperation.EventType.OnEnd, OnEnd);
        }


        private void Place(Vector2 input, bool smooth)
        {
            stickIndicator = Repository.GetSingle<Transform>(Reference.StickIndicator);
            if (smooth)
            {
                stickIndicator.localPosition = Vector2.SmoothDamp(stickIndicator.localPosition, input, ref velocity, smoothing);
            }
            else
            {
                stickIndicator.localPosition = input;
            }
        }

        private void OnUpdate(Vector2 input)
        {
            Place(input, true);
            foreach (Section section in sections)
            {
                if (section.IsInSection(input))
                {
                    section.ScaleAnchor(input);
                }
                else
                {
                    section.SmoothScaleAnchorReset();
                }
                //Distance
                //Angle
                //Actualise section
            }
        }

        private void OnEnd(Vector2 input)
        {
            Place(input, false);
        }

        // Use Game state
        //
        // SELECTION
        //  Check cursor position comparing to emotions position (lerp, grow the section)
        //      IF catch zone magnetize cursor
        //          THEN IF validation Input -> drawing phase 
        //      ELSE
        //          demagnetize cursor and desactivate Inputs
        // DRAWING
        //  
        //
        //
        //
        //
        //
    }
}