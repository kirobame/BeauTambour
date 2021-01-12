using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;
using UnityEngine.UI;
using Event = Flux.Event;

namespace BeauTambour
{
    public class SpeakerProgress : MonoBehaviour
    {
        #region Encapsulated Types

        private class ActorHistory
        {
            public ActorHistory(int branches)
            {
                this.branches = branches;
                emotions = new List<Emotion>();
            }

            public int Branches => branches;
            private int branches;

            public IReadOnlyList<Emotion> Emotions => emotions;
            private List<Emotion> emotions;

            public void Update(Emotion emotion, int branches)
            {
                this.branches = branches;
                emotions.Add(emotion);
            }

            public void Setup(int branches) => this.branches = branches;
            public void Clear()
            {
                branches = 0;
                emotions.Clear();
            }
        }
        #endregion

        [SerializeField] private Image image;
        [SerializeField] private MusicianIconRegistry musicianIconRegistry;
        [SerializeField] private BranchingRepresentation branchingRepresentation;
        
        [Space, SerializeField] private RectTransform tree;
        [SerializeField] private float spacing;

        [Space, SerializeField] private RectTransform linksParent;
        [SerializeField] private PoolableImage linkPrefab;

        [Space, SerializeField] private AnimationCurve apparitionCurve;
        [SerializeField] private float apparitionTime;
        
        [Space, SerializeField] private AnimationCurve disapparitionCurve;
        [SerializeField] private float disapparitionTime;

        private int previousActor;
        private int currentActor;
        
        private List<Image> links;
        private List<ProgressIcon> icons;
        private Dictionary<int, ActorHistory> histories;

        private Coroutine apparitionRoutine;
        private Coroutine disapparitionRoutine;
        
        void Awake()
        {
            previousActor = 0;
            currentActor = 0;
            
            links = new List<Image>();
            icons = new List<ProgressIcon>();
            histories = new Dictionary<int, ActorHistory>();

            Event.Open<Character>(GameEvents.OnSpeakerEntrance);
            Event.Register<Character>(GameEvents.OnSpeakerEntrance, OnSpeakerEntrance);
                
            Event.Register<Character, int>(GameEvents.OnSpeakerSelected, OnSpeakerSelected);
            
            Event.Open<int, Emotion, int, int>(GameEvents.OnDialogueTreeUpdate);
            Event.Register<int, Emotion, int, int>(GameEvents.OnDialogueTreeUpdate, OnDialogueTreeUpdate);

            Event.Register(GameEvents.OnGoingToNextBlock, OnGoingToNextBlock);
        }
        void OnSpeakerEntrance(Character speaker)
        {
            var history = new ActorHistory(speaker.Branches);
            histories.Add(speaker.Id, history);
        }

        void OnSpeakerSelected(Character speaker, int code)
        {
            var id = speaker.Id;
            if (id == currentActor) return;

            image.sprite = musicianIconRegistry[speaker.Actor];
            image.SetNativeSize();

            currentActor = id;
            if (apparitionRoutine != null) // Stop apparition & disappear
            {
                StopCoroutine(apparitionRoutine);
                apparitionRoutine = null;
                
                disapparitionRoutine = StartCoroutine(DisapparitionRoutine());
            }
            else if (disapparitionRoutine != null && previousActor == currentActor) // Resume apparition
            {
                StopCoroutine(disapparitionRoutine);
                disapparitionRoutine = null;
                
                apparitionRoutine = StartCoroutine(ApparitionRoutine());
            }
            else if (disapparitionRoutine == null) // Start apparition
            {
                if (previousActor != 0) disapparitionRoutine = StartCoroutine(DisapparitionRoutine());
                else ShowTree();
            }
        }

