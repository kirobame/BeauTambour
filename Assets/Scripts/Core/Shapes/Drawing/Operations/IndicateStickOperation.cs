using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewIndicateStickOperation", menuName = "Beau Tambour/Operations/Indicate Stick")]
    public class IndicateStickOperation : SingleOperation
    {
        #region Encapsulated Types

        [EnumAddress]
        public enum EventType
        {
            OnScale
        }
        #endregion
        
        [SerializeField] private float smoothing;
        [SerializeField] private Vector2 sizeRange;
        
        private Vector2 velocity;

        public override void Initialize(OperationHandler operationHandler)
        {
            base.Initialize(operationHandler);
            Event.Open<float>(EventType.OnScale);
        }

        public override void OnUpdate(EventArgs inArgs)
        {
            if (!(inArgs is Vector2EventArgs vector2EventArgs)) return;
            Place(vector2EventArgs.value);
        }
        public override void OnEnd(EventArgs inArgs) => Place(Vector2.zero);

        private void Place(Vector2 input)
        {
            var stickIndicator = Repository.GetSingle<Transform>(Reference.StickIndicator);
            stickIndicator.position = Vector2.SmoothDamp(stickIndicator.position, input, ref velocity, smoothing);

            stickIndicator.localScale = Vector3.Lerp(sizeRange.x * Vector3.one, sizeRange.y * Vector3.one, input.magnitude);
            Event.Call<float>(EventType.OnScale, stickIndicator.localScale.z);
        }
    }
}