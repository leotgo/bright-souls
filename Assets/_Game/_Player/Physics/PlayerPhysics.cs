using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerPhysics : MonoBehaviour
{
    private Player player;

    private GroundDetector gndDetector;
    private CharacterController charController;

    private float jumpForce = 5f;
    private Vector3 gravity = new Vector3(0f, -9.81f, 0f);
    private float gravityMult = 1f;

    private Vector3 movement = Vector3.zero;
    private Vector3 Movement {
        get {
            return movement;
        }
        set {
            movement = value;
            player.anim.SetFloat("speed_y", movement.y);
        }
    }

    private void Start()
    {
        player = GetComponent<Player>();
        charController = GetComponentInChildren<CharacterController>();
        gndDetector = GetComponentInChildren<GroundDetector>();
    }

    private void Update()
    {
        GravityUpdate();

        if (player.IsInAnyState(States.Dead))
            return;

        MovementUpdate();

        charController.Move(Movement * Time.deltaTime);
    }


    // Will depend on camera type
    private void MovementUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        if (move.magnitude > 1f)
            move = move.normalized;

        player.anim.SetFloat("move_speed", move.magnitude);

        float mult = player.IsInAnyState(States.Blocking) ? 0.5f : 1f;

        player.anim.SetFloat("move_x", move.x * mult);
        player.anim.SetFloat("move_y", move.y * mult);
    }

    private void GravityUpdate()
    {
        bool grounded = gndDetector.IsGrounded;
        // Animator also applies gravity, so when not grounded disable animator physics
        player.anim.applyRootMotion = grounded;
        player.anim.SetBool("grounded", grounded);
        if (!grounded)
        {
            Movement += gravity * gravityMult * Time.deltaTime;
        }
        else if (Movement.y < 0f)
        {
            if (Mathf.Abs(Movement.y) > 15f)
                player.Stamina.Value -= Mathf.CeilToInt(Mathf.Abs(Movement.y) * 3f);
            Movement = new Vector3(Movement.x, 0f, Movement.z);
            charController.Move(new Vector3(0f, -0.5f, 0f));
        }
    }
}
