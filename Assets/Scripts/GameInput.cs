using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnMainAttackStarted;
    public event EventHandler OnMainAttack;
    public event EventHandler OnMainAttackCancelled;
    public event EventHandler OnSecondaryAttackStarted;
    public event EventHandler OnSecondaryAttack;
    public event EventHandler OnSecondaryAttackCancelled;
    public event EventHandler OnMainSkillStart;
    public event EventHandler OnMainSkillCancelled;
    public event EventHandler OnShield;
    public event EventHandler OnDash;
    public event EventHandler OnInteract;
    private PlayerControl _playerInputActions;
    private void Awake()
    {
        // Singleton pattern
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
        _playerInputActions.Player.MainAttack.started += MainAttack_started;
        _playerInputActions.Player.MainAttack.performed += MainAttack_performed;
        _playerInputActions.Player.MainAttack.canceled += MainAttack_cancelled;
        _playerInputActions.Player.SecondaryAttack.started += SecondaryAttack_started;
        _playerInputActions.Player.SecondaryAttack.performed += SecondaryAttack_performed;
        _playerInputActions.Player.SecondaryAttack.canceled += SecondaryAttack_cancelled;

        _playerInputActions.Player.Shield.performed += Shield_performed;
        _playerInputActions.Player.MainSkill.started += MainSkill_started;
        _playerInputActions.Player.MainSkill.canceled += MainSkill_cancelled;
        _playerInputActions.Player.Dash.performed += Dash_performed;
        
        _playerInputActions.Player.Interact.performed += ctx => OnInteract?.Invoke(this, EventArgs.Empty);
    }



    private void MainAttack_started(InputAction.CallbackContext obj)
    {
        OnMainAttackStarted?.Invoke(this, EventArgs.Empty);
    }
    private void MainAttack_performed(InputAction.CallbackContext obj)
    {
        OnMainAttack?.Invoke(this, EventArgs.Empty);
    }
    private void MainAttack_cancelled(InputAction.CallbackContext obj)
    {
        OnMainAttackCancelled?.Invoke(this, EventArgs.Empty);
    }


    private void SecondaryAttack_started(InputAction.CallbackContext obj)
    {
        OnSecondaryAttackStarted?.Invoke(this, EventArgs.Empty);
    }
    private void SecondaryAttack_performed(InputAction.CallbackContext obj)
    {
        OnSecondaryAttack?.Invoke(this, EventArgs.Empty);
    }
    private void SecondaryAttack_cancelled(InputAction.CallbackContext obj)
    {
        OnSecondaryAttackCancelled?.Invoke(this, EventArgs.Empty);
    }

    private void MainSkill_started(InputAction.CallbackContext obj)
    {
        OnMainSkillStart?.Invoke(this, EventArgs.Empty);
    }
    private void MainSkill_cancelled(InputAction.CallbackContext obj)
    {
        OnMainSkillCancelled?.Invoke(this, EventArgs.Empty);
    }

    private void Shield_performed(InputAction.CallbackContext obj)
    {
        OnShield?.Invoke(this, EventArgs.Empty);
    }
    private void Dash_performed(InputAction.CallbackContext obj)
    {
        OnDash?.Invoke(this, EventArgs.Empty);
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