using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListManager : MonoBehaviour
{
    #region Singleton
    private static ListManager instance = null;

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
    public static ListManager Instance => instance;

    #endregion

    [SerializeField] private HistoryButtonBehavior prefabButton;
    [SerializeField] private Transform shapeHistory;

    private HistoryButtonBehavior currentShapeButton;
    private int numberOfButton = 0;

    public void SetCurrentShapeButton(HistoryButtonBehavior shape)
    {
        currentShapeButton = shape;
    }

    public void AddToHistory(ShapeData shape)
    {
        HistoryButtonBehavior button = Instantiate(prefabButton);
        button.SetCurrentShape(shape);
        button.SelfTransform.parent = shapeHistory;
        button.ButtonText.text = string.Format("Shape number {0}",numberOfButton);
        numberOfButton++;
    }

    public void RemoveFromHistory()
    {
        if (ToolManager.Instance.Action != CurrentAction.HistoryReview) return;
        if (currentShapeButton == null) return;
        Destroy(currentShapeButton.gameObject);
        ShapeDrawer.Instance.ResetLine();
    }

    public void SaveCurrentShape()
    {
        if (ToolManager.Instance.Action != CurrentAction.HistoryReview) return;
        //SAVE
        Debug.Log("save");
    }
}
