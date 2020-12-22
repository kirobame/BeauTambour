using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    #region Singleton
    private static ToolManager instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public static ToolManager Instance => instance;

    #endregion

    private CurrentAction action;

    public CurrentAction Action { get => action; }

    private void Start()
    {
        InputController.Instance.Initialize();
        ShapeDrawer.Instance.Initialize();

        Initialize();
    }

    private void Initialize()
    {
        action = CurrentAction.Drawing;
        InputController.Instance.OnViewHistoryCallback += OnViewHistory;
    }

    private void OnViewHistory()
    {
        if(action == CurrentAction.Drawing)
        {
            action = CurrentAction.HistoryReview;
        }
        else
        {
            action = CurrentAction.Drawing;
        }        
    }
}

public enum CurrentAction
{
    Drawing,
    HistoryReview
}
