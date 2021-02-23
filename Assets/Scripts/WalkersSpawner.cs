using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkersSpawner : MonoBehaviour
{
    private float positionX;
    private float positionZ;
    public GameObject spawnPoint;
    public float distance = 100;
    private RaycastHit hit;
    public GameObject player;

    private void Awake()
    {
        positionX = 0;
        positionZ = 0;
        for (int i = 0; i < (1000 / distance); i++)
        {
            for (int j = 0; j < (1000 / distance); j++)
            {
                Physics.Raycast(new Vector3(positionX + Random.Range(-distance / 10, distance / 10), 260, positionZ + Random.Range(-distance / 10, distance / 10)), Vector3.down, out hit);
                if (hit.point.y > 99 && hit.point.y < 101 && !((hit.point.x > player.transform.position.x -50 && hit.point.x < player.transform.position.x + 50) && (hit.point.z > player.transform.position.z - 50 && hit.point.z < player.transform.position.z + 50)))
                {
                    GameObject SpawnPointWalker = Instantiate(spawnPoint, hit.point, new Quaternion(0, 0, 0, 0));
                    SpawnPointWalker.transform.parent = gameObject.transform;
                }
                positionX += distance;
            }
            positionX = 0;
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
