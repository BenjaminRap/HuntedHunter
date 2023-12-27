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
    private float           bodyWidth;
    [SerializeField]
    private float           bodyHeight;
    private RaycastHit2D    ground;
    public bool             isGrounded;
    private Rigidbody2D     rb;


    void    Start()
    {
        velocity = Vector2.zero;
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
        RaycastHit2D    hit1;
        RaycastHit2D    hit2;

        hit1 = Physics2D.Raycast(transform.position + Vector3.down * bodyHeight / 2 + Vector3.right * bodyWidth / 2, Vector2.down, groundedDistance);
        hit2 = Physics2D.Raycast(transform.position + Vector3.down * bodyHeight / 2 + Vector3.left * bodyWidth / 2, Vector2.down, groundedDistance);
        if (hit1.collider == null && hit2.collider == null)
            return (false);
        if (hit1.collider == null)
            ground = hit2;
        else
            ground = hit1;
        return (true);
    }

    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
        rb.velocity = velocity;
    }

    public Vector2  GetVelocity()
    {
        return (rb.velocity);
    }
}
