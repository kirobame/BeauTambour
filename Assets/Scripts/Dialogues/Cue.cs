using UnityEngine;

namespace BeauTambour
{
    public struct Cue 
    {
        public Cue(Actor actor, string text)
        {
            Actor = actor;
            Text = text;
        }
        
        public readonly Actor Actor;
        public readonly string Text;

        public override string ToString() => $"[{Actor}]-{Text}";
    }
}