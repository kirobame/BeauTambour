using Flux;
using Flux.Editor;
using Ludiq.PeekCore;
using UnityEditor;

namespace BeauTambour.Editor
{
    public abstract class EffectInterpreter<T> : OutcomeInterpreter where T : Effect
    {
        public override void Interpret(string data, Outcome outcome, Sequencer sequencer)
        {
            var effect = sequencer.AddComponent<T>();
            HandleEffect(data, sequencer, effect);
            
            var serializedObject = new SerializedObject(sequencer);
            var effectsProperty = serializedObject.FindProperty("effects");
            
            effectsProperty.AddElement(effect);
            serializedObject.ApplyModifiedProperties();
        }

        protected abstract void HandleEffect(string data, Sequencer sequencer, T effect);
    }
}