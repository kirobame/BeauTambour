using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActorPositionTest
{
    Player,
    Encounter
}

public struct ActorTest
{
    public string name;
    public ActorPositionTest side;
    public Transform selfTransform;
    public Font font;
    public int fontSize;
    public Color fontColor;

    public ActorTest(string name, ActorPositionTest side, Transform selfTransform, Font font, int fontSize, Color fontColor) 
    {
        this.name = name;
        this.side = side;
        this.selfTransform = selfTransform;
        this.font = font;
        this.fontSize = fontSize;
        this.fontColor = fontColor;
    }
}

public class InitTester : MonoBehaviour
{
    #region Singleton
    private static InitTester instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;
    }
    public static InitTester Instance => instance;
    #endregion

    [SerializeField]private DialogManager dialog;
    [SerializeField]private Transform wolfPos;
    [SerializeField]private Transform frogPos;

    public event Action OnPassCue;

    ActorTest wolf;
    ActorTest frog;

    Cue[] cues = new Cue[4];

    float timeToWait = 5f;
    float lastPlayed;

    // Start is called before the first frame update
    void Start()
    {
        lastPlayed = Time.time;

        wolf = new ActorTest("Wolf", ActorPositionTest.Encounter, wolfPos, new Font(),15,Color.blue);
        frog = new ActorTest("Frog", ActorPositionTest.Player,frogPos, new Font(), 14, Color.green);
        cues[0] = new Cue("<b>Je</b> suis un texte <b>incroyable</b>\nc'est trop bien", wolf);
        cues[1] = new Cue("Croa croa croa, croa croa crooooaaaaaa", frog );;
        cues[2] = new Cue("Grrrrrrr Miam ", wolf);
        cues[3] = new Cue("J'ai plus <b>faim</b>", wolf);
        dialog.InitializeDialog(cues);
    }
    

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastPlayed >= timeToWait)
        {
            Debug.Log("PASS");
            lastPlayed = Time.time;
            OnPassCue?.Invoke();            
        }
    }
}
