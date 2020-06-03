using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int _damage;

    // Start is called before the first frame update
    void Start()
    {
        if (Damage <= 0)
        {
            Damage = 10;

            Debug.Log("damage not set on " + name + ". Defaulting to " + Damage);
        }
    }

    
    public int Damage //property (getter and setter)
    {
        get { return _damage; } // get method
        set { _damage = value;} //set method
    }
}
