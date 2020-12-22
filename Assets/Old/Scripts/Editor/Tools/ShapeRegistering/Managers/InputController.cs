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

    private bool isRegistering = false;
    private bool isMoving = false;
    private Vector2 lastPos;
    private Vector2 currentPos;
    private float circleRadius = 1;


    public event Action OnBeginRegisterCallback;
    public event Action OnEndRegisterCallback;
    public event Action OnViewHistoryCallback;

    private void Update()
    {
        if (isMoving)
        {
            Vector2 newPos = moveAction.ReadValue<Vector2>();
            if(Mathf.Pow(newPos.x,2) + Mathf.Pow(newPos.y,2) >= Mathf.Pow(circleRadius,2))
            {
                newPos = newPos.normalized;
            }
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
        if (ToolManager.Instance.Action != CurrentAction.Drawing || isRegistering) return;

        OnBeginRegisterCallback?.Invoke();
        isRegistering = true;
    }
    
    private void OnEndRegister(InputAction.CallbackContext obj)
    {
        if (ToolManager.Instance.Action != CurrentAction.Drawing || !isRegistering) return;

        OnEndRegisterCallback?.Invoke();
        isRegistering = false;
    }
    
    private void OnViewHistory(InputAction.CallbackContext obj)
    {
        if (isRegistering) { isRegistering = false; }
        OnViewHistoryCallback?.Invoke();
    }
}
