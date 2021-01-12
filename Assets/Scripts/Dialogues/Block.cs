using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(7705900795600745325), CreateAssetMenu(fileName = "NewEncounter", menuName = "Beau Tambour/Chapter/Block")]
    public class Block : ScriptableObject
    {
        [SerializeField] private string key;
        
        public Interlocutor Interlocutor => interlocutor;
        [SerializeField] private Interlocutor interlocutor;

        public IReadOnlyList<Musician> Musicians => musicians;
        [SerializeField] private Musician[] musicians;

        public void Execute(Block previousBlock)
        {
            if (previousBlock != null)
            {
                var discardSpot = Repository.GetSingle<Transform>("1.InterlocutorDiscard.0");
                previousBlock.interlocutor.RuntimeLink.transform.position = discardSpot.position;
                
                previousBlock.interlocutor.isActive = false;
                GameState.UnregisterSpeakerForUse(previousBlock.interlocutor);
                
                for (var i = 0; i < previousBlock.musicians.Length; i++)
                {
                    var spot = Repository.GetSingle<Transform>($"1.MusicianDiscard.{i}");
                    var musician = previousBlock.musicians[i];

                    musician.isActive = false;
                    GameState.UnregisterSpeakerForUse(musician);
                    
                    musician.RuntimeLink.transform.position = spot.position;
                }
            }

            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            var dialoguePhase = phaseHandler.Get<DialoguePhase>(PhaseCategory.Dialogue);
            dialoguePhase.SetNewStartingKey(key);

            for (var i = 0; i < musicians.Length; i++)
            {
                var musician = musicians[i];
                var spot = Repository.GetSingle<Transform>($"{musicians.Length}.MusicianSpot.{i}");
                
                musician.RuntimeLink.Reboot();
                musician.RuntimeLink.transform.position = spot.position;
                musician.isActive = true;
                
                GameState.RegisterSpeakerForUse(musician);
            }

            var interlocutorSpot = Repository.GetSingle<Transform>($"1.Interlocutor.{(int)interlocutor.Actor}");
            interlocutor.RuntimeLink.transform.position = interlocutorSpot.position;
            interlocutor.isActive = true;
        }
    }
}