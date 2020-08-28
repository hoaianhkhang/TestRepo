using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    // Method 1: Reference to Rigidbody2D Component attached to Character
    // - Not shown in the Inspector
    Rigidbody2D rb;

    // Method 2: Reference to Rigidbody2D Component attached to Character
    // - Shown in the Inspector
    public Rigidbody2D rb2;

    // Handles movement speed of the Character
    // - public so that it can be changed in "Play Mode"
    public float speed;

    // Handles jumping for Character
    public float jumpForce;             // Tells Character how high to jump
    public bool isGrounded;             // Tells Character if they are on ground
    public LayerMask isGroundLayer;     // Tells Character what they can jump from as long as they are on a Layer named Ground or Platform 
    public Transform groundCheck;       // Used to tell Character if they are near or on Ground
    public float groundCheckRadius;     // Size of circle 'groundCheck' uses to check if Character is on Ground

    // Handles animations
    Animator anim;

    // Handle projectile Instantiation (aka Creation)
    public Transform projectileSpawnPoint;
    public Projectile projectile;
    public float projectileForce;

    // Handles Character flipping
    public bool isFacingLeft;

    // Handles amount of lives

    // Use this for initialization
    void Start()
    {

        // Use Script to save a reference to Rigidbody2D Component attached to Character
        rb = GetComponent<Rigidbody2D>();

        // Check if Component exists
        if (!rb)
        {
            // Prints message to Console
            Debug.LogWarning("No Rigidbody2D found on Character");
        }

        // Check if Component exists
        if (!rb2)
        {
            // Prints message to Console
            Debug.LogWarning("No Rigidbody2D found on Character");
        }

        // Check if 'speed' was set in Inspector 
        if (speed <= 0)
        {
            // Assign a default value of 5 to 'speed'
            speed = 5.0f;

            // Prints message to Console
            Debug.Log("Speed not set. Defaulting to " + speed);
        }

        // Check if 'jumpForce' was set in Inspector 
        if (jumpForce <= 0)
        {
            // Assign a default value of 5 to 'jumpForce'
            jumpForce = 5.0f;

            // Prints message to Console
            Debug.Log("JumpForce not set. Defaulting to " + jumpForce);
        }

        // Check if Component exists
        if (!groundCheck)
        {
            // Prints message to Console
            Debug.LogWarning("No GroundCheck found on Character");
        }

        // Check if 'groundCheckRadius' was set in Inspector 
        if (groundCheckRadius <= 0)
        {
            // Assign a default value of 0.2 to 'groundCheckRadius'
            groundCheckRadius = 0.2f;

            // Prints message to Console
            Debug.Log("GroundCheckRadius not set. Defaulting to " + groundCheckRadius);
        }

        // Use Script to save a reference to Animator Component attached to Character
        anim = GetComponent<Animator>();

        // Check if Component exists
        if (!anim)
        {
            // Prints message to Console
            Debug.LogWarning("No Animator found on Character");
        }

        // Check if Component exists
        if (!projectileSpawnPoint)
        {
            // Prints message to Console
            Debug.LogWarning("No projectileSpawnPoint found on Character");
        }

        // Check if Component exists
        if (!projectile)
        {
            // Prints message to Console
            Debug.LogWarning("No projectile found on Character");
        }

        // Check if 'projectileForce' was set in Inspector 
        if (projectileForce <= 0)
        {
            // Assign a default value of 7.0f to 'projectileForce'
            projectileForce = 7.0f;

            // Prints message to Console
            Debug.Log("projectileForce not set. Defaulting to " + projectileForce);
        }

     
    }

    // Update is called once per frame
    void Update()
    {

        // Check if 'groundCheck' is touching something tagged as "Ground" or "Platform"
        // - Can change 'groundCheckRadius' to a smaller/larger value if needed
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,
            groundCheckRadius, isGroundLayer);

        // Check if Left (or a) or Right (or d) is pressed
        // - "Horizontal" must exist in Input Manager (Edit->Project Settings->Input)
        // - Returns -1(left), 1(right), 0(no left or right pressed)
        // - Use GetAxis() for decimals -1 to 1 (Gradual change in value)
        float moveValue = Input.GetAxisRaw("Horizontal");

        // Check if "Jump" was pressed (Space)
        // - "Jump" must exist in Input Manager (Edit->Project Settings->Input)

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Prints message to Console
            Debug.Log("Jump.");
            anim.SetBool("jump", true);
            // Vector2.up       --> new Vector2(0,1);
            // Vector2.down     --> new Vector2(0,-1);
            // Vector2.left     --> new Vector2(-1,0);
            // Vector2.right    --> new Vector2(1,0);
            // Vector2.zero     --> new Vector2(0,0);

            // Apply a force in the up direction at a speed of "jumpForce"
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Check if Left Control or Left Click was pressed
        // - "Fire1" must exist in Input Manager (Edit-->Project Settings-->Input)
        // - Configuration can be changed later

        if (isGrounded == false && Input.GetButtonDown("Fire1"))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetBool("jump_attack", true);
                fireProjectile();
            }
        }
        if (Input.GetButtonDown("Fire1") && isGrounded == true)
        {
            // Prints a message to Console (Shortcut: Control+Shift+C)
            Debug.Log("Pew Pew");
            anim.SetBool("attack", true);
            anim.SetBool("jump", false);
            fireProjectile();
        }
        // Move using Rigidbody2D
        // - Uses 'moveValue' from GetAxis to move left or right
        rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);
        anim.SetFloat("speed", Mathf.Abs(moveValue));
        // Tells Animator to transition to another Animation Clip
        // - Parameter must be set in Animator panel
        anim.SetFloat("MoveValue", Mathf.Abs(moveValue));

        /*
        if (moveValue < 0 && !isFacingLeft)
            flip();
        else if (moveValue > 0 && isFacingLeft)
            flip();
        */

        // Check if Character should flip and look left and right
        if ((moveValue < 0 && !isFacingLeft) ||
            (moveValue > 0 && isFacingLeft))
        {
            // Call function to flip Character
            flip();
        }
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        //{
        //    anim.SetBool("attack", false);
        //}
       
    }

    void FinishedAttack()
    {
        anim.SetBool("attack", false);
        anim.SetBool("jump_attack", false);
    }

    // Function used to flip direction GameObject (Character) is facing
    void flip()
    {
        // Method 1: Toggle isFacingLeft variable
        isFacingLeft = !isFacingLeft;

        // Method 2: Toggle isFacingLeft variable
        /*if (isFacingLeft)
            isFacingLeft = false;
        else
            isFacingLeft = true;
            */

        // Make a copy of old scale value
        Vector3 scaleFactor = transform.localScale;

        // Flip scale of 'x' variable
        scaleFactor.x *= -1;    // scaleFactor.x = -scaleFactor.x;

        // Update scale to new flipped value
        transform.localScale = scaleFactor;
    }
    void OnTriggerEnter2D(Collider2D c)
    {
        // Prints a message to Console (Shortcut: Control+Shift+C)
        // - Prints what is being collided with Character.CS
        Debug.Log(c.gameObject.tag);

        // Check if Character collides with something tagged as "Collectible"
        if (c.gameObject.tag == "Collectible")
        {
            // Destroy GameObject colliding with Character
            Destroy(c.gameObject);
        }
        else if (c.gameObject.tag == "PowerUp")
        {
            speed += 2;
        }
    }

    // Function used to create and fire a Projectile
    void fireProjectile()
    {
        //anim.SetTrigger("Attack");

        // Creates Projectile and add its to the Scene
        // - projectPrefab is the thing to create
        // - projectileSpawnPoint is where and what rotation to use when created
        Projectile temp = Instantiate(projectile, projectileSpawnPoint.position,
            projectileSpawnPoint.rotation);
        temp.transform.Rotate(0, 0, 90);
        /*temp.GetComponent<Rigidbody2D>().velocity
            = Vector2.right * projectileForce;

        temp.GetComponent<Rigidbody2D>().AddForce(
            Vector2.right * projectileForce, ForceMode2D.Impulse);
       */

        temp.speed = projectileForce;

        // Apply movement speed to Projectile that is spawned
        // - Lets the projectile handle its own movement
        if (isFacingLeft)
        {
            temp.speed = -temp.speed;
            temp.transform.Rotate(0, 0, 180);
        }

    }



    // Give access to private variables (instance variables)
    // - Not needed if using public variables
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 8 is ground layer
        if (collision.gameObject.layer == 8)
        {
            isGrounded = true;
            anim.SetBool("jump", false);
        }
    }
}
