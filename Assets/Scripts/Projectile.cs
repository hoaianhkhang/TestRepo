using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    // Used to tell GameObject (Projectile) how long to live without colliding with anything
    public float lifeTime;

    // Used to tell GameObject (Projectile) how fast to move
    public float speed;

    // Use this for initialization
    void Start()
    {
        // Check if 'lifeTime' was set in Inspector 
        if (lifeTime <= 0)
        {
            // Assign a default value of 2.0f to 'lifeTime'
            lifeTime = 2.0f;

            // Prints message to Console
            Debug.Log("lifeTime not set. Defaulting to " + lifeTime);
        }

        // Take Rigidbody2D component and change its velocity to value passed
        GetComponent<Rigidbody2D>().velocity
            = new Vector2(speed, 0);

        // Destroy gameObject after 'lifeTime' seconds
        Destroy(gameObject, lifeTime);
    }


    // Check for collisions with other GameObjects
    // - One or both GameObjects must have a Rigidbody2D attached
    // - Both need colliders attached
    void OnCollisionEnter2D(Collision2D c)
    {
        // Check if "Projectile" GameObject hits something not named "Player"
        // - Stops "Projectile" GameObject from being destroyed if it hits "Player" GameObject
        if (c.gameObject.tag != "Player")
        {
            // Destory GameObject Script is attached to
            Destroy(gameObject);
        }

        // Destory GameObject that this GameObject collides with
        // - 'c' can be called anything because it is a variable name
        //Destroy(c.gameObject);
    }
}
