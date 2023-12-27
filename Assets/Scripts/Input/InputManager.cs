using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMotor         motor;
    private PlayerInputActions  input;
    [SerializeField]
    private CameraControl       cam;
    private bool                shift;
    private PlayerPower         power;

    private void    Awake()
    {
        input = new PlayerInputActions();
        motor = GetComponent<PlayerMotor>();
        power = GetComponent<PlayerPower>();
        input.OnFoot.Enable();
        input.Sleeping.WakeUp.performed += WakeUp;
        input.OnFoot.Shift.started += EnableShift;
        input.OnFoot.Shift.canceled += DisableShift;
        input.OnFoot.Jump.started += JumpStart;
        input.OnFoot.Jump.canceled += JumpEnd;
        input.OnFoot.mouseLeft.performed += Dash;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        StartCoroutine(power.Dash());
    }

    public void EnableShift(InputAction.CallbackContext context)
    {
        shift = true;
    }

    public void DisableShift(InputAction.CallbackContext context)
    {
        shift = false;
    }

    public void OnFootInput()
    {
        input.OnFoot.Enable();
        input.Sleeping.Disable();
    }

    public void SleepingInput()
    {
        input.OnFoot.Disable();
        input.Sleeping.Enable();
    }

    private void    WakeUp(InputAction.CallbackContext context)
    {
        GameObject.FindGameObjectsWithTag("Levels")[0].GetComponent<Level>().Space();
    }

    private void    JumpStart(InputAction.CallbackContext context)
    {
        StartCoroutine(motor.JumpStart());
    }

    private void    JumpEnd(InputAction.CallbackContext context)
    {
        motor.JumpEnd();
    }

    private void    FixedUpdate()
    {
        Vector2 move_input;

        move_input = input.OnFoot.Movement.ReadValue<Vector2>();
        if (!shift)
            motor.ProcessMove(move_input);
        else
        {
            if (move_input.x != 0)
                cam.MoveX(move_input.x);
            if (move_input.y != 0)
                cam.MoveY(move_input.y);
        }
    }
}
