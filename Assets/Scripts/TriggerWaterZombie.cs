using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWaterZombie : MonoBehaviour
{
    private bool created = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !created)
        {
            created = true;
            BroadcastMessage("SpawnWater");
            Destroy(gameObject);
        }
    }
}
