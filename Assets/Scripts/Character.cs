using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// The GameObject is made to bounce using the space key.
// Also the GameOject can be moved forward/backward and left/right.
// Add a Quad to the scene so this GameObject can collider with a floor.

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    public float speed;
    public float jumpSpeed;
    public float gravity;

    Vector3 moveDirection = Vector3.zero;
    public CharacterController controller;

    public Transform thingToLookFrom;
    public float lookDistance;

    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileForce;

    Animator animator;

    public bool isGodMode;
    public float timerGodMode;

    public float jumpBoost;
    public float timerJumpBoost;

    public float speedBoost = 10.0f;
    public float timerSpeedBoost = 15.0f;

    public float reducedGrav = -8.0f;
    public float timerReducedGrav = 15.0f;

    public float timeSinceLastPain;
    public float painDelay;
    public int health;

    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (speed <= 0)
        {
            speed = 6.0f;

            Debug.Log("Speed not set on " + name + ". Defaulting to " + speed);
        }

        if (jumpSpeed <= 0)
        {
            jumpSpeed = 8.0f;

            Debug.Log("JumpSpeed not set on " + name + ". Defaulting to " + jumpSpeed);
        }

        if (gravity <= 0)
        {
            gravity = 9.81f;

            Debug.Log("Gravity not set on " + name + ". Defaulting to " + gravity);
        }

        if (lookDistance <= 0)
        {
            lookDistance = 10.0f;

            Debug.Log("LookDistance not set on " + name + ". Defaulting to " + lookDistance);
        }

        if (projectileForce <= 0)
        {
            projectileForce = 10.0f;

            Debug.Log("ProjectileForce not set on " + name + ". Defaulting to " + projectileForce);
        }

        isGodMode = false;

        if (timerGodMode <= 0)
        {
            timerGodMode = 2.0f;

            Debug.Log("ProjectileForce not set on " + name + ". Defaulting to " + projectileForce);
        }

        if (jumpBoost <= 0)
        {
            jumpBoost = 20.0f;

            Debug.Log("timer jump boost not set on " + name + ". Defaulting to " + jumpBoost);
        }

        if (timerJumpBoost <= 0)
        {
            timerJumpBoost = 2.0f;

            Debug.Log("timer jump boost not set on " + name + ". Defaulting to " + timerJumpBoost);
        }
        
        if (speedBoost <= 0)
        {
            speedBoost = 10.0f;

            Debug.Log("timer jump boost not set on " + name + ". Defaulting to " + timerJumpBoost);
        }if (timerSpeedBoost <= 0)
        {
            timerSpeedBoost = 5.0f;

            Debug.Log("timer jump boost not set on " + name + ". Defaulting to " + timerJumpBoost);
        }if (reducedGrav <= 0)
        {
            reducedGrav = -8.0f;

            Debug.Log("timer jump boost not set on " + name + ". Defaulting to " + timerJumpBoost);
        }if (timerReducedGrav <= 0)
        {
            timerReducedGrav = 12.0f;

            Debug.Log("timer jump boost not set on " + name + ". Defaulting to " + timerJumpBoost);
        }

        // let the gameObject fall down
        //gameObject.transform.position = new Vector3(0, 5, 0);

        if (painDelay <= 0)
        {
            painDelay = 3.0f;

            Debug.Log("painDelay not set on " + name + ". Defaulting to " + painDelay);
        }

        timeSinceLastPain = 0;

        if (health <= 0)
        {
            health = 100;

            Debug.Log("health not set on " + name + ". Defaulting to " + health);
        }


    }

    void Update()
    {
        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);

        // Used for the Raycast to store information about what it collides with
        RaycastHit hit;

        // If there is no thingToLookFrom, default to Character
        if(!thingToLookFrom)
        {
            Debug.DrawRay(transform.position, transform.forward * lookDistance, Color.red);

            if(Physics.Raycast(transform.position, transform.forward, out hit, lookDistance))
            {
                Debug.Log("Raycast: " + hit.collider.gameObject.name);      
            }
        }
        else
        {
            Debug.DrawRay(thingToLookFrom.position, thingToLookFrom.forward * lookDistance, Color.red);

            if (Physics.Raycast(thingToLookFrom.position, thingToLookFrom.forward, out hit, lookDistance))
            {
                Debug.Log("Raycast: " + hit.collider.gameObject.name);
            }
        }

        if(Input.GetButtonDown("Fire1"))
        {
            //fire();

            //use animation even to create projectile
            animator.SetTrigger("Attack"); //attack uses event to call fire in unity
        }
        
        //use z from controller to set forward action
        animator.SetFloat("Speed", transform.TransformDirection(controller.velocity).z);
        animator.SetBool("IsGrounded", controller.isGrounded); //if this doesnt work manually set it in if statements for jump, isGrounded: true or false

    }

    //void fire()
    //{
    //    Debug.Log("Shooty Wooty");

    //    Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

    //    temp.AddForce(projectileSpawnPoint.forward * projectileForce, ForceMode.Impulse);


    //}

    // Usage Rules:
    // - Both GameObjects must have Colliders
    // - One or both GameObjects must have a Rigidbody
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter with: " + collision.gameObject.name);

        //if (collision.gameObject.name == "Enemy")
        //{
        //    //here would remove lives
        //    animator.SetBool("Death", true);

        //    Destroy(gameObject, 3);
        //    SceneManager.LoadScene("SampleScene");

        //}
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit: " + collision.gameObject.name);

        if (collision.gameObject.name == "Enemy")
        {
            //here would remove lives
            animator.SetBool("Death", true);
            //Destroy(gameObject, 5);
            //SceneManager.LoadScene("Level1");

        }
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay: " + collision.gameObject.name);
    }

    // Usage Rules:
    // - GameObject must have CharacterController
    // - Other GameObject must have a Collider
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("OnControllerColliderHit: " + hit.gameObject.name);

        if (hit.gameObject.name == "Enemy")
        {
            //here would remove lives
            animator.SetBool("Death", true);
            //Destroy(gameObject, 5);
            SceneManager.LoadScene("Level1");

        }

        if (hit.gameObject.name == "Cube_pickup")
        {
            Debug.Log("cube hit " + hit + "Reloading scene");
            //speed = speed * 2;
            SceneManager.LoadScene("Level1");
        }
    }

    // Usage Rules:
    // - GameObject must have Collider marked as "IsTrigger"
    // - One or both GameObjects must have a Collider
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.gameObject.tag);

        if (other.CompareTag("PowerUp_GodMode"))
        {
            isGodMode = true;

            //change colour n stuff
            GetComponentInChildren<Renderer>().material.color = Color.blue;

            Destroy(other.gameObject);

            StartCoroutine(stopGodmode());
        }

        if (other.CompareTag("PowerUp_SuperJump"))
        {
            jumpSpeed += jumpBoost;

            Destroy(other.gameObject);

            StartCoroutine(stopJumpBoost());
        }
        
        if (other.CompareTag("PowerUp_SuperSpeed"))
        {
            speed += speedBoost;
            Destroy(other.gameObject);
            Debug.Log(speed);

            StartCoroutine(stopSpeedBoost());
        }
        
        if (other.CompareTag("PowerUp_ReducedGravity"))
        {
            gravity += reducedGrav;

            Destroy(other.gameObject);
            Debug.Log(gravity);

            StartCoroutine(stopReducedGrav());
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("OnTriggerExit: " + other.gameObject.name);
    //}

    //void OnTriggerStay(Collider other)
    //{
    //    Debug.Log("OnTriggerStay: " + other.gameObject.name);

    //    if (other.CompareTag("Obstacle_TimedPain"))
    //    {
    //        if (Time.time > timeSinceLastPain + painDelay) //fire rate could be used here
    //        {
    //            //grab component obstacle from other and get damage per second 
    //            Obstacle o = other.GetComponent<Obstacle>();

    //            if (o)
    //            {
    //                health -= o.Damage; //knows to get function from obstacle script due to get/set 
    //                timeSinceLastPain = Time.time;
    //            }
    //        }
    //    }
    //}

    IEnumerator stopGodmode()
    {
        yield return new WaitForSeconds(timerGodMode);

        //now when god mode is done it goes back to white
        GetComponentInChildren<Renderer>().material.color = Color.white;

        isGodMode = false;
    }
    IEnumerator stopJumpBoost()
    {
        yield return new WaitForSeconds(timerJumpBoost);

        jumpSpeed -= jumpBoost;
    }
    
    IEnumerator stopSpeedBoost()
    {
        yield return new WaitForSeconds(timerSpeedBoost);

        speed -= speedBoost;
        Debug.Log("powerup ended, speed returned to: " + speed);
    }
    
    IEnumerator stopReducedGrav()
    {
        yield return new WaitForSeconds(timerReducedGrav);

        gravity -= reducedGrav;
    }
}