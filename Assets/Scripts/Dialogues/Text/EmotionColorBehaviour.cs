using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Febucci.UI.Core;
using Flux;
using UnityEngine;
using UnityEngine.Scripting;

namespace BeauTambour
{
    [Preserve, EffectInfo("emcol")]
    public class EmotionColorBehaviour : BehaviorBase
    {
        #region Encapsulated Types

        private struct ColorLerp
        {
            public bool HasBeenInitialized => hasBeenInitialized;
            private bool hasBeenInitialized;
            
            private Color start;
            private float time;

            public void Initialize(Color color)
            {
                start = color;
                hasBeenInitialized = true;
            }
            
            public Color GetValue(float delta, float duration, Color target)
            {
                var ratio = Mathf.Clamp01(time / duration);
                var lerpedColor = Color.magenta;

                if (ratio < 0.25f) lerpedColor =  Color.Lerp(start, target, ratio * 4.0f);
                else if (ratio >= 0.25f && ratio < 0.75f) lerpedColor = target;
                else lerpedColor = Color.Lerp(target, start, (ratio - 0.75f) * 4.0f);

                time += delta;
                return lerpedColor;
            }
        }
        #endregion
        
        private Color target = Color.magenta;
        private float duration = 2.0f;

        private ColorLerp[] lerps;
        
        public override void Initialize(int charactersCount)
        {
            base.Initialize(charactersCount);
            lerps = new ColorLerp[charactersCount];
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            if (!lerps[charIndex].HasBeenInitialized) lerps[charIndex].Initialize(data.tmp_CharInfo.color);

            var color = lerps[charIndex].GetValue(time.deltaTime, duration, target);
            data.colors.SetColor(color);
        }

        public override void SetDefaultValues(BehaviorDefaultValues data) { }
        public override void SetModifier(string modifierName, string modifierValue)
        {
            if (modifierName == "val")
            {
                modifierValue = modifierValue.FirstToUpper();
                if (Enum.TryParse<Emotion>(modifierValue, out var emotion))
                {
                    var emotionColorRegistry = Repository.GetSingle<EmotionColorRegistry>(References.ColorByEmotion);
                    target = emotionColorRegistry[emotion];
                }
                else target = Color.magenta;
            }

            if (modifierName == "dur")
            {
                if (!float.TryParse(modifierValue,NumberStyles.Float, CultureInfo.InvariantCulture, out duration)) duration = 2.0f;
            }
        }
    }
}