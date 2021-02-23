using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammopack : MonoBehaviour
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
            if (other.GetComponent<GunsInventory>().canAddAmmo[gunType] == 1)
            {
                GunsInventory inventory = other.GetComponent<GunsInventory>();
                inventory.SendMessage("addAmmo", new Vector2(ammunition, gunType));
                Destroy(gameObject);
            }
        }
    }
}
