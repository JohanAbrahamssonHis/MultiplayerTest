using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;
using Random = System.Random;

[RequireComponent(typeof(CharacterController))]
public class Movement : NetworkBehaviour
{
    public float speed = 6.0f;
    public float runningSpeed = 8.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    public float staminaBase = 10f;
    private float staminaCurrent;
    public float staminaRefreshRate = 4f;
    public float staminaUsageRate = 6f;
    
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        staminaCurrent = staminaBase;
    }

    void Update()
    {
        if(!IsOwner) return;
        staminaCurrent += staminaRefreshRate * Time.deltaTime;
        if(Input.GetKey(KeyCode.LeftShift)) staminaCurrent -= staminaUsageRate * Time.deltaTime;
        staminaCurrent = Mathf.Clamp(staminaCurrent, 0, staminaBase);

        if(!IsOwner) return;
        GroundMovement();
        
    }

    void GroundMovement()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= Input.GetKey(KeyCode.LeftShift)&& staminaCurrent>0?runningSpeed:speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
