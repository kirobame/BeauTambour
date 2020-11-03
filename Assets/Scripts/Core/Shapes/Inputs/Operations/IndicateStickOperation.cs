using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "New IndicateStickOperation", menuName = "Beau Tambour/Operations/Indicate Stick")]
    public class IndicateStickOperation : SingleOperation
    {
        [SerializeField] private float smoothing;
        //
        private Vector2 velocity;
        
        public override void OnUpdate(EventArgs inArgs)
        {
            if (!(inArgs is Vector2EventArgs vector2EventArgs)) return;

            var stickIndicator = Repository.GetSingle<Transform>(Reference.StickIndicator);
            stickIndicator.transform.position = Vector2.SmoothDamp(stickIndicator.transform.position, vector2EventArgs.value, ref velocity, smoothing);
        }
    }
}