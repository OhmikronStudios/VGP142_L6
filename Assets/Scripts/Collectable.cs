using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectable : MonoBehaviour
{
    GameObject enemyTemp;
    bool enemyDeath = false;
    public GameObject collectable;
    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        //need to set it so if enemy is dead(or null) set this game object to active
        enemyTemp = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyTemp)
        {
            Debug.Log("Enemy was found"); 
        }
        else
        {
            Debug.Log("Enemy was not found");
            collectable.SetActive(true);
            
            enemyDeath = true;
        }
    }

    //collision to affect player on pickup
    private void OnCollisionEnter(Collision collision)
    {
        //only enter when collectible is active
        if (enemyDeath)
        {
            if (collision.gameObject.tag == "Player")
            {
                //do somefin
               
            }
        }
    }
}
