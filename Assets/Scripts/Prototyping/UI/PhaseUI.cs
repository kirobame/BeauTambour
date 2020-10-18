using BeauTambour.Prototyping;
using Ludiq.PeekCore.CodeDom;
using Orion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PhaseUI : MonoBehaviour
{
    /*public Image image;
    public Text text;
    public Text textBeatsToNextPhase;
    public List<PhaseColorPair> phaseColorList;

    private RythmHandler rythm;
    private RoundHandler round;
    private bool phaseChanged = false;
    private Dictionary<PhaseType, UnityEngine.Color> phaseColorDict = new Dictionary<PhaseType, UnityEngine.Color>();

    // Start is called before the first frame update
    void Start()
    {
        round = Repository.Get<RoundHandler>();
        //rythm = Repository.Get<RythmHandler>();

        round.OnPhaseChange += OnPhaseChanged;
        //rythm.OnBeat += OnBeated;

        foreach(PhaseColorPair pair in phaseColorList)
        {
            phaseColorDict.Add(pair.Key, pair.Value);
        }
        OnPhaseChanged(round.CurrentType);
    }

    private void OnPhaseChanged(PhaseType phaseType)
    {
        text.text = phaseType.ToString();
        image.color = phaseColorDict[phaseType];
        phaseChanged = true;
    }

    private void OnBeated(double beat)
    {
        /*if (phaseChanged)
        {
            IPhase phase = round[round.CurrentType];
            if (phase is Phase)
            {
                textBeatsToNextPhase.enabled = true;
                textBeatsToNextPhase.text = String.Format("{0}",0);
                phaseChanged = false;
            }
            else
            {
                textBeatsToNextPhase.enabled = false;
            }
        }
        else
        {
            textBeatsToNextPhase.text = (2).ToString();
        } 
    }*/
}

[Serializable]
public class PhaseColorPair
{
    [SerializeField]private PhaseType key;
    [SerializeField]private UnityEngine.Color value;
    public PhaseType Key => key;
    public UnityEngine.Color Value => value;
}
