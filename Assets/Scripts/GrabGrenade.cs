using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabGrenade : MonoBehaviour
{
    private GameObject grenades;

    // Start is called before the first frame update
    void Start()
    {
        grenades = GameObject.Find("FPSController/FirstPersonCharacter");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0));
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (grenades.GetComponent<GrenadeThrow>().currentGrenades < 2)
            {
                Destroy(gameObject);
                grenades.SendMessage("addGrenade");
            }
        }
    }
}
