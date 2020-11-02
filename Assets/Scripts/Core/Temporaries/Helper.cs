using Flux;
using UnityEngine;
using UnityEngine.InputSystem;
using Event = Flux.Event;

namespace BeauTambour
{
    public class Helper : MonoBehaviour
    {
        #region Encapsualted Types

        [EnumAddress]
        public enum EventType
        {
            Example
        }
        #endregion
        
        [SerializeField] private InputAction standardAction;
        [SerializeField] private InputAction noteAction;
      
        //----------------------------------------------------------------------------------------------------------
        
        [SerializeField] private Encounter encounter;
        [SerializeField] private NoteGenerator[] generators;
        [SerializeField] private bool useHardcoded;
        
        //----------------------------------------------------------------------------------------------------------
        
        [Space, SerializeField] private InputAction rythmAction;
        [SerializeField] private int duration;
        [SerializeField] private int startOffset;
        
        private Note[] specificNotes = new Note[]
        {
            new Note(new NoteAttribute[]
            {
                new EmotionalNoteAttribute(NoteAttributeType.Joy), 
            }), 
            new Note(new NoteAttribute[]
            {
                new EmotionalNoteAttribute(NoteAttributeType.Joy), 
            }), 
            new Note(new NoteAttribute[]
            {
                new EmotionalNoteAttribute(NoteAttributeType.Sadness), 
            }), 
        };
        
        void Awake()
        {
            Event.Open<string>(EventType.Example);
            
            standardAction.Enable();
            standardAction.performed += ctxt => Event.Call<string>(EventType.Example, "Message from primary Helper.");
            
            //----------------------------------------------------------------------------------------------------------
            
            encounter.BootUp();

            noteAction.Enable();
            noteAction.performed += ctxt => Execute();
            
            //----------------------------------------------------------------------------------------------------------
            
            rythmAction.Enable();
            rythmAction.performed += ctxt =>
            {
                var action = new BeatAction(duration, startOffset, beat => Debug.Log($"From primary Helper : Beat n° {beat}"));

                var rythmHandler = Repository.GetSingle<RythmHandler>(Reference.RythmHandler);
                var settings = Repository.GetSingle<BeauTambourSettings>(Reference.Settings);
                
                rythmHandler.TryEnqueue(action, settings.RythmMarginTolerance);
            };
        }

        void Start() => Repository.GetSingle<RythmHandler>(Reference.RythmHandler).BootUp();

        private void Execute()
        {
            if (useHardcoded)
            {
                for (var i = 0; i < specificNotes.Length; i++)
                {
                    foreach (var attribute in specificNotes[i].Attributes) Debug.Log($"Note : {i + 1} : {attribute}");
                }
                
                encounter.Evaluate(specificNotes);
            }
            else
            {
                var notes = new Note[3];
                for (var i = 0; i < 3; i++)
                {
                    var generator = generators[Random.Range(0, generators.Length)];
                    notes[i] = generator.Generate();
                
                    foreach (var attribute in notes[i].Attributes) Debug.Log($"Note : {i + 1} : {attribute}");
                }
                
                encounter.Evaluate(notes);
            }
        }
    }
}