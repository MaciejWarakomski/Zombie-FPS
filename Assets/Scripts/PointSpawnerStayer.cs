using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawnerStayer : MonoBehaviour
{
    public GameObject zombie;
    private bool created = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !created)
        {
            created = true;
            GameObject ZombieStaying = Instantiate(zombie, gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            ZombieStaying.transform.parent = gameObject.transform.parent;
            Destroy(gameObject);
        }
    }
}
