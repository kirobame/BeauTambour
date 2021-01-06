﻿using Deprecated;
using Flux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour
{
    public class ShapeAnalysis
    {
        public Shape Source { get; private set; }

        public readonly float LocalRatio;
        public readonly float GlobalRatio;

        public readonly float Error;
        public readonly bool GoToNext;

        public Vector2 position;
        public int advancement;

        public void SetSource(Shape source) => Source = source;

        public ShapeAnalysis(Shape source, Vector2 position)
        {
            Source = source;
            this.position = position;
        }
        public ShapeAnalysis(float localRatio, float globalRatio, float error, bool goToNext)
        {
            LocalRatio = localRatio;
            GlobalRatio = globalRatio;

            Error = error;
            GoToNext = goToNext;
        }

        public bool IsComplete
        {
            get
            {
                var settings = Repository.GetSingle<BeauTambourSettings>(References.Settings);
                return GlobalRatio >= settings.CompletionQuota;
            }
        }
        public bool IsValid
        {
            get
            {
                var settings = Repository.GetSingle<BeauTambourSettings>(References.Settings);
                return Error <= settings.RecognitionErrorThreshold;
            }
        }
    }
}