using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HistoryButtonBehavior : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Text buttonText;
    [SerializeField] private ShapeData currentShape;
    [SerializeField] private Transform selfTransform;

    public Transform SelfTransform => selfTransform;

    public Text ButtonText => buttonText;

    public void OnClick()
    {
        if (ToolManager.Instance.Action != CurrentAction.HistoryReview) return;
        ListManager.Instance.SetCurrentShapeButton(this);
        ShapeDrawer.Instance.DrawFromShapeData(currentShape);     
    }

    public void SetCurrentShape(ShapeData shape)
    {
        currentShape = shape;
    }
}
