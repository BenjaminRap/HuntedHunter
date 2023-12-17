using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private Animator         anim;
    private Rigidbody2D      rb;
    private PlayerPhysics    player;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerPhysics>();
    }

    void Update()
    {
        anim.SetFloat("velocityX", rb.velocity.x);
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGrounded", player.isGrounded);
    }
}
