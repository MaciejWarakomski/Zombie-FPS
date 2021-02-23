using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KevlarPack : MonoBehaviour
{
    public float ammunition;
    public int gunType;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            if (other.GetComponent<PlayerStats>().currentArmour < 100)
            {
                other.GetComponent<PlayerStats>().SendMessage("addKevlar");
                Destroy(gameObject);
            }
        }
    }
}
