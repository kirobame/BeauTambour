using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField] private LineRenderer prefabLine;

    private HistoryButtonBehavior currentShapeButton;
    private int numberOfButton = 0;

    /// <summary>
    /// Set the selected shape (the button pressed)
    /// </summary>
    /// <param name="shape"></param>
    public void SetCurrentShapeButton(HistoryButtonBehavior shape)
    {
        currentShapeButton = shape;
    }

    /// <summary>
    /// Add a shape to the history (create a button)
    /// </summary>
    /// <param name="shape"></param>
    public void AddToHistory(ShapeData shape)
    {
        HistoryButtonBehavior button = Instantiate(prefabButton);
        button.Initialize(shape);
        button.SelfTransform.parent = shapeHistory;
        button.ButtonText.text = string.Format("Shape number {0}",numberOfButton);
        numberOfButton++;
    }

    /// <summary>
    /// Remove the selected shape from the history (remove the button)
    /// </summary>
    public void RemoveFromHistory()
    {
        if (ToolManager.Instance.Action != CurrentAction.HistoryReview) return;
        if (currentShapeButton == null) return;
        Destroy(currentShapeButton.gameObject);
        currentShapeButton = null;
        ShapeDrawer.Instance.ResetLine();
    }

    /// <summary>
    /// Save the selected shape as a prefab
    /// </summary>
    public void SaveCurrentShape()
    {
        if (ToolManager.Instance.Action != CurrentAction.HistoryReview) return;
        if (currentShapeButton == null) return;

        ShapeDrawer.Instance.DrawFromShapeData(prefabLine, currentShapeButton.CurrentShape);
        PrefabUtility.SaveAsPrefabAsset(prefabLine.gameObject, string.Format("Assets/Scripts/ToolShapeRegister/Registered/{0}.prefab", GUID.Generate()));
        ShapeDrawer.Instance.ResetLine(prefabLine);

        RemoveFromHistory();
    }
}