        private IEnumerator DisapparitionRoutine()
        {
            yield return StartCoroutine(ScaleRoutine(0.0f, disapparitionTime, disapparitionCurve));

            foreach (var link in links) link.gameObject.SetActive(false);
            links.Clear();
            
            foreach (var icon in icons) icon.gameObject.SetActive(false);
            icons.Clear();
            
            yield return new WaitForEndOfFrame();

            disapparitionRoutine = null;
            ShowTree();
        }
        private IEnumerator ScaleRoutine(float goal, float duration, AnimationCurve curve)
        {
            var startingSize = tree.localScale;
            var time = 0.0f;
            
            while (time < duration)
            {
                var ratio = time / duration;
                tree.localScale = Vector3.Lerp(startingSize, Vector3.one * goal, ratio);
                
                yield return  new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            
            tree.localScale = Vector3.Lerp(startingSize, Vector3.one * goal, 1.0f);
        }

        private ProgressIcon GetIcon(IconPool pool, Emotion emotion, int position)
        {
            var icon = pool.RequestSingle();

            icon.RectTransform.parent = tree;
            icon.Set(emotion);
                
            icon.RectTransform.localPosition = new Vector2(spacing * (position + 1), 0.0f);

            icons.Add(icon);
            return icon;
        }
        private void PlaceLink(ImagePool pool, int position, float width)
        {
            var link = pool.RequestSingle(linkPrefab);

            link.rectTransform.parent = linksParent;
            link.rectTransform.localScale = Vector3.one;
            link.rectTransform.localPosition = new Vector2(spacing * position + width / 2.0f, 0.0f);
                
            links.Add(link);
        }
        private void ShowTree()
        {
            previousActor = currentActor;
            
            var linkPool = Repository.GetSingle<ImagePool>(References.ImagePool);
            var iconPool = Repository.GetSingle<IconPool>(References.IconPool);
            
            var history = histories[currentActor];

            var width = 65.0f;
            for (var i = 0; i < history.Emotions.Count; i++)
            {
                var icon = GetIcon(iconPool, history.Emotions[i], i);
                width = icon.RectTransform.sizeDelta.x; ;

                PlaceLink(linkPool, i, width);
            }

            if (history.Branches > 0)
            {
                branchingRepresentation.RectTransform.localScale = Vector3.one;
                branchingRepresentation.RectTransform.localPosition = new Vector2(spacing * history.Emotions.Count + width / 2.0f, 0.0f);
                branchingRepresentation.Prepare(history.Branches);
            }
            else  branchingRepresentation.RectTransform.localScale = Vector3.zero;

            apparitionRoutine = StartCoroutine(ApparitionRoutine());
        }
        private IEnumerator ApparitionRoutine()
        {
            yield return StartCoroutine(ScaleRoutine(1.0f, apparitionTime, apparitionCurve));
            apparitionRoutine = null;
        }

        void OnDialogueTreeUpdate(int id, Emotion emotion, int selection, int branches)
        {
            Debug.Log($"DIALOGUE TREE UPDATE : {id} - {emotion}");
            
            histories[id].Update(emotion, branches);
            StartCoroutine(AdvanceRoutine(emotion, selection, branches));
        }
        private IEnumerator AdvanceRoutine(Emotion emotion, int selection, int branches)
        {
            yield return StartCoroutine(branchingRepresentation.SelectionRoutine(selection));
            
            var iconPool = Repository.GetSingle<IconPool>(References.IconPool);
            var icon = GetIcon(iconPool, emotion, icons.Count);

            icon.RectTransform.localScale = Vector3.zero;
            var width = icon.RectTransform.sizeDelta.x;

            var time = 0.0f;
            var duration = 0.15f;

            while (time < duration)
            {
                icon.RectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / duration);

                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            icon.RectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, 1.0f);
            
            yield return new WaitForSeconds(0.15f);
            
            var linkPool = Repository.GetSingle<ImagePool>(References.ImagePool);
            PlaceLink(linkPool, icons.Count - 1, width);
            
            branchingRepresentation.RectTransform.localScale = Vector3.zero;
            if (branches <= 0) yield break;
            
            branchingRepresentation.RectTransform.localPosition = new Vector2(spacing * icons.Count + width / 2.0f, 0.0f);
            branchingRepresentation.Prepare(branches);
            
            time = 0.0f;
            while (time < duration)
            {
                branchingRepresentation.RectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / duration);

                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            branchingRepresentation.RectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, 1.0f);
        }

        void OnGoingToNextBlock() => StartCoroutine(RebootRoutine());
        private IEnumerator RebootRoutine()
        {
            yield return StartCoroutine(ScaleRoutine(0.0f, disapparitionTime, disapparitionCurve));

            foreach (var history in histories.Values) history.Clear();

            var speakers = Repository.GetAll<Character>(References.Characters);
            foreach (var speaker in speakers)
            {
                var id = speaker.Id;
                if (histories.ContainsKey(id)) histories[id].Setup(speaker.Branches);
            }
        }
    }
}