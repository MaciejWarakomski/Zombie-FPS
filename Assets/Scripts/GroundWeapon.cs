using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundWeapon : MonoBehaviour
{
    public int weaponNumber;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                other.SendMessage("addGun", weaponNumber);
                Destroy(gameObject);
            }
        }
    }
}
