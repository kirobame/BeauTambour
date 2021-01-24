using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [IconIndicator(7705900795600745325), CreateAssetMenu(fileName = "NewEncounter", menuName = "Beau Tambour/Chapter/Block")]
    public class Block : ScriptableObject
    {
        #region Encapsulated Types

        [Serializable]
        public class MusicianEntry
        {
            public Musician Value => value;
            [SerializeField] private Musician value;

            public string[] GivenAttributes
            {
                get
                {
                    if (givenAttributes == string.Empty) return Array.Empty<string>();
                    else return givenAttributes.Split(';');
                }
            } 
            [SerializeField] private string givenAttributes;
        }
        #endregion
        
        public string InfoKey => infoKey;
        
        [SerializeField] private string key;
        [SerializeField] private string infoKey;
        
        public Interlocutor Interlocutor => interlocutor;
        [Space, SerializeField] private Interlocutor interlocutor;

        public Musician[] Musicians => musicianEntries.Select(entry => entry.Value).ToArray();
        [SerializeField] private MusicianEntry[] musicianEntries;

        public void SendAttributes()
        {
            foreach (var entry in musicianEntries)
            {
                foreach (var attribute in entry.GivenAttributes) entry.Value.AddAttribute(attribute);
            }
        }
        
        public void Execute(Block previousBlock)
        {
            if (previousBlock != null)
            {
                var discardSpot = Repository.GetSingle<Transform>("1.InterlocutorDiscard.0");
                previousBlock.interlocutor.RuntimeLink.transform.position = discardSpot.position;
                
                previousBlock.interlocutor.isActive = false;
                GameState.UnregisterSpeakerForUse(previousBlock.interlocutor);
                
                for (var i = 0; i < previousBlock.musicianEntries.Length; i++)
                {
                    var spot = Repository.GetSingle<Transform>($"1.MusicianDiscard.{i}");
                    var musician = previousBlock.musicianEntries[i].Value;

                    musician.isActive = false;
                    GameState.UnregisterSpeakerForUse(musician);
                    
                    musician.RuntimeLink.transform.position = spot.position;
                }
            }

            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            var dialoguePhase = phaseHandler.Get<DialoguePhase>(PhaseCategory.Dialogue);
            dialoguePhase.SetNewStartingKey(key);

            for (var i = 0; i < musicianEntries.Length; i++)
            {
                var musician = musicianEntries[i].Value;
                var spot = Repository.GetSingle<Transform>($"{musicianEntries.Length}.MusicianSpot.{i}");

                if (!musician.HasArcEnded)
                {
                    foreach (var attribute in musicianEntries[i].GivenAttributes) musician.AddAttribute(attribute);
                }
                
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