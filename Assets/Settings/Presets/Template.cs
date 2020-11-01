using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class Template : MonoBehaviour, IComparable
{
    //01 - CONSTANTS
    public const int SomeConstant = 420;
    
    //------------------------------------------------------------------------------------------------------------------
    
    //02 - STATICS
    //02.1 - PUBLIC METHODS
    public static void Print() => Debug.Log(GetMessage());
    
    //02.2 - PRIVATE METHODS
    private static string GetMessage() => message;

    //02.3 - PUBLIC FIELDS
    public static DayOfWeek day;
    
    //02.4 - PRIVATE FIELDS
    private static string message;
    
    //------------------------------------------------------------------------------------------------------------------
    
    //03 - EVENTS
    public event Action SomeAction;
    
    //------------------------------------------------------------------------------------------------------------------
    
    //04 - CONSTRUCTORS
    
    //05 - PROPERTIES
    //05.1 - SIMPLE GETTERS
    public int SomeValue => someValue;

    //05.2 - VALUES GET ONLY
    public bool SomeState { get; private set; }
    
    //05.3 - OTHERS
    public int TextLength
    {
        get => someText.Length;
        set
        {
            if (someText.Length < value)
            {
                var difference = value - TextLength;
                someText += new string(' ', difference);
            }
            else if (someText.Length > value) someText = someText.Substring(0, value);
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    
    //06 - FIELDS
    //06.1 - PUBLIC
    public Vector3 position;
    
    //06.2 - SERIALIZED
    [SerializeField] private Object target;

    //06.3 - NON SERIALIZED
    private int someValue;
    private string someText;
    
    //------------------------------------------------------------------------------------------------------------------
    
    //07 - METHODS
    //07.1 - MONOBEHAVIOUR CALLBACKS
    void Start()
    {
        someText = "Hello again!";
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //07.2 - PUBLIC 
    public void SetValue(int newValue) => someValue = newValue;
    
    //07.3 - PRIVATE
    private void IncrementValue() => someValue++;
    
    //07.4 - CALLBACKS
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) => Debug.Log($"{scene.name} has been loaded !");
    
    //------------------------------------------------------------------------------------------------------------------
    
    //07 - IMPLICIT INTERFACE IMPLEMENTATIONS
    int IComparable.CompareTo(object obj) => 666;
}
