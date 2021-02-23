using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    public Rigidbody grenade;
    public int currentGrenades = 2;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentGrenades > 0 && timer <= 0)
        {
            Rigidbody clone = Instantiate(grenade, transform.position, transform.rotation) as Rigidbody;
            currentGrenades--;
            timer = 2.0f;
            clone.AddForce(transform.TransformDirection(Vector3.forward * 800));
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    void addGrenade()
    {
        currentGrenades++;
    }
}
