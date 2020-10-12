using BeauTambour.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initiator : MonoBehaviour
{
    public RythmHandler rythm;

    // Start is called before the first frame update
    void Start()
    {
        rythm.BootUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
