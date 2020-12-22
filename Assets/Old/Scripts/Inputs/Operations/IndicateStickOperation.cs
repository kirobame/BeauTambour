using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewIndicateStickOperation", menuName = "Beau Tambour/Operations/Indicate Stick")]
    public class IndicateStickOperation : SingleOperation
    {
        [SerializeField] private float smoothing;

        private Vector2 velocity;

        public override void OnUpdate(EventArgs inArgs)
        {
            if (!(inArgs is Vector2EventArgs vector2EventArgs)) return;
            Place(vector2EventArgs.value);
        }
        public override void OnEnd(EventArgs inArgs) => Place(Vector2.zero);

        private void Place(Vector2 input)
        {
            var stickIndicator = Repository.GetSingle<Transform>(Reference.StickIndicator);
            stickIndicator.localPosition = Vector2.SmoothDamp(stickIndicator.localPosition, input, ref velocity, smoothing);
        }
    }
}