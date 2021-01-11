using UnityEngine;
using UnityEngine.UI;

namespace BeauTambour
{
    public class ArcSegment : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Color activationColor;
        [SerializeField] private Color deactivationColor;

        [Space, SerializeField] private GameObject portraitObject;
        [SerializeField] private Image portrait;
        [SerializeField] private MusicianIconRegistry musicianIconRegistry;
        
        public void Reboot()
        {
            image.color = deactivationColor;
            portraitObject.SetActive(false);
        }
        
        public void Complete(Musician musician)
        {
            image.color = activationColor;
            
            portraitObject.SetActive(true);
            portrait.sprite = musicianIconRegistry[musician.Actor];
            portrait.SetNativeSize();
        }
    }
}