using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int ammo;
    [SerializeField] float reloadTime;
    [SerializeField] float bulletSpeed;
    [SerializeField] bool loaded;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawn;

  


    // Start is called before the first frame update
    void Start()
    {
        if (ammo <= 0)
        {
            ammo = 10;

            Debug.Log("ammo not set on " + name + ". Defaulting to " + ammo);
        }
    }

    // Update is called once per frame
    public void Shoot()
    {
        if (ammo > 0 && loaded)
        {
            StartCoroutine(Fire());

        }
        else
        {
            Debug.Log("RELOAD!!!");
        }
    }

    IEnumerator Fire()
    {
        loaded = false;
        ammo--;
        Rigidbody temp = Instantiate(projectile, projectileSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
        temp.AddForce(Vector3.forward * bulletSpeed);
        Debug.Log("Pew Pew");
        yield return new WaitForSeconds(reloadTime);
        loaded = true;
    }
}
