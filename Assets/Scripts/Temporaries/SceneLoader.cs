using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Event = Flux.Event;

namespace BeauTambour
{
    public class SceneLoader : MonoBehaviour
    {
        private string sceneName;
        private AsyncOperation handle;
        
        void Awake()
        {
            Event.Open(ExtraEvents.OnLoadStart);
            Event.Open(ExtraEvents.OnSceneTransitionReady);
            
            Event.Open(ExtraEvents.OnSceneTransitionStart);
            Event.Register(ExtraEvents.OnSceneTransitionStart, OnSceneTransitionStart);
            
            Event.Open(ExtraEvents.OnLoadEnd);
            Event.Register(ExtraEvents.OnLoadEnd, OnLoadEnd);
        }

        public void Load(string name)
        {
            sceneName = name;
            Event.Call(ExtraEvents.OnLoadStart);
        }

        private void OnSceneTransitionStart()
        {
            handle = SceneManager.LoadSceneAsync(sceneName);
            handle.allowSceneActivation = false;
            
            StartCoroutine(LoadRoutine());
        }
        private IEnumerator LoadRoutine()
        {
            while (handle.progress < 0.9f) yield return new WaitForEndOfFrame();
            Event.Call(ExtraEvents.OnSceneTransitionReady);
        }

        void OnLoadEnd()
        {
            Event.Cleanup();
            handle.allowSceneActivation = true;
        }
    }
}