using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    #region Singleton
    private static InputController instance = null;

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
    public static InputController Instance => instance;
    #endregion

    [SerializeField] private InputActionAsset actions;
    [SerializeField] private Transform cursor;

    private InputActionMap registerActions;
    private InputAction moveAction;
    private InputAction beginRegisterAction;
    private InputAction endRegisterAction;
    private InputAction viewHistoryAction;

    private bool isMoving = false;
    private Vector2 lastPos;
    private Vector2 currentPos;    

    public event Action OnBeginRegisterCallback;
    public event Action OnEndRegisterCallback;
    public event Action OnViewHistoryCallback;

    private void Update()
    {
        if (isMoving)
        {
            /*cursor.Translate(moveAction.ReadValue<Vector2>() * Time.deltaTime * 3f);
            if(Mathf.Pow(cursor.position.x,2) + Mathf.Pow(cursor.position.y, 2) > Mathf.Pow(circleRadius, 2))
            {
                cursor.Translate(moveAction.ReadValue<Vector2>() * Time.deltaTime * -3f);
            }*/
            Vector2 newPos = moveAction.ReadValue<Vector2>();
            cursor.position = newPos;
        }
        else
        {
            cursor.position = Vector2.zero;
        }
        if (currentPos == null)
        {
            lastPos = currentPos = cursor.position;
        }
        else
        {
            lastPos = currentPos;
            currentPos = cursor.position;
        }
        if (ToolManager.Instance.Action == CurrentAction.Drawing)
        {
            ShapeDrawer.Instance.DrawLineBetweenTwoPoint(lastPos, currentPos);
        }
    }

    private void OnDisable()
    {
        moveAction.started -= OnMoveStart;
        moveAction.canceled -= OnMoveCanceled;
        moveAction.Disable();

        beginRegisterAction.performed -= OnBeginRegister;
        beginRegisterAction.Disable();

        endRegisterAction.performed -= OnEndRegister;
        endRegisterAction.Disable();

        viewHistoryAction.performed -= OnViewHistory;
        viewHistoryAction.Disable();
    }

    public void Initialize()
    {
        registerActions = actions.FindActionMap("RegisterShapeTool");
        moveAction = registerActions.FindAction("Draw");
        beginRegisterAction = registerActions.FindAction("BeginRegister");
        endRegisterAction = registerActions.FindAction("EndRegister");
        viewHistoryAction = registerActions.FindAction("ViewHistory");

        moveAction.started += OnMoveStart;
        moveAction.canceled += OnMoveCanceled;
        moveAction.Enable();

        beginRegisterAction.performed += OnBeginRegister;
        beginRegisterAction.Enable();

        endRegisterAction.performed += OnEndRegister;
        endRegisterAction.Enable();

        viewHistoryAction.performed += OnViewHistory;
        viewHistoryAction.Enable();
    }   

    private void OnMoveStart(InputAction.CallbackContext obj)
    {
        isMoving = true;
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        isMoving = false;
    }   

    private void OnBeginRegister(InputAction.CallbackContext obj)
    {
        if (ToolManager.Instance.Action != CurrentAction.Drawing) return;

        OnBeginRegisterCallback?.Invoke();
    }
    
    private void OnEndRegister(InputAction.CallbackContext obj)
    {
        if (ToolManager.Instance.Action != CurrentAction.Drawing) return;

        OnEndRegisterCallback?.Invoke();
    }
    
    private void OnViewHistory(InputAction.CallbackContext obj)
    {
        OnViewHistoryCallback?.Invoke();
    }
}
