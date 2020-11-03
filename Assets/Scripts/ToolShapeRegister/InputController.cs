using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private InputActionAsset actions;
    [SerializeField] private Transform cursor;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private LineRenderer line;

    private InputActionMap registerActions;
    private InputAction moveAction;
    private InputAction beginRegisterAction;
    private InputAction endRegisterAction;

    private Vector2 lastPos;
    private Vector2 currentPos;
    private float circleRadius = 1;
    private float precisionBetweenEachPoints = 10;
    [SerializeField]private float timeToRemovePos = 1f;
    private float timeLastRemovedPos = 0;
    bool isMoving = false;

    private void OnEnable()
    {
        Initialize();

        moveAction.performed += OnMove;
        moveAction.started += OnMoveStart;
        moveAction.canceled += OnMoveCanceled;
        moveAction.Enable();
    }

    private void Update()
    {
        if (isMoving)
        {
            /*cursor.Translate(moveAction.ReadValue<Vector2>() * Time.deltaTime * 3f);
            if(Mathf.Pow(cursor.position.x,2) + Mathf.Pow(cursor.position.y, 2) > Mathf.Pow(circleRadius, 2))
            {
                cursor.Translate(moveAction.ReadValue<Vector2>() * Time.deltaTime * -3f);
            }*/
            cursor.position = moveAction.ReadValue<Vector2>();
        }
        DrawCursorLine();
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.started -= OnMoveStart;
        moveAction.canceled -= OnMoveCanceled;
        moveAction.Disable();
    }

    private void DrawCursorLine()
    {
        if (currentPos == null)
        {
            lastPos = currentPos = cursor.position;
        }
        else
        {
            lastPos = currentPos;
            currentPos = cursor.position;
        }
        if (currentPos != lastPos)
        {
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, currentPos);
        }
        if (line.positionCount <= 1)
        {
            timeLastRemovedPos = Time.time;
        }
        if (Time.time - timeLastRemovedPos >= timeToRemovePos)
        {
            timeLastRemovedPos = Time.time;
            RemovePosFromLine();
        }
        Debug.Log(Time.time - timeLastRemovedPos);
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

    private void OnMove(InputAction.CallbackContext obj)
    {
        /*Vector2 input = obj.ReadValue<Vector2>();

        cursor.Translate(input * Time.deltaTime * 10f);*/
    }

    private void OnMoveStart(InputAction.CallbackContext obj)
    {
        isMoving = true;
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        isMoving = false;
    }

    private void Initialize()
    {
        registerActions = actions.FindActionMap("RegisterShapeTool");
        moveAction = registerActions.FindAction("Draw");
        beginRegisterAction = registerActions.FindAction("BeginRegister");
        endRegisterAction = registerActions.FindAction("EndRegister");
        timeLastRemovedPos = Time.time;
    }
}
