using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    NavMeshAgent agent; //agent controls enemy

    Animator animator;

    public GameObject target;

    public bool autoGenPath; //populate an array
    public string pathName;
    public GameObject[] path;
    public int pathIndex;
    public float distanceToNextNode;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        //if string is empty, path name will be PathNode
        if(string.IsNullOrEmpty(pathName))
        {
            pathName = "PatrolNode";
        }

        //This will fill the array, finding all points tagged with PathNode
        if (autoGenPath)
        {
            path = GameObject.FindGameObjectsWithTag("PatrolNode");
        }

        //Determines when to go to next node (if agent is stuck its not getting closer to 1)
        if(distanceToNextNode <= 0)
        {
            distanceToNextNode = 1.0f;
        }

        if(path.Length > 0)
        {
            target = path[pathIndex];
        }

        //if no target -> find player target
        if (!target)
        {
            target = GameObject.FindWithTag("Player");
        }

        //if (target)
        //    agent.SetDestination(target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        //right now we are passing it the target pathnodes instead of player. line 46-55. 
        //Need to set it so if the player enters a certain range set player as the target
        //if player leaves this range set target back to path
        //going to try to use a large trigger, if player enters target = player blah blah blah
        
        //moving agent to nodes
        Debug.DrawLine(transform.position, target.transform.position, Color.red);

        //if its close enough then go to next node (if within distance to next node move to next one) 
        //if(Vector3.Distance(transform.position, target.transform.position) < distanceToNextNode)
        //or
        //if((transform.position - target.transform.position).magnitude < distanceToNextNode)
        //or
        if(agent.remainingDistance < distanceToNextNode)
        {
            if (path.Length > 0)
            {
                pathIndex++; // go to next path node

                pathIndex %= path.Length; //this will reset it
                                          //if (pathIndex >= path.Lnegth)
                                          //path index = 0;

                target = path[pathIndex];
            }
        }

        //if we put this in update it will constantly chase you due to update call every frame
        //get it to move
        if (target)
            agent.SetDestination(target.transform.position);

        if (target.gameObject.tag == "Player")
        {
            animator.SetTrigger("Attack");
        }

        //if we are not on off mesh link (means we are walking)
        animator.SetBool("IsGrounded", !agent.isOnOffMeshLink);
        
        animator.SetFloat("Speed", Mathf.Abs(transform.TransformDirection(agent.velocity).z));
    }

    //Can base it so you can make nodes triggers, lil bubbles
    //Add collider to enemy
    //Add RigidBody to enemy
    //set to IsKinematic
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " has entered the trigger on: " + this);

        if (other.gameObject.name == "Player")
        {
            target = GameObject.FindWithTag("Player");

            //when player enters trigger play attack animation
            animator.SetTrigger("Attack");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other + " has exited the triggor on: " + this);

        if (other.gameObject.name == "Player")
        {
            //animator.SetBool("Attack", false);
            target = path[pathIndex];
        }
    }
    //need to add death from collision with player or projectile
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enemy script collision with: " + collision.gameObject.name);

        if (collision.gameObject.tag == "Projectile")
        {
            animator.SetBool("Death", true);
            Debug.Log("nice shot! destroying enemy...");
            Destroy(gameObject, 3);

            //make target null so it stops moving on collision (then will die in place)
            target = null;
            //might need to add a bool for death -> when true drop a pickup/collectable(set active to true on enemys transform)
           
        }
        //need a collision that once it hits player change target pack to node path (cuz we have death on collision)
        //this would change if you added in lives -> would want continous chase till player dies or leaves trigger
        if (collision.gameObject.tag == "Player")
        {
            target = path[pathIndex];
            Debug.Log("colliison with player setting target to: " + target);
        }
    }
}
