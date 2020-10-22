﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private Animator anim;
    private CharacterController charcontrol;
    private Collider mycollider;
    private float horizontal;
    private float vertical;
    private Vector3 direction;
    private string PARAMETER_STATE = "State";
    private bool isgrounded=true;
    private float grnd_distance;
    private float fallvelocity=0f;
    private float turnsmoothvelocity;

    public float gravity = 1f;
    public float move_speed;
    public float turnsmoothspeed;
    public float Jump_power;
    public float animmove_speed;
    public float animjump_speed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        charcontrol = GetComponent<CharacterController>();
        mycollider = GetComponent<Collider>();
    }

    void Start()
    {
        grnd_distance = mycollider.bounds.extents.y;
    }

    private void Update()
    {
        isgrounded = CheckGrounded();

        GetInput();
        Animation();

        if(!isgrounded)
        { 
            fallvelocity -= gravity * Time.deltaTime;
        }
    }

    public bool CheckGrounded()
    {
        RaycastHit hit;
        if (charcontrol.isGrounded)
        {
            return true;
        }
        if (Physics.Raycast(mycollider.bounds.center, Vector3.down, out hit, grnd_distance + 0.1f))
        {
            return true;
        }
        return false;
    }

    private void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal, 0f, vertical);
        direction.y = fallvelocity;

        if (horizontal != 0 || vertical != 0)
        {
            float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.rotation.y, targetangle, ref turnsmoothvelocity, turnsmoothspeed);
            transform.rotation = Quaternion.Euler(0f, targetangle, 0f);
        }    
       
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        charcontrol.Move(direction * move_speed * Time.deltaTime);
    }

    private void Jump()
    {
        if(isgrounded)
        {
            fallvelocity = Jump_power;
        }
    }

    private void Animation()
    {
        if(isgrounded)
        {
            if (horizontal != 0 || vertical != 0)
            {
                if (anim.GetInteger(PARAMETER_STATE) != 2)
                {
                    anim.SetInteger(PARAMETER_STATE, 1);
                    anim.speed = animmove_speed;
                }
            }
            else
            {
                if (anim.GetInteger(PARAMETER_STATE) != 2)
                {
                    anim.SetInteger(PARAMETER_STATE, 0);
                }
            }
        }
        else
        {
            anim.SetInteger(PARAMETER_STATE, 4);
            anim.speed = animjump_speed;
        }
    }
}
