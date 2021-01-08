using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeauTambour
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private string name;

        public void Load()
        {
            SceneManager.LoadScene(name);
        }
    }
}