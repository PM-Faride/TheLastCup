using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class HandleControll : MonoBehaviour
{
    public InputActionAsset controller;
    private InputActionMap player;
    private InputAction fireAction;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction targetAction;
    
    [SerializeField] private UnityEvent ShootActionEvent;
    [SerializeField] private UnityEvent CancelShootActionEvent;
    [SerializeField] private UnityEvent JumpActionEvent;
    [SerializeField] private UnityEvent DashActionEvent;
    [SerializeField] private UnityEvent TargetActionEvent;
    [SerializeField] private UnityEvent CancelTargetActionEvent;
    [SerializeField] private bool playerOne;
    [SerializeField] private bool playerTwo;
    #region Variables
    private Vector2 move;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        player = controller.FindActionMap("Player", true);
        fireAction = player.FindAction("Fire");
        moveAction = player.FindAction("Move");
        jumpAction = player.FindAction("Jump");
        dashAction = player.FindAction("Dash");
        targetAction = player.FindAction("Target");
        fireAction.performed += Shoot;
        fireAction.canceled += CancelShoot;
        targetAction.performed += Target;
        targetAction.canceled += CancelTarget;
        jumpAction.performed += Jump;
        dashAction.performed += Dash;
        moveAction.performed += ctx => move =  ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => move = Vector2.zero;
        

    }
    private void OnEnable()
    {
        fireAction.Enable();
        moveAction.Enable();
        jumpAction.Enable();
        targetAction.Enable();
        dashAction.Enable();
    }
    private void OnDisable()
    {
        fireAction.Disable();
        moveAction.Disable();
        jumpAction.Disable();
        targetAction.Disable();
        dashAction.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerOne && !Variables.uiActivated)
        {
            PlayerController.moveControl = move;
        }
        else if(playerTwo && !Variables.uiActivated)
        {
            PlayerController2.moveControl = move;
        }
    }
    void Shoot(InputAction.CallbackContext ctx)
    {
        if (!Variables.uiActivated)
        {
            ShootActionEvent.Invoke();
        }
    }
    void CancelShoot(InputAction.CallbackContext ctx)
    {
        if (!Variables.uiActivated)
        {
            CancelShootActionEvent.Invoke();
        }
        
    }
    void Target(InputAction.CallbackContext ctx)
    {
        if (!Variables.uiActivated)
        {
            TargetActionEvent.Invoke();
        }
        
    }
    void CancelTarget(InputAction.CallbackContext ctx)
    {
        if (!Variables.uiActivated)
        {
            CancelTargetActionEvent.Invoke();
        }
        
    }
    void Jump(InputAction.CallbackContext ctx)
    {
        if (!Variables.uiActivated)
        {
            JumpActionEvent.Invoke();
        }
        
    }
    void Dash(InputAction.CallbackContext ctx)
    {
        if (!Variables.uiActivated)
        {
            DashActionEvent.Invoke();
        }
        
    }
    void Test()
    {
        //TestSingleton instance = new TestSingleton();
        //instance.publicTest = 2;
    }

}
