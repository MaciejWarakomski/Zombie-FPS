using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayersSpawner : MonoBehaviour
{
    private float positionX;
    private float positionZ;
    public GameObject spawnPoint;
    public float distance = 100;
    private RaycastHit hit;
    public GameObject player;

    private void Awake()
    {
        positionX = distance / 2;
        positionZ = distance / 2;
        for (int i = 0; i < ((1000 - (distance / 2)) / distance); i++)
        {
            for (int j = 0; j < ((1000 - (distance / 2)) / distance); j++)
            {
                Physics.Raycast(new Vector3(positionX + Random.Range(-distance / 10, distance / 10), 260, positionZ + Random.Range(-distance / 10, distance / 10)), Vector3.down, out hit);
                if (hit.point.y > 95 && hit.point.y < 110 && !((hit.point.x > player.transform.position.x - 50 && hit.point.x < player.transform.position.x + 50) && (hit.point.z > player.transform.position.z - 50 && hit.point.z < player.transform.position.z + 50)))
                {
                    GameObject SpawnPointStayer = Instantiate(spawnPoint, hit.point, new Quaternion(0, 0, 0, 0));
                    SpawnPointStayer.transform.parent = gameObject.transform;
                }
                positionX += distance;
            }
            positionX = distance / 2;
            positionZ += distance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
