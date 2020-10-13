using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public abstract class TimedEffect : SerializedMonoBehaviour
    {
        [SerializeField] protected float length;
        [SerializeField] private IProxy<AnimationCurve> curveProxy;
        protected AnimationCurve curve
        {
            get => curveProxy.Read();
            set => curveProxy.Write(value);
        }

        protected float time;

        public void ReInitialize()
        {
            var rythmHandler = Repository.Get<RythmHandler>();
            
            rythmHandler.OnTimeAdvance -= PrepareExecution;
            time = 0f;
            
            rythmHandler.MakePlainEnqueue((beat, offset) =>
            {
                if (beat == 0) Repository.Get<RythmHandler>().OnTimeAdvance += PrepareExecution;
            }, 1);
        }

        protected virtual void Start() => Repository.Get<RythmHandler>().OnTimeAdvance += PrepareExecution;

        private void PrepareExecution(double delta)
        {
            Execute(Mathf.Clamp01(time/length));
            
            time += (float)delta;
            if (time > length) time = 0f;
        }
        protected abstract void Execute(float ratio);
    }
}