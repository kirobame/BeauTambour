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
            
            Event.Open(TempEvent.OnMusicianPicked, left);
            Event.Open(TempEvent.OnMusicianPicked, right);
            Event.Open(TempEvent.OnMusicianPicked, up);
            Event.Open(TempEvent.OnMusicianPicked, down);
        }
        
        public override void OnStart(EventArgs inArgs)
        {
            if (!(inArgs is Vector2EventArgs axisArgs)) return;

            if (axisArgs.value.x == -1)
            {
                GameplaySequence.pickedMusician = left;
                Begin(new SingleEventArgs<int>(0));
            }
            else if (axisArgs.value.x == 1)
            {
                GameplaySequence.pickedMusician = right;
                Begin(new SingleEventArgs<int>(1));
            }
            else if (axisArgs.value.y == 1)
            {
                GameplaySequence.pickedMusician = up;
                Begin(new SingleEventArgs<int>(2));
            }
            else
            {
                GameplaySequence.pickedMusician = down;
                Begin(new SingleEventArgs<int>(3));
            }
        }
    }
}