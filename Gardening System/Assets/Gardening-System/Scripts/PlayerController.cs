﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GardenSystem;

public enum ToolState
{
    Hoe,
    Seeds,
    WateringCan,
    None
}

// Attach this with a rigidbody and collider to your character
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    // Character movement speed, adjust to match your game scale
    public float speed = 3.0f;

    // Rigidbody, collider, and sprite renderer, and animator attached to your 2D character. It grabs these items at scene start
    Rigidbody2D rigidbody2d;
    //Collider2D collider;
    SpriteRenderer sprite;
    Animator animator;

    Vector2 lookDirection;

    // Define with unity player input button to use for object interaction
    public string plantButton = "Jump";

    // Define the layer your interactable colliders are on
    public LayerMask interactMask;
    public ToolState toolState = ToolState.None;
    public Plant equippedSeed;

    // Tracking the direction your character is looking -> for animating your character
    //Vector2 lookDirection = new Vector2(1, 0);

    // Defined by the DialogueUI script while dialogue is running
    public bool inDialogue = false;

    protected void Start()
    {
        // Get the components attached to your character. Animator commented out for still sprite testing
        rigidbody2d = GetComponentInChildren<Rigidbody2D>();
        //collider = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        //animator = GetComponentInChildren<Animator>();
        instance = this;
    }

    // Update is called once per frame
    protected void Update()
    {
        // Do nothing if dialogue is playing
        if (inDialogue)
        {
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.T))
        {
            toolState = (toolState == ToolState.Hoe) ? ToolState.WateringCan : ToolState.Hoe;
        }

        if (Input.GetButtonDown(plantButton))
        {
            var hit = Physics2D.Raycast(rigidbody2d.position, lookDirection, 1f, interactMask);
            if(hit)
            {
                var gardenTile = hit.collider.gameObject.GetComponent<GardenPlot>();
                if (gardenTile)
                {
                    switch(toolState)
                    {
                        case ToolState.None:
                            gardenTile.Harvest();
                            break;
                        case ToolState.Hoe:
                            gardenTile.Till();
                            break;
                        case ToolState.Seeds:
                            gardenTile.Seed(equippedSeed);
                            break;
                        case ToolState.WateringCan:
                            gardenTile.Water();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        // Get your up/down/left/right player input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Movement direction and rate is your horizontal and vertical input values (good for joystick or arrow buttons)
        Vector2 move = new Vector2(horizontal, vertical);

        // If your moving, set your look direction as move direction and normalize the vector (set it to magnitude of 1)
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        // Set your animator animation direction and speeds
        //animator.SetFloat("Look X", lookDirection.x);
        //animator.SetFloat("Look Y", lookDirection.y);
        //animator.SetFloat("Speed", move.magnitude);

        // Get your current position
        Vector2 position = rigidbody2d.position;

        // Set your position as your position plus your movement vector, times your speed multiplier, and the current game timestep;
        position = position + move * speed * Time.deltaTime;

        // Tell the rigidbody to move to the positon specified
        rigidbody2d.MovePosition(position);


    }


}
