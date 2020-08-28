using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Automatically adds Components when Script is added to GameObject
[RequireComponent(typeof(Rigidbody2D))]     // Rigidbody2D is added if not already added
[RequireComponent(typeof(BoxCollider2D))]   // BoxCollider2D is added if not already added
public class Collectible : MonoBehaviour
{

    // Reference Rigidbody2D and BoxCollider2D through script
    // - Not shown in Inspector
    Rigidbody2D rb;
    BoxCollider2D bc;

    // Use this for initialization
    void Start()
    {
        // Reference Rigidbody2D and BoxCollider2D through script
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();

        // Change Rigidbody2D variables through Script
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Change BoxCollider2D variables through Script
        bc.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        // Check if GameObject "ExtraLife" hits something named "Player"
        if (c.gameObject.tag == "Player")
        {
            // Grab "Character" Script attached to "Player" GameObject
            Character cc = c.gameObject.GetComponent<Character>();

            // Check if Script (Character.CS) was attached to "Player" GameObject before using it
              // Remove GameObject the "Player" GameObject collided with
            Destroy(gameObject);
        }
    }
}
