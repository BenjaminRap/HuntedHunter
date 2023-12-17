using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    private Rigidbody2D     rb;
    private PlayerPhysics   physics;
    [SerializeField]
    private int             jumpForce;
    [SerializeField]
    private int             speed;
    [SerializeField]
    private int             airAcceleration;
    [SerializeField]
    private int             maxSpeed;
    [SerializeField]
    private float           space_duration;
    [SerializeField]
    private int             jump_bonus;
    [SerializeField]
    private float           downForce;
    [SerializeField]
    private float           speedAcceleration;
    private bool            space_pressed;
    private float           currentSpeed;

    void    Start()
    {
        rb = GetComponent<Rigidbody2D>();
        physics = GetComponent<PlayerPhysics>();
        currentSpeed = 0;
    }

    public IEnumerator  JumpStart()
    {
        Vector3 current_velocity;
        float   delta;

        space_pressed = true;
        delta = 0;
        current_velocity = physics.GetVelocity();
        if (!physics.isGrounded)
            yield break ;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        while (space_pressed && delta < space_duration)
        {
            rb.AddForce(Vector3.up * jumpForce * jump_bonus * Time.deltaTime);
            delta += Time.deltaTime;
            yield return null ;
        }
    }

    public void JumpEnd()
    {
        space_pressed = false;
    }

    public void ProcessMove(Vector2 input)
    {
        if (Vector3.Magnitude(rb.velocity) < 0.2f)
            currentSpeed = 0;
        if (physics.isGrounded)
        {
            if (currentSpeed < speed)
                currentSpeed += speedAcceleration * Time.deltaTime;
            rb.velocity = new Vector2(input.x * currentSpeed, rb.velocity.y);
        }
        else if (Mathf.Abs(rb.velocity.x) < maxSpeed || Mathf.Sign(input.x) != Mathf.Sign(rb.velocity.x))
            rb.AddForce(new Vector2(input.x, 0) * airAcceleration * Time.deltaTime);
        if (input.y < 0)
            rb.AddForce(Vector2.down * downForce * Time.deltaTime);
    }
}
