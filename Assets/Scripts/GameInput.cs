using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnMainAttack;
    public event EventHandler OnSecondaryAttackStarted;
    public event EventHandler OnSecondaryAttackCancelled;
    public event EventHandler OnDash;
    public event EventHandler OnParry;
    private PlayerControl _playerInputActions;
    private void Awake()
    {
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _playerInputActions = new PlayerControl();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.MainAttack.performed += MainAttack_performed;
        _playerInputActions.Player.SecondaryAttack.started += SecondaryAttack_started;
        _playerInputActions.Player.SecondaryAttack.canceled += SecondaryAttack_cancelled;
        _playerInputActions.Player.Dash.performed += Dash_performed;
        _playerInputActions.Player.Parry.performed += Parry_performed;
    }
    private void SecondaryAttack_cancelled(InputAction.CallbackContext obj)
    {
        OnSecondaryAttackCancelled?.Invoke(this, EventArgs.Empty);
    }
    private void SecondaryAttack_started(InputAction.CallbackContext obj)
    {
        OnSecondaryAttackStarted?.Invoke(this, EventArgs.Empty);
    }

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        OnDash?.Invoke(this, EventArgs.Empty);
    }

    private void Parry_performed(InputAction.CallbackContext obj)
    {
        OnParry?.Invoke(this, EventArgs.Empty);
    }

    private void MainAttack_performed(InputAction.CallbackContext obj)
    {
        OnMainAttack?.Invoke(this, EventArgs.Empty);
    }


    public Vector2 GetMovementVector()
    {
        return _playerInputActions.Player.Move.ReadValue<Vector2>();
    }

    private void OnDestroy()
    {
        _playerInputActions.Dispose();
    }
}