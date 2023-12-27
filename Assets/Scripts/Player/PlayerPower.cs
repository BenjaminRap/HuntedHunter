using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPower : MonoBehaviour
{
    [SerializeField]
    private float           dashForce;
    [SerializeField]
    private float           dashLength;
    [SerializeField]
    private Camera          cam;
    private bool            canDash;
    private PlayerPhysics   player;
    private PlayerMotor     motor;

    void    Start()
    {
        player = GetComponent<PlayerPhysics>();
        motor = GetComponent<PlayerMotor>();
        canDash = true;
    }

    void Update()
    {
        if (player.isGrounded && !canDash)
            canDash = true;
    }

    public IEnumerator  Dash()
    {
        Vector3 playerOnScreen;
        Vector2 dashVector;

        if (!player.isGrounded && canDash)
        {
            playerOnScreen = cam.WorldToScreenPoint(transform.position);
            dashVector = Vector3.Normalize(new Vector2(Input.mousePosition.x - playerOnScreen.x, Input.mousePosition.y - playerOnScreen.y));
            player.SetVelocity(dashVector * dashForce);
            canDash = false;
            yield return (new WaitForSeconds(dashLength));
            player.SetVelocity(Vector3.Normalize(player.GetVelocity()) * motor.GetSpeed());
        }
    }
}
