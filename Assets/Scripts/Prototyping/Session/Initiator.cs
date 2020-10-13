using BeauTambour.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initiator : MonoBehaviour
{
    public RythmHandler rythm;
    public PlayArea playAera;
    public BlockGenerationHandler blockgenerator;

    public Bloc prefabBlock;
    public Note prefabNote;

    // Start is called before the first frame update
    void Start()
    {
        rythm.BootUp();
        playAera.Generate();
        blockgenerator.BootUp();

        /*Bloc block = Instantiate(prefabBlock);
        block.Place(new Vector2Int(9, 1));

        Note note = Instantiate(prefabNote);
        note.Place(new Vector2Int(0,1));*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
