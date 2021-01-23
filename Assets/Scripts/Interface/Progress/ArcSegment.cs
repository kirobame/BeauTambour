using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BeauTambour
{
    public class ArcSegment : MonoBehaviour
    {
        [SerializeField] private Image image;

        [Space, SerializeField] private Animator reveal;
        [SerializeField] private Image portrait;
        [SerializeField] private MusicianIconRegistry musicianIconRegistry;

        public void Reboot() => reveal.SetTrigger("Out");

        public void Complete(Musician musician)
        {
            reveal.ResetTrigger("Out");
            
            portrait.sprite = musicianIconRegistry[musician.Actor];
            portrait.SetNativeSize();
            
            reveal.SetTrigger("In");
        }
    }
}