using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickerUpper : MonoBehaviour
{
    GameObject weapon;
    public GameObject weaponAttach;
    public GameObject holster;
    public float dropForce;

    // Start is called before the first frame update
    void Start()
    {
        weapon = null;
        if (!weaponAttach)
        {
            weaponAttach = GameObject.Find("WeaponAttach");
        }

        if (dropForce <= 0)
        {
            dropForce = 10;

            Debug.Log("damage not set on " + name + ". Defaulting to " + dropForce);
        }
    }

     void Update()
    {
        if (weapon && Input.GetKeyDown(KeyCode.T)) // t for toss, dropping weapon on Input call
        {
           
                weapon.transform.SetParent(null);
                // weaponAttach.transform.DetachChildren();  this will also work

                //turning physics back on
                weapon.GetComponent<Rigidbody>().isKinematic = false;

                //legit throw it away
                weapon.GetComponent<Rigidbody>().AddForce(Vector3.up * dropForce, ForceMode.Impulse);

                //turn collisions back on
                Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), weapon.GetComponent<Collider>(), false);

                weapon = null;
            
        }
        if (weapon && Input.GetButtonDown("Fire1"))
        {
            Weapon w = weapon.GetComponent<Weapon>();

            if (w)
                w.Shoot();
        }
        HolsterWeapon();
        DrawWeapon();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Weapon"))
        {
            weapon = hit.gameObject;

            weapon.gameObject.transform.SetPositionAndRotation(weaponAttach.transform.position,
                weaponAttach.transform.rotation); //this says make the weapon connected so need a reference to it (2 lines up)


            //need to tell weapon it is now a child of weapon attach
            //to follow it around
            weapon.transform.SetParent(weaponAttach.transform);

            //now get the physics components (turn off physics bc kinematic)
            weapon.GetComponent<Rigidbody>().isKinematic = true;

            //after its been collected can tell it to ingnore collisions using physics
            //on gameObject and weapon
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), weapon.GetComponent<Collider>(), true);
        }


    }

    private void HolsterWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon.gameObject.transform.SetPositionAndRotation(holster.transform.position,
               holster.transform.rotation);
            weapon.transform.SetParent(holster.transform);
            ;
        }
    }

    private void DrawWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon.gameObject.transform.SetPositionAndRotation(weaponAttach.transform.position,
                weaponAttach.transform.rotation);
            weapon.transform.SetParent(weaponAttach.transform);
        }
    } 


}
