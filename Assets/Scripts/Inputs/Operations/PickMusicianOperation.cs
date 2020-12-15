using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewPickMusicianOperation", menuName = "Beau Tambour/Operations/Pick Musician")]
    public class PickMusicianOperation : SingleOperation
    {
        [SerializeField] private Musician left, right, up, down;

        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            
            if (left != null) Event.Open(TempEvent.OnMusicianPicked, left);
            if (right != null) Event.Open(TempEvent.OnMusicianPicked, right);
            if (up != null) Event.Open(TempEvent.OnMusicianPicked, up);
            if (down != null) Event.Open(TempEvent.OnMusicianPicked, down);
        }
        
        public override void OnStart(EventArgs inArgs)
        {
            if (!(inArgs is Vector2EventArgs axisArgs)) return;

            if (axisArgs.value.x == -1 && left != null)
            {
                GameplaySequence.pickedMusician = left;
                Begin(new SingleEventArgs<int>(0));
            }
            else if (axisArgs.value.x == 1 && right != null)
            {
                GameplaySequence.pickedMusician = right;
                Begin(new SingleEventArgs<int>(1));
            }
            else if (axisArgs.value.y == 1 && up != null)
            {
                GameplaySequence.pickedMusician = up;
                Begin(new SingleEventArgs<int>(2));
            }
            else if (down != null)
            {
                GameplaySequence.pickedMusician = down;
                Begin(new SingleEventArgs<int>(3));
            }
        }
    }
}