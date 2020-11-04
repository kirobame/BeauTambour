using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeDrawer : MonoBehaviour
{
    #region Singleton
    private static ShapeDrawer instance = null;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public static ShapeDrawer Instance => instance;
    #endregion

    private bool isRegistering = false;
    private ShapeData currentDrawingShape;

    [SerializeField] private LineRenderer line;

    private float circleRadius = 1;
    private float precisionBetweenEachPoints = 10;

    [SerializeField] private float timeToRemovePos = 1f;
    private float timeLastRemovedPos = 0;

    public void Initialize()
    {
        InputController.Instance.OnBeginRegisterCallback += OnBeginRegistering;
        InputController.Instance.OnEndRegisterCallback += OnEndRegistering;
        InputController.Instance.OnViewHistoryCallback += OnViewHistory;
    }

    public void DrawFromShapeData(ShapeData shape)
    {
        ResetLine();
        foreach(Vector2 point in shape.ShapePoints)
        {
            AddPointToLine(point);
        }
    }

    public void DrawLineBetweenTwoPoint(Vector2 lastPos, Vector2 currentPos )
    {
        if (currentPos != lastPos)
        {
            AddPrecisionPointsBetween(lastPos, currentPos);
            AddPointToLine(currentPos);
        }
        if (line.positionCount <= 1 || isRegistering)
        {
            timeLastRemovedPos = Time.time;
        }
        if (Time.time - timeLastRemovedPos >= timeToRemovePos)
        {
            timeLastRemovedPos = Time.time;
            RemovePosFromLine();
        }
    }

    public void ResetLine()
    {
        line.positionCount = 0;
    }

    private void AddPointToLine(Vector2 position)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, position);
        if (isRegistering)
        {
            currentDrawingShape.AddPoint(position);
        }
    }

    private void AddPrecisionPointsBetween(Vector2 lastPos, Vector2 currentPos)
    {
        Vector2 dir = (currentPos - lastPos).normalized;
        for (int index = 1; index < precisionBetweenEachPoints; index++)
        {
            Vector2 precisionPoint = new Vector2((dir.x / precisionBetweenEachPoints) * index + lastPos.x, (dir.y / precisionBetweenEachPoints) * index + lastPos.y);
            AddPointToLine(precisionPoint);
        }
    }

    private void RemovePosFromLine()
    {
        Vector2 pos;
        for (int index = 0; index < line.positionCount - 1; index++)
        {
            pos = line.GetPosition(index + 1);
            line.SetPosition(index, pos);
        }
        line.positionCount--;
    }

    private void OnBeginRegistering()
    {
        isRegistering = true;
        currentDrawingShape = new ShapeData();
        ResetLine();
    }

    private void OnEndRegistering()
    {
        isRegistering = false;
        //Create the button to display the drawn shape
        ListManager.Instance.AddToHistory(currentDrawingShape);
        currentDrawingShape = null;
    }

    private void OnViewHistory()
    {
        ResetLine();
    }
}
