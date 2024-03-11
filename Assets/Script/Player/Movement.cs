using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Movement : MonoBehaviourPunCallbacks
{

    [Header("Movement Settings")]
    public float walkSpeed = 8f;
    public float sprintSpeed = 30f;
    public float maxVelocityChange = 20f;
    [Space]
    public float jumpHeight = 5f;
    public float airControl = 0.1f;

    private Vector2 input;
    private Rigidbody rb;

    private bool sprinting;
    private bool jumping;

    private bool grounded;

    PhotonView PV;

    public GameObject camera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        if (!PV.IsMine)
            Destroy(camera);
    }
    void Update()
    {
        if (PV.IsMine)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            input.Normalize();
            sprinting = Input.GetButton("Sprint");
            jumping = Input.GetButton("Jump");
        }
    }
    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        Move();
    }
    private void Move()
    {
        if (grounded)
        {
            if (jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            }
            else if (input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed : walkSpeed), ForceMode.VelocityChange);
            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
            grounded = false;
        }
        else
        {
            if (input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(walkSpeed), ForceMode.VelocityChange);
            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        grounded = true;
    }
    Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= _speed;

        Vector3 velocity = rb.velocity;

        if (input.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - velocity;

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

            velocityChange.y = 0;

            return velocityChange;
        }
        else
        {
            return new Vector3();
        }
    }
}
