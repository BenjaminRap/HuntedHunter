using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private Vector3         velocity;
    [SerializeField]
    private float           gravity;
    [SerializeField]
    private float           groundedDistance;
    [SerializeField]
    private float           groundedDuration;
    private RaycastHit2D    ground;
    public bool             isGrounded;
    private float           inAirTime;
    private Rigidbody2D     rb;

    void    Start()
    {
        velocity = Vector2.zero;
        inAirTime = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    void    Update()
    {
        isGrounded = IsGrounded();
        velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
            velocity.y = Mathf.Max(velocity.y, gravity);
        else
        {
            velocity.y = Mathf.Max(velocity.y, 2 * gravity);
            rb.AddForce(velocity * Time.deltaTime);
        }
    }

    private bool    IsGrounded()
    {
        RaycastHit2D    hit;

        hit = Physics2D.Raycast(transform.position, -Vector2.up, groundedDistance);
        if (hit.collider == null)
        {
            if (inAirTime > groundedDuration)
                return (false);
            inAirTime += Time.deltaTime;
        }
        else
            inAirTime = 0;
        ground = hit;
        return (true);
    }

    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    public Vector2  GetVelocity()
    {
        return (velocity);
    }
}
