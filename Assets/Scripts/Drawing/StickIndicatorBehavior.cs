﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flux;
using Event = Flux.Event;
using Deprecated;
using System;

namespace BeauTambour
{
    public class StickIndicatorBehavior : MonoBehaviour
    {  
        public enum EventType
        {
            OnMagnetize,
            OnUnMagnetize,
        }

        [SerializeField] private List<Section> sections;
        [SerializeField] private float smoothing = 0.1f;

        private Vector2 velocity;
        private Transform stickIndicator;
        private Vector2 cursorPos;
        private Section magnetizedSection;

        private void Awake()
        {
            magnetizedSection = null;
            Event.Open(EventType.OnMagnetize);
            Event.Open(EventType.OnUnMagnetize);

            Event.Register<Vector2>(StickIndicatorOperation.EventType.OnUpdate, OnUpdate);
            Event.Register<Vector2>(StickIndicatorOperation.EventType.OnEnd, OnEnd);
        }

        private void Start()
        {
            stickIndicator = Repository.GetSingle<Transform>(Reference.StickIndicator);
        }

        private void OnUpdate(Vector2 input)
        {
            if (magnetizedSection != null)
            {
                MagnetizeCursorToSection();
                if (!magnetizedSection.IsInCatchZone(input))
                {
                    magnetizedSection = null;
                    Event.Call(EventType.OnUnMagnetize);
                    return;
                }
            }
            else
            {
                Place(input, true);
                cursorPos = stickIndicator.localPosition;
            }            
            foreach (Section section in sections)
            {
                if (section.IsInSection(cursorPos))
                {
                    section.ScaleAnchor(cursorPos);
                    if (magnetizedSection == null && section.IsInCatchZone(cursorPos))
                    {
                        magnetizedSection = section;
                        Event.Call(EventType.OnMagnetize);
                    }
                }
                else
                {
                    section.SmoothScaleAnchorReset();
                }
            }
        }

        private void OnEnd(Vector2 input)
        {
            if (magnetizedSection != null)
            {
                magnetizedSection = null;
            }
            Place(input, false);
            cursorPos = stickIndicator.localPosition;
            foreach (Section section in sections)
            {
                section.ScaleAnchor(cursorPos);
            }
        }

        private void Place(Vector2 input, bool smooth)
        {
            if (smooth)
            {
                stickIndicator.localPosition = Vector2.SmoothDamp(stickIndicator.localPosition, input, ref velocity, smoothing);
            }
            else
            {
                stickIndicator.localPosition = input;
            }
        }

        private void MagnetizeCursorToSection()
        {
            Place(magnetizedSection.Anchor.localPosition, true);
            cursorPos = stickIndicator.localPosition;
            magnetizedSection.ScaleAnchor(magnetizedSection.Anchor.localPosition);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var item in sections)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(item.Anchor.position, 0.3f);
            }
        }
#endif
    }
}