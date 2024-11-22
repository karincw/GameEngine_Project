using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[Flags]
public enum Actions : int
{
    InGame = 1,
    UI
}

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReaderSO : ScriptableObject, IInGameActions, IUIActions
{
    private Controls _controls;
    public Actions CurrentAction
    {
        set
        {
            if (((int)value & (int)Actions.InGame) > 0)
            {
                _controls.InGame.Enable();
            }
            else
            {
                _controls.UI.Disable();
            }

            if (((int)value & (int)Actions.UI) > 0)
            {
                _controls.UI.Enable();
            }
            else
            {
                _controls.UI.Disable();
            }
        }
    }

    public Action OnMLBDownEvent;
    public Action OnMLBUpEvent;
    public Action<bool> OnMLBHoldEvent;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();

            _controls.InGame.SetCallbacks(this);
            _controls.UI.SetCallbacks(this);
        }
        CurrentAction = Actions.InGame | Actions.UI;

    }

    private void OnDisable()
    {
        _controls.UI.Disable();
        _controls.InGame.Disable();
    }

    public void OnMLBClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnMLBDownEvent?.Invoke();
            OnMLBHoldEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            OnMLBHoldEvent?.Invoke(false);
            OnMLBUpEvent?.Invoke();
        }
    }

}
